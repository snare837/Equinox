using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Equinox.Models;
using Equinox.Models.ViewModels;
using System.Linq;
using System.Collections.Generic;

namespace Equinox.Controllers
{
    public class HomeController : Controller
    {

        private readonly EquinoxContext _context;
        private const string BookingCookieKey = "Bookings";
        private const string FilterSelectedClubKey = "FilterSelectedClub";
        private const string FilterSelectedCategoryKey = "FilterSelectedCategory";

        public HomeController(EquinoxContext context)
        {
            _context = context;
        }

        public IActionResult Index(string club, string category)
        {

            club ??= HttpContext.Session.GetString(FilterSelectedClubKey) ?? "All";
            HttpContext.Session.SetString(FilterSelectedClubKey, club);

            category ??= HttpContext.Session.GetString(FilterSelectedCategoryKey) ?? "All";      
            HttpContext.Session.SetString(FilterSelectedCategoryKey, category);

            var model = new EquinoxViewModel
            {
                AllClubs = _context.Clubs.ToList(),
                AllCategories = _context.ClassCategories.ToList(),
                ActiveClubName = club,
                ActiveCategoryName = category
            };

            IQueryable<EquinoxClass> query = _context.EquinoxClasses
                .Include(c => c.Club)
                .Include(c => c.ClassCategory)
                .Include(c => c.User);

            if (club != "All")
                query = query.Where(c => c.Club.Name == club);

            if (category != "All")
                query = query.Where(c => c.ClassCategory.Name == category);

            model.AllClasses = query.ToList();

            var bookings = HttpContext.Request.GetObjectFromJson<List<int>>(BookingCookieKey) ?? new List<int>();
            ViewBag.CartCount = bookings.Count;

            return View(model);
        }

        [HttpPost]
        public IActionResult Filter(string club, string category)
        {
            HttpContext.Session.SetString(FilterSelectedClubKey, club);
            HttpContext.Session.SetString(FilterSelectedCategoryKey, category);
            return RedirectToAction("index");
        }

        public IActionResult Details(int id)
        {
            var equinoxClass = _context.EquinoxClasses
                .Include(c => c.Club)
                .Include(c => c.ClassCategory)
                .Include(c => c.User)
                .FirstOrDefault(c => c.EquinoxClassId == id);

            if (equinoxClass == null)
                return NotFound();

            ViewBag.CartCount = HttpContext.Request.GetObjectFromJson<List<int>>(BookingCookieKey)?.Count ?? 0;

            return View(equinoxClass);
        }

        [HttpPost]
        public IActionResult BookClass(int id)
        {
            
            var bookings = HttpContext.Request.GetObjectFromJson<List<int>>(BookingCookieKey) ?? new List<int>();
            bool exists = bookings.Contains(id);
            if (!exists)
            {
                bookings.Add(id);
                HttpContext.Response.SetObjectAsJson(BookingCookieKey, bookings);
                TempData["Message"] = "Class booked successfully!";
            }
            else
            {
                TempData["Message"] = "This class is already booked!";
            }

            return RedirectToAction("Index");
        }

        public IActionResult ViewBookings()
        {
            var bookings = HttpContext.Request.GetObjectFromJson<List<int>>(BookingCookieKey) ?? new List<int>();
            var bookedClasses = _context.EquinoxClasses
                .Include(c => c.Club)
                .Include(c => c.ClassCategory)
                .Include(c => c.User)
                .Where(c => bookings.Contains(c.EquinoxClassId))
                .ToList();

            ViewBag.CartCount = bookings.Count;
            return View(bookedClasses);
        }

        [HttpPost]
        public IActionResult CancelBooking(int id)
        {
            var bookings = HttpContext.Request.GetObjectFromJson<List<int>>(BookingCookieKey) ?? new List<int>();
            bool exists = bookings.Contains(id);

            if (exists)
            {
                bookings.Remove(id);
                HttpContext.Response.SetObjectAsJson(BookingCookieKey, bookings);
                TempData["Message"] = "Booking canceled.";
            }
            else
            {
                TempData["Message"] = "No booking found to cancel.";
            }

            return RedirectToAction("ViewBookings");
        }

        public IActionResult Contact() => View();
        public IActionResult Privacy() => View();
        public IActionResult Terms() => View();
        public IActionResult CookiePolicy() => View();
    }
}
