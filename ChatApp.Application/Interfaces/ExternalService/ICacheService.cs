namespace ChatApp.Application.Interfaces.ExternalService
{
    public interface ICacheService<T>
    {
        Task<T> Get(string key);
        Task Set(string key, T value, TimeSpan? expiration = null);
        Task Remove(string key);
    }
}
