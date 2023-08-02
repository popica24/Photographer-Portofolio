namespace MVCCore.Services.Abstract
{
    public interface IRepo <T>
    {
        Task CreateAsync(T entity);
        Task<T> GetAsync(string id);
        Task<List<T>> GetAll();
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity); 
    }
}
