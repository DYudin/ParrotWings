using System.ComponentModel.DataAnnotations;

namespace ParrotWings.ViewModel
{
    public class RegistrationViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        //[Display(Name = "Confirm password")]
        //[DataType(DataType.Password)]
        //[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        //public string ConfirmPassword { get; set; }
    }
}