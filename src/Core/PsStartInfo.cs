using System.Runtime.Versioning;
using System.Security;
using System.Text;

using Bearz.Diagnostics;

namespace Bearz;

public class PsStartInfo
{
    private List<IPsCapture>? stdOutCaptures;

    private List<IPsCapture>? stdErrorCaptures;

    public PsArgs Args { get; set; } = new PsArgs();

    public string? Cwd { get; set; }

    public IDictionary<string, string?>? Env { get; set; }

    public Stdio StdOut { get; set; }

    public Stdio StdErr { get; set; }

    public Stdio StdIn { get; set; }

    public string? User { get; set; }

    public string? Verb { get; set; }

    [SupportedOSPlatform("windows")]
    [CLSCompliant(false)]
    public SecureString? Password { get; set; }

    [SupportedOSPlatform("windows")]
    public string? PasswordInClearText { get; set; }

    [SupportedOSPlatform("windows")]
    public string? Domain { get; set; }

    public bool LoadUserProfile { get; set; } = false;

    public bool CreateNoWindow { get; set; } = true;

    public bool UseShellExecute { get; set; } = false;

    protected internal IReadOnlyList<IPsCapture> StdOutCaptures
    {
        get
        {
            if (this.stdOutCaptures is null)
                return Array.Empty<IPsCapture>();

            return this.stdOutCaptures;
        }
    }

    protected internal IReadOnlyList<IPsCapture> StdErrorCaptures
    {
        get
        {
            if (this.stdErrorCaptures is null)
                return Array.Empty<IPsCapture>();

            return this.stdErrorCaptures;
        }
    }

    public PsStartInfo Capture(ICollection<string> collection)
    {
        return this.Capture(new PsCollectionCapture(collection));
    }

    public PsStartInfo Capture(TextWriter writer, bool dispose = false)
    {
        return this.Capture(new PsTextWriterCapture(writer, dispose));
    }

    public PsStartInfo Capture(Stream stream, Encoding? encoding = null, int bufferSize = -1, bool leaveOpen = false)
    {
        return this.Capture(new PsTextWriterCapture(stream, encoding, bufferSize, leaveOpen));
    }

    public PsStartInfo Capture(FileInfo file, Encoding? encoding = null, int bufferSize = -1)
    {
        return this.Capture(new PsTextWriterCapture(file, encoding, bufferSize));
    }

    public PsStartInfo Capture(IPsCapture capture)
    {
        this.StdOut = Stdio.Piped;
        this.stdOutCaptures ??= new List<IPsCapture>();
        this.stdOutCaptures.Add(capture);
        return this;
    }

    public PsStartInfo CaptureError(TextWriter writer, bool dispose = false)
    {
        return this.CaptureError(new PsTextWriterCapture(writer, dispose));
    }

    public PsStartInfo CaptureError(Stream stream, Encoding? encoding = null, int bufferSize = -1, bool leaveOpen = false)
    {
        return this.CaptureError(new PsTextWriterCapture(stream, encoding, bufferSize, leaveOpen));
    }

    public PsStartInfo CaptureError(FileInfo file, Encoding? encoding = null, int bufferSize = -1)
    {
        return this.CaptureError(new PsTextWriterCapture(file, encoding, bufferSize));
    }

    public PsStartInfo CaptureError(ICollection<string> collection)
    {
        return this.CaptureError(new PsCollectionCapture(collection));
    }

    public PsStartInfo CaptureError(IPsCapture capture)
    {
        this.StdErr = Stdio.Piped;
        this.stdErrorCaptures ??= new List<IPsCapture>();
        this.stdErrorCaptures.Add(capture);
        return this;
    }
}