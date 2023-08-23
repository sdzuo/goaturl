using System.Data.HashFunction;
using System.Data.HashFunction.MurmurHash;
namespace urlgoatbackend.Helper
{
    public static class HashingHelper
    {
            public static string HashLongUrl(string longUrl)
            {
                // Convert the long URL to bytes (UTF-8 encoded)
                byte[] data = System.Text.Encoding.UTF8.GetBytes(longUrl);

                // Create an instance of MurmurHash3
                 var murmur = MurmurHash3Factory.Instance.Create();

                // Compute the hash
                IHashValue hashResult = murmur.ComputeHash(data);
                byte[] hashBytes = hashResult.Hash;

                // Convert the hash bytes to a hexadecimal string
                string hashHex = BitConverter.ToString(hashBytes).Replace("-", "");

                // Determine the length of the hash (between 5 and 8 characters)
                int desiredLength = Math.Min(Math.Max(5, hashHex.Length), 8);

                // Take the first 'desiredLength' characters from the hashHex
                string shortenedHash = hashHex.Substring(0, desiredLength);

                return shortenedHash;
        }
    }
}
