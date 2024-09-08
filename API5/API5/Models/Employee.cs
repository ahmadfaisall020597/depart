using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace API5.Models
{
    public class Employee
    {
        [Key]
        public string Employee_Id { get; set; } = string.Empty;

        [Required(ErrorMessage = "Data Tidak Terisi, Tolong Isi Datanya yaa..!!")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Data Tidak Terisi, Tolong Isi Datanya yaa..!!")]
        public string LastName { get; set; } = string.Empty;

        public string? Email { get; set; }

        [JsonIgnore]
        public virtual Department? Department { get; set; }

        [ForeignKey("Department")]
        public string Dept_Id { get; set; } = string.Empty;
    }
}
