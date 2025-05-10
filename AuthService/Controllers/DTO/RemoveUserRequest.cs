using System.ComponentModel.DataAnnotations;

namespace AuthService.Controllers.DTO
{
    public class RemoveUserRequest
    {
        [Required]
        public string Login { get; set; }

        public bool? Permanent { get; set; }
    }
}
