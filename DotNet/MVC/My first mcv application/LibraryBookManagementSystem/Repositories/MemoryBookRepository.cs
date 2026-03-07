using System;
using LibraryBookManagementSystem.Models;
namespace LibraryBookManagementSystem.Repositories
{
    public class MemoryBookRepository : IBookRepository
    {
        private static List<Book> books = new List<Book>()
        {
new Book { BookId = 1, Title = "Clean Code ", Author = "Robert C. Martin", Price = 10.99m },
new Book { BookId = 2, Title = "Design Patterns ", Author = "Gof", Price = 8.99m },
new Book { BookId = 3, Title = "Refactoring", Author = "Martin Fowler", Price = 9.99m }
        };


        public void AddBook(Book book)
        {
            books.Add(book);
        }

        public void DeleteBook(int id)
        {
            var book = GetBookById(id);
            if (book != null)
            {
                books.Remove(book);
            }
        }

        public IEnumerable<Book> GetAllBooks()
        {
            return books;
        }

        public Book GetBookById(int id)
        {
            return books.FirstOrDefault(b => b.BookId == id);
        }

    }
}
