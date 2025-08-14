using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using Equinox.Models;
using Equinox.Models.DataLayer;
using Equinox.Models.DataLayer.Repositories;



namespace Equinox.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class HomeController : Controller
    {
        private readonly EquinoxContext _context;

        private Repository<Club> clubData { get; set; }
        private Repository<ClassCategory> categoryData { get; set; }
        private Repository<User> coachData { get; set; }
        private Repository<Membership> membershipData { get; set; }
        public HomeController(EquinoxContext context)
        {
            _context = context;                       // used for booking guard checks
            clubData = new Repository<Club>(context);
            categoryData = new Repository<ClassCategory>(context);
            coachData = new Repository<User>(context);
            membershipData = new Repository<Membership>(context);
        }

        public IActionResult Index() => View();

        //  Lists (Repository + QueryOptions) 
        public ViewResult ManageClub() =>
            View(clubData.List(new QueryOptions<Club> { OrderBy = c => c.Name }));

        public ViewResult ManageClassCategory() =>
            View(categoryData.List(new QueryOptions<ClassCategory> { OrderBy = c => c.Name }));

        public ViewResult ManageUser() =>
            View(coachData.List(new QueryOptions<User> { OrderBy = u => u.Name }));

        //  Club 
        public IActionResult AddClub()
        {
            ViewBag.Action = "Add";
            return View("EditClub", new Club());
        }

        [HttpGet]
        public IActionResult EditClub(int id)
        {
            ViewBag.Action = "Edit";
            var club = clubData.Get(id);
            return club == null ? NotFound() : View("EditClub", club);
        }

        [HttpPost]
        public IActionResult SaveClub(Club club)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Action = club.ClubId == 0 ? "Add" : "Edit";
                return View("EditClub", club);
            }

            if (club.ClubId == 0) clubData.Insert(club); else clubData.Update(club);
            clubData.Save();
            return RedirectToAction("ManageClub");
        }


        [HttpPost]
        public IActionResult DeleteClub(int id)
        {
            var club = clubData.Get(id);
            if (club == null)
            {
                return NotFound();
            }

            var session = new EquinoxSession(HttpContext.Session);
            var bookings = session.GetMyBookings() ?? new List<int>();

            // Use repository and QueryOptions to get booked classes from session
            var options = new QueryOptions<EquinoxClass>
            {
                Includes = "Club",
                Where = c => bookings.Contains(c.EquinoxClassId)
            };
            var bookedClasses = new Repository<EquinoxClass>(_context).List(options).ToList();

            // Check if any booked class belongs to this club
            bool clubHasBookedClasses = bookedClasses.Any(c => c.ClubId == id);

            if (clubHasBookedClasses)
            {
                TempData["message"] = $"Can't delete club '{club.Name}' because it has booked classes.";
                return RedirectToAction("ManageClub", "Home", new { area = "Admin" });
            }

            // Get unbooked classes for this club
            var allEquinoxClassRepo = new Repository<EquinoxClass>(_context);
            var unbookedOptions = new QueryOptions<EquinoxClass>
            {
                Where = c => c.ClubId == id && !bookings.Contains(c.EquinoxClassId)
            };
            var unbookedClasses = allEquinoxClassRepo.List(unbookedOptions).ToList();

            // Remove unbooked classes
            foreach (var c in unbookedClasses)
                allEquinoxClassRepo.Delete(c);

            // Remove the club
            clubData.Delete(club);

            // Save all changes
            clubData.Save();
            allEquinoxClassRepo.Save();

            TempData["message"] = $"{club.Name} removed from Clubs.";
            return RedirectToAction("ManageClub", "Home", new { area = "Admin" });
        }


        //  Class Category 
        public IActionResult AddClassCategory()
        {
            ViewBag.Action = "Add";
            return View("EditClassCategory", new ClassCategory());
        }

        [HttpGet]
        public IActionResult EditClassCategory(int id)
        {
            ViewBag.Action = "Edit";
            var category = categoryData.Get(id);
            return category == null ? NotFound() : View("EditClassCategory", category);
        }

        [HttpPost]
        public IActionResult SaveClassCategory(ClassCategory category)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Action = category.ClassCategoryId == 0 ? "Add" : "Edit";
                return View("EditClassCategory", category);
            }

            if (category.ClassCategoryId == 0) categoryData.Insert(category); else categoryData.Update(category);
            categoryData.Save();
            return RedirectToAction("ManageClassCategory");
        }

        [HttpPost]
        public IActionResult DeleteClassCategory(int id)
        {
            var category = categoryData.Get(id);
            if (category == null)
            {
                return NotFound();
            }

            var session = new EquinoxSession(HttpContext.Session);
            var bookings = session.GetMyBookings() ?? new List<int>();

            var equinoxClassRepo = new Repository<EquinoxClass>(_context);

            // Get booked classes from session bookings that belong to this category
            var bookedOptions = new QueryOptions<EquinoxClass>
            {
                Includes = "ClassCategory",
                Where = c => bookings.Contains(c.EquinoxClassId)
            };
            var bookedClasses = equinoxClassRepo.List(bookedOptions).ToList();

            if (bookedClasses.FirstOrDefault(c => c.ClassCategoryId == id) != null)
            {
                TempData["message"] = $"Cannot delete category '{category.Name}' because it has booked classes.";
                return RedirectToAction("ManageClassCategory", "Home", new { area = "Admin" });
            }

            // Get unbooked classes in this category
            var unbookedOptions = new QueryOptions<EquinoxClass>
            {
                Where = c => c.ClassCategoryId == id && !bookings.Contains(c.EquinoxClassId)
            };
            var unbookedClasses = equinoxClassRepo.List(unbookedOptions).ToList();

            foreach (var c in unbookedClasses)
                equinoxClassRepo.Delete(c);

            categoryData.Delete(category);

            // Save changes
            equinoxClassRepo.Save();
            categoryData.Save();

            TempData["message"] = $"{category.Name} removed from Categories.";
            return RedirectToAction("ManageClassCategory", "Home", new { area = "Admin" });
        }



        //  User / Coach 
        public IActionResult AddUser()
        {
            ViewBag.Action = "Add";
            return View("EditUser", new User());
        }

        [HttpGet]
        public IActionResult EditUser(int id)
        {
            ViewBag.Action = "Edit";
            var user = coachData.Get(id);
            return user == null ? NotFound() : View("EditUser", user);
        }

        [HttpPost]
        public IActionResult SaveUser(User user)
        {
            // keep your existing server-side checks if you have them; otherwise this is enough for Phase 4
            if (!ModelState.IsValid)
            {
                ViewBag.Action = user.UserId == 0 ? "Add" : "Edit";
                return View("EditUser", user);
            }

            if (user.UserId == 0) coachData.Insert(user); else coachData.Update(user);
            coachData.Save();
            return RedirectToAction("ManageUser");
        }

        [HttpPost]
        public IActionResult DeleteUser(int id)
        {
            var user = coachData.Get(id);
            if (user == null)
            {
                return NotFound();
            }

            var session = new EquinoxSession(HttpContext.Session);
            var bookings = session.GetMyBookings() ?? new List<int>();

            var equinoxClassRepo = new Repository<EquinoxClass>(_context);

            // Get booked classes from session bookings that belong to this coach/user
            var bookedOptions = new QueryOptions<EquinoxClass>
            {
                Includes = "User",
                Where = c => bookings.Contains(c.EquinoxClassId)
            };
            var bookedClasses = equinoxClassRepo.List(bookedOptions).ToList();

            bool coachHasBookedClasses = bookedClasses.Any(c => c.UserId == id);

            if (coachHasBookedClasses)
            {
                TempData["message"] = $"Can't delete coach '{user.Name}' because they have booked classes.";
                return RedirectToAction("ManageUser", "Home", new { area = "Admin" });
            }

            // Get unbooked classes for this coach/user
            var unbookedOptions = new QueryOptions<EquinoxClass>
            {
                Where = c => c.UserId == id && !bookings.Contains(c.EquinoxClassId)
            };
            var unbookedClasses = equinoxClassRepo.List(unbookedOptions).ToList();

            foreach (var c in unbookedClasses)
                equinoxClassRepo.Delete(c);

            coachData.Delete(user);

            // Save changes
            equinoxClassRepo.Save();
            coachData.Save();

            TempData["message"] = $"{user.Name} removed from Coaches.";
            return RedirectToAction("ManageUser", "Home", new { area = "Admin" });
        }

        public ViewResult ManageMembership() =>
            View(membershipData.List(new QueryOptions<Membership> { OrderBy = m => m.Name }));

        public IActionResult AddMembership()
        {
            ViewBag.Action = "Add";
            return View("EditMembership", new Membership());
        }

        [HttpGet]
        public IActionResult EditMembership(int id)
        {
            ViewBag.Action = "Edit";
            var membership = membershipData.Get(id);
            return membership == null ? NotFound() : View("EditMembership", membership);
        }

        [HttpPost]
        public IActionResult SaveMembership(Membership membership)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Action = membership.MembershipId == 0 ? "Add" : "Edit";
                return View("EditMembership", membership);
            }

            if (membership.MembershipId == 0)
                membershipData.Insert(membership);
            else
                membershipData.Update(membership);

            membershipData.Save();
            return RedirectToAction("ManageMembership");
        }

        [HttpPost]
        public IActionResult DeleteMembership(int id)
        {
            var membership = membershipData.Get(id);
            if (membership == null)
                return NotFound();

            // Optional: Check for any dependencies (like bookings) if needed

            membershipData.Delete(membership);
            membershipData.Save();

            TempData["message"] = $"{membership.Name} removed from Memberships.";
            return RedirectToAction("ManageMembership", "Home", new { area = "Admin" });
        }

    }
}
