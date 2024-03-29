﻿using System.Runtime.CompilerServices;

namespace DotNet.Extensions.Require;

public static class Preconditions
{
    public static T Require<T>(
        this T @this,
        bool condition,
        string? expectation = null,
        [CallerArgumentExpression("condition")]
        string argExpr = "?"
    ) =>
        condition ? @this : throw new ArgumentException(expectation ?? $"expected: {argExpr}");

    public static T Require<T>(this T @this, bool condition, Func<string> expectation) =>
        condition ? @this : throw new ArgumentException(expectation());

    public static T Require<T>(this T @this, bool condition, Func<T, string> expectation) =>
        condition ? @this : throw new ArgumentException(expectation(@this));

    public static T Require<T>(
        this T @this,
        Predicate<T> requirement,
        string? expectation = null,
        [CallerArgumentExpression("requirement")]
        string argExpr = "?"
    ) =>
        requirement(@this) ? @this : throw new ArgumentException(expectation ?? $"expected: {argExpr}");

    public static T Require<T>(this T @this, Predicate<T> requirement, Func<string> expectation) =>
        requirement(@this) ? @this : throw new ArgumentException(expectation());

    public static T Require<T>(this T @this, Predicate<T> requirement, Func<T, string> expectation) =>
        requirement(@this) ? @this : throw new ArgumentException(expectation(@this));
}
