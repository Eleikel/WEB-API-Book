using API_Books.Data;
using API_Books.Models;
using API_Books.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Books.Repository
{
    public class BookRepository : IBookRepository
    {
        private readonly ApplicationDbContext _db;

        public BookRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<bool> Insert(Book book)
        {
            _db.Book.Add(book);
            return await Save();
        }

        public async Task<bool> Delete(Book book)
        {
            _db.Book.Remove(book);
            return await Save();
        }

        public Task<bool> ExistsBook(string BookName)
        {
            bool value = _db.Book.Any(a => a.Title.Trim().ToLower() == BookName.Trim().ToLower());
            //return Task.FromResult(value);
            //bool value = _db.Book.Any(a => a.Title.ToLower().Trim() == BookName.ToLower().Trim());
            return Task.FromResult(value);
        }

        public Task<bool> ExistsBook(int BookId)
        {
            //bool value = _db.Book.Any(a => a.ID == BookId);
            //return Task.FromResult(value);
            return Task.FromResult(_db.Book.Any(c => c.ID == BookId));
        }

        public async Task<Book> Get(int BookId)
        {
            return await _db.Book.FindAsync(BookId);


            //return (ICollection<Book>) await _db.Book.FirstOrDefaultAsync(c => c.ID == BookId);
        }

        public async Task<ICollection<Book>> GetAll()
        {
            return await _db.Book.OrderBy(c => c.Title).ToListAsync();
        }

        public async Task<bool> Save()
        {
            return await _db.SaveChangesAsync() >= 0 ? true : false;
        }

        public async Task<IEnumerable<Book>> SearchBook(string bookName)
        {
            IQueryable<Book> query = _db.Book;

            if (!string.IsNullOrEmpty(bookName))
            {
                query = query.Where(e => e.Title.Contains(bookName) || e.Description.Contains(bookName));
            }
            return await query.ToListAsync();
        }

        public async Task<bool> Update(Book book)
        {
            _db.Book.Update(book); return await Save();
        }
    }
}
