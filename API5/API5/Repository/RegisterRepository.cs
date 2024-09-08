//using System;
//using System.Diagnostics.Metrics;
//using System.Threading.Tasks;
//using API5.Context;
//using API5.Models;
//using API5.Repository.Interfaces;
//using Microsoft.EntityFrameworkCore;

//namespace API5.Repository
//{
//    public class RegisterRepository : IRegisterRepository
//    {
//        private readonly MyContext _myContext;
//        private int _counter;

//        public RegisterRepository(MyContext myContext)
//        {
//            _myContext = myContext;
//        }

//        public async Task CreateAccountAsync(RegisterVM registerVM)
//        {

//            var employee = await _myContext.Employees
//                .FirstOrDefaultAsync(e => e.Employee_Id == registerVM.EmployeeId);

//            if (employee == null)
//            {
//                throw new ArgumentException("Employee not found.");
//            }

          
//            var newAccount = new Account
//            {
//                AccountId = registerVM.EmployeeId,
//                Username = GenerateUsername(employee.FirstName, employee.LastName),
//                Password = registerVM.Password
//            };

//            _myContext.Accounts.Add(newAccount);
//            await _myContext.SaveChangesAsync();
//        }

//        public async Task<Account> GetAccountByEmployeeIdAsync(string employeeId)
//        {
//            return await _myContext.Accounts
//                .Include(a => a.Employee)
//                .FirstOrDefaultAsync(a => a.AccountId == employeeId);
//        }

//        private string GenerateUsername(string firstName, string lastName)
//        {
         
//            _counter++;
//            var suffix = _counter.ToString("D3");

//            return $"{firstName}.{lastName}{suffix}";
//        }
//    }
//}

