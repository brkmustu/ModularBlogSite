using CoreModule.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace SurveyModule.Common
{
    public interface ISurveyModuleDbContext : ICoreModuleDbContext
    {
        // DbSet'ler burada tanımlanacak

        DbSet<Survey> Surveys { get; }
    }
}

