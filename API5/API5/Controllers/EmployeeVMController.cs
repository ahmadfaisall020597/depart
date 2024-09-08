using Microsoft.AspNetCore.Mvc;
using System.Linq;
using API5.Models;
using API5.Repository.Interfaces;

namespace API5.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        [HttpGet]
        public IActionResult GetAllEmployeeData()
        {
            var employees = _employeeRepository.GetAllEmployeeData();
            int employeeCount = employees.Count();

            if (employees.Any())
            {
                return Ok(new
                {
                    Status = 200,
                    Message = $"{employeeCount} Data Berhasil Ditemukan",
                    AmountOfData = employeeCount,
                    Data = employees
                });
            }

            return NotFound(new
            {
                Status = 404,
                Message = "Data Tidak Ditemukan",
                AmountOfData = 0,
                Data = (object[])null
            });
        }
    }
}

