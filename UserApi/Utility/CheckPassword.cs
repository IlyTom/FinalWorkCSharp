using System.Text.RegularExpressions;

namespace UserApi.Utility
{
    public class CheckPassword
    {
        public static bool Check(string password)
        {
            string symbols = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$";
            Regex regex = new Regex(symbols);
            Match match = regex.Match(password);
            return match.Success;
        }
    }
}
