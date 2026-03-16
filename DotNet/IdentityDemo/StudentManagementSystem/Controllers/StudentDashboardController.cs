using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Data;
using StudentManagementSystem.Models;

namespace StudentManagementSystem.Controllers
{
    public class StudentDashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        public StudentDashboardController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: StudentDashboardController
        public IActionResult Index()
        {
            string email = HttpContext.Session.GetString("UserEmail");

            if (email == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var student = _context.Students
                .Include(s => s.Department)
                .Include(s => s.Course)
                .FirstOrDefault(s => s.Email == email);

            if (student == null)
            {
                return Content("Student record not found.");
            }

            return View(student);
        }

        public IActionResult EditProfile()
        {
            string email = HttpContext.Session.GetString("UserEmail");

            if (email == null)
                return RedirectToAction("Login", "Account");

            var student = _context.Students
                .FirstOrDefault(s => s.Email == email);

            if (student == null)
                return Content("Student record not found.");

            ViewBag.Departments = _context.Departments.ToList();
            ViewBag.Courses = _context.Courses.ToList();

            return View(student);
        }
        [HttpPost]
        public IActionResult EditStudent(Student student)
        {
            _context.Students.Update(student);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

    }
}
