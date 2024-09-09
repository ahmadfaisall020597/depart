using API5.Context;
using API5.Models;
using API5.Repositories.Interfaces;
using API5.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata.Ecma335;

namespace API5.Repository
{
    public class RegisterRepository : IRegisterRepository
    {
        private readonly MyContext _myContext;

        public RegisterRepository(MyContext context)
        {
            _myContext = context;
        }

        public async Task<Employee> GetEmployeeByEmailAsync(string email)
        {
            return await _myContext.Employees
                .Include(e => e.Department)
                .FirstOrDefaultAsync(e => e.Email == email);
        }

        public async Task<Department> GetDepartmentByIdAsync(string deptId)
        {
            return await _myContext.Departments
                .FirstOrDefaultAsync(d => d.Dept_Id == deptId);
        }

        public async Task<Account> GetAccountByEmailAsync(string email)
        {
            return await _myContext.Accounts
                .Include(a => a.Employee)
                .FirstOrDefaultAsync(a => a.Employee.Email == email);
        }

        public async Task<string> GenerateUniqueUsernameAsync(string baseUsername)
        {
            string username = baseUsername;
            int suffix = 1;

            while (await _myContext.Accounts.AnyAsync(a => a.Username == username))
            {
                username = $"{baseUsername}{suffix:D3}";
                suffix++;
            }

            return username;
        }

        public async Task AddAccountAsync(Account account)
        {
            _myContext.Accounts.Add(account);
            await _myContext.SaveChangesAsync();
        }

        public async Task<List<EmployeeAccountData>> GetAllEmployeeAccountDataAsync()
        {
            return await _myContext.Employees
                .Include(e => e.Department)
                .Join(_myContext.Accounts,
                    e => e.Employee_Id,
                    a => a.AccountId,
                    (e, a) => new EmployeeAccountData
                    {
                        EmployeeId = e.Employee_Id,
                        Name = $"{e.FirstName} {e.LastName}",
                        DepartmentName = e.Department != null ? e.Department.Dept_Name : "No Department",
                        Username = a.Username,
                        Email = e.Email
                    })
                .ToListAsync();
        }
        public class EmployeeAccountData
        {
            public string EmployeeId { get; set; }
            public string Name { get; set; }
            public string DepartmentName { get; set; }
            public string Username { get; set; }
            public string Email { get; set; }
        }
    }
}