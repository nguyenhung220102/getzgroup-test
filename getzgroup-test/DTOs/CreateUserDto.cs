using System.ComponentModel.DataAnnotations;

namespace getzgroup_test.DTOs
{
    public class CreateUserDto
    {
        [Required]
        public string Name { get; set; } = default!;

        public string? Title { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; } = default!;

        [Required]
        public string Password { get; set; } = default!;
    }
}
