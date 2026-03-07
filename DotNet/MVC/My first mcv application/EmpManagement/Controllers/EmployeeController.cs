using EmpManagement.Models;
using EmpManagement.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EmpManagement.Controllers
{
    public class EmployeeController : Controller
    {
        // GET: EmployeeController
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Employee employee)
        {
            EmployeeRepository.Create(employee);
            return View("Thanks", employee);
        }


    }
}
