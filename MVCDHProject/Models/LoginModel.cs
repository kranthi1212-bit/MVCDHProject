using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Net.Sockets;

namespace MVCDHProject.Models
{
    public class LoginModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public String Password { get; set; }
        [Display(Name="Remember Me")]
        public bool RememberMe { get; set; }
        public string ReturnUrl { get; set; } = "";
    }
}
