using System.ComponentModel.DataAnnotations;

namespace AuthService.Services.DTO
{
    public class User
    {
        public Guid Id { get; set; }

        public string Login { get; set; }

        public string Name { get; set; }

        public Gender Gender { get; set; }

        public DateTime? BirthDay { get; set; }

        public bool Admin { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        public DateTime ModifiedOn { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime? RevokedOn { get; set; }

        public string? RevokedBy { get; set; }
    }
}
