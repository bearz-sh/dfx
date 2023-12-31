using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

using Bearz.Collections.Generic;
using Bearz.Diagnostics;
using Bearz.Extra.Object;
using Bearz.Extra.Strings;
using Bearz.Text.Serialization;

namespace Bearz;

public partial class PsArgs
{
    [SuppressMessage("ReSharper", "SuspiciousTypeConversion.Global")]
    public static PsArgs Splat(Splattable obj)
    {
        if (obj is IPsArgsBuilder builder)
            return builder.BuildPsArgs();

        var splat = new PsArgs();
        var options = obj.BuildSplatOptions();
        var extraArgs = new PsArgs();
        var separateArgs = new PsArgs();
        var orderedArgs = new PsArgs();

        if (options.Command.Count > 0)
            splat.AddRange(options.Command);

        if (options.Arguments.Count > 0)
            orderedArgs = new PsArgs(new string[options.Arguments.Count]);

        var assign = options.Assignment != " ";
        foreach (var prop in obj.GetType().GetProperties())
        {
            var name = prop.Name;
            var value = prop.GetValue(obj);

            if (name == options.ExtraArgumentsName)
            {
                if (value is IEnumerable enumerable)
                {
                    foreach (var n in enumerable)
                    {
                        extraArgs.Add(n.ToSafeString());
                    }
                }
                else
                {
                    extraArgs.Add(value.ToSafeString());
                }
            }

            if (name == options.SeparateArgumentsName)
            {
                if (value is IEnumerable enumerable)
                {
                    foreach (var n in enumerable)
                    {
                        separateArgs.Add(n.ToSafeString());
                    }
                }
                else
                {
                    separateArgs.Add(value.ToSafeString());
                }
            }

            var argIndex = options.Arguments.IndexOf(name, StringComparison.OrdinalIgnoreCase);
            if (argIndex > -1)
            {
                switch (value)
                {
                    case string str:
                        orderedArgs[argIndex] = str;
                        break;

                    case IEnumerable _:
                        {
                            throw new NotSupportedException(
                                "Objects of IEnumerable are not supported for positional arguments");
                        }

                    default:
                        orderedArgs[argIndex] = value.ToSafeString();
                        break;
                }

                orderedArgs[argIndex] = value.ToSafeString();
                continue;
            }

            if (options.Included.Count > 0 && !options.Included.Contains(name, StringComparison.OrdinalIgnoreCase))
                continue;

            if (options.Excluded.Count > 0 && options.Excluded.Contains(name, StringComparison.OrdinalIgnoreCase))
                continue;

            if (options.Aliases.TryGetValue(name, out var alias))
            {
                name = alias;
            }
            else
            {
                var prefix = options!.ShortFlag && name.Length == 1 ? "-" : options.Prefix;
                name = options.PreserveCase ? $"{prefix}{name}" : $"{prefix}{name.Hyphenate()}";
            }

            var attr = prop.GetCustomAttribute<FormatAttribute>();

            switch (value)
            {
                case bool bit:
                    {
                        if (bit)
                        {
                            splat.Add(name);
                        }
                    }

                    break;

                case string str:
                    {
                        if (assign)
                            splat.Add($"{name}{options.Assignment}{str}");
                        else
                            splat.Add(name, str);
                    }

                    break;

                case IEnumerable<string> enumerable:
                    {
                        foreach (var n in enumerable)
                        {
                            if (assign)
                                splat.Add($"{name}{options.Assignment}{n}");
                            else
                                splat.Add(name, n);
                        }
                    }

                    break;

                default:
                    {
                        var formatted = attr != null ? string.Format(attr.Format, value) : value.ToSafeString();

                        if (assign)
                            splat.Add($"{name}{options.Assignment}{formatted}");
                        else
                            splat.Add(name, formatted);
                    }

                    break;
            }
        }

        splat.AddRange(extraArgs);

        if (separateArgs.Count > 0)
        {
            splat.Add(options.SeparateArgumentsPrefix);
            splat.AddRange(separateArgs);
        }

        if (orderedArgs.Count > 0)
        {
            if (options.AppendArguments)
            {
                foreach (var n in orderedArgs)
                {
                    if (string.IsNullOrWhiteSpace(n))
                        continue;

                    splat.Add(n);
                }
            }
            else
            {
                var filtered = orderedArgs.Where(o => !string.IsNullOrWhiteSpace(o));
                splat.InsertRange(0, filtered);
            }
        }

        return splat;
    }
}

public class SplatOptions
{
    public static SplatOptions Default { get; } = new();

    public StringList Command { get; set; } = new();

    public Dictionary<string, string> Aliases { get; set; } = new(StringComparer.OrdinalIgnoreCase);

    public StringList Arguments { get; set; } = new();

    public StringList Excluded { get; set; } = new();

    public StringList Included { get; set; } = new();

    public string Prefix { get; set; } = "--";

    public string Assignment { get; set; } = " ";

    public bool PreserveCase { get; set; }

    public string ExtraArgumentsName { get; set; } = "ExtraArguments";

    public string SeparateArgumentsName { get; set; } = "SeparateArguments";

    public string SeparateArgumentsPrefix { get; set; } = "--";

    public bool ShortFlag { get; set; }

    public bool AppendArguments { get; set; }

    public SplatOptions WithCommand(params string[] command)
    {
        this.Command.AddRange(command);
        return this;
    }

    public SplatOptions WithAliases(IEnumerable<KeyValuePair<string, string>> aliases)
    {
        foreach (var kvp in aliases)
        {
            this.Aliases[kvp.Key] = kvp.Value;
        }

        return this;
    }

    public SplatOptions WithAliases(IEnumerable<(string, string)> aliases)
    {
        foreach (var (key, value) in aliases)
        {
            this.Aliases[key] = value;
        }

        return this;
    }

    public SplatOptions WithArguments(params string[] arguments)
    {
        this.Arguments.AddRange(arguments);
        return this;
    }

    public SplatOptions WithExcluded(params string[] excluded)
    {
        this.Excluded.AddRange(excluded);
        return this;
    }

    public SplatOptions WithIncluded(params string[] included)
    {
        this.Included.AddRange(included);
        return this;
    }

    public SplatOptions WithExtraArgumentsName(string name)
    {
        this.ExtraArgumentsName = name;
        return this;
    }

    public SplatOptions WithSeparateArgumentsName(string name)
    {
        this.SeparateArgumentsName = name;
        return this;
    }

    public SplatOptions WithSeparateArgumentsPrefix(string prefix)
    {
        this.SeparateArgumentsPrefix = prefix;
        return this;
    }

    public SplatOptions WithPrefix(string prefix)
    {
        this.Prefix = prefix;
        return this;
    }

    public SplatOptions WithAssignment(string assignment)
    {
        this.Assignment = assignment;
        return this;
    }

    public SplatOptions WithPreserveCase(bool preserveCase)
    {
        this.PreserveCase = preserveCase;
        return this;
    }

    public SplatOptions WithShortFlag(bool shortFlag)
    {
        this.ShortFlag = shortFlag;
        return this;
    }

    public SplatOptions WithAppendArguments(bool appendArguments)
    {
        this.AppendArguments = appendArguments;
        return this;
    }
}