using BookStoreApp.Blazor.Server.UI.Models;
using BookStoreApp.Blazor.Server.UI.Services.Base;

namespace BookStoreApp.Blazor.Server.UI.Services
{
    public interface IBookService
    {
        Task<Response<BookReadOnlyDtoVirtualizeResponse>> Get(QueryParameters queryParams);
        Task<Response<List<BookReadOnlyDto>>> Get();
        Task<Response<BookDetailsDto>> Get(int id);
        Task<Response<BookUpdateDto>> GetForUpdate(int id);
        Task<Response<int>> Create(BookCreateDto author);
        Task<Response<int>> Edit(int id, BookUpdateDto author);
        Task<Response<int>> Delete(int id);
    }
}