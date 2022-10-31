using System.Text.RegularExpressions;

namespace CwRetail.Api.Extensions
{
    public static class UserValidationExtensions
    {
        public static bool IsValidEmail(this string email)
        {
            try
            {
                var trimmedEmail = email.Trim();

                if (trimmedEmail.EndsWith("."))
                {
                    return false;
                }

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
            try
            {
                var phoneNumber = number.Trim()
                .Replace(" ", "")
                .Replace("-", "")
                .Replace("(", "")
                .Replace(")", "");
                return Regex.Match(phoneNumber, @"^\+\d{5,15}$").Success;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
