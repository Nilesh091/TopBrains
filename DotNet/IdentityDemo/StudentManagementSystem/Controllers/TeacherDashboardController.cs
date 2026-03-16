using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Data;
using StudentManagementSystem.Models;

namespace StudentManagementSystem.Controllers
{
    public class TeacherDashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TeacherDashboardController(ApplicationDbContext context)
        {
            _context = context;
        }


        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("UserRole") != "Teacher")
            {
                return RedirectToAction("Login", "Account");
            }

            return View();
        }


        public IActionResult Students()
        {
            var students = _context.Students
                .Include(s => s.Department)
                .Include(s => s.Course)
                .ToList();

            return View(students);
        }


        public IActionResult CreateStudent()
        {
            ViewBag.Departments = _context.Departments.ToList();
            ViewBag.Courses = _context.Courses.ToList();

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateStudent(Student student)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Departments = _context.Departments.ToList();
                ViewBag.Courses = _context.Courses.ToList();
                return View(student);
            }

            student.StudentName = student.StudentName?.Trim() ?? string.Empty;
            student.Email = student.Email?.Trim() ?? string.Empty;
            student.PhoneNumber = student.PhoneNumber?.Trim() ?? string.Empty;
            student.Address = student.Address?.Trim() ?? string.Empty;

            _context.Students.Add(student);
            _context.SaveChanges();

            return RedirectToAction("Students");
        }


        public IActionResult EditStudent(int id)
        {
            var student = _context.Students.Find(id);

            ViewBag.Departments = _context.Departments.ToList();
            ViewBag.Courses = _context.Courses.ToList();

            return View(student);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditStudent(Student student)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Departments = _context.Departments.ToList();
                ViewBag.Courses = _context.Courses.ToList();
                return View(student);
            }

            var existingStudent = _context.Students.Find(student.StudentId);
            if (existingStudent == null)
            {
                return NotFound();
            }

            existingStudent.StudentName = student.StudentName?.Trim() ?? string.Empty;
            existingStudent.Email = student.Email?.Trim() ?? string.Empty;
            existingStudent.PhoneNumber = student.PhoneNumber?.Trim() ?? string.Empty;
            existingStudent.Address = student.Address?.Trim() ?? string.Empty;
            existingStudent.DepartmentId = student.DepartmentId;
            existingStudent.CourseId = student.CourseId;

            _context.SaveChanges();

            return RedirectToAction("Students");
        }


        public IActionResult DeleteStudent(int id)
        {
            var student = _context.Students.Find(id);

            if (student != null)
            {
                _context.Students.Remove(student);
                _context.SaveChanges();
            }

            return RedirectToAction("Students");
        }

    }
}