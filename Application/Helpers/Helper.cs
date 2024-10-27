using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Application.Helpers
{
    /// <summary>
    /// Helper class that helps to use cases
    /// </summary>
    public static class Helper
    {
        /// <summary>
        /// Generates a hashed password and salt using HMACSHA256.
        /// </summary>
        /// <param name="password">The plain text password to hash.</param>
        /// <param name="passwordHash">The resulting hashed password as a byte array.</param>
        /// <param name="passwordSalt">The generated salt used for hashing as a byte array.</param>
        public static void PasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            // create a new instance of HMACSHA256 to generate a hash for the password
            using (var hmac = new HMACSHA256())
            {
                // generate a cryptographic key (salt) for the HMAC algorithm
                passwordSalt = hmac.Key;

                // compute the hash of the input password (converted to bytes using UTF-8 encoding)
                // this ensures the password is stored in a secure hashed format
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        /// <summary>
        /// Verifies if the provided password matches the stored password hash using the provided salt.
        /// </summary>
        /// <param name="password">The plain text password to verify.</param>
        /// <param name="passwordHash">The stored hashed password as a byte array.</param>
        /// <param name="passwordSalt">The salt used to hash the stored password.</param>
        /// <returns>
        /// Returns true if the password is valid; otherwise, false.
        /// </returns>
        public static bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            // use the HMACSHA256 algorithm to generate a hash from the password, using the provided salt
            using (var hmac = new HMACSHA256(passwordSalt))
            {
                // compute the hash of the input password (converting the plain text password to bytes)
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

                // compare the computed hash with the stored password hash to check if they are the same
                // sequenceEqual checks if each byte in both arrays matches
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        /// <summary>
        /// Checks whether the given <see cref="IEnumerable{T}"/> has any non-null elements.
        /// </summary>
        /// <param name="data">The collection of elements to check for non-null values.</param>
        /// <returns>
        /// <c>true</c> if the collection is not null and contains at least one non-null element; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasValue<T>(this IEnumerable<T> data)
        {
            return data != null && data.Any();
        }
    }
}
