using System.Security.Cryptography;

namespace Otus.SocNet.WebApi
{
    public static class HashHalper
    {
        private const int SaltSize = 16; // 128 бит
        private const int KeySize = 32;  // 256 бит
        private const int Iterations = 100_000;

        public static string Hash(string password)
        {
            using var rng = RandomNumberGenerator.Create();
            var salt = new byte[SaltSize];
            rng.GetBytes(salt);

            var hash = GetPbkdf2Bytes(password, salt);

            var result = new byte[SaltSize + KeySize];
            Buffer.BlockCopy(salt, 0, result, 0, SaltSize);
            Buffer.BlockCopy(hash, 0, result, SaltSize, KeySize);

            return Convert.ToBase64String(result);
        }

        public static bool Verify(string password, string hashBase64)
        {
            var hashBytes = Convert.FromBase64String(hashBase64);

            var salt = new byte[SaltSize];
            var storedHash = new byte[KeySize];

            Buffer.BlockCopy(hashBytes, 0, salt, 0, SaltSize);
            Buffer.BlockCopy(hashBytes, SaltSize, storedHash, 0, KeySize);

            var computedHash = GetPbkdf2Bytes(password, salt);

            return CryptographicOperations.FixedTimeEquals(storedHash, computedHash);
        }

        private static byte[] GetPbkdf2Bytes(string password, byte[] salt)
        {
            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256);
            return pbkdf2.GetBytes(KeySize);
        }
    }
}
