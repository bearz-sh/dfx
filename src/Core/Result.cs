namespace Bearz;

public readonly struct Result<TValue, TError>
{
    private readonly TValue? value;

    private readonly TError? error;

    private readonly bool hasError;

    public Result()
    {
        this.value = default(TValue);
        this.hasError = this.value != null;
    }

    internal Result(TValue value)
    {
        this.hasError = false;
        this.value = value;
    }

    internal Result(TError error)
    {
        this.hasError = true;
        this.error = error;
    }

    internal Result(TValue? value, TError? error)
    {
        this.value = value;
        this.error = error;
        this.hasError = error is not null;
    }

    public bool IsOk => !this.hasError;

    public bool IsError => this.hasError;

    public static implicit operator TValue(Result<TValue, TError> result)
    {
        return result.Unwrap();
    }

    public static implicit operator Result<TValue, TError>(TValue value)
        => new Result<TValue, TError>(value);

    public static implicit operator Result<TValue, TError>(TError error)
        => new Result<TValue, TError>(error);

    public static Result<TValue, TError> Ok(TValue value)
        => new Result<TValue, TError>(value);

    public static Result<TValue, TError> Err(TError value)
        => new Result<TValue, TError>(value);

    public Result<TValue2, TError2> And<TValue2, TError2>(Result<TValue2, TError2> other)
    {
        if (this.IsOk)
            return other;

        return Result<TValue2, TError2>.Err(other.UnwrapError());
    }

    public void Deconstruct(out TValue? value, out TError? error)
    {
        value = this.value;
        error = this.error;
    }

    public bool IsOkAnd(Func<TValue, bool> predicate)
    {
        if (this.IsOk && predicate(this.value!))
            return true;

        return false;
    }

    public bool IsErrorAnd(Func<TError, bool> predicate)
    {
        if (this.IsError && predicate(this.error!))
            return true;

        return false;
    }

    public Option<TError> Error()
    {
        if (this.hasError)
            return Option.Some(this.error!);

        return Option<TError>.None();
    }

    public Option<TValue> Ok()
    {
        if (this.hasError)
            return Option<TValue>.None();

        return Option.Some(this.value!);
    }

    public TValue Unwrap()
    {
        if (this.hasError)
            throw new InvalidOperationException($"Unwrap is invalid when result has an error: {this.error}.");

        return this.value!;
    }

    public TValue UnwrapOr(TValue defaultValue)
    {
        if (this.hasError)
            return defaultValue;

        return this.value!;
    }

    public TValue UnwrapOrElse(Func<TValue> defaultValue)
    {
        if (this.hasError)
            return defaultValue();

        return this.value!;
    }

    public TError UnwrapError()
    {
        if (!this.hasError)
            throw new InvalidOperationException($"UnwrapError is invalid when result has value: {this.value}.");

        return this.error!;
    }

    public void Throw()
    {
        if (!this.hasError)
            return;

        if (this.error is Exception exception)
            throw exception;

        throw new InvalidOperationException(this.error!.ToString());
    }
}

public static class Result
{
    public static Result<TValue, Error> From<TValue>(Func<TValue> func)
    {
        try
        {
            var value = func();
            return Ok(value);
        }
        catch (Exception exception)
        {
            return Err<TValue>(exception);
        }
    }

    public static Result<TValue, TError> Ok<TValue, TError>(TValue value)
        => new(value);

    public static Result<TValue, Error> Ok<TValue>(TValue value)
        => new(value);

    public static Result<TValue, TError> Err<TValue, TError>(TError error)
        => new(error);

    public static Result<TValue, Error> Err<TValue>(Error error)
        => new(error);
}