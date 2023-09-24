using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;

using Bearz.Diagnostics;

using PsResult = Bearz.Result<Bearz.Diagnostics.PsOutput, System.Exception>;

namespace Bearz;

public sealed partial class Ps
{
    private static Process? s_process;

    private readonly List<IDisposable> disposables = new();

    public Ps(string fileName)
    {
        this.FileName = fileName;
        this.StartInfo = new PsStartInfo();
    }

    public Ps(string fileName, PsStartInfo startInfo)
    {
        this.FileName = fileName;
        this.StartInfo = startInfo;
    }

    public static int Id => Current.Id;

    public static IReadOnlyList<string> Argv => Environment.GetCommandLineArgs();

    public static string CommandLine => Environment.CommandLine;

    public static Process Current
    {
        get
        {
            return s_process ??= Process.GetCurrentProcess();
        }
    }

    public static int ExitCode
    {
        get => Environment.ExitCode;
        set => Environment.ExitCode = value;
    }

    public string FileName { get; private set; }

    public PsStartInfo StartInfo { get; }

    public static void Kill(int pid)
    {
        Process.GetProcessById(pid).Kill();
    }

    public static void Exit(int code)
    {
        Environment.Exit(code);
    }

    public static Ps New(string fileName)
    {
        return new Ps(fileName);
    }

    public static Ps New(string fileName, PsStartInfo startInfo)
    {
        return new Ps(fileName, startInfo);
    }

    public Ps WithExecutable(string fileName)
    {
        this.FileName = fileName;
        return this;
    }

    public Ps WithArgs(PsArgs args)
    {
        this.StartInfo.Args = args;
        return this;
    }

    public Ps WithCwd(string cwd)
    {
        this.StartInfo.Cwd = cwd;
        return this;
    }

    public Ps WithEnv(IDictionary<string, string?> env)
    {
        this.StartInfo.Env = env;
        return this;
    }

    public Ps SetEnv(string name, string value)
    {
        this.StartInfo.Env ??= new Dictionary<string, string?>();
        this.StartInfo.Env[name] = value;
        return this;
    }

    public Ps SetEnv(IEnumerable<KeyValuePair<string, string?>> values)
    {
        this.StartInfo.Env ??= new Dictionary<string, string?>();
        foreach (var kvp in values)
        {
            this.StartInfo.Env[kvp.Key] = kvp.Value;
        }

        return this;
    }

    public Ps WithDisposable(IDisposable disposable)
    {
        this.disposables.Add(disposable);
        return this;
    }

    public Ps WithDisposable(Action action)
    {
        this.disposables.Add(new DisposeAction(action));
        return this;
    }

    public Ps WithStdOut(Stdio stdio)
    {
        this.StartInfo.StdOut = stdio;
        return this;
    }

    public Ps WithStdErr(Stdio stdio)
    {
        this.StartInfo.StdErr = stdio;
        return this;
    }

    public Ps WithStdIn(Stdio stdio)
    {
        this.StartInfo.StdIn = stdio;
        return this;
    }

    public Ps WithStdio(Stdio stdio)
    {
        this.StartInfo.StdOut = stdio;
        this.StartInfo.StdErr = stdio;
        this.StartInfo.StdIn = stdio;
        return this;
    }

    public Ps WithVerb(string verb)
    {
        this.StartInfo.Verb = verb;
        return this;
    }

    public Ps AsWindowsAdmin()
    {
        this.StartInfo.Verb = "runas";
        return this;
    }

    public Ps AsSudo()
    {
        this.StartInfo.Verb = "sudo";
        return this;
    }

    [SupportedOSPlatform("windows")]
    public Ps WithUser(string user)
    {
        this.StartInfo.User = user;
        return this;
    }

    [SupportedOSPlatform("windows")]
    public Ps WithPassword(string password)
    {
        this.StartInfo.PasswordInClearText = password;
        return this;
    }

    [SupportedOSPlatform("windows")]
    public Ps WithDomain(string domain)
    {
        this.StartInfo.Domain = domain;
        return this;
    }

    public Ps AddCapture(ICollection<string> lines)
    {
        this.StartInfo.Capture(lines);
        return this;
    }

    public Ps AddCapture(TextWriter writer, bool dispose = false)
    {
        this.StartInfo.Capture(writer, dispose);
        return this;
    }

    public Ps AddCapture(Stream stream, Encoding? encoding = null, int bufferSize = -1, bool leaveOpen = false)
    {
        this.StartInfo.Capture(stream, encoding, bufferSize, leaveOpen);
        return this;
    }

    public Ps AddCapture(FileInfo file, Encoding? encoding = null, int bufferSize = -1)
    {
        this.StartInfo.Capture(file, encoding, bufferSize);
        return this;
    }

    public Ps AddErrorCapture(ICollection<string> lines)
    {
        this.StartInfo.CaptureError(lines);
        return this;
    }

    public Ps AddErrorCapture(TextWriter writer, bool dispose = false)
    {
        this.StartInfo.CaptureError(writer, dispose);
        return this;
    }

    public Ps AddErrorCapture(Stream stream, Encoding? encoding = null, int bufferSize = -1, bool leaveOpen = false)
    {
        this.StartInfo.CaptureError(stream, encoding, bufferSize, leaveOpen);
        return this;
    }

    public Ps AddErrorCapture(FileInfo file, Encoding? encoding = null, int bufferSize = -1)
    {
        this.StartInfo.CaptureError(file, encoding, bufferSize);
        return this;
    }

    public PsChild Spawn()
    {
        return new PsChild(this.FileName, this.StartInfo);
    }

    public Result<PsOutput, Error> Output()
    {
        var stdOut = new List<string>();
        var stdError = new List<string>();
        this.StartInfo.Capture(stdOut);
        this.StartInfo.Capture(stdError);

        using var child = new PsChild(this.FileName, this.StartInfo);
        var ec = child.Wait();
        if (ec.IsError)
            return ec.UnwrapError();

        return new PsOutput(this.FileName, ec.Unwrap(), stdOut, stdError, child.StartTime, child.ExitTime);
    }

    public async Task<Result<PsOutput, Error>> OutputAsync(CancellationToken cancellationToken = default)
    {
        var stdOut = new List<string>();
        var stdError = new List<string>();
        this.StartInfo.Capture(stdOut);
        this.StartInfo.Capture(stdError);

        using var child = new PsChild(this.FileName, this.StartInfo);
        var ec = await child.WaitAsync(cancellationToken).ConfigureAwait(false);
        if (ec.IsError)
            return ec.UnwrapError();

        return new PsOutput(this.FileName, ec.Unwrap(), stdOut, stdError, child.StartTime, child.ExitTime);
    }
}