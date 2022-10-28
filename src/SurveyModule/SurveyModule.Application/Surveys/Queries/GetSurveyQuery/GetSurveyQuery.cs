using AutoMapper;
using CoreModule.Application.Common.Contracts;
using CoreModule.Application.Common.Interfaces;
using SurveyModule.Common;

namespace SurveyModule.Surveys.Queries.GetSurveyQuery
{
    public class GetSurveyQuery : IQuery<SurveyDto>
    {
        public int Id { get; set; }

        public class Handler : SurveyModuleApplicationService, IQueryHandler<GetSurveyQuery, SurveyDto>
        {
            public Handler(ISurveyModuleDbContext dbContext, IMapper mapper)
                : base(dbContext, mapper)
            {
            }

            public async Task<SurveyDto> Handle(GetSurveyQuery query)
            {
                var survey = _dbContext.Surveys.FirstOrDefault(x => x.Id == query.Id);
                return _mapper.Map<SurveyDto>(survey);
            }
        }
    }
}
