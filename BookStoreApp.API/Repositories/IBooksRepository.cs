using BookStoreApp.API.Data;
using BookStoreApp.API.Models;
using BookStoreApp.API.Models.Book;

namespace BookStoreApp.API.Repositories
{
    public interface IBooksRepository : IGenericRepository<Book>
    {
        Task<List<BookReadOnlyDto>> GetAllBooksAsync();
        Task<BookDetailsDto> GetBookAsync(int id);
    }
}
