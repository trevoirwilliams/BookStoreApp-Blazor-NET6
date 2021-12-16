using BookStoreApp.Blazor.Server.UI.Services.Base;
using BookStoreApp.Blazor.Server.UI.Models;

namespace BookStoreApp.Blazor.Server.UI.Services
{
    public interface IAuthorService
    {
        Task<Response<AuthorReadOnlyDtoVirtualizeResponse>> Get(QueryParameters queryParams);
        Task<Response<List<AuthorReadOnlyDto>>> Get();
        Task<Response<AuthorDetailsDto>> Get(int id);
        Task<Response<AuthorUpdateDto>> GetForUpdate(int id);
        Task<Response<int>> Create(AuthorCreateDto author);
        Task<Response<int>> Edit(int id, AuthorUpdateDto author);
        Task<Response<int>> Delete(int id);
    }
}