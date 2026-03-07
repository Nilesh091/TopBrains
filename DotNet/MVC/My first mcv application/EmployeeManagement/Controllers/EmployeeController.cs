using EmployeeManagement.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Controllers
{
    public class EmployeeController : Controller
    {
        // GET: EmployeeController
        public ActionResult Index()
        {
            // ViewData["message"] = "Welcome to the Employee Management System!";
            // ViewData["Today"] = DateTime.Now.ToShortDateString();

            // ViewBag.Name = "John Doe";
            // ViewBag.Department = "Human Resources";
            // ViewBag.salary = 50000;

            List<Employee> employees = new List<Employee>()
            {
                new Employee{Id=101,Name="Meehir",Department="CSE"},
                new Employee{Id=102,Name="Sheevam",Department="CSE"}
            };
            return View(employees);
        }
        public IActionResult Create()
        {
            TempData["success"] = "Employee created successfully!";
            return RedirectToAction("Index");
        }

        public IActionResult ListCourse()
        {
            ViewBag.Message = "This is Listcourse Action";

            List<string> courses = new List<string>();
            courses.Add("C++");
            courses.Add("Dsa");
            courses.Add("C#");
            courses.Add("Java");
            ViewBag.Courses = courses;
            return View();
        }

        public IActionResult ListProduct()
        {
            ViewBag.Message = "Prosuct List Action";
            List<Product> products = new List<Product>()
            {
                new Product{productCode=101,ProductName="Macbook",Price=89890},
                new Product{productCode=102,ProductName="Iphone",Price=71990},
                new Product{productCode=103,ProductName="Ipad",Price=50990}
            };
            ViewBag.Products = products;
            return View();
        }
    }
}
