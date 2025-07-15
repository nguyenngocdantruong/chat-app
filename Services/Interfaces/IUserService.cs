using ChatApp.Models;

namespace ChatApp.Services.Interfaces
{
    public interface IUserService : IGenericService<User>
    {
        Task<User?> GetByUsernameAsync(string username);
    }

}
