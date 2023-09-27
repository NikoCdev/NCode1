using System.ComponentModel.DataAnnotations;

namespace MusicPortal.DAL.Entities
{
    public class RegisterModel
    {       

        [Required]
        public string? Login { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        [DataType(DataType.Password)]
        public string? PasswordConfirm { get; set; }
    }
}