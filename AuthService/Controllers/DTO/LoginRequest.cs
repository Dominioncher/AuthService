using System.ComponentModel.DataAnnotations;

namespace AuthService.Controllers.DTO
{
    public class LoginRequest
    {
        [Required]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
