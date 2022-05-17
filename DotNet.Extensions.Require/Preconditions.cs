namespace DotNet.Extensions.Require;

public static class Preconditions
{
    public static T Require<T>(this T @this, bool condition, string expectation) =>
        condition ? @this : throw new ArgumentException(expectation);
}
