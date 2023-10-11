using Microsoft.Extensions.Logging;

namespace Bearz.VirtualTerminal;

public static class LoggingExtensions
{
    public static IVirtualTerminal Debug(this IVirtualTerminal terminal, string? message, params object?[] args)
    {
        terminal.Log(LogLevel.Debug, null, message, args);
        return terminal;
    }

    public static IVirtualTerminal Debug(this IVirtualTerminal terminal, Exception? exception, string? message, params object?[] args)
    {
        terminal.Log(LogLevel.Debug, exception, message, args);
        return terminal;
    }

    public static IVirtualTerminal Debug(this IVirtualTerminal terminal, EventId eventId, string? message, params object?[] args)
    {
        terminal.Log(LogLevel.Debug, eventId, null, message, args);
        return terminal;
    }

    public static IVirtualTerminal Debug(this IVirtualTerminal terminal, EventId eventId, Exception? exception, string? message, params object?[] args)
    {
        terminal.Log(LogLevel.Debug, eventId, exception, message, args);
        return terminal;
    }

    public static IVirtualTerminal Error(this IVirtualTerminal terminal, string? message, params object?[] args)
    {
        terminal.Log(LogLevel.Error, null, message, args);
        return terminal;
    }

    public static IVirtualTerminal Error(this IVirtualTerminal terminal, Exception? exception, string? message, params object?[] args)
    {
        terminal.Log(LogLevel.Error, exception, message, args);
        return terminal;
    }

    public static IVirtualTerminal Error(this IVirtualTerminal terminal, EventId eventId, string? message, params object?[] args)
    {
        terminal.Log(LogLevel.Error, eventId, null, message, args);
        return terminal;
    }

    public static IVirtualTerminal Error(this IVirtualTerminal terminal, EventId eventId, Exception? exception, string? message, params object?[] args)
    {
        terminal.Log(LogLevel.Error, eventId, exception, message, args);
        return terminal;
    }

    public static IVirtualTerminal Fatal(this IVirtualTerminal terminal, string? message, params object?[] args)
    {
        terminal.Log(LogLevel.Critical, null, message, args);
        return terminal;
    }

    public static IVirtualTerminal Fatal(this IVirtualTerminal terminal, Exception? exception, string? message, params object?[] args)
    {
        terminal.Log(LogLevel.Critical, exception, message, args);
        return terminal;
    }

    public static IVirtualTerminal Fatal(this IVirtualTerminal terminal, EventId eventId, string? message, params object?[] args)
    {
        terminal.Log(LogLevel.Critical, eventId, null, message, args);
        return terminal;
    }

    public static IVirtualTerminal Fatal(this IVirtualTerminal terminal, EventId eventId, Exception? exception, string? message, params object?[] args)
    {
        terminal.Log(LogLevel.Critical, eventId, exception, message, args);
        return terminal;
    }

    public static IVirtualTerminal Info(this IVirtualTerminal terminal, string? message, params object?[] args)
    {
        terminal.Log(LogLevel.Information, null, message, args);
        return terminal;
    }

    public static IVirtualTerminal Info(this IVirtualTerminal terminal, Exception? exception, string? message, params object?[] args)
    {
        terminal.Log(LogLevel.Information, exception, message, args);
        return terminal;
    }

    public static IVirtualTerminal Info(this IVirtualTerminal terminal, EventId eventId, string? message, params object?[] args)
    {
        terminal.Log(LogLevel.Information, eventId, null, message, args);
        return terminal;
    }

    public static IVirtualTerminal Info(this IVirtualTerminal terminal, EventId eventId, Exception? exception, string? message, params object?[] args)
    {
        terminal.Log(LogLevel.Information, eventId, exception, message, args);
        return terminal;
    }

    public static IVirtualTerminal Trace(this IVirtualTerminal terminal, string? message, params object?[] args)
    {
        terminal.Log(LogLevel.Trace, null, message, args);
        return terminal;
    }

    public static IVirtualTerminal Trace(this IVirtualTerminal terminal, Exception? exception, string? message, params object?[] args)
    {
        terminal.Log(LogLevel.Trace, exception, message, args);
        return terminal;
    }

    public static IVirtualTerminal Trace(this IVirtualTerminal terminal, EventId eventId, string? message, params object?[] args)
    {
        terminal.Log(LogLevel.Trace, eventId, null, message, args);
        return terminal;
    }

    public static IVirtualTerminal Trace(this IVirtualTerminal terminal, EventId eventId, Exception? exception, string? message, params object?[] args)
    {
        terminal.Log(LogLevel.Trace, eventId, exception, message, args);
        return terminal;
    }

    public static IVirtualTerminal Warn(this IVirtualTerminal terminal, string? message, params object?[] args)
    {
        terminal.Log(LogLevel.Warning, null, message, args);
        return terminal;
    }

    public static IVirtualTerminal Warn(this IVirtualTerminal terminal, Exception? exception, string? message, params object?[] args)
    {
        terminal.Log(LogLevel.Warning, exception, message, args);
        return terminal;
    }

    public static IVirtualTerminal Warn(this IVirtualTerminal terminal, EventId eventId, string? message, params object?[] args)
    {
        terminal.Log(LogLevel.Warning, eventId, null, message, args);
        return terminal;
    }

    public static IVirtualTerminal Warn(this IVirtualTerminal terminal, EventId eventId, Exception? exception, string? message, params object?[] args)
    {
        terminal.Log(LogLevel.Warning, eventId, exception, message, args);
        return terminal;
    }
}