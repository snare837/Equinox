using Microsoft.AspNetCore.Mvc;

namespace Equinox.Controllers
{
    public class ClubController : Controller
    {
        public IActionResult List(string id = "All") =>
            Content($"Main Area - ClubController, List action, id: {id}");
    }
}
