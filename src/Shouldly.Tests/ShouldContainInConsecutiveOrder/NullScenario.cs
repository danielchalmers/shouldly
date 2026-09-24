namespace Shouldly.Tests.ShouldContainInConsecutiveOrder;

public class NullScenario
{
    [Fact]
    public void ActualIsNullShouldFail()
    {
        int[]? nullableCollection = null;
        Verify.ShouldFail(() =>
            nullableCollection.ShouldContainInConsecutiveOrder([1, 2], "Some additional context"));
    }

    [Fact]
    public void ExpectedIsNullShouldThrow()
    {
        Should.Throw<ArgumentNullException>(() =>
            new[] { 1, 2 }.ShouldContainInConsecutiveOrder(null!));
    }

    [Fact]
    public void NullComparerShouldUseDefaultComparer()
    {
        new[] { "a", "b", "c" }.ShouldContainInConsecutiveOrder(["b", "c"], (IEqualityComparer<string>)null!);
        Should.Throw<ShouldAssertException>(() =>
            new[] { "a", "b", "c" }.ShouldContainInConsecutiveOrder(["B", "C"], (IEqualityComparer<string>)null!));
    }
}