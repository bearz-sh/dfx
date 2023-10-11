namespace Bearz.ColorSpaces;

public interface IArgb : IEquatable<IArgb>
{
    byte A { get; }

    byte R { get; }

    byte G { get; }

    byte B { get; }

    void Deconstruct(out byte a, out byte r, out byte g, out byte b);
}