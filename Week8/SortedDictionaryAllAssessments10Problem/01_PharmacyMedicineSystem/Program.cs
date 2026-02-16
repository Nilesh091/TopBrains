using System;
using Domain;
using Services;
using Exceptions;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            MedicineUtility medicineUtility = new MedicineUtility();

            while (true)
            {
                Console.WriteLine("1 Display all medicines");
                Console.WriteLine("2  Update medicine price");
                Console.WriteLine("3  Add medicine");
                Console.WriteLine("4  Exit");

                int choice = Convert.ToInt32(Console.ReadLine());

                try
                {
                    switch (choice)
                    {
                        case 1:
                            medicineUtility.GetAllMedicines();
                            break;

                        case 2:
                            Console.WriteLine("Enter Medicine Id:");
                            string id = Console.ReadLine();

                            Console.WriteLine("Enter New Price:");
                            int newPrice = Convert.ToInt32(Console.ReadLine());

                            medicineUtility.UpdateMedicinePrice(id, newPrice);
                            Console.WriteLine("Price Updated Successfully");
                            break;

                        case 3:
                            Console.WriteLine("Enter Id:");
                            string id1 = Console.ReadLine();

                            Console.WriteLine("Enter Name:");
                            string name = Console.ReadLine();

                            Console.WriteLine("Enter Price:");
                            int price = Convert.ToInt32(Console.ReadLine());

                            Console.WriteLine("Enter Expiry Year:");
                            int expiryYear = Convert.ToInt32(Console.ReadLine());

                            Medicine medicine = new Medicine
                            {
                                Id = id1,
                                Name = name,
                                Price = price,
                                ExpiryYear = expiryYear
                            };

                            medicineUtility.AddMedicine(medicine);
                            Console.WriteLine("Medicine Added Successfully");
                            break;

                        case 4:
                            return;

                        default:
                            Console.WriteLine("Invalid Choice");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
