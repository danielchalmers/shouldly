namespace Shouldly.Tests.ShouldHaveCount;

public class ShouldHaveCountFamilyTests
{
    [Fact]
    public void Exact_count_passes_and_fails()
    {
        new[] { 1, 2, 3 }.ShouldHaveCount(3);
        var ex = Should.Throw<ShouldAssertException>(() => new[] { 1, 2 }.ShouldHaveCount(3));
        ex.Message.ShouldContain("should have count");
        ex.Message.ShouldContain("but had 2");
    }

    [Fact]
    public void Greater_than_family()
    {
        new[] { 1, 2, 3 }.ShouldHaveCountGreaterThan(2);
        new[] { 1, 2, 3 }.ShouldHaveCountGreaterThanOrEqualTo(3);
        Should.Throw<ShouldAssertException>(() => new[] { 1, 2 }.ShouldHaveCountGreaterThan(2));
    }

    [Fact]
    public void Less_than_family()
    {
        new[] { 1, 2 }.ShouldHaveCountLessThan(3);
        new[] { 1, 2, 3 }.ShouldHaveCountLessThanOrEqualTo(3);
        Should.Throw<ShouldAssertException>(() => new[] { 1, 2, 3 }.ShouldHaveCountLessThan(3));
    }

    [Fact]
    public void Null_collection_fails()
    {
        int[]? items = null;
        Should.Throw<ShouldAssertException>(() => items.ShouldHaveCount(0));
    }

    [Fact]
    public void Custom_message_is_included()
    {
        var ex = Should.Throw<ShouldAssertException>(() => new[] { 1 }.ShouldHaveCount(2, "wrong size"));
        ex.Message.ShouldContain("wrong size");
    }
}
