using Microsoft.EntityFrameworkCore;
using ChatApp.Infrastructure.Data;
using ChatApp.Domain.Interfaces;
using ChatApp.Domain.Entities;

namespace ChatApp.Infrastructure.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(DbContext context) : base(context) { }

        public Task<bool> ExistsByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsByUsername(string username)
        {
            throw new NotImplementedException();
        }

        public Task<User?> GetByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task<User?> GetByPhoneNumberAsync(string phoneNumber)
        {
            throw new NotImplementedException();
        }

        public Task<User?> GetByUID(string uid)
        {
            throw new NotImplementedException();
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Username == username);
        }

        public Task<IEnumerable<User>> SearchUsersAsync(string searchTerm, int page = 1, int pageSize = 20)
        {
            throw new NotImplementedException();
        }
    }

}
