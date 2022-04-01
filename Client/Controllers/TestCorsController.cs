using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Client.Controllers
{
    public class TestCorsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
