using getzgroup_test.DTOs;

namespace getzgroup_test.Services
{
    public interface IAuthService
    {
        Task<string?> SignInAsync(SignInDto dto);

    }
}
