using System.ComponentModel;

namespace Shouldly;

/// <summary>
/// Extension methods asserting that an enumerable contains a set of values in a given order.
/// Mirrors FluentAssertions' ContainInOrder (relative order, gaps allowed) and
/// ContainInConsecutiveOrder (contiguous subsequence).
/// </summary>
[DebuggerStepThrough]
[ShouldlyMethods]
[EditorBrowsable(EditorBrowsableState.Never)]
public static partial class ShouldContainInOrderExtensions
{
    /// <summary>
    /// Asserts that the enumerable contains all of the expected values in the given relative order.
    /// The matched values need not be adjacent, but each must appear after the previous one.
    /// </summary>
    public static void ShouldContainInOrder<T>(this IEnumerable<T> actual, params T[] expected) =>
        actual.ShouldContainInOrder((IEnumerable<T>)expected, null);

    /// <summary>
    /// Asserts that the enumerable contains all of the expected values in the given relative order.
    /// </summary>
    public static void ShouldContainInOrder<T>(this IEnumerable<T> actual, IEnumerable<T> expected, string? customMessage = null,
        [CallerArgumentExpression(nameof(actual))] string? actualExpression = null)
    {
        var actualList = actual.ToList();
        var expectedList = expected.ToList();
        var comparer = EqualityComparer<T>.Default;

        var searchIndex = 0;
        foreach (var item in expectedList)
        {
            var found = false;
            while (searchIndex < actualList.Count)
            {
                var current = actualList[searchIndex++];
                if (comparer.Equals(current, item))
                {
                    found = true;
                    break;
                }
            }

            if (!found)
                throw new ShouldAssertException(new ExpectedActualShouldlyMessage(expectedList, actualList, customMessage, actualExpression: actualExpression).ToString());
        }
    }

    /// <summary>
    /// Asserts that the enumerable contains all of the expected values as a contiguous subsequence, in order.
    /// </summary>
    public static void ShouldContainInConsecutiveOrder<T>(this IEnumerable<T> actual, params T[] expected) =>
        actual.ShouldContainInConsecutiveOrder((IEnumerable<T>)expected, null);

    /// <summary>
    /// Asserts that the enumerable contains all of the expected values as a contiguous subsequence, in order.
    /// </summary>
    public static void ShouldContainInConsecutiveOrder<T>(this IEnumerable<T> actual, IEnumerable<T> expected, string? customMessage = null,
        [CallerArgumentExpression(nameof(actual))] string? actualExpression = null)
    {
        var actualList = actual.ToList();
        var expectedList = expected.ToList();
        if (expectedList.Count == 0)
            return;

        var comparer = EqualityComparer<T>.Default;
        for (var start = 0; start + expectedList.Count <= actualList.Count; start++)
        {
            var match = true;
            for (var offset = 0; offset < expectedList.Count; offset++)
            {
                if (!comparer.Equals(actualList[start + offset], expectedList[offset]))
                {
                    match = false;
                    break;
                }
            }

            if (match)
                return;
        }

        throw new ShouldAssertException(new ExpectedActualShouldlyMessage(expectedList, actualList, customMessage, actualExpression: actualExpression).ToString());
    }
}
