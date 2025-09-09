using getzgroup_test.DTOs;
using getzgroup_test.Models;
using getzgroup_test.Repositories;
using System.Security.Cryptography;
using System.Text;

namespace getzgroup_test.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository) => _userRepository = userRepository;

        public async Task<UserDto> CreateAsync(CreateUserDto dto)
        {

            //Check if email already exists
            var existingUser = await _userRepository.GetByEmailAsync(dto.Email.Trim().ToLowerInvariant());
            if (existingUser != null) throw new InvalidOperationException("Email already existed!");

            var user = new User
            {
                Name = dto.Name,
                Title = dto.Title,
                Email = dto.Email.Trim().ToLowerInvariant(),
                PasswordHash = HashPassword(dto.Password)
            };

            await _userRepository.CreateAsync(user);
            return ToDto(user);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var existing = await _userRepository.GetByIdAsync(id);
            if (existing == null) return false;
            await _userRepository.SoftDeleteAsync(id);
            return true;
        }

        public async Task<UserDto?> GetByIdAsync(Guid id)
        {
            var u = await _userRepository.GetByIdAsync(id);
            if (u == null) return null;
            return ToDto(u);
        }

        public async Task<IEnumerable<UserDto>> GetUsersAsync(string? search, int page, int pageSize)
        {
            page = Math.Max(1, page);

            var users = await _userRepository.GetUsersAsync(search, page, pageSize);

            return users.Select(ToDto);
        }

        public async Task<UserDto?> UpdateAsync(Guid id, UpdateUserDto dto)
        {
            var existingUser = await _userRepository.GetByIdAsync(id);
            if (existingUser == null) return null;

            if (!string.IsNullOrWhiteSpace(dto.Email) &&
                !string.Equals(existingUser.Email, dto.Email.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                var byEmail = await _userRepository.GetByEmailAsync(dto.Email.Trim().ToLowerInvariant());
                if (byEmail != null && byEmail.Id != id)
                    throw new InvalidOperationException("Email already exists!");
                existingUser.Email = dto.Email.Trim().ToLowerInvariant();
            }

            if (!string.IsNullOrWhiteSpace(dto.Name)) existingUser.Name = dto.Name.Trim();
            if (dto.Title != null) existingUser.Title = dto.Title.Trim();

            existingUser.UpdatedAt = DateTime.UtcNow;
            await _userRepository.UpdateAsync(existingUser);
            return ToDto(existingUser);
        }

        public static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToHexString(hash);
        }

        private static UserDto ToDto(User u) =>
            new UserDto
            {
                Id = u.Id,
                Name = u.Name,
                Title = u.Title,
                Email = u.Email,
                CreatedAt = u.CreatedAt,
                UpdatedAt = u.UpdatedAt
            };
    }
}
