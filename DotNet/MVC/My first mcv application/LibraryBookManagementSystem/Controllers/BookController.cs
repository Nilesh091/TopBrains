using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;
using LibraryBookManagementSystem.Models;
using LibraryBookManagementSystem.Repositories;
namespace LibraryBookManagementSystem.Controllers
{
    public class BookController : Controller
    {
        // Constructor injection of the repository
        private readonly IBookRepository _bookRepository;
        public BookController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }
        // GET: BookController
        public ActionResult Index()
        {
            return View();
        }

        //list of all books
        public IActionResult List()
        {
            var books = _bookRepository.GetAllBooks();
            return View(books);
        }

        [HttpGet]
        public IActionResult Display(int id)
        {
            var book = _bookRepository.GetBookById(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        // GET : Show Form
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        // POST : Save Book
        [HttpPost]
        public IActionResult Create(Book book)
        {
            _bookRepository.AddBook(book);
            return RedirectToAction("List");
        }

        public IActionResult Delete(int id)
        {
            _bookRepository.DeleteBook(id);
            return RedirectToAction("List");
        }


    }
}
