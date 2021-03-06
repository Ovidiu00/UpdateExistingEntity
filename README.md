# UpdateExistingEntity
My approach in updating an existing entity in a disconnected scenario


## The reason behind this

Lets say you recieve an excel containing all the employees, every week, in the excel recieved this week an employee's name might have changed or his department possibly, this changes can happen from week to week.

You read the excel, create the proper employee entity and now you want to update the values coresponding to that employee in the database, typically you would use context.Update(<your Excel entity>) and ef core will atach it to the context with the state of modified and run an update to all the columns.This might work for small objects but having multiple proprieties might cause performance issues.
  
Another use-case would be recieving form data containing changes of an existing db record through an http put endpoint.
  
  ### Example of editing an entity recieved in API (BL Layer method)
   ```c#
     public async Task<DeviceDTO> Handle(EditDeviceCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var existingDevice = await unitOfWork.DeviceRepository.FindSingle(x => x.Id == request.id);

                if (existingDevice == null)
                    throw new DeviceNotFoundException($"Cant find device with id of {request.id}");

                Device modifiedDevice = await deviceMapper.MapEditDeviceDTOtoDeviceEntity(request.deviceToBeEdited);

                unitOfWork.DeviceRepository.UpdateIfModified(existingDevice, modifiedDevice, nameof(existingDevice.Id));

                await unitOfWork.Commit();

                return deviceMapper.MapDeviceEntityToDeviceDTO(modifiedDevice);
            }
            catch (System.Exception)
            {
                throw;
            }
        }
   ```
 

  

  
## What does this solution do?
  
Once you have your newly entity which already exists in the database and might or might not have any property changes, you retrive the database object for the corresponding primay key and pass them both to UpdateIfChanged generic method which will detect the changes between the disconected object and the database object and persist the changes to the database if there is any ,but it will update only the columns that changed.However it will not detect the changes for Reference Proprieties to different tables and Navigation Proprieties since only the FK property is relevant.
  
  
 ## Other solution
  
  Another solution would be using multiple ifs for every property and if the values differ then set it's state to modified , this assures that ef core will launch update only on the modified props but this may become tedious and error prone if you have a fat object.
  
 ```c#
   public void UpdateIfModified<T>(Employee existingEntity, Employee modifiedExistingEntity)
        {
            if(existingEntity.FirstName != modifiedExistingEntity.FirstName)
            {
                existingEntity.FirstName = modifiedExistingEntity.FirstName;
            }

            if (existingEntity.LastName != modifiedExistingEntity.LastName)
            {
                 existingEntity.LastName = modifiedExistingEntity.LastName;
            }

            // etc...
  
            //Once the property is set as modified , ef core's change tracker will take care of generating the proper update statements by calling DetectChanges inside context.SaveChanges()
        }
```
  
  Using this approach also means you have to do the same for every entity which you want to check for changes, UpdateIfModified can be reused with different types due to it's generic implementation
  
 ## SQL generated
  ### Using the common context.Update 
   ```c#
            using var context = new DemoDBContext();
            var modifiedExistingEntity = new Employee() // we know that an employee with the key '123_x' already exists in the DB
            {
                EmployeeId = "123_x",
                FirstName = "Dorel"
            };

            context.Update(modifiedExistingEntity);
            context.SaveChanges();
   ```
  <img src="https://github.com/Ovidiu00/UpdateExistingEntity/blob/main/Images/updateSql_usingUpdate.png" height=260px>
  
   ### Using UpdateIfModified
  ```c#
           using var context = new DemoDBContext();
            
            var existingEmployee = context.Employees.Find("123_x");

            var modifiedExistingEmployee = new Employee()
            {
                EmployeeId = "123_x",
                FirstName = "sfdsfd"
            };


            var utilityObject = new UtilityClass(context);
            utilityObject.UpdateIfModified<Employee>(existingEmployee, modifiedExistingEmployee);

            context.SaveChanges();
  
  
  ```
  <img src="https://github.com/Ovidiu00/UpdateExistingEntity/blob/main/Images/update_sql_usingCustomUpdate.png">
  
