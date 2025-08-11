using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Equinox.Models;
using Equinox.Models.DataLayer;
using Equinox.Models.DataLayer.Repositories;
using Equinox.Models.ViewModels;
using Equinox.Models.DTOs;

namespace Equinox.Controllers
{
    public class HomeController : Controller
    {
        private readonly Repository<Club> clubData;
        private readonly Repository<ClassCategory> categoryData;
        private readonly Repository<EquinoxClass> classData;

        public HomeController(EquinoxContext context)
        {
            clubData = new Repository<Club>(context);
            categoryData = new Repository<ClassCategory>(context);
            classData = new Repository<EquinoxClass>(context);
        }

        public IActionResult Index(string club, string category)
        {
            var session = new EquinoxSession(HttpContext.Session);
            if (club == null)
            {
                var sessionClub = session.GetActiveClub();
                club = sessionClub ?? "All";
            }
            session.SetActiveClub(club);
            if (category == null)
            {
                var sessionCategory = session.GetActiveCategory();
                category = sessionCategory ?? "All";
            }
            session.SetActiveCategory(category);
            var allClubs = clubData.List(new QueryOptions<Club> { OrderBy = c => c.Name }).ToList();
            var allCategories = categoryData.List(new QueryOptions<ClassCategory> { OrderBy = c => c.Name }).ToList();
            var model = new EquinoxViewModel
            {
                AllClubs = allClubs,
                AllCategories = allCategories,
                ActiveClubName = club,
                ActiveCategoryName = category
            };
            var classQueryOptions = new QueryOptions<EquinoxClass>
            {
                Includes = "Club,ClassCategory,User",
                Where = c =>
                    (club == "All" || c.Club.Name == club) &&
                    (category == "All" || c.ClassCategory.Name == category)
            };


            model.AllClasses = classData.List(classQueryOptions)
            .Select(MapToDto)
            .ToList();

           ViewBag.CartCount = session.GetMyBookings()?.Count ?? 0;
            return View(model);
        }

        public IActionResult Details(int id)
        {
            var session = new EquinoxSession(HttpContext.Session);

            var options = new QueryOptions<EquinoxClass>
            {
                Includes = "Club,ClassCategory,User",
                Where = c => c.EquinoxClassId == id
            };

            var classDataItem = classData.Get(options);
            if (classDataItem == null)
                return NotFound();

            var model = new EquinoxViewModel
            {
                EquinoxClass = MapToDto(classDataItem),
                ActiveClubName = session.GetActiveClub(),
                ActiveCategoryName = session.GetActiveCategory()
            };

            ViewBag.CartCount = session.GetMyBookings()?.Count ?? 0;

            return View(model);
        }

        [HttpPost]
        public IActionResult BookClass(int id)
        {
            var session = new EquinoxSession(HttpContext.Session);


            var bookings = session.GetMyBookings() ?? new List<int>();

            if (!bookings.Contains(id))
            {
                bookings.Add(id);
                session.SetMyBookings(bookings);
                TempData["Message"] = "Class booked successfully!";
            }
            else
            {
                TempData["Message"] = "You already booked this class.";
            }

            return RedirectToAction("Index", new
            {
                club = session.GetActiveClub(),
                category = session.GetActiveCategory()
            });
        }

        public IActionResult ViewBookings()
        {
            var session = new EquinoxSession(HttpContext.Session);
            var bookings = session.GetMyBookings() ?? new List<int>();

            var options = new QueryOptions<EquinoxClass>
            {
                Includes = "Club,ClassCategory,User",
                Where = c => bookings.Contains(c.EquinoxClassId)
            };

            var bookedClasses = classData.List(options).ToList();

            ViewBag.CartCount = bookings.Count;

            return View(bookedClasses);
        }

        [HttpPost]
        public IActionResult CancelBooking(int id)
        {
            var session = new EquinoxSession(HttpContext.Session);

            var bookings = session.GetMyBookings() ?? new List<int>();

            if (bookings.Contains(id))
            {
                bookings.Remove(id);
                session.SetMyBookings(bookings);

                TempData["Message"] = "Booking canceled.";
            }
            else
            {
                TempData["Message"] = "No booking found to cancel.";
            }

            return RedirectToAction("ViewBookings");
        }

        private EquinoxClassDto MapToDto(EquinoxClass ec) => new EquinoxClassDto
        {
            EquinoxClassId = ec.EquinoxClassId,
            Name = ec.Name,
            ClassPicture = ec.ClassPicture,
            ClassDay = ec.ClassDay,
            Time = ec.Time,
            ClubName = ec.Club?.Name ?? "",
            ClassCategoryName = ec.ClassCategory?.Name ?? "",
            UserName = ec.User?.Name ?? ""
        };

        public IActionResult Contact() => View();
        public IActionResult Privacy() => View();
        public IActionResult Terms() => View();
        public IActionResult CookiePolicy() => View();
    }
}
