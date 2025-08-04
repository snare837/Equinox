using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;

using Equinox.Models;
using Equinox.Models.ViewModels;

namespace Equinox.Controllers
{
    public class HomeController : Controller
    {
        private readonly EquinoxContext context;

        public HomeController(EquinoxContext ctx)
        {
            context = ctx;
        }
        
        public IActionResult Index(EquinoxViewModel model)
        {
            var session = new EquinoxSession(HttpContext.Session);
            session.SetActiveClub(model.ActiveClubName);
            session.SetActiveCategory(model.ActiveCategoryName);

            List<int> bookings = session.GetMyBookings();
            if (bookings == null || bookings.Count == 0)
            {
                var cookies = new EquinoxCookie(Request.Cookies, Response.Cookies);
                bookings = cookies.GetMyBookings();
                if (bookings.Count > 0)
                {
                    session.SetMyBookings(bookings);
                }
            }

        var allClubs = context.Clubs.OrderBy(c => c.Name).ToList();
        if (model.ActiveClubName != "All")
         {
          var activeClub = allClubs.FirstOrDefault(c => c.Name == model.ActiveClubName);
        if (activeClub != null)
         {
        allClubs.Remove(activeClub);
        allClubs.Insert(0, activeClub);
         }
         }
           model.AllClubs = allClubs;

            List<ClassCategory> allCategories;

         if (model.ActiveClubName != "All")
        {
          allCategories = context.EquinoxClasses
          .Where(c => c.Club.Name == model.ActiveClubName)
          .Select(c => c.ClassCategory)
          .Distinct()
          .OrderBy(c => c.Name)
          .ToList();
        }
          else
        {
          allCategories = context.ClassCategories.OrderBy(c => c.Name).ToList();
        }

             if (model.ActiveCategoryName != "All")
              {
                var activeCat = allCategories.FirstOrDefault(c => c.Name == model.ActiveCategoryName);
              if (activeCat != null)
              {
        allCategories.Remove(activeCat);
        allCategories.Insert(0, activeCat);
            }
          }
        model.AllCategories = allCategories;


            IQueryable<EquinoxClass> query = context.EquinoxClasses
                .Include(c => c.Club)
                .Include(c => c.ClassCategory)
                .Include(c => c.User);

            if (model.ActiveClubName != "All")
            {
                query = query.Where(c => c.Club.Name == model.ActiveClubName);
            }

            if (model.ActiveCategoryName != "All")
            {
                query = query.Where(c => c.ClassCategory.Name == model.ActiveCategoryName);
            }

            model.AllClasses = query.ToList();
            ViewBag.CartCount = bookings.Count;

            return View(model);
        }

       public IActionResult Details(int id)
        {
    var session = new EquinoxSession(HttpContext.Session);

    var classData = context.EquinoxClasses
        .Include(c => c.Club)
        .Include(c => c.ClassCategory)
        .Include(c => c.User)
        .FirstOrDefault(c => c.EquinoxClassId == id);

    if (classData == null)
    {
    return NotFound(); // or handle appropriately
    }

     var model = new EquinoxViewModel
   {
    EquinoxClass = classData,
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
            var cookies = new EquinoxCookie(Request.Cookies, Response.Cookies);


            var bookings = session.GetMyBookings() ?? new List<int>();

            if (!bookings.Contains(id))
            {
                bookings.Add(id);
                session.SetMyBookings(bookings);
                cookies.SetMyBookings(bookings);
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

            var bookedClasses = context.EquinoxClasses
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
         var session = new EquinoxSession(HttpContext.Session);
         var cookies = new EquinoxCookie(Request.Cookies, Response.Cookies);

          var bookings = session.GetMyBookings() ?? new List<int>();

     if (bookings.Contains(id))
       {
        bookings.Remove(id);
        session.SetMyBookings(bookings);
        cookies.SetMyBookings(bookings);
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
