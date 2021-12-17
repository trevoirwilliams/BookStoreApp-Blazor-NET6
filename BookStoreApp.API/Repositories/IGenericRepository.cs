using BookStoreApp.API.Models;

namespace BookStoreApp.API.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> AddAsync(T entity);
        Task DeleteAsync(int id);
        Task<bool> Exists(int id);
        Task<List<T>> GetAllAsync();
        Task<VirtualizeResponse<TResult>> GetAllAsync<TResult>(QueryParameters queryParam) where TResult : class;
        Task<T> GetAsync(int? id);
        Task UpdateAsync(T entity);
    }
}