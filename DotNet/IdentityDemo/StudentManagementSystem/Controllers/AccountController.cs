using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Data;
using StudentManagementSystem.ViewModel;
using StudentManagementSystem.Models;

namespace StudentManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public AccountController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        // GET: AccountController
        public ActionResult Index()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (model.Password != model.ConfirmPassword)
            {
                ModelState.AddModelError("", "Password missmatch.");
                return View(model);
            }
            User user = new User
            {
                FullName = model.FullName,
                Email = model.EmailId,
                Password = model.Password,
                Role = model.Role
            };
            _applicationDbContext.Users.Add(user);
            _applicationDbContext.SaveChanges();

            if (model.Role == "Student")
            {
                Student student = new Student
                {
                    StudentName = model.FullName,
                    Email = model.EmailId,
                    PhoneNumber = "",
                    Address = "",
                    DepartmentId = 1,
                    CourseId = 1
                };

                _applicationDbContext.Students.Add(student);
                _applicationDbContext.SaveChanges();
            }
            return RedirectToAction("Login");

        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            var user = _applicationDbContext.Users.FirstOrDefault(s => s.Email == model.Email && s.Password == model.Password);
            if (user == null)
            {
                ModelState.AddModelError("", "Invalid email or password");
                return View(model);
            }
            HttpContext.Session.SetString("UserEmail", user.Email);
            HttpContext.Session.SetString("UserRole", user.Role);
            if (user.Role == "Teacher")
                return RedirectToAction("Index", "TeacherDashboard");

            return RedirectToAction("Index", "StudentDashboard");

        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }




    }
}
