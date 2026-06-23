namespace Shouldly.Tests.ShouldAllSatisfy;

public class ShouldAllSatisfyTests
{
    [Fact]
    public void Passes_when_every_element_satisfies_the_assertion()
    {
        new[] { 2, 4, 6 }.ShouldAllSatisfy(x => (x % 2).ShouldBe(0));
    }

    [Fact]
    public void Aggregates_every_failing_element()
    {
        var ex = Should.Throw<ShouldAssertException>(() =>
            new[] { 2, 3, 6, 7 }.ShouldAllSatisfy(x => (x % 2).ShouldBe(0)));

        // Both odd elements (indices 1 and 3) are reported, not just the first.
        ex.Message.ShouldContain("[1]");
        ex.Message.ShouldContain("[3]");
        ex.Message.ShouldContain("2 did not");
    }

    [Fact]
    public void Null_collection_fails()
    {
        int[]? items = null;
        Should.Throw<ShouldAssertException>(() => items.ShouldAllSatisfy(x => x.ShouldBe(0)));
    }

    [Fact]
    public void Custom_message_is_included()
    {
        var ex = Should.Throw<ShouldAssertException>(() =>
            new[] { 1 }.ShouldAllSatisfy(x => x.ShouldBe(0), "all should be zero"));
        ex.Message.ShouldContain("all should be zero");
    }
}
