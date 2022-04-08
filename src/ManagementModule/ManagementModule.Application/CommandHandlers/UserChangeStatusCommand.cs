using AutoMapper;
using CoreModule.Application.Common.Contracts;
using CoreModule.Application.Common.Interfaces;
using CoreModule.Application.Common.MessageContracts;
using CoreModule.Application.Common.RabbitMqExtensions;
using CoreModule.Domain.Users;
using ManagementModule.Common;
using MassTransit;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

namespace ManagementModule.CommandHandlers;

/// <summary>
/// kayıt kabul ret bilgisi için hazırlanmış olan api servisidir.
/// </summary>
public class UserChangeStatusCommand : ICommand
{
    [Required]
    public Guid UserId { get; set; }
    [Required]
    public bool IsApproved { get; set; }

    public class Handler : ManagementModuleApplicationService, ICommandHandler<UserChangeStatusCommand>
    {
        public Handler(
                IManagementModuleDbContext dbContext, 
                IMapper mapper, 
                IPublishEndpoint publishEndpoint
            ) : base(dbContext, mapper, publishEndpoint)
        {
        }

        public async Task<Result> Handle(UserChangeStatusCommand command)
        {
            var user = _dbContext.Users.Where(x => x.Id == command.UserId).FirstOrDefault();

            if (user == null) return Result.Failure(
                    new[] { $@"Talep ettiğiniz kullanıcı id'si ile kullanıcı bulunamadığından işlem gerçekleştirilemedi! UserId: {command.UserId}" }
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
            await _dbContext.SaveChangesAsync(cancellationToken: CancellationToken.None);

            if (command.IsApproved)
            {
                await _publishEndpoint.Publish<UserApprovedEvent>(new
                {
                    UserId = command.UserId
                });
            }

            return Result.Success();
        }
    }
}
