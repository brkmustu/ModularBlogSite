using CoreModule.Application.Common.Interfaces;

namespace SurveyModule.Infrastructure;

public class MachineDateTime : IDateTime
{
    public DateTime Now => DateTime.UtcNow;
}

