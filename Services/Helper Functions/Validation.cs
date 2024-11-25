namespace IC_Eval_FE.Services.Helper_Functions
{
    public class Validation
{
        public bool ValidateEmail(string email)
        {

            if (string.IsNullOrWhiteSpace(email))
                return false;


            var emailRegex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return System.Text.RegularExpressions.Regex.IsMatch(email, emailRegex);
        }
    }
}
