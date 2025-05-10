using System.ComponentModel.DataAnnotations;

namespace AuthService.Controllers.DTO
{
    public class ChangePasswordRequest
    {
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Password does not comply with the rules")]
        [Required]
        public string NewPassword { get; set; }
    }
}
