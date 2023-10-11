using System.Security;

using Microsoft.Extensions.Logging;

namespace Bearz.VirtualTerminal;

public interface IVirtualTerminal
{
    public bool IsFeatureEnabled(string featureName);

    public bool IsEnabled(LogLevel logLevel);

    char ReadChar();

    string? ReadLine();

    SecureString ReadLineAsSecureString();

    ReadOnlySpan<char> ReadLineAsSpan();

    ConsoleKeyInfo ReadKey();

    ConsoleKeyInfo ReadKey(bool intercept);

    IVirtualTerminal Write(string? message, params object?[] args);

    IVirtualTerminal Write(string? message);

    IVirtualTerminal WriteCommand(string command, string[] args);

    IVirtualTerminal WriteProgress(int percentComplete, string? description = null);

    IVirtualTerminal WriteLine();

    IVirtualTerminal WriteLine(string? message, params object?[] args);

    IVirtualTerminal WriteLine(string? message);

    IVirtualTerminal Log(LogLevel level, EventId eventId, Exception? exception, string? message, params object?[] args);

    IVirtualTerminal Log(LogLevel level, Exception? exception, string? message, params object?[] args);
}