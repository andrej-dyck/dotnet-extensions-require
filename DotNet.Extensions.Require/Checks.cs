using System.Runtime.CompilerServices;

namespace DotNet.Extensions.Require;

public static class Checks
{
    public static T Check<T>(
        this T @this,
        Predicate<T> requirement,
        string? expectation = null,
        [CallerArgumentExpression("requirement")]
        string argExpr = "?"
    ) =>
        requirement(@this) ? @this : throw new CheckFailed(expectation ?? $"expected: {argExpr}");

    public static T Check<T>(this T @this, Predicate<T> requirement, Func<string> expectation) =>
        requirement(@this) ? @this : throw new CheckFailed(expectation());

    public static T Check<T>(this T @this, Predicate<T> requirement, Func<T, string> expectation) =>
        requirement(@this) ? @this : throw new CheckFailed(expectation(@this));

    public static T Check<T>(this T @this, Predicate<T> requirement, Func<Exception> exception) =>
        requirement(@this) ? @this : throw exception();

    public static T Check<T>(this T @this, Predicate<T> requirement, Func<T, Exception> exception) =>
        requirement(@this) ? @this : throw exception(@this);
}

public sealed class CheckFailed : Exception
{
    public CheckFailed(string? message) : base(message) { }
}
