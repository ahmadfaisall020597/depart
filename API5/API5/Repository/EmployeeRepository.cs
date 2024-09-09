using API5.Context;
using API5.Models;
using API5.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace API5.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly MyContext _myContext;

        public EmployeeRepository(MyContext myContext)
        {
            _myContext = myContext;
        }

        private string GenerateEmployeeId()
        {
            var now = DateTime.Now;
            string yearMonth = $"{now:yyyyMM}";

            var lastEmployeeId = _myContext.Employees
                .Where(e => e.Employee_Id.StartsWith(yearMonth))
                .OrderByDescending(e => e.Employee_Id)
                .Select(e => e.Employee_Id)
                .FirstOrDefault();

            if (lastEmployeeId != null)
            {
                string lastNumber = lastEmployeeId.Substring(yearMonth.Length);
                if (int.TryParse(lastNumber, out int sequenceNumber))
                {
                    int nextSequenceNumber = sequenceNumber + 1;
                    return $"{yearMonth}{nextSequenceNumber:D4}";
                }
            }
            return $"{yearMonth}0001";
        }

        public int AddEmployee(Employee employee)
        {

            var emailExists = _myContext.Employees
            .Any(e => e.Email == employee.Email);

            if (emailExists)
            {
                return -1;
            }

            var departmentExists = _myContext.Departments
                .Any(d => d.Dept_Id == employee.Dept_Id);

            if (!departmentExists)
            {
                employee.Dept_Id = "DEFAULT_ID";
            }

            employee.Employee_Id = GenerateEmployeeId();
            _myContext.Employees.Add(employee);
            return _myContext.SaveChanges();
        }

        public int DeleteEmployee(string employeeId)
        {
            var employeeToDelete = _myContext.Employees.Find(employeeId);
            if (employeeToDelete != null)
            {
                _myContext.Employees.Remove(employeeToDelete);
                return _myContext.SaveChanges();
            }
            return 0;
        }

        public Employee GetEmployeeById(string employeeId)
        {
            if (string.IsNullOrEmpty(employeeId))
            {
                return null;
            }
            return _myContext.Employees
               .Include(e => e.Department)
               .FirstOrDefault(e => e.Employee_Id == employeeId);
        }

        public int UpdateEmployee(Employee employee)
        {
            _myContext.Entry(employee).State = EntityState.Modified;
            return _myContext.SaveChanges();
        }

        public IEnumerable<EmployeeVM> GetAllEmployeeData()
        {
            return _myContext.Employees
               .Include(e => e.Department)
               .Select(e => new EmployeeVM
               {
                   Employee_Id = e.Employee_Id,
                   FirstName = e.FirstName,
                   LastName = e.LastName,
                   Email = e.Email,
                   Dept_Name = e.Department != null ? e.Department.Dept_Name : null
               })
               .ToList();


        }
        public IEnumerable<Employee> GetAllEmployee()
        {
            return _myContext.Employees.ToList();
        }

        public async Task<Employee> GetByEmployeeIdAsync(string employeeId)
        {
            if (employeeId == null)
            {
                return null;
            }

            return await _myContext.Employees
                .Include(e => e.Department)
                .FirstOrDefaultAsync(e => e.Employee_Id == employeeId.ToString());
        }
        public async Task<Employee?> GetByEmailAsync(string email)
        {
            return await _myContext.Employees
                .FirstOrDefaultAsync(e => e.Email == email);
        }

    }
} 
