using System.Net.Mail;

namespace UserApi.Utility
{
    public class CheckEmail
    {
        public static bool Check(string email)
        {
            try
            {
                MailAddress mail = new MailAddress(email);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
