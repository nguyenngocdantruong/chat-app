using ChatApp.Domain.Entities;

namespace ChatApp.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync();
    }
}
