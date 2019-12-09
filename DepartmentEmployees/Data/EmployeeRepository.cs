using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using DepartmentEmployees.Models;

namespace DepartmentsEmployees.Data
{
    /// <summary>
    ///  An object to contain all database interactions.
    /// </summary>
    public class EmployeeRepository
    {
        /// <summary>
        ///  Represents a connection to the database.
        ///   This is a "tunnel" to connect the application to the database.
        ///   All communication between the application and database passes through this connection.
        /// </summary>
        public SqlConnection Connection
        {
            get
            {
                // This is "address" of the database
                string _connectionString = "Data Source=localhost\\SQLEXPRESS;Initial Catalog=DepartmentsEmployees;Integrated Security=True";
                return new SqlConnection(_connectionString);
            }
        }

        /// <summary>
        ///  Returns a list of all departments in the database
        /// </summary>
        public List<Employee> GetAllEmployees()
        {
            //  We must "use" the database connection.
            //  Because a database is a shared resource (other applications may be using it too) we must
            //  be careful about how we interact with it. Specifically, we Open() connections when we need to
            //  interact with the database and we Close() them when we're finished.
            //  In C#, a "using" block ensures we correctly disconnect from a resource even if there is an error.
            //  For database connections, this means the connection will be properly closed.
            using (SqlConnection conn = Connection)
            {
                // Note, we must Open() the connection, the "using" block   doesn't do that for us.
                conn.Open();

                // We must "use" commands too.
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    // Here we setup the command with the SQL we want to execute before we execute it.
                    cmd.CommandText = "SELECT Id, FirstName, LastName FROM Employee";

                    // Execute the SQL in the database and get a "reader" that will give us access to the data.
                    SqlDataReader reader = cmd.ExecuteReader();

                    // A list to hold the employee we retrieve from the database.
                    List<Employee> employees = new List<Employee>();
                    
                    // Read() will return true if there's more data to read
                    while (reader.Read())
                    {
                        // The "ordinal" is the numeric position of the column in the query results.
                        //  For our query, "Id" has an ordinal value of 0 and "DeptName" is 1.
                        int idColumnPosition = reader.GetOrdinal("Id");

                        // We user the reader's GetXXX methods to get the value for a particular ordinal.
                        int idValue = reader.GetInt32(idColumnPosition);

                        int empFirstNameColumnPosition = reader.GetOrdinal("FirstName");
                        string empFirstNameValue = reader.GetString(empFirstNameColumnPosition);

                        int empLastNameColumnPosition = reader.GetOrdinal("LastName");
                        string empLastNameValue = reader.GetString(empLastNameColumnPosition);

                        // Now let's create a new employees object using the data from the database.
                        Employee employee = new Employee
                        {
                            Id = idValue,
                            FirstName = empFirstNameValue,
                            LastName = empLastNameValue
                            
                         };

                        // ...and add that employees object to our list.
                        employees.Add(employee);
                    }

                    // We should Close() the reader. Unfortunately, a "using" block won't work here.
                    reader.Close();

                    // Return the list of employees who whomever called this method.
                    return employees;
                }
            }
        }

        /// <summary>
        ///  Returns a single employees with the given id.
        /// </summary>
        public Employee GetEmployeeById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT FirstName, LastName, DepartmentId FROM Employee WHERE Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = cmd.ExecuteReader();

                    Employee  employee = null;

                    // If we only expect a single row back from the database, we don't need a while loop.
                    if (reader.Read())
                    {
                        employee = new Employee
                        {
                            Id = id,
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            DepartmentId = reader.GetInt32(reader.GetOrdinal("DepartmentId"))
                        };
                    }

                    reader.Close();

                    return employee;
                }
            }
        }

        public List<Employee> GetAllEmployeesWithDepartment()
        {
            using (SqlConnection conn = Connection)
            {
                // Note, we must Open() the connection, the "using" block   doesn't do that for us.
                conn.Open();
                // We must "use" commands too.
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    // Here we setup the command with the SQL we want to execute before we execute it.
                    cmd.CommandText = @"SELECT e.Id, e.FirstName, e.LastName, d.DeptName FROM Employee e 
                                        LEFT JOIN  Department d ON e.DepartmentId = d.Id";


                    // Execute the SQL in the database and get a "reader" that will give us access to the data.
                    SqlDataReader reader = cmd.ExecuteReader();
                    // A list to hold the departments we retrieve from the database.
                    List<Employee> employees = new List<Employee>();
                    // Read() will return true if there's more data to read
                    while (reader.Read())
                    {
                        // The "ordinal" is the numeric position of the column in the query results.
                        //  For our query, "Id" has an ordinal value of 0 and "DeptName" is 1.
                        int idColumnPosition = reader.GetOrdinal("Id");
                        // We user the reader's GetXXX methods to get the value for a particular ordinal.
                        int idValue = reader.GetInt32(idColumnPosition);
                        int firstNameColumnPosition = reader.GetOrdinal("FirstName");
                        string firstNameValue = reader.GetString(firstNameColumnPosition);
                        int lastNameColumnPosition = reader.GetOrdinal("LastName");
                        string lastNameValue = reader.GetString(lastNameColumnPosition);
                        int departmentNameColumnPosition = reader.GetOrdinal("DeptName");
                        string departmentNameValue = reader.GetString(departmentNameColumnPosition);

                        // Now let's create a new department object using the data from the database.
                        Employee employee = new Employee
                        {
                            Id = idValue,
                            FirstName = firstNameValue,
                            LastName = lastNameValue,
                            EmployeeDepartment = departmentNameValue
                        };
                        // ...and add that department object to our list.
                        employees.Add(employee);
                    }
                    // We should Close() the reader. Unfortunately, a "using" block won't work here.
                    reader.Close();
                    // Return the list of departments who whomever called this method.
                    return employees;
                }
            }
        }

        /// <summary>
        ///  Add a new employees to the database
        ///   NOTE: This method sends data to the database,
        ///   it does not get anything from the database, so there is nothing to return.
        /// </summary>
        public void AddEmployee(Employee employee)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    // These SQL parameters are annoying. Why can't we use string interpolation?
                    // ... sql injection attacks!!!
                    cmd.CommandText = "INSERT INTO Employee (FirstName, LastName, DepartmentId) OUTPUT INSERTED.Id Values (@FirstName, @LastName, @DepartmentId)";
                    cmd.Parameters.Add(new SqlParameter("@FirstName", employee.FirstName));
                    cmd.Parameters.Add(new SqlParameter("@LastName", employee.LastName));
                    cmd.Parameters.Add(new SqlParameter("@DepartmentId", employee.DepartmentId));
                   
                    int id = (int)cmd.ExecuteScalar();

                    employee.Id = id;

                }
            }

            // when this method is finished we can look in the database and see the new employees.
        }

        /// <summary>
        ///  Updates the employees with the given id
        /// </summary>
        public void UpdateEmployee(int id, Employee employee)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE Employee
                                     SET FirstName = @firstName,
                                         LastName = @lastName,
                                         DepartmentId = @deptId
                                     WHERE Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@firstName", employee.FirstName));
                    cmd.Parameters.Add(new SqlParameter("@lastName", employee.LastName));
                    cmd.Parameters.Add(new SqlParameter("@deptId", employee.DepartmentId));
                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    cmd.ExecuteNonQuery();
                }
            }
        }

        ///// <summary>
        /////  Delete the employees with the given id
        ///// </summary>
        public void DeleteEmployee(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM Employee WHERE Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    cmd.ExecuteNonQuery();
                }
            }
        }

    }
}