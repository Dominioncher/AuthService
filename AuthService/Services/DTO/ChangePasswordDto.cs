using System.ComponentModel.DataAnnotations;

namespace AuthService.Services.DTO
{
    public class ChangePasswordDto
    {
        public required Guid UserId { get; init; }

        [RegularExpression(@"^[a-zA-Z0-9]+$")]
        public required string Password { get; init; }

        public Guid? Initiator { get; set; } = null;
    }
}
