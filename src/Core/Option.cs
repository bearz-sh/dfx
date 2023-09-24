using System.Diagnostics.CodeAnalysis;

namespace Bearz;

public struct Option<TValue> : IEquatable<Option<TValue>>
{
    private readonly TValue? value;

    private readonly bool isNone;

    public Option()
        : this(false)
    {
    }

    internal Option(bool isNone)
    {
        if (isNone)
        {
            this.isNone = true;
            return;
        }

        this.value = default;
        this.isNone = this.value is null or ValueTuple or DBNull;
    }

    internal Option(TValue value)
    {
        this.value = value;
        this.isNone = value is null or ValueTuple or DBNull;
    }

    public bool IsSome => this.value is not null;

    public bool IsNone => this.value is null;

    public static implicit operator Option<TValue>(TValue value)
        => new Option<TValue>(value);

    public static implicit operator TValue(Option<TValue> option)
    {
        return option.value!;
    }

    public static Option<TValue> Some(TValue value)
    {
        return new Option<TValue>(value);
    }

    public static Option<TValue> None()
    {
        return new Option<TValue>(true);
    }

    public bool Equals(TValue other)
    {
        if (ReferenceEquals(this.value, other))
            return true;

        return object.Equals(this.value, other);
    }

    public bool Equals(Option<TValue> other)
    {
        if (this.value is null)
            return other.isNone;

        if (this.isNone)
            return other.isNone;

        return this.value!.Equals(other.value);
    }

    // override object.Equals
    public override bool Equals(object? obj)
    {
        if (obj is Option<TValue> other)
            return this.Equals(other);

        if (this.isNone)
        {
            return obj is Option<ValueTuple> or ValueTuple or DBNull;
        }

        return false;
    }

    // override object.GetHashCode
    public override int GetHashCode()
    {
        return HashCode.Combine(this.isNone, this.value);
    }

    public TValue Expect(string message)
    {
        if (this.value is null)
        {
            throw new InvalidOperationException(message);
        }

        return this.value;
    }

    public TValue Expect(string message, Exception inner)
    {
        if (this.value is null)
        {
            throw new InvalidOperationException(message, inner);
        }

        return this.value;
    }

    public TValue Expect(string message, Func<Exception> inner)
    {
        if (this.value is null)
        {
            throw new InvalidOperationException(message, inner());
        }

        return this.value;
    }

    public Option<TValue> Filter(Func<TValue, bool> predicate)
    {
        if (this.IsNone || !predicate(this.value!))
            return new Option<TValue>();

        return this;
    }

    public Option<TOther> Map<TOther>(Func<TValue, TOther> map)
    {
        if (this.IsNone)
            return new Option<TOther>();

        return new Option<TOther>(map(this.value!));
    }

    public Option<TOther> MapOr<TOther>(TOther defaultValue, Func<TValue, TOther> map)
    {
        if (this.IsNone)
            return new Option<TOther>(defaultValue);

        return new Option<TOther>(map(this.value!));
    }

    public Option<TOther> MapOrElse<TOther>(Func<TOther> defaultValue, Func<TValue, TOther> map)
    {
        if (this.IsNone)
            return new Option<TOther>(defaultValue());

        return new Option<TOther>(map(this.value!));
    }

    public Option<TValue> Or(TValue other)
    {
        if (this.IsNone)
            return new Option<TValue>(other);

        return this;
    }

    public Option<TValue> OrElse(Func<TValue> factory)
    {
        if (this.IsNone)
            return new Option<TValue>(factory());

        return this;
    }

    public TValue Unwrap()
    {
        return this.value!;
    }

    public TValue UnwrapOr(TValue defaultValue)
    {
        return this.value ?? defaultValue;
    }

    public TValue UnwrapOrElse(Func<TValue> defaultValue)
    {
        return this.value ?? defaultValue();
    }

    public Option<(TValue, TOther)> Zip<TOther>(Option<TOther> other)
    {
        if (this.IsNone || other.IsNone)
            return new Option<(TValue, TOther)>();

        return new Option<(TValue, TOther)>((this.value!, other.value!));
    }
}

public static class Option
{
    private static readonly Option<ValueTuple> none = new Option<ValueTuple>();

    public static Option<ValueTuple> None()
      => none;

    public static Option<T> None<T>()
        => new(true);

    public static Option<TValue> Some<TValue>(TValue value)
        => new(value);
}