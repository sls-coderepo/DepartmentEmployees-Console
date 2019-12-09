using DepartmentEmployees.Models;
using DepartmentsEmployees.Data;
using System;

namespace DepartmentEmployees
{
    class Program
    {
        static void Main(string[] args)
        {
            //Department
            var departmentRepo = new DepartmentRepository();
            var allDepartments = departmentRepo.GetAllDepartments();

            Console.WriteLine("All Departments");
            Console.WriteLine("---------------");
            foreach (var dept in allDepartments)
            {
                Console.WriteLine(dept.DepartmentName);
            }

            var hardCodedId = 2;
            var departmentWithId = departmentRepo.GetDepartmentById(hardCodedId);
            Console.WriteLine("---------------");
            Console.WriteLine($"Department with id {hardCodedId} is {departmentWithId.DepartmentName}");
            Console.WriteLine("---------------");

            //Employee List
            var employeeRepo = new EmployeeRepository();
            var allEmployee = employeeRepo.GetAllEmployees();

            Console.WriteLine("All Employees");
            Console.WriteLine("---------------");
            foreach (var employee in allEmployee)
            {
                Console.WriteLine($"{employee.FirstName} {employee.LastName}");
            }

            var hardCodedEmployeeId = 2;
            var employeeWithId = employeeRepo.GetEmployeeById(hardCodedEmployeeId);
            Console.WriteLine("---------------");
            Console.WriteLine($"Department with id {hardCodedEmployeeId} is {employeeWithId.FirstName} {employeeWithId.LastName}");

            //Employee with Department

            var allEmployeeRepo = new EmployeeRepository();
            var allEmployeeWithDepartment = allEmployeeRepo.GetAllEmployeesWithDepartment();

            Console.WriteLine("All Employees");
            Console.WriteLine("---------------");
            foreach (var employee in allEmployeeWithDepartment)
            {
                Console.WriteLine($"{employee.FirstName} {employee.LastName} works in {employee.EmployeeDepartment}");
            }

            Console.WriteLine("--------------------");
           
            //Add Department

            var legal = new Department();
            Console.WriteLine("What department do you like to add?");
            legal.DepartmentName = Console.ReadLine();
            departmentRepo.AddDepartment(legal);
            //Update Department
            Console.WriteLine("What Department (ID) would you like to update?");
            var departmentToUpdate = Int32.Parse(Console.ReadLine());
            Console.WriteLine("What should the new department name be called?");
            var newDepartmentName = Console.ReadLine();

            departmentRepo.UpdateDepartment(departmentToUpdate, new Department { DepartmentName = newDepartmentName });

            //Delete Department
            Console.WriteLine("What department do you want to delete?");
            var deleteDept = int.Parse(Console.ReadLine());
            departmentRepo.DeleteDepartment(deleteDept);
          
            Console.WriteLine("--------------------");
            //Add Employee
            var newEmployee = new Employee();
            Console.WriteLine("What is your first name?");
            newEmployee.FirstName = Console.ReadLine();
            Console.WriteLine("What is your last name?");
            newEmployee.LastName = Console.ReadLine();
            Console.WriteLine("What department do you want to work?");
            newEmployee.DepartmentId = Int32.Parse(Console.ReadLine());

            employeeRepo.AddEmployee(newEmployee);

            //Update Employee

            Console.WriteLine("Which Employee (ID) would you like to update?");
            var employeeToUpdate = Int32.Parse(Console.ReadLine());

            var updateEmployee = new Employee();
            Console.WriteLine("What is your first name?");
            updateEmployee.FirstName = Console.ReadLine();
            Console.WriteLine("What is your last name?");
            updateEmployee.LastName = Console.ReadLine();
            Console.WriteLine("What department do you want to work?");
            updateEmployee.DepartmentId = Int32.Parse(Console.ReadLine());

            employeeRepo.UpdateEmployee(employeeToUpdate, updateEmployee);

            //Delete Employee
            Console.WriteLine("Which employee do you want to delete?");
            var deleteEmp = int.Parse(Console.ReadLine());
            employeeRepo.DeleteEmployee(deleteEmp);
        }

    }
}
