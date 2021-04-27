using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace VDCore.Authorization
{
    public static class HashPasswordGenerator
    {
        /*
         * Password hash settings. 
         */
        private const int SaltSize = 128 / 8;
        private const int NumBytesRequested = 256 / 8;
        private const int IterCount = 1228;
        private const KeyDerivationPrf Prf = KeyDerivationPrf.HMACSHA1;
        
        /// <summary>
        /// Function generates hash from password and returns it as String. To verify use VerifyHash function.
        /// </summary>
        /// <param name="rawString"></param>
        /// <returns></returns>
        public static string GetHashFromString(string rawString)
        {
            byte[] salt = new byte[SaltSize];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            byte[] subKey = KeyDerivation.Pbkdf2(
                password: rawString,
                salt: salt,
                prf: Prf,
                iterationCount: IterCount,
                numBytesRequested: NumBytesRequested
            );
            
            var outputBytes = new byte[1 + SaltSize + NumBytesRequested];
            outputBytes[0] = 0x00; // Format marker.
            Buffer.BlockCopy(salt, 0, outputBytes, 1, SaltSize);
            Buffer.BlockCopy(subKey, 0, outputBytes, 1 + SaltSize, NumBytesRequested);

            return Convert.ToBase64String(outputBytes);
        }

        public static bool VerifyHash(string hashedPasswordString, string password)
        {
            byte[] hashedPassword = Convert.FromBase64String(hashedPasswordString);

            //We know ahead of time the exact length of a valid hashed password payload.
            if (hashedPassword.Length != 1 + SaltSize + NumBytesRequested)
            {
                return false; // bad size
            }

            byte[] salt = new byte[SaltSize];
            Buffer.BlockCopy(hashedPassword, 1, salt, 0, salt.Length);

            byte[] expectedSubkey = new byte[NumBytesRequested];
            Buffer.BlockCopy(hashedPassword, 1 + salt.Length, expectedSubkey, 0, expectedSubkey.Length);

            byte[] actualSubkey = KeyDerivation.Pbkdf2(password, salt, Prf, IterCount, NumBytesRequested);
            
            return CryptographicOperations.FixedTimeEquals(actualSubkey, expectedSubkey);
        }
    }
}