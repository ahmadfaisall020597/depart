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
    public class AccountRepository : IAccountRepository
    {
        private readonly MyContext _context;

        public AccountRepository(MyContext context)
        {
            _context = context;
        }

        public async Task<Account> GetByUsernameAsync(string username)
        {
            return await _context.Accounts
                .Include(a => a.Employee)
                .FirstOrDefaultAsync(a => a.Username == username);
        }

        public async Task AddAsync(Account account)
        {
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Account>> GetAllAccountsAsync()
        {
            return await _context.Accounts
                .Include(a => a.Employee)
                .ThenInclude(e => e.Department)
                .ToListAsync();
        }
    }
}
