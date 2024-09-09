using System.ComponentModel.DataAnnotations;

namespace API5.Models
{
    public class RegisterVM
    {
        [Required(ErrorMessage = "First Name is required.")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last Name is required.")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Department ID is required.")]
        public string Dept_Id { get; set; } = string.Empty;
    }
}