# UpdateExistingEntity
My approach in updating an existing entity in a disconnected scenario


## The reason behind this

Lets say you recieve an excel containg all the employees, every week, in the excel recieved this week an employee's name might have changed or his department possibly, this changes can happen from week to week.

You read the excel, create the proper employee entity and now you want to update the values corespondig to that employee in the database, typically you would use context.Update(<your Excel entity>) and ef core will atach it to the context with the state of modified and run an update to all the columns.This might work for small objects but having multiple proprieties might cause performance issues.
  

  
## What does this solution do?
  
Once you have your newly entity which already exists in the database and might or might not have any proprieties changes, you retrive the database object for the corresponding primay key and pass them both to UpdateIfChanged generic method which will detect the changes between the disconected object and the database object and persist the changes to the database if there is any ,but it will update only the columns that changed.However it will not detect the changes for Reference Proprieties to different tables and Navigation Proprieties since only the FK property is relevant.
  
  
  
 # Other solution
