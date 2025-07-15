using ChatApp.Models;

namespace ChatApp.Repo.Interfaces
{
        public interface IUserRepository : IGenericRepository<User>
        {
            Task<User?> GetByUsernameAsync(string username);
        }
}
