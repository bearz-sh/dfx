namespace Bearz.VirtualTerminal;

public class VirtualTerminalOptions
{
    public LogLevelSwitch LogLevelSwitch { get; set; } = new LogLevelSwitch();

    public IDictionary<string, bool> Features { get; set; } = new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
}