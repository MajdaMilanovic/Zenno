namespace ZennoServices.Interfaces
{
    public interface IService<T, TSearch> where T : class where TSearch : class
    {
        Task<List<T>> GetAsync(TSearch search);
        Task<T?> GetByIdAsync(int id);
    }
}