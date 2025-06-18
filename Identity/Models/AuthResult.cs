using IdentityDatabase.Models;

namespace Identity.Models
{
    public class AuthResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public User User { get; set; }

        public static AuthResult SuccessResult(string message, User user = null)
        {
            return new AuthResult { Success = true, Message = message, User = user };
        }

        public static AuthResult FailedResult(string message)
        {
            return new AuthResult { Success = false, Message = message };
        }
    }
}
