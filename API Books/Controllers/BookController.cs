using API_Books.Models;
using API_Books.Models.Dtos;
using API_Books.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Books.Controllers
{
    [Route("api/Book")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "ApiBooks")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]

    public class BookController : Controller
    {

        private readonly IBookRepository _repoBook;
        private readonly IMapper _mapper;


        public BookController(IBookRepository _repoBook, IMapper _mapper)
        {
            this._repoBook = _repoBook;
            this._mapper = _mapper;
        }

        /// <summary>
        /// Get all the books
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<BookDto>))]
        [ProducesResponseType(400)]
        public async Task<ActionResult> GetAll()
        {
            var booksList = await _repoBook.GetAll();

            var booksListDto = new List<BookDto>();

            foreach (var item in booksList)
            {
                booksListDto.Add(_mapper.Map<BookDto>(item));
            }

            return Ok(booksListDto);
        }
        /// <summary>
        /// Get book by an ID
        /// </summary>
        /// <param name="BookId"></param>
        /// <returns></returns>
        [HttpGet("{BookId:int}", Name = "Get")]
        [ProducesResponseType(200, Type = typeof(List<BookDto>))]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Get(int BookId)
        {
            var itemBook = await _repoBook.Get(BookId);

            if (itemBook == null)
            {
                return NotFound();
            }

            var itemBookDto = _mapper.Map<BookDto>(itemBook);
            return Ok(itemBook);
        }

        /// <summary>
        /// Search for a specific book by it's name
        /// </summary>
        /// <param name="bookName"></param>
        /// <returns></returns>
        [HttpGet("Search")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> SearchBook(string bookName)
        {
            try
            {
                var result = await _repoBook.SearchBook(bookName);
                if (result.Any())
                {
                    return Ok(result);
                }
                return NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error ocurred while recovering datas");
            }
        }


        /// <summary>
        /// Insert/Create a new book
        /// </summary>
        /// <param name="bookDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200, Type = typeof(List<BookDto>))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Insert([FromBody] BookDto bookDto)
        {
            if (bookDto == null)
            {
                return BadRequest(ModelState);
            }
            if (await _repoBook.ExistsBook(bookDto.Title))
            {
                ModelState.AddModelError("", "The book's name already exists");
                return StatusCode(404, ModelState);
            }

            var book = _mapper.Map<Book>(bookDto);

            if (!await _repoBook.Insert(book))
            {
                ModelState.AddModelError("", $"Something went wrong while trying saving {bookDto.Title} ");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("Get", new { bookId = book.ID }, book);
        }

        /// <summary>
        /// Update an existing book
        /// </summary>
        /// <param name="bookId"></param>
        /// <param name="bookDto"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpPatch("{bookId:int}", Name = "Update")]
        public async Task<ActionResult> Update(int bookId, [FromBody] BookDto bookDto)
        {
            if (bookDto == null || bookId != bookDto.ID)
            {
                return BadRequest(ModelState);
            }

            var book = _mapper.Map<Book>(bookDto);

            if (!await _repoBook.Update(book))
            {
                ModelState.AddModelError("", $"Hey! Something went wrong while trying to update {book.Title}");
                return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
            }

            return Content("The Book have been update successfully");
        }

        /// <summary>
        /// Delete a book and existing book
        /// </summary>
        /// <param name="bookId"></param>
        /// <returns></returns>
        [HttpDelete("{bookId:int}", Name = "Delete")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Delete(int bookId)
        {
            if (!await _repoBook.ExistsBook(bookId))
            {
                return NotFound();
            }

            var book = await _repoBook.Get(bookId);

            if (!await _repoBook.Delete(book))
            {
                ModelState.AddModelError("", $"Hey! Something went wrong while trying to delete {book.Title}");
                return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
            }

            return Content("The Book have been deleted successfully");
        }

    }
}
