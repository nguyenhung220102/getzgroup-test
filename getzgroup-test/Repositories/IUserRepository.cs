using getzgroup_test.Models;

namespace getzgroup_test.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid id);
        Task<User?> GetByEmailAsync(string email);
        Task<IEnumerable<User>> GetUsersAsync(string? search = null, int page = 1, int pageSize = 10);
        Task<IEnumerable<User>> GetAllUsersAsync();

        Task CreateAsync(User user);
        Task UpdateAsync(User user);
        Task SoftDeleteAsync(Guid id);
    }
}
