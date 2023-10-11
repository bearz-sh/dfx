using System.Security;
using System.Text;

using Bearz.VirtualTerminal;

using Microsoft.Extensions.Logging;

namespace Bearz.VirtualTerminal;

public abstract class VirtualTerminal : IVirtualTerminal
{
    private readonly LogLevelSwitch logLevelSwitch;

    private readonly IDictionary<string, bool> features;

    protected VirtualTerminal(VirtualTerminalOptions options)
    {
        this.logLevelSwitch = options.LogLevelSwitch;
        this.features = options.Features;
        this.IsInteractive = Environment.UserInteractive;
    }

    public bool IsInteractive { get; set; }

    public AnsiColorMode StdOutColor { get; set; } = AnsiColorMode.Auto;

    public AnsiColorMode StdErrColor { get; set; } = AnsiColorMode.Auto;

    public bool IsFeatureEnabled(string featureName)
    {
        return this.features.TryGetValue(featureName, out var enabled) && enabled;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return this.logLevelSwitch.LogLevel <= logLevel;
    }

    public abstract string? ReadLine();

    public abstract SecureString ReadLineAsSecureString();

    public abstract ReadOnlySpan<char> ReadLineAsSpan();

    public abstract ConsoleKeyInfo ReadKey();

    public abstract ConsoleKeyInfo ReadKey(bool intercept);

    public abstract char ReadChar();

    public IVirtualTerminal Write(string? message, params object?[] args)
    {
        if (message is null)
            return this;

        return this.Write(string.Format(message, args));
    }

    public abstract IVirtualTerminal Write(char value);

    public abstract IVirtualTerminal Write(string? message);

    public virtual IVirtualTerminal Write(Style style, string? message)
    {
        if (this.StdOutColor == AnsiColorMode.None)
            return this.Write(message);

        var sb = new StringBuilder();
        sb.Append()

        this.Write("\x1b[");
    }

    public virtual IVirtualTerminal Write(Rgb foreground, string? message)
    {
        return this.Write(new Style { Foreground = foreground }, message);
    }

    public abstract IVirtualTerminal WriteError(string? message);

    public abstract IVirtualTerminal WriteCommand(string command, string[] args);

    public abstract IVirtualTerminal WriteProgress(int percentComplete, string? description = null);

    public IVirtualTerminal WriteLine()
    {
        this.WriteLine(string.Empty);
        return this;
    }

    public IVirtualTerminal WriteLine(string? message, params object?[] args)
    {
        if (message is null)
            return this.WriteLine();

        return this.WriteLine(string.Format(message, args));
    }

    public abstract IVirtualTerminal WriteLine(string? message);

    public abstract IVirtualTerminal Log(LogLevel level, EventId eventId, Exception? exception, string? message, params object?[] args);

    public abstract IVirtualTerminal Log(LogLevel level, Exception? exception, string? message, params object?[] args);
}