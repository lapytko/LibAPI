using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LibAPI.Utils
{
    public static class HashUtil
    {
        public static async Task<string> GetHash(string str)
        {
            return await Task.Run(() =>
            {
                var sb = new StringBuilder();
                using var hash = SHA256.Create();
                var computeHash = hash.ComputeHash(Encoding.UTF8.GetBytes(str));
                foreach (var b in computeHash)
                    sb.Append(b.ToString("X2"));

                return sb.ToString();
            });
        }
    }
}