using Microsoft.AspNetCore.Mvc;

namespace Equinox.Controllers
{
    public class MembershipController : Controller
    {
        public IActionResult List(string id = "All") =>
            Content($"Main Area - MembershipController, List action, id: {id}");
    }
}
