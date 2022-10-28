﻿using CoreModule.Domain.Permissions;
using System.Reflection;
using UserPortalModule.Common;
using CorePermissionExtensions = CoreModule.Application.Extensions.PermissionExtensions;

namespace UserPortalModule.System.Permissions;

public static class PermissionExtensions
{
    private static readonly Assembly[] ApplicationLayerAssemblies = new[] { Assembly.GetExecutingAssembly() };

    /// <summary>
    /// mevcutta kodda bulunan komut ve sorguların isimlerinin listesini veren yardımcı metoddur.
    /// ÖNEMLİ NOT: bu kısımlar db'de bulunan yetki tanımlarıyla karıştırılmamalı! zira bu bilgileri kullanarak db'ye yetki tanımı kaydediyoruz.
    /// ÖNEMLİ NOT: yetki tanımı olabilmesi için ilgili handler'da AuthorizationDecoratorAttribute 'un attribute olarak kullanılmış olması gerekiyor.
    /// </summary>
    /// <returns></returns>
    public static List<Permission> GetAuthSystemPermissions()
    {
        return CorePermissionExtensions.GetAuthSystemPermissions(ApplicationLayerAssemblies, ModuleConsts.ModuleName);
    }

    public static List<Permission> GetUserPermissions(IUserPortalModuleDbContext dbContext, int userId)
    {
        return CorePermissionExtensions.GetUserPermissions(dbContext, userId);
    }
}
