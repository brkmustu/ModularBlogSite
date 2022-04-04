using AutoMapper;
using CoreModule.Application.Common.Contracts;
using CoreModule.Application.Common.Interfaces;
using CoreModule.Domain.Users;
using ManagementModule.Common;
using System.ComponentModel.DataAnnotations;

namespace ManagementModule.CommandHandlers;

/// <summary>
/// kayıt kabul ret bilgisi için hazırlanmış olan api servisidir.
/// </summary>
public class UserRegistrationStatusCommand : ICommand
{
    [Required]
    public Guid UserId { get; set; }
    [Required]
    public bool IsApproved { get; set; }

    public class Handler : ManagementModuleApplicationService, ICommandHandler<UserRegistrationStatusCommand>
    {
        public Handler(IManagementModuleDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public Task<Result> Handle(UserRegistrationStatusCommand command)
        {
            var user = _dbContext.Users.Where(x => x.Id == command.UserId).FirstOrDefault();

            if (user == null) return Task.FromResult(
                    Result.Failure(
                            new[] { $@"Talep ettiğiniz kullanıcı id'si ile kullanıcı bulunamadığından işlem gerçekleştirilemedi! UserId: {command.UserId}" }
                            )
                );

            if (command.IsApproved)
            {
                user.Activate();
                user.SetUserStatus(UserStatusType.Active);
            }
            else
            {
                user.Deactivate();
                user.SetUserStatus(UserStatusType.Rejected);
            }

            _dbContext.Users.Add(user);
            _dbContext.SaveChangesAsync(cancellationToken: CancellationToken.None);

            return Task.FromResult(Result.Success());
        }
    }
}
