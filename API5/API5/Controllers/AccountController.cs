using API5.Context;
using API5.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace API5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly MyContext _context;

        public AccountController(MyContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (string.IsNullOrEmpty(request.Password) ||
                string.IsNullOrEmpty(request.Email) ||
                string.IsNullOrEmpty(request.EmployeeId))
            {
                return BadRequest("All fields except Username are required.");
            }

            var employee = await _context.Employees
                .Include(e => e.Department)
                .Where(e => e.Employee_Id == request.EmployeeId)
                .FirstOrDefaultAsync();

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

            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();

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

            while (await _context.Accounts.AnyAsync(a => a.Username == username))
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

            var account = await _context.Accounts
                .Include(a => a.Employee)
                .Where(a => a.Username == request.Username)
                .FirstOrDefaultAsync();

            if (account == null || !BCrypt.Net.BCrypt.Verify(request.Password, account.Password))
            {
                return Unauthorized("Invalid username or password.");
            }

            var response = new
            {
                status = 200,
                message = "Login successful.",
            };

            return Ok(response);
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