using CodeFirstDemo.Context;
using CodeFirstDemo.Repositories;
using Microsoft.AspNetCore.Mvc;
using CodeFirstDemo.Models;

namespace CodeFirstDemo.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository employeeRepository;
        public EmployeeController(IEmployeeRepository context)
        {
            employeeRepository = context;
        }
        // GET: EmployeeController
        public async Task<IActionResult> Index()
        {
            var employees = await employeeRepository.GetAll();
            return View(employees);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Employee employee)
        {
            await employeeRepository.Add(employee);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(int id)
        {
            return View(await employeeRepository.GetById(id));
        }

        public async Task<IActionResult> Edit(int id)
        {
            return View(await employeeRepository.GetById(id));
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Employee employee)
        {
            await employeeRepository.Update(employee);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            // if(id==null)return NotFound();
            // await employeeRepository.Delete(id);
            var employee = await employeeRepository.GetById(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // if (id == null) return NotFound();
            await employeeRepository.Delete(id);
            return RedirectToAction("Index");
        }


    }
}
