using System.ComponentModel.DataAnnotations;

namespace Application.ModelsDTO
{
    public class RegisterDTO
    {
        [MaxLength(256)]
        public string? Identifier { get; set; }

        [Required]
        [MaxLength(256)]
        public string Username { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [MinLength(8)]
        public string Password { get; set; } = null!;
    }
}