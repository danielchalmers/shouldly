namespace Shouldly.Tests.ShouldContainInOrder;

public class NullScenario
{
    [Fact]
    public void ActualIsNullShouldFail()
    {
        int[]? nullableCollection = null;
        Verify.ShouldFail(() =>
            nullableCollection.ShouldContainInOrder([1, 2], "Some additional context"));
    }

    [Fact]
    public void ExpectedIsNullShouldThrow()
    {
        Should.Throw<ArgumentNullException>(() =>
            new[] { 1, 2 }.ShouldContainInOrder(null!));
    }

    [Fact]
    public void NullComparerShouldUseDefaultComparer()
    {
        new[] { "a", "b", "c" }.ShouldContainInOrder(["a", "c"], (IEqualityComparer<string>)null!);
        Should.Throw<ShouldAssertException>(() =>
            new[] { "a", "b", "c" }.ShouldContainInOrder(["A", "C"], (IEqualityComparer<string>)null!));
    }
}