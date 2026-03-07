using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using My_first_mcv_application.Models;

namespace My_first_mcv_application.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }
    public IActionResult Add(int a, int b)
    {
        return Content($"Sum of {a} and {b} is {a + b}");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
