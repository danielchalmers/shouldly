namespace Shouldly.Internals;

/// <summary>
/// Where a ShouldContainInOrder or ShouldContainInConsecutiveOrder check broke down, for the failure message.
/// </summary>
internal sealed class ContainInOrderMismatch
{
    public ContainInOrderMismatch(int expectedIndex, object? expectedItem, int actualIndex)
    {
        ExpectedIndex = expectedIndex;
        ExpectedItem = expectedItem;
        ActualIndex = actualIndex;
    }

    /// <summary>
    /// The index into the expected values of the first one that could not be matched. For a consecutive-order check this is also the length of the longest partial run.
    /// </summary>
    public int ExpectedIndex { get; }

    /// <summary>The expected value at <see cref="ExpectedIndex"/>.</summary>
    public object? ExpectedItem { get; }

    /// <summary>
    /// An index into the actual values: for an in-order check, where the previous expected value was matched; for a consecutive-order check, where the longest partial run starts. -1 when nothing was matched.
    /// </summary>
    public int ActualIndex { get; }
}