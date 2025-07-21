using Microsoft.AspNetCore.Mvc;

namespace Equinox.Controllers
{
    public class PrivateClassController : Controller
    {
        public IActionResult List(string id = "All") =>
            Content($"Main Area - PrivateClassController, List action, id: {id}");
    }
}
