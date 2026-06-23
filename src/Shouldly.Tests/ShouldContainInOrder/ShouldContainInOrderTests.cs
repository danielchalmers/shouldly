namespace Shouldly.Tests.ShouldContainInOrder;

public class ShouldContainInOrderTests
{
    [Fact]
    public void Passes_when_items_appear_in_relative_order_with_gaps()
    {
        new[] { 1, 2, 3, 4, 5 }.ShouldContainInOrder(1, 3, 5);
        new[] { "a", "b", "c" }.ShouldContainInOrder(new[] { "a", "c" });
    }

    [Fact]
    public void Fails_when_items_are_out_of_order()
    {
        var ex = Should.Throw<ShouldAssertException>(() => new[] { 1, 2, 3 }.ShouldContainInOrder(3, 1));
        ex.Message.ShouldContain("should contain in order");
    }

    [Fact]
    public void Fails_when_an_expected_item_is_missing()
    {
        Should.Throw<ShouldAssertException>(() => new[] { 1, 2, 3 }.ShouldContainInOrder(2, 9));
    }

    [Fact]
    public void Honours_a_custom_comparer()
    {
        new[] { "A", "B", "C" }.ShouldContainInOrder(new[] { "a", "c" }, StringComparer.OrdinalIgnoreCase);
    }

    [Fact]
    public void Includes_the_custom_message()
    {
        var ex = Should.Throw<ShouldAssertException>(() => new[] { 1, 2 }.ShouldContainInOrder(new[] { 2, 1 }, "ordering matters"));
        ex.Message.ShouldContain("ordering matters");
    }

    [Fact]
    public void Consecutive_passes_for_a_contiguous_run()
    {
        new[] { 1, 2, 3, 4 }.ShouldContainInConsecutiveOrder(2, 3);
    }

    [Fact]
    public void Consecutive_fails_when_run_is_not_contiguous()
    {
        var ex = Should.Throw<ShouldAssertException>(() => new[] { 1, 2, 4, 3 }.ShouldContainInConsecutiveOrder(2, 3));
        ex.Message.ShouldContain("should contain in consecutive order");
    }
}
