#!/usr/bin/env dotnet-script
using System;
using System.Buffers.Binary;
using System.Numerics;
using System.Linq;

Console.WriteLine(0xFFFFFFF);

public class Test
{
    public static ReadOnlySpan<byte> Aries => "😁"u8;

    public static void Write()
    {
        Console.OpenStandardOutput().Write(Test.Aries);
        Console.WriteLine();
        var data = new Span<byte>(new byte[4]);
        BinaryPrimitives.WriteInt32BigEndian(data, -257976191);
        Console.OpenStandardOutput().Write(data);
        Console.WriteLine();
        Console.WriteLine("test");
        Console.WriteLine();
    }

    public static void W(FormattableString s)
    {
        Console.WriteLine(s.ToString());
    }

    public static void W2()
    {
        W($"");
    }
}

Test.Write();


