using AutoMapper;
using CoreModule.Application.Common.Contracts;
using CoreModule.Application.Common.Interfaces;
using ManagementModule.Common;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ManagementModule.CommandHandlers;

/// <summary>
/// kullanıcıyı aktifleştiren yada pasife alınmasına yardımcı olan api servisidir.
/// </summary>
public class UserActivationStatusCommand : CommandRequest
{
    [Required]
    public Guid UserId { get; set; }
    public bool IsActive { get; set; }

    [JsonIgnore]
    public override CrossCuttingConcerns[] ApplicableConcerns => new[] 
    {
        CrossCuttingConcerns.Auditing,
        CrossCuttingConcerns.Authorization,
        CrossCuttingConcerns.Validation
    };

    [JsonIgnore]
    public override string OperationName => "UserActivationStatusCommand";

    public class Handler : ManagementModuleApplicationService, ICommandHandler<UserActivationStatusCommand>
    {
        public Handler(IManagementModuleDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public Task<Result> Handle(UserActivationStatusCommand command)
        {
            var user = _dbContext.Users.Where(x => x.Id == command.UserId).FirstOrDefault();

            if (user == null) return Task.FromResult(
                    Result.Failure(
                            new[] { $@"Talep ettiğiniz kullanıcı id'si ile kullanıcı bulunamadığından işlem gerçekleştirilemedi! UserId: {command.UserId}" }
                            )
                );

            if (command.IsActive && !user.IsActive)
            {
                user.Activate();
                _dbContext.SaveChangesAsync(cancellationToken: CancellationToken.None);
            }
            else if (!command.IsActive && user.IsActive)
            {
                user.Deactivate();
                _dbContext.SaveChangesAsync(cancellationToken: CancellationToken.None);
            }

            return Task.FromResult(Result.Success());
        }
    }
}
