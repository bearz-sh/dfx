using System;
using System.Linq;

namespace Bearz.Extensions.Hosting.CommandLine;

internal class HandlerTypeHolder
{
    public HandlerTypeHolder([Dam(Dat.PublicConstructors | Dat.PublicMethods)] Type handlerType)
    {
        this.HandlerType = handlerType;
    }

    [Dam(Dat.PublicConstructors | Dat.PublicMethods)]
    public Type HandlerType { get; }
}