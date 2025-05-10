using System.ComponentModel.DataAnnotations;

namespace AuthService.Controllers.DTO
{
    public class ChangeLoginRequest
    {
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "login does not comply with the rules")]
        [Required]
        public string NewLogin { get; set; }
    }
}
