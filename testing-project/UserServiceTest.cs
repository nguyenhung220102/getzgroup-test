using getzgroup_test.DTOs;
using getzgroup_test.Models;
using getzgroup_test.Repositories;
using getzgroup_test.Services;
using Moq;
namespace testing_project
{
    public class UserServiceTest
    {
        private readonly Mock<IUserRepository> _userRepository;
        private readonly UserService _userService;
        public UserServiceTest()
        {
            _userRepository = new Mock<IUserRepository>();
            _userService = new UserService(_userRepository.Object);
        }

        [Fact]
        public async Task CreateUser_EmailExisted()
        {
            // Arrange
            var dto = new CreateUserDto { Email = "test@gmail.com", Name = "User", Password = "123456" };
            _userRepository.Setup(r => r.GetByEmailAsync(dto.Email))
                     .ReturnsAsync(new User { Email = dto.Email });

            // Act, Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _userService.CreateAsync(dto));
        }

        //[Fact]
        //public async Task SignIn_WrongPassword()
        //{
        //    // Arrange
        //    var dto = new SignInDto { Email = "test@gmnail.com", Password = "wrongpass" };
        //    var existingUsr = new User { Email = dto.Email, PasswordHash = UserService.HashPassword("password") };

        //    _userRepository.Setup(r => r.GetByEmailAsync(dto.Email)).ReturnsAsync(existingUsr);

        //    // Act
        //    var token = await _userService.SignInAsync(dto);

        //    // Assert
        //    Assert.Null(token);
        //}
        [Fact]
        public async Task GetUsers_ReturnsData()
        {
            // Arrange
            var users = Enumerable.Range(1, 21).Select(i => new User
            {
                Email = $"test{i}@gmail.com",
                Name = $"User{i}",
                PasswordHash = "password"
            }).ToList();

            _userRepository.Setup(r => r.GetUsersAsync(null, 2, 10))
                     .ReturnsAsync(users.Skip(10).Take(10));

            // Act
            var result = await _userService.GetUsersAsync(null, 2, 10);

            // Assert
            Assert.Equal(10, result.Count());
        }
    }
}