using System.Security;

using Microsoft.Extensions.Logging;

namespace Bearz.CommandLine;

public class DefaultVtHost
{
    private LogLevel level = LogLevel.Information;

    public bool IsInteractive { get; set; }

    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel >= this.level;
    }

    public void SetLogLevel(LogLevel logLevel)
    {
        this.level = logLevel;
    }

    public string? ReadLine()
    {
        return Console.ReadLine();
    }

    public SecureString ReadLineAsSecureString()
    {
        var password = new SecureString();
        while (true)
        {
            var key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.Enter)
                break;

            if (key.Key == ConsoleKey.Backspace)
            {
                if (password.Length > 0)
                {
                    password.RemoveAt(password.Length - 1);
                    this.Write("\b \b");
                }
            }
            else
            {
                password.AppendChar(key.KeyChar);
                this.Write("*");
            }
        }

        this.WriteLine();
        return password;
    }

    public ReadOnlySpan<char> ReadLineAsCharSpan()
    {
        var password = new Span<char>(new char[1024]);
        var position = 0;
        while (true)
        {
            var key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.Enter)
                break;

            if (key.Key == ConsoleKey.Backspace)
            {
                if (password.Length > 0)
                {
                    password[position] = default;
                    position--;
                    this.Write("\b \b");
                }
            }
            else
            {
                position++;
                password[position] = key.KeyChar;
                this.Write("*");
            }
        }

        this.WriteLine();
        return password.Slice(0, position);
    }

    public bool Confirm(string question)
    {
        this.Write(question);
        this.Write(" [y/N]: ");
        var key = Console.ReadKey(true);
        this.WriteLine();
        return key.Key == ConsoleKey.Y;
    }

    public string Prompt(string question)
    {
        this.Write(question);
        this.Write(": ");
        while (true)
        {
            var line = Console.ReadLine();
            if (line is not null)
                return line;
        }
    }

    public SecureString PromptForPassword(string question)
    {
        this.Write(question);
        this.Write(": ");
        var password = new SecureString();
        while (true)
        {
            var key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.Enter)
                break;

            if (key.Key == ConsoleKey.Backspace)
            {
                if (password.Length > 0)
                {
                    password.RemoveAt(password.Length - 1);
                    this.Write("\b \b");
                }
            }
            else
            {
                password.AppendChar(key.KeyChar);
                this.Write("*");
            }
        }

        this.WriteLine();
        return password;
    }

    public int PromptForChoice(string[] choices)
    {
        return this.PromptForChoice(null, null, choices, null);
    }

    public int PromptForChoice(string? title, string? prompt, string[] choices, string? invalidMessage, int defaultChoice = 0)
    {
        if (choices is null)
            throw new ArgumentNullException(nameof(choices));

        if (choices.Length <= 0)
            throw new ArgumentException($"{choices} is empty", nameof(choices));

        title ??= "Menu...";
        prompt ??= $"Choose an option (1-{choices.Length}): ";
        invalidMessage ??= "Invalid Input. Try Again...";

        // render menu
        Console.WriteLine(title);
        for (int i = 0; i < choices.Length; i++)
        {
            Console.WriteLine($"{i + 1}. {choices[i]}");
        }

        // get user input
        int inputValue;
        Console.Write(prompt);
        while (!int.TryParse(Console.ReadLine(), out inputValue) || inputValue < 1 || choices.Length < inputValue)
        {
            Console.WriteLine(invalidMessage);
            Console.Write(prompt);
        }

        // invoke the action relative to the user input
        return inputValue - 1;
    }

    public void Write(string? message)
    {
        Console.Write(message);
    }

    public void Write(ReadOnlySpan<char> message)
    {
#if NETLEGACY
        var tmp = message.ToArray();
        Console.Write(tmp, 0, tmp.Length);
#else
        Console.Write(message.ToString());
#endif
    }

    public void WriteLine()
    {
        Console.WriteLine();
    }

    public void WriteLine(string? value)
    {
        Console.WriteLine(value);
    }

    public void WriteLine(string value, params object?[] args)
    {
        Console.WriteLine(value, args);
    }

    public void WriteCommand(string command, string[] args)
    {
        var defaultColor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write("$ ");
        Console.Write(command);
        Console.Write(' ');
        Console.WriteLine(string.Join(' ', args));
        Console.ForegroundColor = defaultColor;
    }

    public void Debug(string? message, params object?[] args)
    {
        if (message is null)
            return;

        this.WriteLevel(LogLevel.Debug, null, message, args);
    }

    public void Debug(Exception exception, string? message, params object?[] args)
    {
        if (message is null)
            return;

        this.WriteLevel(LogLevel.Debug, exception, message, args);
    }

    public void Error(string? message, params object?[] args)
    {
        if (message is null)
            return;

        this.WriteLevel(LogLevel.Error, null, message, args);
    }

    public void Error(Exception exception, string? message, params object?[] args)
    {
        if (message is null)
            return;

        this.WriteLevel(LogLevel.Error, exception, message, args);
    }

    public void Info(string? message, params object?[] args)
    {
        if (message is null)
            return;

        this.WriteLevel(LogLevel.Information, null, message, args);
    }

    public void Info(Exception exception, string? message, params object?[] args)
    {
        if (message is null)
            return;

        this.WriteLevel(LogLevel.Information, exception, message, args);
    }

    public void Warn(string? message, params object?[] args)
    {
        if (message is null)
            return;

        this.WriteLevel(LogLevel.Warning, null, message, args);
    }

    public void Warn(Exception exception, string? message, params object?[] args)
    {
        if (message is null)
            return;

        this.WriteLevel(LogLevel.Warning, exception, message, args);
    }

    public void Trace(string? message, params object?[] args)
    {
        if (message is null)
            return;

        this.WriteLevel(LogLevel.Trace, null, message, args);
    }

    public void Trace(Exception exception, string? message, params object?[] args)
    {
        if (message is null)
            return;

        this.WriteLevel(LogLevel.Trace, exception, message, args);
    }

    private void WriteLevel(LogLevel level, Exception? exeception, string? message, params object?[] args)
    {
        if (this.level > level)
            return;

        bool useStdError = level >= LogLevel.Error;

        var defaultColor = Console.ForegroundColor;

        switch (level)
        {
            case LogLevel.Trace:
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("[TRC]: ");
                break;
            case LogLevel.Debug:
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("[DBG]: ");
                break;
            case LogLevel.Information:
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("[INF]: ");
                break;
            case LogLevel.Warning:
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("[WRN]: ");
                break;
            case LogLevel.Error:
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Error.Write("[ERR]: ");
                break;
            case LogLevel.Critical:
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Error.Write("[CRT]: ");
                break;
            default:
                Console.ForegroundColor = ConsoleColor.White;
                break;
        }

        if (message is not null)
        {
            if (useStdError)
                Console.Error.WriteLine(message, args);
            else
                Console.WriteLine(message, args);
        }

        if (exeception is not null)
        {
            if (useStdError)
            {
                if (message is not null)
                    Console.Error.Write('\t');

                Console.Error.WriteLine(exeception.Message);
                var lines = exeception.StackTrace?.Split('\n') ?? Array.Empty<string>();
                foreach (var line in lines)
                {
                    Console.Error.Write('\t');
                    Console.Error.WriteLine(line);
                }
            }
            else
            {
                if (message is not null)
                    Console.Write('\t');

                Console.WriteLine(exeception.Message);
                var lines = exeception.StackTrace?.Split('\n') ?? Array.Empty<string>();
                foreach (var line in lines)
                {
                    Console.Write('\t');
                    Console.WriteLine(line);
                }
            }
        }

        Console.ForegroundColor = defaultColor;
    }
}