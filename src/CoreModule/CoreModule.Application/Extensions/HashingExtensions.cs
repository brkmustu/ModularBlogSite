using System.Security.Cryptography;
using System.Text;

namespace CoreModule.Application.Extensions.Hashing;

public static class HashingExtensions
{
    public static (byte[] PasswordSalt, byte[] PasswordHash) CreatePasswordHash(this string password)
    {
        using (var hmac = new HMACSHA512())
        {
            var passwordSalt = hmac.Key;
            var passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            return (passwordHash, passwordSalt);
        }
    }

    public static bool VerifyPasswordHash(this string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512(passwordSalt))
        {
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != passwordHash[i])
                {
                    return false;
                }
            }
        }

        return true;
    }
}
