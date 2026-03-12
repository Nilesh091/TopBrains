using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace CodeFirstDemo.Controllers
{
    public class ActionResultController : Controller
    {
        // GET: ActionResultController
        public ActionResult Index()
        {
            return View();
        }
        public IActionResult Youtube()
        {
            return Redirect("https://www.youtube.com/");
        }
        public IActionResult RouteDemo()
        {
            return RedirectToRoute(new { controller = "Home", action = "Index" });
        }
        public IActionResult JsonDemo()
        {
            var data = new { Name = "John", Age = 30 };
            return Json(data);
        }

        public IActionResult Success()
        {
            return Ok("Operation successful!");
        }

        public IActionResult ValidateAge(int age)
        {
            if (age < 18)
            {
                return BadRequest("Age must be at least 18.");
            }
            return Ok("Age is valid.");
        }

    }
}
