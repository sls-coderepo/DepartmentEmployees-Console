using System;
using System.Collections.Generic;
using System.Text;

namespace DepartmentEmployees.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int DepartmentId { get; set; }
        public string EmployeeDepartment { get; set; }
        
    }
}
