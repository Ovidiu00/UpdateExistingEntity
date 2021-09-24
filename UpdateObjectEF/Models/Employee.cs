using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdateObjectEF.Models
{
    public class Employee
    {
        public string EmployeeId { get; set; }
        public string CNP { get; set; }
        public DateTime EmploymentDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }


        public int? DepartmentId { get; set; }
        public Department Department { get; set; }
        

    }
}
