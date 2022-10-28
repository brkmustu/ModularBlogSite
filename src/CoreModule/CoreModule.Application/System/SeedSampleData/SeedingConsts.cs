using CoreModule.Application.Extensions.Hashing;
using CoreModule.Domain.Users;

namespace CoreModule.Application.SeedSampleData;

public class SeedingConsts
{
    private const string AdminPassword = "1qaz!2wsx";

    public static User AdminUser()
    {
        User admin = new User("admin", "admin", "admin", "admin@demoapp.com");

        var encryptedPassword = AdminPassword.CreatePasswordHash();

        admin.SetPasswordHash(encryptedPassword.PasswordHash);
        admin.SetPasswordSalt(encryptedPassword.PasswordSalt);

        admin.Id = 1;

        admin.SetUserStatus(UserStatusType.Active);
        admin.Activate();

        return admin;
    }
}
