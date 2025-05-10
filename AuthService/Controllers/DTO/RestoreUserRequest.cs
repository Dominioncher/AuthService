using System.ComponentModel.DataAnnotations;

namespace AuthService.Controllers.DTO
{
    public class RestoreUserRequest
    {
        [Required]
        public string Login { get; set; }
    }
}
