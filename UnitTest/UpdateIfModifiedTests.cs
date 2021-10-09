using Microsoft.EntityFrameworkCore;
using System;
using UpdateObjectEF.Models;
using Xunit;

namespace UnitTests
{
    public class UpdateIfModifiedTests
    {
        private UtilityClass utilityClass;
        public UpdateIfModifiedTests()
        {
            var options = new DbContextOptionsBuilder<DemoDBContext>()
                                 .UseSqlServer("tesT")
                                 .Options;

            utilityClass = new UtilityClass(new DemoDBContext(options));
        }
        [Fact]
        public void UpdateIfModified_FirstNameChangedOfExistingEmployee_ChangesAreReflectedOnExistingEntity()
        {
            //Arange
            Employee existingEmployee = new Employee()
            {
                EmployeeId="1",
                FirstName = "foo",

            };
            Employee modifiedExistingEmployee = new Employee()
            {
                EmployeeId = "1",
                FirstName = "bar",
            };

            //Act

            utilityClass.UpdateIfModified(existingEmployee, modifiedExistingEmployee);



            //Assert
            Assert.Equal(existingEmployee.FirstName, modifiedExistingEmployee.FirstName);


        }
        [Fact]
        public void UpdateIfModified_LastNameChangedOfExistingEmployee_ChangesAreReflectedOnExistingEntity()
        {
            //Arange
            Employee existingEmployee = new Employee()
            {
                EmployeeId ="1",
                LastName = "FOO"
            };
            Employee modifiedExistingEmployee = new Employee()
            {
                EmployeeId = "1",
                LastName = "BAR"
            };

            //Act

            utilityClass.UpdateIfModified(existingEmployee, modifiedExistingEmployee);



            //Assert
            Assert.Equal(existingEmployee.LastName, modifiedExistingEmployee.LastName);

        }


        [Fact]
        public void UpdateIfModified_DepartmentIDChangedOfExistingEmployee_ChangesAreReflectedOnExistingEntity()
        {
            //Arange
            Employee existingEmployee = new Employee()
            {
                EmployeeId = "1",
                DepartmentId = 1
            };
            Employee modifiedExistingEmployee = new Employee()
            {
                EmployeeId = "1",
                DepartmentId = 2
            };

            //Act
            utilityClass.UpdateIfModified(existingEmployee, modifiedExistingEmployee);

            //Assert
            Assert.Equal(existingEmployee.EmploymentDate, modifiedExistingEmployee.EmploymentDate);
        }

        [Fact]
        public void UpdateIfModified_DepartmentReferencePropertyChangedOfExistingEmployee_ChangedNotReflected()
        {
            //Arange
            Employee existingEmployee = new Employee()
            {
                EmployeeId = "1",
                Department = new Department()
                {
                    Id = 1,
                    Name = "foo"
                }
            };
            Employee modifiedExistingEmployee = new Employee()
            {
                EmployeeId ="1",
                Department = new Department()
                {
                    Id = 2,
                    Name = "bar"
                }
            };

            //Act
            utilityClass.UpdateIfModified(existingEmployee, modifiedExistingEmployee);

            //Assert
            Assert.NotEqual(existingEmployee.Department.Id, modifiedExistingEmployee.Department.Id);
        }

        [Fact]
        public void UpdateIfModified_SexEnumTypeyChangedOfExistingEmployee_ChangesAreReflectedOnExistingEntity()
        {
            //Arange
            Employee existingEmployee = new Employee()
            {
                EmployeeId = "1",
                Sex = Sex.FEMALE
            };
            Employee modifiedExistingEmployee = new Employee()
            {
                EmployeeId = "1",
                Sex = Sex.MALE
            };

            //Act

            utilityClass.UpdateIfModified(existingEmployee, modifiedExistingEmployee);



            //Assert
            Assert.Equal(existingEmployee.Sex, modifiedExistingEmployee.Sex);
        }
    }
}
