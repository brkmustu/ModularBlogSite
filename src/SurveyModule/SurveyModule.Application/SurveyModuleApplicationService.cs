using AutoMapper;
using SurveyModule.Common;

namespace SurveyModule
{
    public abstract class SurveyModuleApplicationService
    {
        protected readonly ISurveyModuleDbContext _dbContext;
        protected readonly IMapper _mapper;

        protected SurveyModuleApplicationService(ISurveyModuleDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
    }
}

