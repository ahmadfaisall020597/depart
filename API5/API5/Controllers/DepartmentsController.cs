using API5.Models;
using API5.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;
using System.ComponentModel.DataAnnotations;
using API5.Repositories.Interfaces;

namespace API5.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class DepartmentsController : ControllerBase
    {
        private DepartmentRepository _departmentRepository;
        public DepartmentsController(DepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }


        [HttpGet]
        public IActionResult GetAllDepartments()
        {
            var departments = _departmentRepository.GetAllDepartment();
            int departmentCount = departments.Count();
            if (departments.Any())
            {
                return Ok(new { Status = "200", 
                                Message = $"{departmentCount} Data Berhasil Ditemukan", 
                                Data = departments });
            }
            return NotFound(new { Status = 404, 
                                  Message = "Tidak Terdapat Data" });
        }


        //[HttpGet]
        //public IActionResult GetAllDepartments()
        //{
        //    var departments = _departmentRepository.GetAllDepartment();

        //    if (departments.Count() != 0)
        //    {
        //        return Ok(departments);
        //    }
        //    else
        //    {
        //        return NotFound("Tidak Terdapat Data");
        //    }
        //}

        [HttpGet("{deptId}")]
        public IActionResult GetDepartmentById(string deptId)
        {
            var department = _departmentRepository.GetDepartmentById(deptId);
            if (department == null)
            {
                return NotFound(new { Status = 404, 
                                      Message = "Data Tidak Ditemukan" });
            }
            return Ok(new { Status = "200", 
                            Message = "Data Berhasil Ditemukan", 
                            Data = department });
        }

        [HttpPost]
        public IActionResult AddDepartment(Department department)
        {
            var addDepartment = _departmentRepository.AddDepartment(department);
            if (addDepartment > 0)
            {
                return Ok(new { Status = "200", 
                                Message = "Data Berhasil Ditambahkan", 
                                Data = department });
            }
            else
            {
                return NotFound(new { Status = 404, 
                                      Message = "Data Gagal Ditambahkan" });
            }
        }

        //[HttpPut("{deptId}")]
        //public IActionResult UpdateDepartment(Department department)
        //{
        //    var updateDepartment = _departmentRepository.UpdateDepartment(department);

        //    if (updateDepartment > 0)
        //    {
        //        return Ok();

        //    }
        //    return NotFound();
        //}

         [HttpPut("{deptId}")]
        public IActionResult UpdateDepartment(Department department)
        {
            var updateDepartment = _departmentRepository.UpdateDepartment(department);

            if (updateDepartment > 0)
            {
                return Ok(new { 
                    Status = "200", 
                    Message = "Data Berhasil Diubah", 
                    Data = department });

            }
            return NotFound(new { Status = 404,
                                  Message = "Data Gagal DiUbah" });
        }




    [HttpDelete("{deptId}")]

        public IActionResult DeleteDepartment(string deptId)
        {
            int deleteDepartment = _departmentRepository.DeleteDepartment(deptId);
            if (deleteDepartment > 0)
            {
                return Ok(new { Status = "200", 
                                Message = $"Data {deptId} Berhasil Dihapus", 
                                Data = deleteDepartment });
                //return Ok($"Data {deptId} Berhasil diHapus");
            }
            return NotFound( new { Status = 404, 
                                   Message = $"Terjadi Error, Data {deptId} Tidak ada" });
        }

    }
}
