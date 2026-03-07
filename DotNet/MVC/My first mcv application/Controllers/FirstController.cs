using Microsoft.AspNetCore.Mvc;

namespace My_first_mcv_application.Controllers
{
    public class FirstController : Controller
    {
        // GET: FirstController
        public string Index()
        {
            return "HelloWorld";
        }

        public IActionResult Hello()
        {
            return View();
        }

    }
}
