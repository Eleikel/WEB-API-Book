using API_Books.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Books.Repository.IRepository
{
    public interface IBookRepository
    {
        Task<ICollection<Book>> GetAll();
        Task<Book> Get(int BookId);
        Task<bool> ExistsBook(string BookName);
        Task<bool> ExistsBook(int BookId);
        Task<IEnumerable<Book>> SearchBook(string BookName);
        Task<bool> Insert(Book book);
        Task<bool> Update(Book book);
        Task<bool> Delete(Book book);
        Task<bool> Save();

    }
}
