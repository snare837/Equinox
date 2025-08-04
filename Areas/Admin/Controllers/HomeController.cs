using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Equinox.Models;
using Equinox.Models.ViewModels;
using System.Linq;
using System.Collections.Generic;

namespace Equinox.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class HomeController : Controller
    {
        private readonly EquinoxContext _context;
        public HomeController(EquinoxContext context)
        {
            _context = context;
        }
        public IActionResult Index() => View();

        public IActionResult ManageClub()
        {
            var clubs = _context.Clubs.ToList();
            return View(clubs);
        }


        public IActionResult ManageClassCategory()
        {
            var classCategories = _context.ClassCategories.ToList();


            return View(classCategories);
        }

        public IActionResult ManageUser()
        {
            var users = _context.Coaches.ToList();


            return View(users);
        }

        [HttpPost]
        public IActionResult DeleteClub(int id)
        {
            var club = _context.Clubs.FirstOrDefault(c => c.ClubId == id);
            if (club == null)
            {
                return NotFound();
            }

            _context.Clubs.Remove(club);
            _context.SaveChanges();

            return RedirectToAction("ManageClub");
        }

        [HttpPost]
        public IActionResult DeleteClassCategory(int id)
        {
            var classCategory = _context.ClassCategories.FirstOrDefault(c => c.ClassCategoryId == id);
            if (classCategory == null)
            {
                return NotFound();
            }

            _context.ClassCategories.Remove(classCategory);
            _context.SaveChanges();

            return RedirectToAction("ManageClassCategory");
        }

        [HttpPost]
        public IActionResult DeleteUser(int id)
        {
            var user = _context.Coaches.FirstOrDefault(u => u.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Coaches.Remove(user);
            _context.SaveChanges();

            return RedirectToAction("ManageUser");
        }

        public IActionResult AddClub()
        {
            ViewBag.Action = "Add";
            return View("EditClub", new Club());
        }


        [HttpGet]
        public IActionResult EditClub(int id)
        {
            ViewBag.Action = "Edit";

            var club = _context.Clubs.FirstOrDefault(c => c.ClubId == id);
            if (club == null)
            {
                return NotFound();
            }

            return View("EditClub", club);
        }

        [HttpPost]
        public IActionResult SaveClub(Club club)
        {
            if (ModelState.IsValid)
            {
                if (club.ClubId == 0)
                {
                    _context.Clubs.Add(club);
                }
                else
                {
                    _context.Clubs.Update(club);
                }

                _context.SaveChanges();
                return RedirectToAction("ManageClub");
            }

            ViewBag.Action = club.ClubId == 0 ? "Add" : "Edit";
            return View("EditClub", club);
        }

        public IActionResult AddClassCategory()
        {
            ViewBag.Action = "Add";
            return View("EditClassCategory", new ClassCategory());
        }

        [HttpGet]
        public IActionResult EditClassCategory(int id)
        {
            ViewBag.Action = "Edit";

            var category = _context.ClassCategories.FirstOrDefault(c => c.ClassCategoryId == id);
            if (category == null)
            {
                return NotFound();
            }

            return View("EditClassCategory", category);
        }
        [HttpPost]
        public IActionResult SaveClassCategory(ClassCategory category)
        {
            if (ModelState.IsValid)
            {
                if (category.ClassCategoryId == 0)
                {
                    _context.ClassCategories.Add(category);
                }
                else
                {
                    _context.ClassCategories.Update(category);
                }

                _context.SaveChanges();
                return RedirectToAction("ManageClassCategory");
            }

            ViewBag.Action = category.ClassCategoryId == 0 ? "Add" : "Edit";
            return View("EditClassCategory", category);
        }

        public IActionResult AddUser()
        {
            ViewBag.Action = "Add";
            return View("EditUser", new User());
        }

        [HttpGet]
        public IActionResult EditUser(int id)
        {
            ViewBag.Action = "Edit";

            var user = _context.Coaches.FirstOrDefault(u => u.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View("EditUser", user);
        }

        [HttpPost]
        public IActionResult SaveUser(User user)
        {
            if (user.UserId == 0)
            {

                if (TempData["okPhoneNumber"] == null)
                {
                    string msg = Check.PhoneNumberExists(_context, user.PhoneNumber);
                    if (!String.IsNullOrEmpty(msg))
                    {
                        ModelState.AddModelError(nameof(user.PhoneNumber), msg);
                    }
                }
                if (TempData["okEmail"] == null)
                {
                    string msg = Check.EmailExists(_context, user.Email);
                    if (!String.IsNullOrEmpty(msg))
                    {
                        ModelState.AddModelError(nameof(user.Email), msg);
                    }
                }
            }
            if (ModelState.IsValid)
                {
                    if (user.UserId == 0)
                    {
                        _context.Coaches.Add(user);
                    }
                    else
                    {
                        _context.Coaches.Update(user);
                    }

                    _context.SaveChanges();
                    return RedirectToAction("ManageUser");
                }

            ViewBag.Action = user.UserId == 0 ? "Add" : "Edit";
            return View("EditUser", user);
        }



    }
}
