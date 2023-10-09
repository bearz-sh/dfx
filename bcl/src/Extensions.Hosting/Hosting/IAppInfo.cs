using System;
using System.Collections.Generic;
using System.Text;

namespace Bearz.Extensions.Hosting;

public interface IAppInfo : IEnumerable<KeyValuePair<string, object?>>
{
    string Name { get; }

    string Version { get; }

    string Id { get; }

    string InstanceName { get; }

    string EnvironmentName { get; }

    object? this[string key] { get; set; }

    bool IsEnvironment(string environment);
}