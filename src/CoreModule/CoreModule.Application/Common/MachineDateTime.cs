using CoreModule.Application.Common.Interfaces;

namespace CoreModule.Application.Common;

public class MachineDateTime : IDateTime
{
    public DateTime Now => DateTime.UtcNow;
}

