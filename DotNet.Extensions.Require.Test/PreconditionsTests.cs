using NUnit.Framework;
using NUnit.Framework.Internal;

namespace DotNet.Extensions.Require.Test;

public abstract class PreconditionsTests
{
    protected abstract T Require<T>(T subject, bool condition, string expectation = "");

    [Test]
    public void Require_ThrowsArgumentException_WhenConditionIsFalse() =>
        Assert.Throws<ArgumentException>(
            () => Require(subject: 1, condition: false, "message")
        )?.WithMessage("message");

    [Test]
    public void Require_DoesNotThrow_WhenConditionIsTrue() =>
        Assert.DoesNotThrow(
            () => Require(subject: 1, condition: true)
        );

    [Test]
    public void Require_ReturnsTheValueOnWhichItIsCalled_WhenConditionIsTrue() =>
        Assert.AreEqual(
            expected: 1,
            actual: Require(subject: 1, condition: true)
        );
}

[TestFixture]
public sealed class BasicRequire : PreconditionsTests
{
    protected override T Require<T>(T subject, bool condition, string expectation = "") =>
        subject.Require(condition, expectation);
}

public sealed class RequireWithLazyMessage : PreconditionsTests
{
    protected override T Require<T>(T subject, bool condition, string expectation = "") =>
        subject.Require(condition, expectation: () => expectation);

    [Test]
    public void Require_DoesNotCreateExpectationMessage_WhenConditionIsTrue() =>
        Assert.DoesNotThrow(
            () => 1.Require(condition: true, () => throw new NUnitException("must not be called"))
        );
}

public sealed class RequireWithLazilyConstructedMessage : PreconditionsTests
{
    protected override T Require<T>(T subject, bool condition, string expectation = "") =>
        subject.Require(condition, expectation: _ => expectation);

    [Test]
    public void Require_CanUseTheValue_ToConstructTheExpectation() =>
        Assert.Throws<ArgumentException>(
            () => 1.Require(condition: false, value => $"message with {value}")
        )?.WithMessage("message with 1");

    [Test]
    public void Require_DoesNotBuildExpectationMessage_WhenConditionIsTrue() =>
        Assert.DoesNotThrow(
            () => 1.Require(condition: true, value => throw new NUnitException($"must not be called for {value}"))
        );
}

public abstract class RequireWithPredicate : PreconditionsTests
{
    protected abstract T Require<T>(T subject, Predicate<T> requirement, string expectation = "");

    protected override T Require<T>(T subject, bool condition, string expectation = "") =>
        Require(subject, requirement: _ => condition, expectation);

    [Test]
    public void RequirePredicate_WillBeGivenTheValue() =>
        Assert.DoesNotThrow(
            () => 1.Require(requirement: value => value == 1, string.Empty)
        );
}

[TestFixture]
public sealed class RequireWithPredicateAndConstantMessage : RequireWithPredicate
{
    protected override T Require<T>(T subject, Predicate<T> requirement, string expectation = "") =>
        subject.Require(requirement, expectation);
}

public sealed class RequireWithPredicateAndLazyMessage : RequireWithPredicate
{
    protected override T Require<T>(T subject, Predicate<T> requirement, string expectation = "") =>
        subject.Require(requirement, expectation: () => expectation);

    [Test]
    public void Require_DoesNotCreateExpectationMessage_WhenRequirementIsSatisfied() =>
        Assert.DoesNotThrow(
            () => 1.Require(requirement: _ => true, () => throw new NUnitException("must not be called"))
        );
}

public sealed class RequireWithPredicateAndLazilyConstructedMessage : RequireWithPredicate
{
    protected override T Require<T>(T subject, Predicate<T> requirement, string expectation = "") =>
        subject.Require(requirement, expectation: _ => expectation);

    [Test]
    public void Require_CanUseTheValue_ToConstructTheExpectation() =>
        Assert.Throws<ArgumentException>(
            () => 1.Require(requirement: _ => false, value => $"message with {value}")
        )?.WithMessage("message with 1");

    [Test]
    public void Require_DoesNotBuildExpectationMessage_WhenRequirementIsSatisfied() =>
        Assert.DoesNotThrow(
            () => 1.Require(requirement: _ => true, value => throw new NUnitException($"must not be called for {value}"))
        );
}
