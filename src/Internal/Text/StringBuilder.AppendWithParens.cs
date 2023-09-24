using System.Buffers;
using System.IO;
using System.Text;

namespace Bearz.Text;

#if DFX_CORE
public
#else
internal
#endif
   static partial class StringBuilderExtensions
{
    public static StringBuilder AppendWithParens(this StringBuilder stringBuilder, string value)
    {
        return stringBuilder
            .Append('(')
            .Append(value)
            .Append(')');
    }
}