using System.Text.RegularExpressions;

namespace CwRetail.Api.Helpers
{
    public static class UserValidationHelper
    {
        public static bool IsValidEmail(this string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false;
            }

            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return string.Equals(addr.Address, trimmedEmail);
            }
            catch
            {
                return false;
            }
        }

        public static bool IsValidPhone(this string number)
        {
            var phoneNumber = number.Trim()
                .Replace(" ", "")
                .Replace("-", "")
                .Replace("(", "")
                .Replace(")", "");
            return Regex.Match(phoneNumber, @"^\+\d{5,15}$").Success;
        }
    }
}
