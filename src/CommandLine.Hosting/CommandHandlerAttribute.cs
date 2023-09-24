using System.CommandLine;

namespace Bearz.CommandLine;

[AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
public sealed class CommandHandlerAttribute : Attribute
{
    public CommandHandlerAttribute([Dam(Dat.PublicConstructors | Dat.PublicMethods)] Type commandHandlerType)
    {
        this.CommandHandlerType = commandHandlerType;
    }

    [Dam(Dat.PublicConstructors | Dat.PublicMethods)]
    public Type CommandHandlerType { get; }
}