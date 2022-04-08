namespace CoreModule.Domain.Permissions;

public static class PermissionNames
{
    /// <summary>
    /// sistem admin rolüdür.
    /// </summary>
    public const string SystemAdmin = "Admin";

    /// <summary>
    /// Portal uygulaması için admin rolüdür. Sadece portal modülünde tam yetkiye sahiptir.
    /// Varsayılan olarak management modülünde herhangi bir yetkisi yoktur.
    /// </summary>
    public const string PortalAdmin = "PortalAdmin";

    /// <summary>
    /// Standart portal uygulaması rolüdür.
    /// </summary>
    public const string Portal = "Portal";
}
