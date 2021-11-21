using Microsoft.EntityFrameworkCore;
using System;
using UpdateObjectEF.Models;

namespace UpdateObjectEF
{
    class Program
    {
        static void Main(string[] args)
        {

            using var context = new DemoDBContext();
            
            var existingEmployee = context.Employees.Find("123_x");

            var modifiedExistingEntity = new Employee()
            {
                EmployeeId = "123_x",
                FirstName = "sfdsfd"
            };


            var utilityObject = new UtilityClass(context);
            utilityObject.UpdateIfModified<Employee>(existingEmployee, modifiedExistingEntity, nameof(existingEmployee.EmployeeId));

            context.SaveChanges();
        }

     
    }
}
