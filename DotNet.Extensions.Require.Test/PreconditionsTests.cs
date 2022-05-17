using NUnit.Framework;

namespace DotNet.Extensions.Require.Test;

public sealed class PreconditionsTests
{
    [Test]
    public void Require_ThrowsArgumentException_WhenConditionIsFalse() =>
        Assert.Throws<ArgumentException>(
            () => 1.Require(condition: false, "message")
        )?.WithMessage("message");

    [Test]
    public void Require_DoesNotThrow_WhenConditionIsTrue() =>
        Assert.DoesNotThrow(
            () => 1.Require(condition: true, string.Empty)
        );

    [Test]
    public void Require_Returns_TheValueOnWhichItIsCalled_WhenConditionIsTrue() =>
        Assert.AreEqual(
            expected: 1,
            actual: 1.Require(condition: true, string.Empty)
        );
}
