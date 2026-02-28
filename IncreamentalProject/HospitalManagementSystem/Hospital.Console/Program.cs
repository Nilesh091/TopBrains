using System;
using Hospital.Infrastructure.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
// using Infrastructure.Data;

using System;
using Hospital.Infrastructure.Data;
namespace Hospital.ConsoleApp
{
  class Program
  {
    static void Main(string[] args)
    {
      bool exit = false;

      while (!exit)
      {
        try
        {
          Console.Clear();
          Console.WriteLine("=================================");
          Console.WriteLine(" HOSPITAL MANAGEMENT SYSTEM");
          Console.WriteLine("=================================");
          Console.WriteLine("1. Add Doctor");
          Console.WriteLine("2. Exit");
          Console.WriteLine("=================================");
          Console.Write("Enter your choice: ");

          int choice = int.Parse(Console.ReadLine());

          switch (choice)
          {
            case 1:
              Console.WriteLine("Doctor Added (Simulation Only)");
              Console.ReadKey();
              break;

            case 2:
              exit = true;
              break;

            default:
              Console.WriteLine("Invalid choice!");
              Console.ReadKey();
              break;
          }
        }
        catch (Exception ex)
        {
          Logger.Log(ex);
          Console.WriteLine("An error occurred. Check errorlog.txt");
          Console.ReadKey();
        }
      }

      // 1️⃣ Build configuration
      var configuration = new ConfigurationBuilder()
          .SetBasePath(Directory.GetCurrentDirectory())
          .AddJsonFile("appsettings.json")
          .Build();

      var connectionString = configuration
          .GetConnectionString("DefaultConnection");

      // 2️⃣ Setup DI container
      var services = new ServiceCollection();

      services.AddDbContext<AppDbContext>(options =>
          options.UseSqlServer(connectionString));

      var serviceProvider = services.BuildServiceProvider();

      // 3️⃣ Ensure Database Created (optional)
      using (var scope = serviceProvider.CreateScope())
      {
        var context = scope.ServiceProvider
            .GetRequiredService<AppDbContext>();

        context.Database.EnsureCreated();
      }

      Console.WriteLine("Database Ready!");
    }
  }
}