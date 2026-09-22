using System.ComponentModel;

namespace Shouldly;

/// <summary>
/// Extension methods asserting that an enumerable contains a sequence of values in a given order. <see cref="ShouldContainInOrder{T}(IEnumerable{T}, IEnumerable{T}, string?, string?)"/> requires the expected values to appear in the same relative order (gaps allowed); <see cref="ShouldContainInConsecutiveOrder{T}(IEnumerable{T}, IEnumerable{T}, string?, string?)"/> requires them to appear as a contiguous run.
/// </summary>
[DebuggerStepThrough]
[ShouldlyMethods]
[EditorBrowsable(EditorBrowsableState.Never)]
public static partial class ShouldContainInOrderExtensions
{
    /// <summary>
    /// Asserts that the enumerable contains all of the expected values in the given relative order. The matched values need not be adjacent, but each must appear after the previous one.
    /// </summary>
    public static void ShouldContainInOrder<T>([NotNull] this IEnumerable<T>? actual, IEnumerable<T> expected, string? customMessage = null,
        [CallerArgumentExpression(nameof(actual))] string? actualExpression = null) =>
        ContainsInOrder(actual, expected, EqualityComparer<T>.Default, customMessage, actualExpression);

    /// <summary>
    /// Asserts that the enumerable contains all of the expected values in the given relative order, using the specified comparer.
    /// </summary>
    public static void ShouldContainInOrder<T>([NotNull] this IEnumerable<T>? actual, IEnumerable<T> expected, IEqualityComparer<T> comparer, string? customMessage = null,
        [CallerArgumentExpression(nameof(actual))] string? actualExpression = null) =>
        ContainsInOrder(actual, expected, comparer, customMessage, actualExpression);

    /// <summary>
    /// Asserts that the enumerable contains all of the expected values as a contiguous subsequence, in order.
    /// </summary>
    public static void ShouldContainInConsecutiveOrder<T>([NotNull] this IEnumerable<T>? actual, IEnumerable<T> expected, string? customMessage = null,
        [CallerArgumentExpression(nameof(actual))] string? actualExpression = null) =>
        ContainsInConsecutiveOrder(actual, expected, EqualityComparer<T>.Default, customMessage, actualExpression);

    /// <summary>
    /// Asserts that the enumerable contains all of the expected values as a contiguous subsequence, using the specified comparer.
    /// </summary>
    public static void ShouldContainInConsecutiveOrder<T>([NotNull] this IEnumerable<T>? actual, IEnumerable<T> expected, IEqualityComparer<T> comparer, string? customMessage = null,
        [CallerArgumentExpression(nameof(actual))] string? actualExpression = null) =>
        ContainsInConsecutiveOrder(actual, expected, comparer, customMessage, actualExpression);

    private static void ContainsInOrder<T>([NotNull] IEnumerable<T>? actual, IEnumerable<T> expected, IEqualityComparer<T>? comparer,
        string? customMessage, string? actualExpression, [CallerMemberName] string shouldlyMethod = null!)
    {
        var expectedItems = Materialize(expected);
        if (actual == null)
            throw new ShouldAssertException(new ExpectedActualShouldlyMessage(expectedItems, actual, customMessage, shouldlyMethod, actualExpression).ToString());

        // A null comparer means the default one, as with the other enumerable assertions that take a comparer.
        comparer ??= EqualityComparer<T>.Default;
        var actualItems = Materialize(actual);

        var previousMatch = -1;
        for (var expectedIndex = 0; expectedIndex < expectedItems.Count; expectedIndex++)
        {
            var match = IndexOf(actualItems, expectedItems[expectedIndex], previousMatch + 1, comparer);
            if (match < 0)
            {
                var mismatch = new Internals.ContainInOrderMismatch(expectedIndex, expectedItems[expectedIndex], previousMatch);
                throw new ShouldAssertException(new ExpectedActualShouldlyMessage(mismatch, expectedItems, actualItems, customMessage, shouldlyMethod, actualExpression).ToString());
            }

            previousMatch = match;
        }
    }

    private static void ContainsInConsecutiveOrder<T>([NotNull] IEnumerable<T>? actual, IEnumerable<T> expected, IEqualityComparer<T>? comparer,
        string? customMessage, string? actualExpression, [CallerMemberName] string shouldlyMethod = null!)
    {
        var expectedItems = Materialize(expected);
        if (actual == null)
            throw new ShouldAssertException(new ExpectedActualShouldlyMessage(expectedItems, actual, customMessage, shouldlyMethod, actualExpression).ToString());

        if (expectedItems.Count == 0)
            return;

        // A null comparer means the default one, as with the other enumerable assertions that take a comparer.
        comparer ??= EqualityComparer<T>.Default;
        var actualItems = Materialize(actual);

        // Try every start position, remembering the longest partial run so the message can say where it broke.
        var longestStart = -1;
        var longestLength = 0;
        for (var start = 0; start < actualItems.Count; start++)
        {
            var length = 0;
            while (length < expectedItems.Count
                   && start + length < actualItems.Count
                   && comparer.Equals(actualItems[start + length], expectedItems[length]))
            {
                length++;
            }

            if (length == expectedItems.Count)
                return;

            if (length > longestLength)
            {
                longestStart = start;
                longestLength = length;
            }
        }

        var mismatch = new Internals.ContainInOrderMismatch(longestLength, expectedItems[longestLength], longestStart);
        throw new ShouldAssertException(new ExpectedActualShouldlyMessage(mismatch, expectedItems, actualItems, customMessage, shouldlyMethod, actualExpression).ToString());
    }

    // Lists and arrays are indexed in place, and the failure message shows them as passed. Anything else is enumerated exactly once into an array, which the message then shows, so a lazy or single-pass source is never enumerated again.
    private static IReadOnlyList<T> Materialize<T>(IEnumerable<T> source) =>
        source as IReadOnlyList<T> ?? source.ToArray();

    private static int IndexOf<T>(IReadOnlyList<T> items, T value, int startIndex, IEqualityComparer<T> comparer)
    {
        for (var i = startIndex; i < items.Count; i++)
        {
            if (comparer.Equals(items[i], value))
                return i;
        }

        return -1;
    }
}