using AutoMapper;
using CoreModule.Application.Common.Contracts;
using CoreModule.Application.Common.Interfaces;
using CoreModule.Application.CrossCuttingConcerns;
using CoreModule.Domain.Users;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using UserPortalModule.Common;
using UserPortalModule.Common.Contracts;

namespace UserPortalModule.CommandHandlers;

public class UserUpdateProfileCommand : CommandRequest
{
    [Required]
    public Guid UserId { get; set; }

    [Required(AllowEmptyStrings = false)]
    [StringLength(70)]
    public string UserName { get; set; }

    [Required(AllowEmptyStrings = false)]
    [StringLength(70)]
    public string FirstName { get; set; }

    [Required(AllowEmptyStrings = false)]
    [StringLength(70)]
    public string LastName { get; set; }

    [Required(AllowEmptyStrings = false)]
    [StringLength(20)]
    public string MobileNumber { get; set; }

    [Required(AllowEmptyStrings = false)]
    [StringLength(70)]
    public string EmailAddress { get; set; }

    public long[] RoleIds { get; set; }

    [JsonIgnore]
    public override string OperationName => "UserUpdateProfileCommand";

    [AuditingDecorator]
    [AuthorizationDecorator]
    [ValidationDecorator]
    public class Handler : UserPortalModuleApplicationService, ICommandHandler<UserUpdateProfileCommand>
    {
        public Handler(IUserPortalModuleDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public async Task<Result> Handle(UserUpdateProfileCommand command)
        {
            var currentUser = _dbContext.Users.Where(x => x.Id == command.UserId).FirstOrDefault();
            var user = _mapper.Map<User>(command);
            user.Id = command.UserId;
            
            user.CreatedBy = currentUser.CreatedBy;
            user.CreatedDate = currentUser.CreatedDate;
            user.LastModifiedBy = currentUser.LastModifiedBy;
            user.LastModifiedDate = currentUser.LastModifiedDate;
            
            if (currentUser.IsActive)
                user.Activate();
            else
                user.Deactivate();

            user.SetUserStatus((UserStatusType)currentUser.UserStatusId);

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync(cancellationToken: CancellationToken.None);
            return Result.Success();
        }
    }
}
