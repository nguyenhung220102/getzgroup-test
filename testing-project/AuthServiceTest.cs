using getzgroup_test.DTOs;
using getzgroup_test.Models;
using getzgroup_test.Repositories;
using getzgroup_test.Services;
using Moq;
using Xunit;
namespace testing_project
{
    public class AuthServiceTest
    {
        private readonly Mock<IUserRepository> _userRepository;
        private readonly AuthService _authService;
        public AuthServiceTest()
        {
            _userRepository = new Mock<IUserRepository>();
            _authService = new AuthService(_userRepository.Object);
        }


        [Fact]
        public async Task SignIn_WrongPassword()
        {
            // Arrange
            var dto = new SignInDto { Email = "test@gmnail.com", Password = "wrongpass" };
            var existingUsr = new User { Email = dto.Email, PasswordHash = UserService.HashPassword("password") };

            _userRepository.Setup(r => r.GetByEmailAsync(dto.Email)).ReturnsAsync(existingUsr);

            // Act
            var token = await _authService.SignInAsync(dto);

            // Assert
            Assert.Null(token);
        }
    }
}