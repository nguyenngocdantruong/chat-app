
using ChatApp.Domain.Entities;

namespace ChatApp.Application.Interfaces
{
    public interface IUserService : IGenericService<User>
    {
        Task<User?> GetByUsernameAsync(string username);
    }

}
