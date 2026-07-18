using System.ComponentModel;

namespace Shouldly;

/// <summary>
/// Extension methods asserting that an enumerable contains a set of values in a given order.
/// <see cref="ShouldContainInOrder{T}(IEnumerable{T}, T[])"/> requires the expected values to appear in the
/// same relative order (gaps allowed); <see cref="ShouldContainInConsecutiveOrder{T}(IEnumerable{T}, T[])"/>
/// requires them to appear as a contiguous run.
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
    public static void ShouldContainInOrder<T>(this IEnumerable<T> actual, params T[] expected)
    {
        // params arrays cannot carry a [CallerArgumentExpression]; opt into the stack-walk fallback for the message.
        using (ShouldlyConfiguration.AllowStackWalking())
            ContainsInOrder(actual, expected, EqualityComparer<T>.Default, null, null);
    }

    /// <summary>
    /// Asserts that the enumerable contains all of the expected values in the given relative order.
    /// </summary>
    public static void ShouldContainInOrder<T>(this IEnumerable<T> actual, IEnumerable<T> expected, string? customMessage = null,
        [CallerArgumentExpression(nameof(actual))] string? actualExpression = null) =>
        ContainsInOrder(actual, expected, EqualityComparer<T>.Default, customMessage, actualExpression);

    /// <summary>
    /// Asserts that the enumerable contains all of the expected values in the given relative order, using the specified comparer.
    /// </summary>
    public static void ShouldContainInOrder<T>(this IEnumerable<T> actual, IEnumerable<T> expected, IEqualityComparer<T> comparer, string? customMessage = null,
        [CallerArgumentExpression(nameof(actual))] string? actualExpression = null) =>
        ContainsInOrder(actual, expected, comparer, customMessage, actualExpression);

    /// <summary>
    /// Asserts that the enumerable contains all of the expected values as a contiguous subsequence, in order.
    /// </summary>
    public static void ShouldContainInConsecutiveOrder<T>(this IEnumerable<T> actual, params T[] expected)
    {
        // params arrays cannot carry a [CallerArgumentExpression]; opt into the stack-walk fallback for the message.
        using (ShouldlyConfiguration.AllowStackWalking())
            ContainsInConsecutiveOrder(actual, expected, EqualityComparer<T>.Default, null, null);
    }

    /// <summary>
    /// Asserts that the enumerable contains all of the expected values as a contiguous subsequence, in order.
    /// </summary>
    public static void ShouldContainInConsecutiveOrder<T>(this IEnumerable<T> actual, IEnumerable<T> expected, string? customMessage = null,
        [CallerArgumentExpression(nameof(actual))] string? actualExpression = null) =>
        ContainsInConsecutiveOrder(actual, expected, EqualityComparer<T>.Default, customMessage, actualExpression);

    /// <summary>
    /// Asserts that the enumerable contains all of the expected values as a contiguous subsequence, using the specified comparer.
    /// </summary>
    public static void ShouldContainInConsecutiveOrder<T>(this IEnumerable<T> actual, IEnumerable<T> expected, IEqualityComparer<T> comparer, string? customMessage = null,
        [CallerArgumentExpression(nameof(actual))] string? actualExpression = null) =>
        ContainsInConsecutiveOrder(actual, expected, comparer, customMessage, actualExpression);

    private static void ContainsInOrder<T>(IEnumerable<T> actual, IEnumerable<T> expected, IEqualityComparer<T> comparer,
        string? customMessage, string? actualExpression, [CallerMemberName] string shouldlyMethod = null!)
    {
        var actualList = actual.ToList();
        var expectedList = expected.ToList();

        var searchIndex = 0;
        foreach (var item in expectedList)
        {
            var found = false;
            while (searchIndex < actualList.Count)
            {
                if (comparer.Equals(actualList[searchIndex++], item))
                {
                    found = true;
                    break;
                }
            }

            if (!found)
                throw new ShouldAssertException(new ExpectedActualShouldlyMessage(expectedList, actualList, customMessage, shouldlyMethod, actualExpression).ToString());
        }
    }

    private static void ContainsInConsecutiveOrder<T>(IEnumerable<T> actual, IEnumerable<T> expected, IEqualityComparer<T> comparer,
        string? customMessage, string? actualExpression, [CallerMemberName] string shouldlyMethod = null!)
    {
        var actualList = actual.ToList();
        var expectedList = expected.ToList();
        if (expectedList.Count == 0)
            return;

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

        throw new ShouldAssertException(new ExpectedActualShouldlyMessage(expectedList, actualList, customMessage, shouldlyMethod, actualExpression).ToString());
    }
}
