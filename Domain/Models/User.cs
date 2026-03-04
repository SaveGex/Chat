using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    [PrimaryKey(nameof(Id))]
    [Index(nameof(Identifier))]
    public class User
    {
        public Guid Id { get; set; }
        /// <summary>
        /// An <b>@UniqueUserSpecifiedTag</b>
        /// <u><br/> don't add '@' on the beginning</u>
        /// </summary>
        public string Identifier { get; set; } = null!;
        [Length(1, 256, ErrorMessage = "Username cannot be less than 1 and more than 256 characters")]
        public string Username { get; set; } = null!;
        public string? IconUrl { get; set; }
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;

        public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
    }
}
