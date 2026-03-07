using System;
using LibraryBookManagementSystem.Models;
namespace LibraryBookManagementSystem.Repositories
{
    public interface IBookRepository
    {

        IEnumerable<Book> GetAllBooks();
        Book GetBookById(int id);
        void AddBook(Book book);
        void DeleteBook(int id);
    }
}
