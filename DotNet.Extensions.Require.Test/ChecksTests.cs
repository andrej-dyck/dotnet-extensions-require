using NUnit.Framework;
using NUnit.Framework.Internal;

namespace DotNet.Extensions.Require.Test;

public sealed class ChecksTests
{
    [Test]
    public void Check_ThrowsCheckFailed_WhenRequirementIsNotSatisfied() =>
        Assert.Throws<CheckFailed>(
            () => 1.Check(v => v != 1, expectation: "message")
        )?.WithMessage("message");

    [Test]
    public void Check_UsesArgumentExpressionAsExpectation_WhenNoMessageIsGiven() =>
        Assert.Throws<CheckFailed>(
            () => 1.Check(v => v != 1)
        )?.WithMessage("expected: v => v != 1");

    [Test]
    public void Check_DoesNotThrow_WhenRequirementIsSatisfied() =>
        Assert.DoesNotThrow(
            () => 1.Check(v => v == 1, expectation: string.Empty)
        );

    [Test]
    public void Check_ThrowsBuiltException_WhenRequirementIsNotSatisfied() =>
        Assert.Throws<SomeExceptionType>(
            () => 1.Check(v => v != 1, () => new SomeExceptionType("message"))
        )?.WithMessage("message");

    [Test]
    public void Check_CanConstructException_WithTheValue() =>
        Assert.Throws<SomeExceptionType>(
            () => 1.Check(v => v != 1, v => new SomeExceptionType($"message with {v}"))
        )?.WithMessage("message with 1");

    [Test]
    public void Check_DoesNotBuildException_WhenRequirementIsSatisfied() => Assert.Multiple(
        () =>
        {
            Assert.DoesNotThrow(
                () => 1.Check(v => v == 1, expectation: () => throw new NUnitException("must not be called"))
            );
            Assert.DoesNotThrow(
                () => 1.Check(v => v == 1, exception: () => throw new NUnitException("must not be called"))
            );
        }
    );

    [Test]
    public void Check_DoesNotConstructException_WhenRequirementIsSatisfied() => Assert.Multiple(
        () =>
        {
            Assert.DoesNotThrow(
                () => 1.Check(
                    v => v == 1,
                    expectation: v => throw new NUnitException($"must not be called for {v}"))
            );
            Assert.DoesNotThrow(
                () => 1.Check(v => v == 1, exception: v => throw new NUnitException($"must not be called for {v}"))
            );
        }
    );

    [Test]
    public void Check_ReturnsTheValueOnWhichItIsCalled_WhenRequirementIsSatisfied() => Assert.Multiple(
        () =>
        {
            Assert.AreEqual(1, 1.Check(value => value == 1, string.Empty));
            Assert.AreEqual(1, 1.Check(value => value == 1, () => string.Empty));
            Assert.AreEqual(1, 1.Check(value => value == 1, value => $"{value}"));
            Assert.AreEqual(1, 1.Check(value => value == 1, () => new CheckFailed(string.Empty)));
            Assert.AreEqual(1, 1.Check(value => value == 1, value => new CheckFailed($"{value}")));
        }
    );
}

public sealed class SomeExceptionType : Exception
{
    public SomeExceptionType(string? message = null) : base(message) { }
}
