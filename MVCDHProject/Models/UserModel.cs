using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MVCDHProject.Models
{
    public class UserModel
    {
        [Required]
        public string Name { get;set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name="Confirm Password")]
        [Compare("Password",ErrorMessage="Confirm password should match with password")]
        public string ComparePassword { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name="Email id")]
        public string Email { get; set; }
        [Required]
        [RegularExpression("[6-9]\\d{9}",ErrorMessage="Mobile No.is Invalid.")]
        public string Mobile { get ; set; } 

    }
}
