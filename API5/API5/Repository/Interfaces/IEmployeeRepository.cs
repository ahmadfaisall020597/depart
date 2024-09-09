using API5.Models;
using System.ComponentModel.DataAnnotations;

namespace API5.Repository.Interfaces
{
    public interface IEmployeeRepository
    {
        IEnumerable<EmployeeVM> GetAllEmployeeData();
        Employee GetEmployeeById(string employeeId);
        IEnumerable<Employee> GetAllEmployee();
        int AddEmployee(Employee employee);

        int UpdateEmployee(Employee employee);

        int DeleteEmployee(string employeeId);
        Task<Employee> GetByEmployeeIdAsync(object employeeId);
    }
}

