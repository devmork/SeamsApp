using System.Security.Cryptography;
using System.Text;

namespace SeamsApp.Utilities
{
    public static class EmailUtility
    {
        /// <summary>
        /// Generates a cryptographically secure 6-digit OTP using rejection
        /// sampling to avoid modulo bias.
        /// </summary>
        public static string GenerateOtp()
        {
            const int max = 1_000_000; // 000000–999999
            uint value;
            do
            {
                value = BitConverter.ToUInt32(RandomNumberGenerator.GetBytes(4), 0);
            } while (value >= uint.MaxValue - (uint.MaxValue % max));

            return (value % max).ToString("D6");
        }

        /// <summary>
        /// SHA-256 hash of the OTP. Short-lived value, so a plain hash is sufficient.
        /// </summary>
        public static string HashOtp(string otp)
        {
            var bytes = Encoding.UTF8.GetBytes(otp);
            return Convert.ToBase64String(SHA256.HashData(bytes));
        }

        /// <summary>
        /// Constant-time comparison to prevent timing attacks.
        /// </summary>
        public static bool VerifyOtp(string otp, string storedHash)
        {
            var computed = Encoding.UTF8.GetBytes(HashOtp(otp));
            var stored = Encoding.UTF8.GetBytes(storedHash);
            return CryptographicOperations.FixedTimeEquals(computed, stored);
        }

        /// <summary>
        /// Random 10-char temp password emailed to newly approved applicants.
        /// Mixes uppercase, lowercase, digits, and symbols.
        /// </summary>
        public static string GenerateTempPassword()
        {
            const string upper = "ABCDEFGHJKLMNPQRSTUVWXYZ";   // no I, O
            const string lower = "abcdefghijkmnpqrstuvwxyz";   // no l, o
            const string digits = "23456789";                  // no 0, 1
            const string symbols = "!@#$%&*";
            const string all = upper + lower + digits + symbols;

            Span<char> buffer = stackalloc char[10];

            // Ensure at least one of each character class
            buffer[0] = upper[RandomNumberGenerator.GetInt32(upper.Length)];
            buffer[1] = lower[RandomNumberGenerator.GetInt32(lower.Length)];
            buffer[2] = digits[RandomNumberGenerator.GetInt32(digits.Length)];
            buffer[3] = symbols[RandomNumberGenerator.GetInt32(symbols.Length)];

            for (int i = 4; i < buffer.Length; i++)
                buffer[i] = all[RandomNumberGenerator.GetInt32(all.Length)];

            // Fisher–Yates shuffle so the guaranteed chars aren't always in slots 0–3
            for (int i = buffer.Length - 1; i > 0; i--)
            {
                int j = RandomNumberGenerator.GetInt32(i + 1);
                (buffer[i], buffer[j]) = (buffer[j], buffer[i]);
            }

            return new string(buffer);
        }
    }
}
