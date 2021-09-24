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
        public void UpdateIfModified_LastNameChangedOfExistingEmployee_ChangesAreReflectedOnExistingEntity()
        {
            //Arange
            Employee existingEmployee = new Employee()
            {
                LastName = "FOO"
            };
            Employee modifiedExistingEmployee = new Employee()
            {
                LastName = "BAR"
            };

            //Act

            utilityClass.UpdateIfModified(existingEmployee, modifiedExistingEmployee);



            //Assert
            Assert.Equal(existingEmployee.LastName, modifiedExistingEmployee.LastName);

        }
    }
}
