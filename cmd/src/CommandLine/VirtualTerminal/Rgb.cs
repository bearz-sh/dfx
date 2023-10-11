using System.Drawing;
using System.Reflection.Metadata.Ecma335;

namespace Bearz.VirtualTerminal;

public readonly partial struct Rgb : IEquatable<Rgb>
{
    public Rgb()
    {
        this.IsDefault = true;
    }

    public Rgb(byte r, byte g, byte b)
    {
        this.R = r;
        this.G = g;
        this.B = b;
    }

    internal Rgb(int id, byte r, byte g, byte b)
    {
        this.Id = id;
        this.R = r;
        this.G = g;
        this.B = b;
    }

    public int Id { get; } = -1;

    public byte R { get; }

    public byte G { get; }

    public byte B { get; }

    public bool IsDefault { get; }

    public static bool operator ==(Rgb left, Rgb right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Rgb left, Rgb right)
    {
        return !(left == right);
    }

    public bool Equals(Rgb other)
    {
        return this.R == other.R && this.G == other.G && this.B == other.B;
    }

    public override bool Equals(object? obj)
    {
        return obj is Rgb other && this.Equals(other);
    }

    public void Deconstruct(out byte r, out byte g, out byte b)
    {
        r = this.R;
        g = this.G;
        b = this.B;
    }

    public bool Is3Bit()
    {
        if (this.Id > -1 && this.Id < 7)
            return true;

        return (this.R is 0 or 128) && (this.G is 0 or 128) && (this.B is 0 or 128);
    }

    public bool Is4Bit()
    {
        if (this.Id > -1 && this.Id < 16)
            return true;

        if ((this.R is 0 or 128) && (this.G is 0 or 128) && (this.B is 0 or 128))
            return true;

        return (this.R is 192 && this.G is 192 && this.B is 192) ||
                (this.R is 0 or 255 && this.G is 0 or 255 && this.B is 0 or 255);
    }

    public bool Is8Bit()
    {
        if (this.Id > -1 && this.Id < 256)
            return true;

        if ((this.R is 0 or 128) && (this.G is 0 or 128) && (this.B is 0 or 128))
            return true;

        if (this.R is 192 && this.G is 192 && this.B is 192)
            return true;

        var r = new ReadOnlySpan<byte>(
            new byte[]
            {
                8, 18, 28, 38, 48, 58, 68,
                78, 88, 98, 108, 128, 138,
                148, 158, 168, 178, 188,
                198, 208, 218, 228, 238,
            });

        // grayscale
        if (r.IndexOf(this.R) > -1 && this.R == this.G && this.R == this.B)
            return true;

        if (this.R is 0 or 95 or 135 or 175 or 215 or 255
                && this.G is 0 or 95 or 135 or 175 or 215 or 255
                && this.B is 0 or 95 or 135 or 175 or 215 or 255)
        {
            return true;
        }

        return false;
    }

    public Rgb Closest(AnsiColorMode colorMode)
    {
        switch(colorMode)
        {
            case AnsiColorMode.TrueColor:
            case AnsiColorMode.None:
            case AnsiColorMode.Auto:
                return this;

            case AnsiColorMode.EightBit:
                if (this.Is8Bit())
                    return this;
                {
                    return this.ToRgb8Bit();
                }

                 
        }
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(this.R, this.G, this.B);
    }

    public string ToHex()
    {
        return $"#{this.R:X2}{this.G:X2}{this.B:X2}";
    }

    public int ToRgb4()
    {
        return (this.R / 64) << 4 | (this.G / 64) << 2 | (this.B / 64);
    }

    private Rgb ToRgb8Bit()
    {
        var q2c = new int[] { 0x00, 0x5f, 0x87, 0xaf, 0xd7, 0xff };
        static int ColorTo6Cube(int v)
        {
            if (v < 48)
                return 0;

            if (v < 114)
                return 1;

            return (v - 35) / 40;
        }

        static int Distance(int r1, int g1, int b1, int r2, int g2, int b2)
        {
            return ((r1 - r2) * (r1 - r2)) + ((g1 - g2) * (g1 - g2)) + ((b1 - b2) * (b1 - b2));
        }

        // convert true color to 256
        var r = ColorTo6Cube(this.R);
        var g = ColorTo6Cube(this.G);
        var b = ColorTo6Cube(this.B);
        var cr = q2c[r];
        var cg = q2c[g];
        var cb = q2c[b];

        var greyAvg = (this.R + this.G + this.B) / 3;
        int greyIdx = 0;
        if (greyAvg > 238)
            greyIdx = 23;
        else
            greyIdx = (greyAvg - 3) / 10;

        var grey = 8 + (10 * greyIdx);
        var d = Distance(cr, cg, cb, this.R, this.G, this.B);
        if (Distance(grey, grey, grey, this.R, this.G, this.B) < d)
            return new Rgb((byte)grey, (byte)grey, (byte)grey);
        else
            return new Rgb((byte)cr, (byte)cg, (byte)cb);
    }
}