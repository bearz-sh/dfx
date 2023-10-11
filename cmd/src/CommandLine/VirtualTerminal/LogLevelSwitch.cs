using Microsoft.Extensions.Logging;

namespace Bearz.VirtualTerminal;

public class LogLevelSwitch
{
    public LogLevelSwitch()
    {
    }

    public LogLevel LogLevel { get; set; } = LogLevel.Information;
}