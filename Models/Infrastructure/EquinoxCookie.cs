using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Equinox.Models
{
    public class EquinoxCookie
    {
        private const string BookingKey = "MyBookings";
        private const string Delimiter = "-";

        private IRequestCookieCollection requestCookies;
        private IResponseCookies responseCookies;

        public EquinoxCookie(IRequestCookieCollection request)
        {
            requestCookies = request;
        }

        public EquinoxCookie(IResponseCookies response)
        {
            responseCookies = response;
        }

        public EquinoxCookie(IRequestCookieCollection request, IResponseCookies response)
        {
            requestCookies = request;
            responseCookies = response;
        }

        public void SetMyBookings(List<int> bookings)
        {
            string idString = string.Join(Delimiter, bookings);
            CookieOptions options = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(7)
            };
            responseCookies.Append(BookingKey, idString, options);
        }

        public List<int> GetMyBookings()
        {
            if (requestCookies.TryGetValue(BookingKey, out string? idsString) && !string.IsNullOrEmpty(idsString))
            {
                return idsString.Split(Delimiter)
                                .Select(id => int.TryParse(id, out int result) ? result : -1)
                                .Where(id => id != -1)
                                .ToList();
            }

            return new List<int>();
        }

        public void RemoveMyBookings()
        {
            responseCookies.Delete(BookingKey);
        }
    }
}
