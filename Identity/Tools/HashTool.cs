using System.Security.Cryptography;

namespace Identity.Tools
{
    public static class HashingHelper
    {
        public static byte[] GenerateSalt()
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                var salt = new byte[32];
                rng.GetBytes(salt);
                return salt;
            }
        }

        public static string HashPassword(string password, byte[] salt)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt))
            {
                return Convert.ToBase64String(pbkdf2.GetBytes(32));
            }
        }

        public static bool VerifyPassword(string password, string hash, byte[] salt)
        {
            var computedHash = HashPassword(password, salt);
            return computedHash == hash;
        }
    }
}
