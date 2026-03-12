using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace LoginAuth.Controllers
{
    public class StateManagementController : Controller
    {
        // GET: StateManagementController
        public ActionResult Index()
        {
            return View();
        }
        public IActionResult SetCookie()
        {
            CookieOptions options = new CookieOptions();
            options.Expires = DateTime.Now.AddMinutes(10);

            Response.Cookies.Append("Username", "ravi", options);
            return Content("Cookie Created");
        }
        public IActionResult GetCookie()
        {
            string name = Request.Cookies["Username"];
            return Content("Cookie value: " + name);
        }
        public IActionResult DeleteCookie()
        {
            Response.Cookies.Delete("Username");
            return Content("Cookie Deleted");
        }
        public IActionResult SaveData()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SaveData(int userid)
        {
            return Content("UserId" + userid);
        }

        public IActionResult SetSession()
        {
            HttpContext.Session.SetString("UserName", "Nilu");
            return Content("Session Created");
        }
        public IActionResult GetSession()
        {
            string name = HttpContext.Session.GetString("UserName");
            return Content("Session name:" + name);
        }

        private readonly IMemoryCache _cache;
        public StateManagementController(IMemoryCache cache)
        {
            _cache = cache;
        }

        public IActionResult CacheDemo()
        {
            _cache.Set("User", "Nilu");
            string cac = _cache.Get<string>("User");
            return Content(cac);
        }

    }
}
