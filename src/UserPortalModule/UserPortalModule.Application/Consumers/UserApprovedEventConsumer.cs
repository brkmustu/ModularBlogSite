using CoreModule.Application.Common.MessageContracts;
using CoreModule.Domain.Users;
using MassTransit;
using UserPortalModule.Common;

namespace UserPortalModule.Consumers
{
    public class UserApprovedEventConsumer : IConsumer<UserApprovedEvent>
    {
        private readonly IUserPortalModuleDbContext dbContext;

        public UserApprovedEventConsumer(IUserPortalModuleDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task Consume(ConsumeContext<UserApprovedEvent> context)
        {
            if (context.Message.UserId == Guid.Empty)
                return;
            
            var approvedUser = dbContext.Users.FirstOrDefault(user => user.Id == context.Message.UserId);
            
            if (approvedUser == null)
                return;

            approvedUser.SetUserStatus(UserStatusType.Active);

            approvedUser.Activate();

            dbContext.Users.Update(approvedUser);

            await dbContext.SaveChangesAsync(context.CancellationToken);
        }
    }
}
