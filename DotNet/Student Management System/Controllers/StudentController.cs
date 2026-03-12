using System.Runtime.Intrinsics.X86;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Student_Management_System.Context;
using Student_Management_System.Models;
using Student_Management_System.UnitOfWork;
using System.Linq;

namespace Student_Management_System.Controllers
{
    public class StudentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public StudentController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        // GET: StudentController
        public ActionResult Index()
        {
            var students = _unitOfWork.Students.GetAll();
            return View(students);
        }

        public IActionResult Create()
        {
            ViewBag.Departments = _unitOfWork.Departments.GetAll();
            return View();
        }
        [HttpPost]
        public IActionResult Create(Student student)
        {
            _unitOfWork.Students.Insert(student);
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var student = _unitOfWork.Students.GetById(id);
            return View(student);
        }
        [HttpPost]
        public IActionResult Edit(Student student)
        {
            _unitOfWork.Students.Update(student);
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var student = _unitOfWork.Students.GetById(id);
            return View(student);
        }
        [HttpPost]
        public IActionResult ConfirmDelete(int id)
        {

            _unitOfWork.Students.Delete(id);
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }
        public IActionResult Details(int id)
        {
            var student = _unitOfWork.Students.GetById(id);
            return View(student);
        }

        public IActionResult Search(string name)
        {

            var students = _unitOfWork.Students.Find(s => s.Name.Contains(name));

            return View("Index", students);
        }

        public IActionResult StudentsOlderThan20()
        {
            var students = _unitOfWork.Students.Find(s => s.Age > 20);
            return View("Index", students);
        }
        public IActionResult OrderByName()
        {
            var students = _unitOfWork.Students.GetAll().OrderBy(s => s.Name).ToList();
            return View("Index", students);
        }
        public IActionResult StudentCount()
        {
            var total = _unitOfWork.Students.GetAll().Count();

            ViewBag.TotalStudents = total;

            return View();
        }
        public IActionResult GroupByDepartment()
        {
            var result = _unitOfWork.Students.GetAll()
                            .GroupBy(s => s.DepartmentId)
                            .Select(g => new
                            {
                                Department = g.Key,
                                TotalStudents = g.Count()
                            })
                            .ToList();

            return View(result);
        }
    }
}
