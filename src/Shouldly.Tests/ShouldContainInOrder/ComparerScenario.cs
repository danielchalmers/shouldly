namespace Shouldly.Tests.ShouldContainInOrder;

public class ComparerScenario
{
    [Fact]
    public void ComparerEqualsShouldPass()
    {
        new[] { "A", "B", "C" }.ShouldContainInOrder(new List<string> { "a", "c" }, StringComparer.OrdinalIgnoreCase);
    }

    [Fact]
    public void ComparerNotEqualsShouldFail()
    {
        Verify.ShouldFail(() =>
            new[] { "A", "B", "C" }.ShouldContainInOrder(new List<string> { "c", "a" }, StringComparer.OrdinalIgnoreCase, "Some additional context"));
    }
}