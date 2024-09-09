using API5.Models;
using API5.Repositories;
using API5.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {

        private EmployeeRepository _employeeRepository;
        public EmployeesController(EmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        [HttpGet("data")]
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


        [HttpGet]
        public IActionResult GetAllEmployee()
        {
            var employees = _employeeRepository.GetAllEmployee();
            if (employees.Any())
            {
                return Ok(new
                {
                    Status = "200",
                    Message = "Data Berhasil Ditemukan",
                    Data = employees
                });
            }
            return NotFound(new
            {
                Status = 404,
                Message = "Tidak Terdapat Data"
            });
        }


        [HttpGet("{employeeId}")]
        public IActionResult GetEmployeeById(string employeeId)
        {
            var employee = _employeeRepository.GetEmployeeById(employeeId);

            if (employee != null)
            {
                return Ok(employee);
            }

            return NotFound(new
            {
                Status = 404,
                Message = "Employee Not Found",
                Data = (object)null
            });
        }

        [HttpPost]
        public IActionResult AddEmployee(Employee employee)
        {
            var addEmployee = _employeeRepository.AddEmployee(employee);
            if (addEmployee > 0)
            {
                return Ok(new
                {
                    Status = "200",
                    Message = "Data Berhasil Ditambahkan",
                    Data = employee
                });
            }
            else if (addEmployee == -1)
            {
                return NotFound(new
                {
                    Status = "400",
                    Message = "Email Already exists"
                });
            } else
            {
                return NotFound(new
                {
                    Status = 404,
                    Message = "Data gagal Ditambahkan"
                });
            }
        }

        [HttpPut("{employeeId}")]
        public IActionResult UpdateEmployee(Employee employee)
        {
            var updateEmployee = _employeeRepository.UpdateEmployee(employee);

            if (updateEmployee > 0)
            {
                return Ok(new
                {
                    Status = "200",
                    Message = "Data Berhasil Diubah",
                    Data = employee
                });

            }
            return NotFound(new
            {
                Status = 404,
                Message = "Data Gagal DiUbah"
            });
        }




        [HttpDelete("{employeeId}")]

        public IActionResult DeleteDepartment(string employeeId)
        {
            int deleteEmployee = _employeeRepository.DeleteEmployee(employeeId);
            if (deleteEmployee > 0)
            {
                return Ok(new
                {
                    Status = "200",
                    Message = $"Data {employeeId} Berhasil Dihapus",
                    Data = deleteEmployee
                });
                //return Ok($"Data {employeeId} Berhasil diHapus");
            }
            return NotFound(new
            {
                Status = 404,
                Message = $"Terjadi Error, Data {employeeId} Tidak ada"
            });
        }

    }
}
