using System;

namespace BookStoreApplication
{
    public class BookUtility
    {
        private Book _book;

        public BookUtility(Book book)
        {
            // TODO: Assign book object
            _book = book;
        }

        public void GetBookDetails()
        {
            Console.WriteLine($"{_book.Id} {_book.Title} {_book.Price} {_book.Stock}");
            // TODO:
            // Print format:
            // Details: <BookId> <Title> <Price> <Stock>
        }

        public void UpdateBookPrice(int newPrice)
        {
            if (newPrice <= 0)
            {
                Console.WriteLine("Price must be greater than 0");
                return;
            }
            // TODO:
            // Validate new price
            // Update price
            // Print: Updated Price: <newPrice>
            _book.Price = newPrice;
            
        }

        public void UpdateBookStock(int newStock)
        {
            if (newStock <= 0)
            {
                Console.WriteLine("newstock must be greater than 0");
                return;
            }
            // TODO:
            // Validate new stock
            // Update stock
            // Print: Updated Stock: <newStock>
            _book.Stock = newStock;
        }
    }
}
