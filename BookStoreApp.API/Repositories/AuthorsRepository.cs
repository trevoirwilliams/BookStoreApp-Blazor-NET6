using AutoMapper;
using AutoMapper.QueryableExtensions;
using BookStoreApp.API.Data;
using BookStoreApp.API.Models;
using BookStoreApp.API.Models.Author;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApp.API.Repositories
{
    public class AuthorsRepository : GenericRepository<Author>, IAuthorsRepository
    {
        private readonly BookStoreDbContext context;
        private readonly IMapper mapper;

        public AuthorsRepository(BookStoreDbContext context, IMapper mapper) : base(context, mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<AuthorDetailsDto> GetAuthorDetailsAsync(int id)
        {
            var author = await context.Authors
                    .Include(q => q.Books)
                    .ProjectTo<AuthorDetailsDto>(mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(q => q.Id == id);
            
            return author;
        }
    }
}
