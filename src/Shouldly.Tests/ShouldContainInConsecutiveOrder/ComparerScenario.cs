namespace Shouldly.Tests.ShouldContainInConsecutiveOrder;

public class ComparerScenario
{
    [Fact]
    public void ComparerEqualsShouldPass()
    {
        new[] { "A", "B", "C" }.ShouldContainInConsecutiveOrder(new List<string> { "b", "c" }, StringComparer.OrdinalIgnoreCase);
    }

    [Fact]
    public void ComparerNotEqualsShouldFail()
    {
        Verify.ShouldFail(() =>
            new[] { "A", "B", "C" }.ShouldContainInConsecutiveOrder(new List<string> { "a", "c" }, StringComparer.OrdinalIgnoreCase, "Some additional context"));
    }
}