using API5.Context;
using API5.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using API5.Repository.Interfaces;

namespace API5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly IRegisterRepository _repository;

        public RegisterController(IRegisterRepository repository)
        {
            _repository = repository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterVM request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    statusCode = 202,
                    status = "Error",
                    message = "Invalid request data.",
                    errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                });
            }

            var employee = await _repository.GetEmployeeByEmailAsync(request.Email);

            if (employee == null)
            {
                return BadRequest(new
                {
                    statusCode = StatusCodes.Status400BadRequest,
                    status = "Error",
                    message = "Employee not found."
                });
            }

            var department = await _repository.GetDepartmentByIdAsync(request.Dept_Id);

            if (department == null)
            {
                return BadRequest(new
                {
                    statusCode = StatusCodes.Status400BadRequest,
                    status = "Error",
                    message = "Department not found."
                });
            }

            var existingAccount = await _repository.GetAccountByEmailAsync(request.Email);

            if (existingAccount != null)
            {
                return BadRequest(new
                {
                    statusCode = StatusCodes.Status400BadRequest,
                    status = "Error",
                    message = "An account with this email already exists."
                });
            }

            string baseUsername = $"{request.FirstName.ToLower()}{request.LastName.ToLower()}".Replace(" ", "");
            string username = await _repository.GenerateUniqueUsernameAsync(baseUsername);

            string defaultPassword = "12345";

            var account = new Account
            {
                Username = username,
                Password = defaultPassword,
                AccountId = employee.Employee_Id,
                Employee = employee
            };

            try
            {
                await _repository.AddAccountAsync(account);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    statusCode = 500,
                    status = "Error",
                    message = "An error occurred while saving the account.",
                    details = ex.Message
                });
            }

            var response = new
            {
                statusCode = "200",
                status = "Success",
                message = "Registration successful.",
                data = new
                {
                    employeeId = employee.Employee_Id,
                    name = $"{request.FirstName} {request.LastName}",
                    departmentName = department.Dept_Name,
                    username,
                    email = request.Email
                }
            };

            return Ok(response);
        }

        [HttpGet("alldata")]
        public async Task<IActionResult> GetAllData()
        {
            var data = await _repository.GetAllEmployeeAccountDataAsync();

            int count = data.Count();

            if (data.Any())
            {
                return Ok(new
                {
                    statusCode = StatusCodes.Status200OK,
                    status = "Success",
                    message = "Data successfully retrieved.",
                    totalDatas = count,
                    data
                });
            }

            return NotFound(new
            {
                statusCode = StatusCodes.Status404NotFound,
                status = "Error",
                message = "No data found.",
                totalCount = 0
            });
        }
    }
}