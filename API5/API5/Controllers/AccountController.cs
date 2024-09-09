using API5.Context;
using API5.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using API5.Repository.Interfaces;

namespace API5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public AccountController(IAccountRepository accountRepository, IEmployeeRepository employeeRepository)
        {
            _accountRepository = accountRepository;
            _employeeRepository = employeeRepository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (string.IsNullOrEmpty(request.Password) || string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.EmployeeId))
            {
                return BadRequest("All fields except Username are required.");
            }

            var employee = await _employeeRepository.GetByEmployeeIdAsync(request.EmployeeId);

            if (employee == null)
            {
                return BadRequest("Employee not found.");
            }

            string baseUsername = string.IsNullOrEmpty(request.Username)
                ? $"{employee.FirstName.ToLower()}{employee.LastName.ToLower()}".Replace(" ", "")
                : request.Username.ToLower().Replace(" ", "");

            string username = await GenerateUniqueUsername(baseUsername);

            var account = new Account
            {
                Username = username,
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                AccountId = request.EmployeeId,
                Employee = employee
            };

            await _accountRepository.AddAsync(account);

            var response = new
            {
                status = 200,
                message = "Registration successful.",
                data = new
                {
                    employeeId = employee.Employee_Id,
                    name = $"{employee.FirstName} {employee.LastName}",
                    departmentName = employee.Department?.Dept_Name ?? "No Department",
                    username,
                    email = employee.Email
                }
            };

            return Ok(response);
        }

        private async Task<string> GenerateUniqueUsername(string baseUsername)
        {
            string username = baseUsername;
            int suffix = 1;

            while (await _accountRepository.GetByUsernameAsync(username) != null)
            {
                username = $"{baseUsername}{suffix:D3}";
                suffix++;
            }

            return username;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("Username and Password are required.");
            }

            var account = await _accountRepository.GetByUsernameAsync(request.Username);

            if (account == null || !BCrypt.Net.BCrypt.Verify(request.Password, account.Password))
            {
                return Unauthorized("Invalid username or password.");
            }

            return Ok(new { status = 200, message = "Login successful." });
        }

        [HttpGet("getAllAccounts")]
        public async Task<IActionResult> GetAllAccounts()
        {
            var accounts = await _accountRepository.GetAllAccountsAsync();

            if (accounts == null || accounts.Count == 0)
            {
                return NotFound("No accounts found.");
            }

            var response = accounts.Select(account => new
            {
                account_Id = account.AccountId,
                fullName = $"{account.Employee?.FirstName} {account.Employee?.LastName}",
                dept_Name = account.Employee?.Department?.Dept_Name ?? "No Department",
                email = account.Employee?.Email,
                username = account.Username
            }).ToList();

            return Ok(new
            {
                statusCode = 200,
                status = "Success",
                message = $"{accounts.Count} accounts retrieved successfully.",
                data = response
            });
        }
    }
public class RegisterRequest
    {
        public string? Username { get; set; }
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string EmployeeId { get; set; } = string.Empty;
    }

    public class LoginRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}