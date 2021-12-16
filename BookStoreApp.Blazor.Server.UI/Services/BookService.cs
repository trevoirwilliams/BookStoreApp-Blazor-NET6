using AutoMapper;
using Blazored.LocalStorage;
using BookStoreApp.Blazor.Server.UI.Models;
using BookStoreApp.Blazor.Server.UI.Services.Base;

namespace BookStoreApp.Blazor.Server.UI.Services
{
    public class BookService : BaseHttpService, IBookService
    {
        private readonly IClient client;
        private readonly IMapper mapper;

        public BookService(IClient client, ILocalStorageService localStorage, IMapper mapper) : base(client, localStorage)
        {
            this.client = client;
            this.mapper = mapper;
        }

        public async Task<Response<int>> Create(BookCreateDto book)
        {
            Response<int> response = new();

            try
            {
                await GetBearerToken();
                await client.BooksPOSTAsync(book);
            }
            catch (ApiException exception)
            {
                response = ConvertApiExceptions<int>(exception);
            }

            return response;
        }

        public async Task<Response<int>> Delete(int id)
        {
            Response<int> response = new();

            try
            {
                await GetBearerToken();
                await client.BooksDELETEAsync(id);
            }
            catch (ApiException exception)
            {
                response = ConvertApiExceptions<int>(exception);
            }

            return response;
        }

        public async Task<Response<int>> Edit(int id, BookUpdateDto book)
        {
            Response<int> response = new();

            try
            {
                await GetBearerToken();
                await client.BooksPUTAsync(id, book);
            }
            catch (ApiException exception)
            {
                response = ConvertApiExceptions<int>(exception);
            }

            return response;
        }

        public async Task<Response<BookDetailsDto>> Get(int id)
        {
            Response<BookDetailsDto> response;

            try
            {
                await GetBearerToken();
                var data = await client.BooksGET2Async(id);
                response = new Response<BookDetailsDto>
                {
                    Data = data,
                    Success = true
                };
            }
            catch (ApiException exception)
            {
                response = ConvertApiExceptions<BookDetailsDto>(exception);
            }

            return response;
        }

        public async Task<Response<BookReadOnlyDtoVirtualizeResponse>> Get(QueryParameters queryParams)
        {
            Response<BookReadOnlyDtoVirtualizeResponse> response;

            try
            {
                await GetBearerToken();
                var data = await client.BooksGETAsync(queryParams.StartIndex, queryParams.PageSize);
                response = new Response<BookReadOnlyDtoVirtualizeResponse>
                {
                    Data = data,
                    Success = true
                };
            }
            catch (ApiException exception)
            {
                response = ConvertApiExceptions<BookReadOnlyDtoVirtualizeResponse>(exception);
            }

            return response;
        }

        public async Task<Response<List<BookReadOnlyDto>>> Get()
        {
            Response<List<BookReadOnlyDto>> response;

            try
            {
                await GetBearerToken();
                var data = await client.GetAll2Async();
                response = new Response<List<BookReadOnlyDto>>
                {
                    Data = data.ToList(),
                    Success = true
                };
            }
            catch (ApiException exception)
            {
                response = ConvertApiExceptions<List<BookReadOnlyDto>>(exception);
            }

            return response;
        }

        public async Task<Response<BookUpdateDto>> GetForUpdate(int id)
        {
            Response<BookUpdateDto> response;

            try
            {
                await GetBearerToken();
                var data = await client.BooksGET2Async(id);
                response = new Response<BookUpdateDto>
                {
                    Data = mapper.Map<BookUpdateDto>(data),
                    Success = true
                };
            }
            catch (ApiException exception)
            {
                response = ConvertApiExceptions<BookUpdateDto>(exception);
            }

            return response;
        }
    }

}
