namespace CoreModule.Application.Common;

public class MachineDateTime : IDateTime
{
    public DateTime Now => DateTime.UtcNow;
}

