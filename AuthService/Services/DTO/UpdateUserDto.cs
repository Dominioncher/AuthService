using System.ComponentModel.DataAnnotations;

namespace AuthService.Services.DTO
{
    public class UpdateUserDto
    {
        public required Guid UserId { get; init; }

        [RegularExpression(@"^[a-zA-Zа-яА-Я0-9]+$")]
        public string? Name { get; set; }

        public Gender? Gender { get; set; }

        public DateTime? BirthDay { get; set; }

        public Guid? Initiator { get; set; }
    }
}
