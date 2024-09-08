using API5.Models;
using System.ComponentModel.DataAnnotations;

namespace API5.Repositories.Interfaces
{
    public interface IDepartmentRepository
    {
        IEnumerable<Department> GetAllDepartment();
        Department GetDepartmentById(string deptId);


        int AddDepartment(Department department);

        int UpdateDepartment(Department department);

        int DeleteDepartment(string Dept_Id);
    }

}
