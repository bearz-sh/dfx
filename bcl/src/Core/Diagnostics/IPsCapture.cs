using System.Diagnostics;

namespace Bearz.Diagnostics;

public interface IPsCapture
{
    void OnStart(Process process);

    void WriteLine(string line);

    void OnExit();
}