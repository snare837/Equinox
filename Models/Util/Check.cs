namespace Equinox.Models.Util
{
    public static class Check
    {
        public static string PhoneNumberExists(EquinoxContext ctx, string phonenumber)
        {
            string msg = string.Empty;
            if (!string.IsNullOrEmpty(phonenumber))
            {
                var user = ctx.Coaches.FirstOrDefault(
                    c => c.PhoneNumber.ToLower() == phonenumber.ToLower());
                if (user != null)
                    msg = $"PhoneNumber {phonenumber} already in use.";
            }
            return msg;
        }
        public static string EmailExists(EquinoxContext ctx, string email)
        {
            string msg = string.Empty;
            if (!string.IsNullOrEmpty(email)) {
                var user = ctx.Coaches.FirstOrDefault(
                    c => c.Email.ToLower() == email.ToLower());
                if (user != null) 
                    msg = $"Email {email} already in use.";
            }
            return msg;
        }
    }
}