namespace Shouldly.Tests.ShouldContainInOrder;

public class IntegerArrayScenario
{
    [Fact]
    public void IntegerArrayScenarioShouldFail()
    {
        Verify.ShouldFail(() =>
            new[] { 1, 2, 3 }.ShouldContainInOrder([3, 1], "Some additional context"));
    }

    [Fact]
    public void MissingItemShouldFail()
    {
        Verify.ShouldFail(() =>
            new[] { 1, 2, 3 }.ShouldContainInOrder(new List<int> { 2, 9 }, "Some additional context"));
    }

    [Fact]
    public void FirstItemMissingShouldFail()
    {
        Verify.ShouldFail(() =>
            new[] { 1, 2, 3 }.ShouldContainInOrder(new[] { 9, 1 }, "Some additional context"));
    }

    [Fact]
    public void DuplicateExpectationShouldFail()
    {
        Verify.ShouldFail(() =>
            new[] { 1, 2 }.ShouldContainInOrder([1, 1], "Some additional context"));
    }

    [Fact]
    public void EmptyActualShouldFail()
    {
        Verify.ShouldFail(() =>
            Array.Empty<int>().ShouldContainInOrder([1]));
    }

    [Fact]
    public void ListVariableShouldFail()
    {
        var list = new List<int> { 1, 2, 3 };
        Verify.ShouldFail(() =>
            list.ShouldContainInOrder([3, 1]));
    }

    [Fact]
    public void ShouldPass()
    {
        new[] { 1, 2, 3, 4, 5 }.ShouldContainInOrder([1, 3, 5]);
        new[] { 1, 2, 3 }.ShouldContainInOrder(new[] { 1, 2, 3 });
        new[] { 1, 2, 2, 3 }.ShouldContainInOrder(new List<int> { 2, 2 });
        new[] { 1, 2, 1 }.ShouldContainInOrder([1, 1]);
    }

    [Fact]
    public void EmptyExpectedShouldPass()
    {
        new[] { 1, 2 }.ShouldContainInOrder([]);
        Array.Empty<int>().ShouldContainInOrder(new List<int>());
    }
}