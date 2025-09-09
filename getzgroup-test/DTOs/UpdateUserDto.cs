using System.ComponentModel.DataAnnotations;

namespace getzgroup_test.DTOs
{
    public class UpdateUserDto
    {
        public string? Name { get; set; }

        public string? Title { get; set; }

        [EmailAddress]
        public string? Email { get; set; }
    }
}
