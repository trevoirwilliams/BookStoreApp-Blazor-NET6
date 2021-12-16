using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookStoreApp.API.Data;
using AutoMapper;
using BookStoreApp.API.Models.Book;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using BookStoreApp.API.Repositories;
using BookStoreApp.API.Models;
using BookStoreApp.API.Static;

namespace BookStoreApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BooksController : ControllerBase
    {
        private readonly IBooksRepository booksRepository;
        private readonly IMapper mapper;
        private readonly IWebHostEnvironment webHostEnvironment;

        public BooksController(IBooksRepository booksRepository, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            this.booksRepository = booksRepository;
            this.mapper = mapper;
            this.webHostEnvironment = webHostEnvironment;
        }

        // GET: api/Books/?startindex=0&pagesize=15
        [HttpGet]
        public async Task<ActionResult<VirtualizeResponse<BookReadOnlyDto>>> GetAuthors([FromQuery] QueryParameters queryParameters)
        {
            return await booksRepository.GetAllAsync<BookReadOnlyDto>(queryParameters);
        }

        // GET: api/Books/GetAll
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<BookReadOnlyDto>>> GetAllBooks()
        {
            var bookDtos = await booksRepository.GetAllBooksAsync();
            return Ok(bookDtos);
        }

        // GET: api/Books/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BookDetailsDto>> GetBook(int id)
        {
            var book = await booksRepository.GetBookAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }

        // PUT: api/Books/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> PutBook(int id, BookUpdateDto bookDto)
        {
            
            if (id != bookDto.Id)
            {
                return BadRequest();
            }

            var book = await booksRepository.GetAsync(id);

            if(book == null)
            {
                return NotFound();
            }

            if (string.IsNullOrEmpty(bookDto.ImageData) == false)
            {
                bookDto.Image = CreateFile(bookDto.ImageData, bookDto.OriginalImageName);

                var picName = Path.GetFileName(book.Image);
                var path = $"{webHostEnvironment.WebRootPath}\\bookcoverimages\\{picName}";
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
            }

            mapper.Map(bookDto, book);

            try
            {
                await booksRepository.UpdateAsync(book);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (! await BookExistsAsync(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Books
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<BookCreateDto>> PostBook(BookCreateDto bookDto)
        {
            var book = mapper.Map<Book>(bookDto);
            if (string.IsNullOrEmpty(bookDto.ImageData) == false)
            {
                book.Image = CreateFile(bookDto.ImageData, bookDto.OriginalImageName);
            }
            await booksRepository.AddAsync(book);

            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
        }

        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await booksRepository.GetAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            await booksRepository.DeleteAsync(id);

            return NoContent();
        }

        private string CreateFile(string imageBase64, string imageName)
        {
            var url = HttpContext.Request.Host.Value;
            var ext = Path.GetExtension(imageName);
            var fileName = $"{Guid.NewGuid()}{ext}";
            
            var path = $"{webHostEnvironment.WebRootPath}\\bookcoverimages\\{fileName}";

            byte[] image = Convert.FromBase64String(imageBase64);

            var fileStream = System.IO.File.Create(path);
            fileStream.Write(image, 0, image.Length);
            fileStream.Close();

            return $"https://{url}/bookcoverimages/{fileName}";
        }

        private async Task<bool> BookExistsAsync(int id)
        {
            return await booksRepository.Exists(id);
        }
    }
}
