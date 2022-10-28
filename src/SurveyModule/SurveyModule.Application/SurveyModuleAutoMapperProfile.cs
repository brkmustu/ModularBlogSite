using AutoMapper;
using CoreModule.Domain.Users;
using SurveyModule.Surveys;

namespace SurveyModule
{
    public class SurveyModuleAutoMapperProfile : Profile
    {
        public SurveyModuleAutoMapperProfile()
        {
            CreateMap<Survey, SurveyDto>();
            CreateMap<SurveyDto, Survey>();
        }
    }
}

