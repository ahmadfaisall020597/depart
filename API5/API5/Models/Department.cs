using System.ComponentModel.DataAnnotations;

namespace API5.Models
{


    public class Department
    {
        [Key]

        public string? Dept_Id { get; set; }


        [Required(ErrorMessage = "Data Tidak Terisi, Tolong Isi Datanya yaa..!!")]
        public string? Dept_Initial { get; set; }


        [Required(ErrorMessage = "Data Tidak Terisi, Tolong Isi Datanya yaa..!!")]
        public string? Dept_Name { get; set; }

    }
}
