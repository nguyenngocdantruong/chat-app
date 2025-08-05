using ChatApp.Domain.Interfaces;
using ChatApp.Domain.Exceptions.Database;

namespace ChatApp.Infrastructure.Data
{
    public class UnitOfWork(AppDbContext context) : IUnitOfWork
    {
        public async Task<int> SaveChangesAsync()
        {
            try
            {
                return await context.SaveChangesAsync(); 
            }
            catch (Exception ex)
            {
                throw new DatabaseOperationException($"An error occurred while saving changes to the database: {ex.Message}");
            }
        }

    }
}
