namespace Shouldly.Tests.ShouldBeEquivalentTo;

public class IgnoreOrderScenario
{
    private class Person
    {
        public string Name { get; set; } = "";
        public int Age { get; set; }
    }

    [Fact]
    public void Ignores_order_for_value_collections()
    {
        new[] { 3, 1, 2 }.ShouldBeEquivalentTo(new[] { 1, 2, 3 }, ignoreOrder: true);
    }

    [Fact]
    public void Ignores_order_for_complex_objects()
    {
        var actual = new[]
        {
            new Person { Name = "B", Age = 2 },
            new Person { Name = "A", Age = 1 },
        };
        var expected = new[]
        {
            new Person { Name = "A", Age = 1 },
            new Person { Name = "B", Age = 2 },
        };

        actual.ShouldBeEquivalentTo(expected, ignoreOrder: true);
        actual.ShouldBeEquivalentTo(expected, new EquivalencyOptions { IgnoreOrder = true });
    }

    [Fact]
    public void Handles_duplicates_via_multiset_matching()
    {
        new[] { 1, 1, 2 }.ShouldBeEquivalentTo(new[] { 1, 2, 1 }, ignoreOrder: true);

        // Same elements but different multiplicity must fail.
        Should.Throw<ShouldAssertException>(() =>
            new[] { 1, 1, 2 }.ShouldBeEquivalentTo(new[] { 1, 2, 2 }, ignoreOrder: true));
    }

    [Fact]
    public void Fails_on_content_mismatch_and_count_mismatch()
    {
        Should.Throw<ShouldAssertException>(() => new[] { 1, 2, 3 }.ShouldBeEquivalentTo(new[] { 1, 2, 4 }, ignoreOrder: true));
        Should.Throw<ShouldAssertException>(() => new[] { 1, 2 }.ShouldBeEquivalentTo(new[] { 1, 2, 3 }, ignoreOrder: true));
    }

    [Fact]
    public void Order_still_enforced_when_not_ignored()
    {
        new[] { 1, 2, 3 }.ShouldBeEquivalentTo(new[] { 1, 2, 3 }, ignoreOrder: false);
        Should.Throw<ShouldAssertException>(() => new[] { 3, 2, 1 }.ShouldBeEquivalentTo(new[] { 1, 2, 3 }, ignoreOrder: false));
    }
}
