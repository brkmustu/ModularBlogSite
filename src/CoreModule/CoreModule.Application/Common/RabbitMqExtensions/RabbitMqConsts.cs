namespace CoreModule.Application.Common.RabbitMqExtensions
{
    public static class RabbitMqConsts
    {
        public const string ManagementModule_SyncPermission_QueueName = "mbs.management.syncpermission";
        public const string ManagementModule_UserRegistration_QueueName = "mbs.management.userregistration";
        public const string UserPortalModule_SyncPermission_QueueName = "mbs.userportal.syncpermission";
        public const string UserPortalModule_UserApproved_QueueName = "mbs.userportal.userapproved";
    }
}
