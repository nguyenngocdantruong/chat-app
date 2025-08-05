
using ChatApp.Domain.Entities;

namespace ChatApp.Domain.Interfaces
{
        public interface IUserRepository : IGenericRepository<User>
        {
            Task<User?> GetByUsernameAsync(string username);
            Task<User?> GetByEmailAsync(string email);
            Task<User?> GetByUID(Guid uid);
            Task<User?> GetByPhoneNumberAsync(string phoneNumber);

            Task<bool> ExistsByEmail(string email);
            Task<bool> ExistsByUsername(string username);

            Task<IEnumerable<User>> SearchUsersAsync(string searchTerm, int page = 1, int pageSize = 20);
    }
}
