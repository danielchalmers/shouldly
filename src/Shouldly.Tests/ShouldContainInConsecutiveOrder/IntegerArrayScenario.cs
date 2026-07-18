namespace Shouldly.Tests.ShouldContainInConsecutiveOrder;

public class IntegerArrayScenario
{
    [Fact]
    public void IntegerArrayScenarioShouldFail()
    {
        Verify.ShouldFail(() =>
            new[] { 1, 2, 4, 3 }.ShouldContainInConsecutiveOrder(new List<int> { 2, 3 }, "Some additional context"));
    }

    [Fact]
    public void ShouldPass()
    {
        new[] { 1, 2, 3, 4 }.ShouldContainInConsecutiveOrder(new List<int> { 2, 3 });
        new[] { 1, 2, 3 }.ShouldContainInConsecutiveOrder(new List<int> { 1, 2, 3 });
        new[] { 1, 2, 3 }.ShouldContainInConsecutiveOrder(new List<int>());
    }
}