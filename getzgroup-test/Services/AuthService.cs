using getzgroup_test.DTOs;
using getzgroup_test.Repositories;
using System.Text;
using System.Text.Json;
using System.Security.Cryptography;

namespace getzgroup_test.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        public AuthService(IUserRepository userRepository) => _userRepository = userRepository;

        public async Task<string?> SignInAsync(SignInDto dto)
        {
            var user = await _userRepository.GetByEmailAsync(dto.Email.Trim().ToLowerInvariant());
            if (user == null || user.IsDeleted) return null;

            if (!string.Equals(HashPassword(dto.Password), user.PasswordHash, StringComparison.OrdinalIgnoreCase))
                return null;

            return Convert.ToBase64String(Guid.NewGuid().ToByteArray()); // jwt sample token
        }

        private static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToHexString(hash);
        }
    }
}
