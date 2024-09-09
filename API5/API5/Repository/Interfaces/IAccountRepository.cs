using API5.Models;

namespace API5.Repository.Interfaces
{
    public interface IAccountRepository
    {
        Task<Account> GetByUsernameAsync(string username);
        Task AddAsync(Account account);
        Task<List<Account>> GetAllAccountsAsync();
    }
}
