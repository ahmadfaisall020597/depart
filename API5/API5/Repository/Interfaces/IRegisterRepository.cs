using System.Threading.Tasks;
using API5.Models;
using static API5.Repository.RegisterRepository;

namespace API5.Repository.Interfaces
{
    public interface IRegisterRepository
    {
        Task<Employee> GetEmployeeByEmailAsync(string email);
        Task<Department> GetDepartmentByIdAsync(string deptId);
        Task<Account> GetAccountByEmailAsync(string email);
        Task<string> GenerateUniqueUsernameAsync(string baseUsername);
        Task AddAccountAsync(Account account);
        Task<List<EmployeeAccountData>> GetAllEmployeeAccountDataAsync();
    }
}