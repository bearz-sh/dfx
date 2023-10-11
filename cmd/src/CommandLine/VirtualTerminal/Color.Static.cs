using System.Globalization;

namespace Bearz.VirtualTerminal;

public readonly partial struct Rgb
{
    /// <summary>
    /// Gets the default color which will be evaluated as
    /// <c>[39m</c> or <c>[49m</c>depending on the context.
    /// </summary>
    public static Rgb Default { get; } = new Rgb();

    /// <summary>
    /// Gets the color <c>Black</c> which is <see cref="ConsoleColor.Black"/>.
    /// RGB(0,0,0).
    /// </summary>
    public static Rgb Black { get; } = new Rgb(0, 0, 0, 0);

    /// <summary>
    /// Gets the color <c>Maroon</c> which is <see cref="ConsoleColor.DarkRed"/>.
    /// RGB(128,0,0).
    /// </summary>
    public static Rgb Maroon { get; } = new Rgb(1, 128, 0, 0);

    /// <summary>
    /// Gets the color <c>Green</c> which is <see cref="ConsoleColor.DarkGreen"/>.
    /// RGB(0,128,0).
    /// </summary>
    public static Rgb Green { get; } = new Rgb(2, 0, 128, 0);

    /// <summary>
    /// Gets the color <c>Olive</c> which is <see cref="ConsoleColor.DarkYellow"/>.
    /// RGB(128,128,0).
    /// </summary>
    public static Rgb Olive { get; } = new Rgb(3, 128, 128, 0);

    /// <summary>
    /// Gets the color <c>Navy</c> which is <see cref="ConsoleColor.DarkBlue"/>.
    /// RGB(0,0,128).
    /// </summary>
    public static Rgb Navy { get; } = new Rgb(4, 0, 0, 128);

    /// <summary>
    /// Gets the color <c>Purple</c> which is <see cref="ConsoleColor.DarkMagenta"/>.
    /// RGB(128,0,128).
    /// </summary>
    public static Rgb Purple { get; } = new Rgb(5, 128, 0, 128);

    /// <summary>
    /// Gets the color <c>Teal</c> which is which is <see cref="ConsoleColor.DarkCyan"/>.
    /// RGB(0,128,128).
    /// </summary>
    public static Rgb Teal { get; } = new Rgb(6, 0, 128, 128);

    /// <summary>
    /// Gets the color <c>Silver</c> which is <see cref="ConsoleColor.Gray"/>.
    /// RGB(192,192,192).
    /// </summary>
    public static Rgb Silver { get; } = new Rgb(7, 192, 192, 192);

    /// <summary>
    /// Gets the color <c>Grey</c> which is <see cref="ConsoleColor.DarkGray"/>.
    /// RGB(128,128,128).
    /// </summary>
    public static Rgb Grey { get; } = new Rgb(8, 128, 128, 128);

    /// <summary>
    /// Gets the color <c>Red</c> which is <see cref="ConsoleColor.Red"/>.
    /// RGB(255,0,0).
    /// </summary>
    public static Rgb Red { get; } = new Rgb(9, 255, 0, 0);

    /// <summary>
    /// Gets the color <c>Lime</c> which is <see cref="ConsoleColor.Green"/>.
    /// RGB(0,255,0).
    /// </summary>
    public static Rgb Lime { get; } = new Rgb(10, 0, 255, 0);

    /// <summary>
    /// Gets the color <c>Yellow</c> which is <see cref="ConsoleColor.Yellow"/>.
    /// RGB(255,255,0).
    /// </summary>
    public static Rgb Yellow { get; } = new Rgb(11, 255, 255, 0);

    /// <summary>
    /// Gets the color <c>Blue</c> which is <see cref="ConsoleColor.Blue"/>.
    /// RGB(0,0,255).
    /// </summary>
    public static Rgb Blue { get; } = new Rgb(12, 0, 0, 255);

    /// <summary>
    /// Gets the color <c>Fuchsia</c> which is <see cref="ConsoleColor.Magenta"/>.
    /// RGB (255,0,255).
    /// </summary>
    public static Rgb Fuchsia { get; } = new Rgb(13, 255, 0, 255);

    /// <summary>
    /// Gets the color <c>Aqua</c> which is <see cref="ConsoleColor.Cyan"/>.
    /// RGB (0,255,255).
    /// </summary>
    public static Rgb Aqua { get; } = new Rgb(14, 0, 255, 255);

    /// <summary>
    /// Gets the color <c>White</c> which is <see cref="ConsoleColor.White"/>.
    /// RGB (255,255,255).
    /// </summary>
    public static Rgb White { get; } = new Rgb(15, 255, 255, 255);

#if !NETLEGACY
    public static Rgb FromHex(ReadOnlySpan<char> hex)
    {
        switch (hex.Length)
        {
            case 7:
                {
                    if (hex[0] == '#')
                    {
                        return new Rgb(
                            byte.Parse(hex.Slice(1, 2), NumberStyles.HexNumber),
                            byte.Parse(hex.Slice(3, 2), NumberStyles.HexNumber),
                            byte.Parse(hex.Slice(5, 2), NumberStyles.HexNumber));
                    }
                    else
                    {
                        throw new ArgumentException($"Invalid hex string {hex}", nameof(hex));
                    }
                }

            case 6:
                {
                    return new Rgb(
                        byte.Parse(hex.Slice(0, 2), NumberStyles.HexNumber),
                        byte.Parse(hex.Slice(2, 2), NumberStyles.HexNumber),
                        byte.Parse(hex.Slice(4, 2), NumberStyles.HexNumber));
                }

            case 4:
                {
                    if (hex[0] == '#')
                    {
                        return new Rgb(
                            byte.Parse(hex.Slice(1, 1), NumberStyles.HexNumber),
                            byte.Parse(hex.Slice(2, 1), NumberStyles.HexNumber),
                            byte.Parse(hex.Slice(3, 1), NumberStyles.HexNumber));
                    }
                    else
                    {
                        throw new ArgumentException($"Invalid hex string {hex}", nameof(hex));
                    }
                }

            case 3:
                {
                    return new Rgb(
                        byte.Parse(hex.Slice(0, 1), NumberStyles.HexNumber),
                        byte.Parse(hex.Slice(1, 1), NumberStyles.HexNumber),
                        byte.Parse(hex.Slice(2, 1), NumberStyles.HexNumber));
                }

            default:
                {
                    throw new ArgumentException($"Invalid hex string {hex}", nameof(hex));
                }
        }
    }
#endif

    public static Rgb FromHex(string hex)
    {
        switch (hex.Length)
        {
            case 7:
                {
                    if (hex[0] == '#')
                    {
                        return new Rgb(
                            byte.Parse(hex.Substring(1, 2), NumberStyles.HexNumber),
                            byte.Parse(hex.Substring(3, 2), NumberStyles.HexNumber),
                            byte.Parse(hex.Substring(5, 2), NumberStyles.HexNumber));
                    }
                    else
                    {
                        throw new ArgumentException($"Invalid hex string {hex}", nameof(hex));
                    }
                }

            case 6:
                {
                    return new Rgb(
                        byte.Parse(hex.Substring(0, 2), NumberStyles.HexNumber),
                        byte.Parse(hex.Substring(2, 2), NumberStyles.HexNumber),
                        byte.Parse(hex.Substring(4, 2), NumberStyles.HexNumber));
                }

            case 4:
                {
                    if (hex[0] == '#')
                    {
                        return new Rgb(
                            byte.Parse(hex.Substring(1, 1), NumberStyles.HexNumber),
                            byte.Parse(hex.Substring(2, 1), NumberStyles.HexNumber),
                            byte.Parse(hex.Substring(3, 1), NumberStyles.HexNumber));
                    }
                    else
                    {
                        throw new ArgumentException($"Invalid hex string {hex}", nameof(hex));
                    }
                }

            case 3:
                {
                    return new Rgb(
                        byte.Parse(hex.Substring(0, 1), NumberStyles.HexNumber),
                        byte.Parse(hex.Substring(1, 1), NumberStyles.HexNumber),
                        byte.Parse(hex.Substring(2, 1), NumberStyles.HexNumber));
                }

            default:
                {
                    throw new ArgumentException($"Invalid hex string {hex}", nameof(hex));
                }
        }
    }

    public static Rgb FromConsoleColor(ConsoleColor consoleColor)
    {
        return consoleColor switch
        {
            ConsoleColor.Black => Black,
            ConsoleColor.DarkRed => Maroon,
            ConsoleColor.DarkGreen => Green,
            ConsoleColor.DarkYellow => Olive,
            ConsoleColor.DarkBlue => Navy,
            ConsoleColor.DarkMagenta => Purple,
            ConsoleColor.DarkCyan => Teal,
            ConsoleColor.Gray => Silver,
            ConsoleColor.DarkGray => Grey,
            ConsoleColor.Red => Red,
            ConsoleColor.Green => Green,
            ConsoleColor.Yellow => Yellow,
            ConsoleColor.Blue => Blue,
            ConsoleColor.Magenta => Fuchsia,
            ConsoleColor.Cyan => Aqua,
            ConsoleColor.White => White,
            _ => throw new ArgumentOutOfRangeException(nameof(consoleColor), consoleColor, null),
        };
    }

    public static int ToInt(Rgb rgb) => rgb.R << 16 | rgb.G << 8 | rgb.B;

    public static Rgb FromInt(int rgb) => new((byte)(rgb >> 16), (byte)(rgb >> 8), (byte)rgb);

    public static Rgb FromRgb(byte r, byte g, byte b) => new(r, g, b);

    public static Rgb FromRgb(int r, int g, int b) => new((byte)r, (byte)g, (byte)b);

    public static Rgb FromRgb(float r, float g, float b) => new((byte)(r * 255), (byte)(g * 255), (byte)(b * 255));

    public static Rgb FromRgb(double r, double g, double b) => new((byte)(r * 255), (byte)(g * 255), (byte)(b * 255));
}