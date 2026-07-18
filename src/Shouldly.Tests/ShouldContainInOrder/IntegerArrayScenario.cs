namespace Shouldly.Tests.ShouldContainInOrder;

public class IntegerArrayScenario
{
    [Fact]
    public void IntegerArrayScenarioShouldFail()
    {
        Verify.ShouldFail(() =>
            new[] { 1, 2, 3 }.ShouldContainInOrder(new List<int> { 3, 1 }, "Some additional context"));
    }

    [Fact]
    public void MissingItemShouldFail()
    {
        Verify.ShouldFail(() =>
            new[] { 1, 2, 3 }.ShouldContainInOrder(new List<int> { 2, 9 }, "Some additional context"));
    }

    [Fact]
    public void DuplicateExpectationShouldFail()
    {
        Verify.ShouldFail(() =>
            new[] { 1, 2, 3 }.ShouldContainInOrder(new List<int> { 2, 2 }, "Some additional context"));
    }

    [Fact]
    public void ShouldPass()
    {
        new[] { 1, 2, 3, 4, 5 }.ShouldContainInOrder(new List<int> { 1, 3, 5 });
        new[] { 1, 2, 3 }.ShouldContainInOrder(new List<int> { 1, 2, 3 });
        new[] { 1, 2, 2, 3 }.ShouldContainInOrder(new List<int> { 2, 2 });
        new[] { 1, 2 }.ShouldContainInOrder(new List<int>());
    }
}