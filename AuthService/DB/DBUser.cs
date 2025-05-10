using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace AuthService.DB
{
    [Index(nameof(Login), IsUnique = true)]
    public class DBUser
    {
        [Key]
        public Guid Id { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public string Name { get; set; }

        public int Gender { get; set; }

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
