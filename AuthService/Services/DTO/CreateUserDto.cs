using System.ComponentModel.DataAnnotations;

namespace AuthService.Services.DTO
{
    public class CreateUserDto
    {
        public required Guid Initiator { get; init; }

        [RegularExpression(@"^[a-zA-Z0-9]+$")]
        public required string Login { get; init; }

        [RegularExpression(@"^[a-zA-Z0-9]+$")]
        public required string Password { get; init; }

        [RegularExpression(@"^[a-zA-Zа-яА-Я0-9]+$")]
        public string Name { get; set; } = "";

        public Gender Gender { get; set; } = Gender.Unknown;

        public DateTime? BirthDay { get; set; } = null;

        public bool Admin { get; set; } = false;
    }
}
