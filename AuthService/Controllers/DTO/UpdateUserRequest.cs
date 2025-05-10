using AuthService.Services.DTO;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AuthService.Controllers.DTO
{
    public class UpdateUserRequest
    {
        [RegularExpression(@"^[a-zA-Zа-яА-Я0-9]+$", ErrorMessage = "Name does not comply with the rules")]
        public string? Name { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Gender? Gender { get; set; }

        [DataType(DataType.Date)]
        public DateTime? BirthDay { get; set; }
    }
}
