using API5.Context;
using API5.Models;
using API5.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata.Ecma335;

namespace API5.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly MyContext _myContext;
        public DepartmentRepository(MyContext myContext)
        {
            _myContext = myContext;
        }

        private string GenerateDeptId()
        {

            var LastDept_Id = _myContext.Departments.OrderByDescending(d => d.Dept_Id).Select(d => d.Dept_Id).FirstOrDefault();

            if (LastDept_Id != null)
            {
                int lastNumber = int.Parse(LastDept_Id.Substring(1));
                int nextNumber = lastNumber + 1;
                return $"D{nextNumber:D3}";
            }

            return "D001";
        }

        public IEnumerable<Department> GetAllDepartment()
        {
            return _myContext.Departments.ToList();
        }

        public Department GetDepartmentById(string deptId)
        {
            if (deptId == null)
            {
                return null;
            }
            return _myContext.Departments.Find(deptId);

            //return _myContext.Departments.SingleOrDefault();
        }


        public int AddDepartment(Department department)
        {

            department.Dept_Id = GenerateDeptId();
            _myContext.Departments.Add(department);
            return _myContext.SaveChanges();
        }

        public int UpdateDepartment(Department department)
        {
            _myContext.Entry(department).State = EntityState.Modified;
            return _myContext.SaveChanges();
        }

        public int DeleteDepartment(string deptId)
        {
            var departmentToDelete = _myContext.Departments.Find(deptId);
            if (departmentToDelete != null)
            {
                _myContext.Departments.Remove(departmentToDelete);
                return _myContext.SaveChanges();
            }
            return 0;
        }

    
    }
}
