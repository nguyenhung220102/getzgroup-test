using getzgroup_test.DTOs;
using System.Threading.Tasks;

namespace getzgroup_test.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetUsersAsync(string? search, int page, int pageSize);
        Task<UserDto> CreateAsync(CreateUserDto dto);
        Task<UserDto?> UpdateAsync(Guid id, UpdateUserDto dto);
        Task<bool> DeleteAsync(Guid id); 
        Task<UserDto?> GetByIdAsync(Guid id);
    }
}
