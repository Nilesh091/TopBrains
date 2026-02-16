using System;
using Services;
using Domain;
using Exceptions;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            StudentUtility service = new StudentUtility();

            while (true)
            {
                Console.WriteLine("\n1. Display");
                Console.WriteLine("2. Add");
                Console.WriteLine("3. Update");
                Console.WriteLine("4. Remove");
                Console.WriteLine("5. Exit");
                Console.Write("Enter choice: ");

                int choice;
                if (!int.TryParse(Console.ReadLine(), out choice))
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                    continue;
                }

                try
                {
                    switch (choice)
                    {
                        case 1:
                            var students = service.GetAll();
                            Console.WriteLine("\n--- Student Ranking (Descending GPA) ---");
                            foreach (var s in students)
                            {
                                Console.WriteLine($"Id: {s.Id}, Name: {s.Name}, GPA: {s.GPA}");
                            }
                            break;

                        case 2:
                            Console.Write("Enter Id: ");
                            string id = Console.ReadLine();

                            Console.Write("Enter Name: ");
                            string name = Console.ReadLine();

                            Console.Write("Enter GPA: ");
                            double gpa = double.Parse(Console.ReadLine());

                            Student student = new Student
                            {
                                Id = id,
                                Name = name,
                                GPA = gpa
                            };

                            service.AddEntity(gpa, student);
                            Console.WriteLine("Student added successfully.");
                            break;

                        case 3:
                            Console.Write("Enter Student Id to update: ");
                            string updateId = Console.ReadLine();

                            Console.Write("Enter new GPA: ");
                            double newGpa = double.Parse(Console.ReadLine());

                            service.UpdateEntity(updateId, newGpa);
                            Console.WriteLine("Student GPA updated successfully.");
                            break;

                        case 4:
                            Console.Write("Enter Student Id to remove: ");
                            string removeId = Console.ReadLine();

                            service.RemoveEntity(removeId);
                            Console.WriteLine("Student removed successfully.");
                            break;

                        case 5:
                            Console.WriteLine("Thank You");
                            return;

                        default:
                            Console.WriteLine("Invalid choice. Please select 1-5.");
                            break;
                    }
                }
                catch (InvalidGPAException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
                catch (StudentNotFoundException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unexpected Error: {ex.Message}");
                }
            }
        }
    }
}
