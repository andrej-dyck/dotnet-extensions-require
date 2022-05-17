using NUnit.Framework;

namespace DotNet.Extensions.Require.Test;

public static class AssertExtensions
{
    public static void WithMessage(this Exception @this, string expectedMessage) =>
        Assert.AreEqual(expectedMessage, @this.Message);

}
