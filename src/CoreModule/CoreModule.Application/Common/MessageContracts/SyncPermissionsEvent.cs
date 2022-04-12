namespace CoreModule.Application.Common.MessageContracts;

public class SyncManagementPermissionsEvent
{
    public List<string> Permissions { get; set; }
}

public class SyncUserPortalPermissionsEvent
{
    public List<string> Permissions { get; set; }
}
