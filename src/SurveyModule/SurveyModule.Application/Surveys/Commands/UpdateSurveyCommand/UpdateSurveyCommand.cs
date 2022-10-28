using AutoMapper;
using CoreModule.Application.Common.Contracts;
using CoreModule.Application.Common.Interfaces;
using SurveyModule.Common;

namespace SurveyModule.Surveys.Commands.UpdateSurveyCommand;

public class UpdateSurveyCommand : ICommand
{
    public int Id { get; set; }
    public string SurveyName { get; set; }
    public string Notes { get; set; }
    public bool Kvkk { get; set; }

    public class Handler : SurveyModuleApplicationService, ICommandHandler<UpdateSurveyCommand>
    {
        public Handler(ISurveyModuleDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public Task<Result> Handle(UpdateSurveyCommand command)
        {
            // update ile ilgili işlemler bu kısma yazılacak.
            return Task.FromResult(Result.Success());
        }
    }
}
