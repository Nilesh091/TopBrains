using System;

namespace BookStoreApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            
            Console.WriteLine("Intial input");
            string input = Console.ReadLine();
            string[] inputarr = input.Split(" ");


            Book book = new Book();

            book.Id = inputarr[0];
            book.Title = inputarr[1];
   
            book.Price = Convert.ToInt32(inputarr[2]);
            book.Stock = Convert.ToInt32((inputarr[3]));



            BookUtility utility = new BookUtility(book);

            while (true)
            {
                // TODO:
                // Display menu:
                // 1 -> Display book details
                // 2 -> Update book price
                // 3 -> Update book stock
                // 4 -> Exit
                Console.WriteLine("1 -> Display book details");
                Console.WriteLine("2-> update book price");
                Console.WriteLine("3-> update book stock");
                Console.WriteLine("4-> Exit");
                Console.WriteLine("Enter your choice");
                int choice = Convert.ToInt32(Console.ReadLine()); // TODO: Read user choice

                switch (choice)
                {
                    case 1:
                        utility.GetBookDetails();
                        break;

                    case 2:
                        Console.WriteLine("Enter new price");
                        int newprice = Convert.ToInt32(Console.ReadLine());
                        utility.UpdateBookPrice(newprice);
                        break;

                    case 3:
                        Console.WriteLine("Enter new stock");
                        int newstock = Convert.ToInt32(Console.ReadLine());
                        utility.UpdateBookStock(newstock);
                        // TODO:
                        // Read new stock
                        // Call UpdateBookStock()
                        break;

                    case 4:
                        Console.WriteLine("Thank You");
                        return;

                    default:
                        Console.WriteLine("Please Enter valid choice");
                        break;
                }
            }
        }
    }
}
