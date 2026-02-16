using System;
using Services;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            MemberUtility service = new MemberUtility();

            while (true)
            {
                Console.WriteLine("1. Display member by fine");
                Console.WriteLine("2. pay fine");
                Console.WriteLine("3. Add member");
                Console.WriteLine("4. Exit");

                int choice = Convert.ToInt32(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        service.DisplayMemberByFine();
                        break;

                    case 2:
                        try
                        {
                            service.PayFine();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;

                    case 3:
                        try
                        {
                            service.AddMember();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;

                    case 4:
                        Console.WriteLine("Exiting");
                        return;

                    default:
                        Console.WriteLine("Invalid choice");
                        break;
                }

            }
        }
    }
}
