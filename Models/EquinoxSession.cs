using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;

namespace Equinox.Models
{
    public class EquinoxSession
    {
        private const string BookingKey = "MyBookings";
        private const string CountKey = "BookingCount";
        private const string ClubKey = "FilterSelectedClub";
        private const string CategoryKey = "FilterSelectedCategory";
        private const string Delimiter = "-";

        private ISession session;

        public EquinoxSession(ISession s)
        {
            session = s;
        }

        public void SetMyBookings(List<int> classIds)
        {
            string ids = string.Join(Delimiter, classIds);
            session.SetString(BookingKey, ids);
            session.SetInt32(CountKey, classIds.Count);
        }

        public List<int> GetMyBookings()
        {
            string data = session.GetString(BookingKey);
            if (string.IsNullOrEmpty(data))
                return new List<int>();

            return data.Split(Delimiter)
                       .Where(id => int.TryParse(id, out _))
                       .Select(int.Parse)
                       .ToList();
        }

        public int? GetMyBookingCount()
        {
            return session.GetInt32(CountKey);
        }

        public void SetActiveClub(string club)
        {
            session.SetString(ClubKey, club);
        }

        public string GetActiveClub()
        {
            return session.GetString(ClubKey) ?? "All";
        }

        public void SetActiveCategory(string category)
        {
            session.SetString(CategoryKey, category);
        }

        public string GetActiveCategory()
        {
            return session.GetString(CategoryKey) ?? "All";
        }

        public void RemoveMyBookings()
        {
            session.Remove(BookingKey);
            session.Remove(CountKey);
        }
    }
}
