using System;

namespace BookStoreApplication
{
    public class Book
    {
        public string Id { get; set; }
        public string Title { get; set; }   
        public string Author { get; set; }

        public int Price { get; set; }
        public int Stock { get; set; }  
    }
}
