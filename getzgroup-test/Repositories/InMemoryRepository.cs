using getzgroup_test.Models;
using System.Collections.Concurrent;

namespace getzgroup_test.Repositories
{
    public class InMemoryRepository : IUserRepository
    {
        private readonly ConcurrentDictionary<Guid, Models.User> _users = new();
        public Task CreateAsync(User user)
        {
            // Create user
            if (!_users.TryAdd(user.Id, user))
                throw new InvalidOperationException("Failed to create user!");
            return Task.CompletedTask;
        }
        public Task<User?> GetByEmailAsync(string email)
        {
            // Find existing user by email
            var u = _users.Values.FirstOrDefault(x => !x.IsDeleted && string.Equals(x.Email, email, StringComparison.OrdinalIgnoreCase));
            return Task.FromResult<User?>(u);
        }
        public Task<User?> GetByIdAsync(Guid id)
        {
            // Find existing user by id
            _users.TryGetValue(id, out var user);
            // if user is soft deleted, return null
            if (user != null && user.IsDeleted) user = null;
            return Task.FromResult(user);
        }

        public Task<IEnumerable<User>> GetUsersAsync(string? search = null, int page = 1, int pageSize = 10)
        {
            var query = _users.Values
                // Exclude deleted users
                .Where(u => !u.IsDeleted)
                .AsQueryable();

            // Search for name and email
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(u =>
                    u.Name.Contains(search, StringComparison.OrdinalIgnoreCase) 
                    || u.Email.Contains(search, StringComparison.OrdinalIgnoreCase)
                );
            }

            // Pagination
            var result = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return Task.FromResult<IEnumerable<User>>(result);
        }

        public Task SoftDeleteAsync(Guid id)
        {
            // Soft delete user
            if (_users.TryGetValue(id, out var user))
            {
                user.IsDeleted = true;
                user.UpdatedAt = DateTime.UtcNow;
            }
            else
            {
                throw new KeyNotFoundException("User not found");
            }

            return Task.CompletedTask;
        }

        public Task UpdateAsync(User user)
        {
            // Update user
            if (_users.ContainsKey(user.Id))
            {
                _users[user.Id] = user;
            }
            return Task.CompletedTask;
        }
        public Task<IEnumerable<User>> GetAllUsersAsync()
        {
            var result = _users.Values.Where(u => !u.IsDeleted);
            return Task.FromResult(result.AsEnumerable());
        }
    }
}
