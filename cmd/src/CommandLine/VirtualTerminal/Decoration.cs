namespace Bearz.VirtualTerminal;

public readonly struct Decoration
{
    public Decoration(byte value, byte resetValue)
    {
        this.Value = value;
        this.ResetValue = resetValue;
    }

    public static Decoration Reset { get; } = new(0, 0);

    public static Decoration Bold { get; } = new(1, 22);

    public static Decoration Dim { get; } = new(2, 22);

    public static Decoration Italic { get; } = new(3, 23);

    public static Decoration Underline { get; } = new(4, 24);

    public static Decoration SlowBlink { get; } = new(5, 25);

    public static Decoration RapidBlink { get; } = new(6, 25);

    public static Decoration Inverse { get; } = new(7, 27);

    public static Decoration Hidden { get; } = new(8, 28);

    public static Decoration CrossedOut { get; } = new(9, 29);

    public static Decoration Framed { get; } = new(51, 54);

    public static Decoration Encircled { get; } = new(52, 54);

    public static Decoration Overlined { get; } = new(53, 55);

    public byte Value { get; }

    public byte ResetValue { get; }
}