using System.Security.Cryptography;
using System.Text;

namespace ERP.Security
{
    public static class PasswordHashing
    {
        // Preserve compatibility with existing HMAC-SHA512 password records.
        public static void Create(string password, out byte[] hash, out byte[] salt)
        {
            using var hmac = new HMACSHA512();
            salt = hmac.Key;
            hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        public static bool Verify(string password, byte[] hash, byte[] salt)
        {
            using var hmac = new HMACSHA512(salt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return CryptographicOperations.FixedTimeEquals(computedHash, hash);
        }
    }
}
