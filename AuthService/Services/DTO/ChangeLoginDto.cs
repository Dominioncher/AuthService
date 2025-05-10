using System.ComponentModel.DataAnnotations;

namespace AuthService.Services.DTO
{
    public class ChangeLoginDto
    {
        public required Guid UserId { get; init; }

        [RegularExpression(@"^[a-zA-Z0-9]+$")]
        public required string Login { get; set; }

        public Guid? Initiator { get; set; } = null;
    }
}
