using Microsoft.AspNetCore.Mvc;
using Equinox.Models;
using Equinox.Models.Util;  

namespace Equinox.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ValidationController : Controller
    {
        private EquinoxContext context;
        public ValidationController(EquinoxContext ctx) => context = ctx;

        public JsonResult CheckPhoneNumber(string phonenumber, int userId)
        {
            if (userId == 0)
            {
                string msg = Check.PhoneNumberExists(context, phonenumber);
                if (string.IsNullOrEmpty(msg))
                {
                    TempData["okPhoneNumber"] = true;
                    return Json(true);
                }
                else return Json(msg);
            }
             return Json(true);
        }
        public JsonResult CheckEmail(string email, int userId)
        {
            if (userId == 0)
            {
                string msg = Check.EmailExists(context, email);
                if (string.IsNullOrEmpty(msg))
                {
                    TempData["okEmail"] = true;
                    return Json(true);
                }
                else return Json(msg);
            }
            return Json(true);
        }

    }

}
