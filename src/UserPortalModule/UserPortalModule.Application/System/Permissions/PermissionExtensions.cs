using CoreModule.Domain.Permissions;

namespace UserPortalModule.System.Permissions;

public static class PermissionExtensions
{
    /// <summary>
    /// mevcutta kodda bulunan komut ve sorguların isimlerinin listesini veren yardımcı metoddur.
    /// ÖNEMLİ NOT: bu kısımlar db'de bulunan yetki tanımlarıyla karıştırılmamalı! zira bu bilgileri kullanarak db'ye yetki tanımı kaydediyoruz.
    /// </summary>
    /// <returns></returns>
    public static List<Permission> GetSystemPermissions()
    {
        var commandTypes = ApplicationLayerBootstrapper.GetCommandTypes();
        var queryTypes = ApplicationLayerBootstrapper.GetQueryTypes();

        var commandPermissions = commandTypes.Select(x => new Permission { Name = x.Name });
        var queryPermissions = queryTypes.Select(x => new Permission { Name = x.QueryType.Name });
        var allNewPermissions = new List<Permission>();
        allNewPermissions.AddRange(commandPermissions);
        allNewPermissions.AddRange(queryPermissions);

        return allNewPermissions;
    }
}
