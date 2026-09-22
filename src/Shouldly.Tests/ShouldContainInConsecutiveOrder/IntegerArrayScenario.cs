namespace Shouldly.Tests.ShouldContainInConsecutiveOrder;

public class IntegerArrayScenario
{
    [Fact]
    public void IntegerArrayScenarioShouldFail()
    {
        Verify.ShouldFail(() =>
            new[] { 1, 2, 4, 3 }.ShouldContainInConsecutiveOrder([2, 3], "Some additional context"));
    }

    [Fact]
    public void FirstItemMissingShouldFail()
    {
        Verify.ShouldFail(() =>
            new[] { 1, 2, 3 }.ShouldContainInConsecutiveOrder(new List<int> { 9, 1 }, "Some additional context"));
    }

    [Fact]
    public void RunReachesEndShouldFail()
    {
        Verify.ShouldFail(() =>
            new[] { 1, 2, 3 }.ShouldContainInConsecutiveOrder(new[] { 2, 3, 4 }));
    }

    [Fact]
    public void LongestRunShouldFail()
    {
        Verify.ShouldFail(() =>
            new[] { 1, 2, 5, 1, 2, 3, 5 }.ShouldContainInConsecutiveOrder([1, 2, 3, 4]));
    }

    [Fact]
    public void DuplicateExpectationShouldFail()
    {
        Verify.ShouldFail(() =>
            new[] { 1, 2, 1 }.ShouldContainInConsecutiveOrder([1, 1]));
    }

    [Fact]
    public void EmptyActualShouldFail()
    {
        Verify.ShouldFail(() =>
            Array.Empty<int>().ShouldContainInConsecutiveOrder([1]));
    }

    [Fact]
    public void ShouldPass()
    {
        new[] { 1, 2, 3, 4 }.ShouldContainInConsecutiveOrder([2, 3]);
        new[] { 1, 2, 3 }.ShouldContainInConsecutiveOrder(new[] { 1, 2, 3 });
        new[] { 1, 2, 1, 1 }.ShouldContainInConsecutiveOrder(new List<int> { 1, 1 });
    }

    [Fact]
    public void RestartAfterPartialMatchShouldPass()
    {
        new[] { 1, 1, 2 }.ShouldContainInConsecutiveOrder([1, 2]);
        new[] { 1, 2, 1, 2, 3 }.ShouldContainInConsecutiveOrder([1, 2, 3]);
    }

    [Fact]
    public void EmptyExpectedShouldPass()
    {
        new[] { 1, 2, 3 }.ShouldContainInConsecutiveOrder([]);
        Array.Empty<int>().ShouldContainInConsecutiveOrder(new List<int>());
    }
}