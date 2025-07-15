using ChatApp.Models;
using Microsoft.EntityFrameworkCore;
using ChatApp.Repo;
using ChatApp.Repo.Interfaces;
namespace ChatApp.Repo.Implementations
{
        public class UserRepository : GenericRepository<User>, IUserRepository
        {
            public UserRepository(AppDbContext context) : base(context) { }

            public async Task<User?> GetByUsernameAsync(string username)
            {
                return await _dbSet.FirstOrDefaultAsync(u => u.Username == username);
            }
        }

    }
