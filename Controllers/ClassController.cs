using Microsoft.AspNetCore.Mvc;

namespace Equinox.Controllers
{
    public class ClassController : Controller
    {
        public IActionResult List(string id = "All") =>
            Content($"Main Area - ClassController, List action, id: {id}");
    }
}
