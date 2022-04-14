using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace LibAPI.Options
{
    public static class AuthOptions
    {
        public const string Issuer = "MyAuthServer"; // издатель токена
        public const string Audience = "MyAuthClient"; // потребитель токена
        const string Key = "DZaN|HG@Kh6@77kGG*i*n1vxRfXvR8f{F5yG@MOF";   // ключ для шифрации
        public const int Lifetime = 525960; // время жизни токена - 1 минута
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));
        }
    }
}
