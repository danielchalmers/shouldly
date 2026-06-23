using System.ComponentModel;

namespace Shouldly;

/// <summary>
/// Ergonomic count assertions for enumerables. Equivalent to <c>actual.Count().ShouldBe(n)</c> but
/// reads better, avoids requiring System.Linq at the call site, and produces a count-aware message.
/// Mirrors FluentAssertions' HaveCount / HaveCountGreaterThan / HaveCountGreaterThanOrEqualTo / etc.
/// </summary>
[DebuggerStepThrough]
[ShouldlyMethods]
[EditorBrowsable(EditorBrowsableState.Never)]
public static partial class ShouldHaveCountExtensions
{
    /// <summary>
    /// Asserts that the enumerable contains exactly the expected number of elements.
    /// </summary>
    public static void ShouldHaveCount<T>([NotNull] this IEnumerable<T>? actual, int expected, string? customMessage = null,
        [CallerArgumentExpression(nameof(actual))] string? actualExpression = null) =>
        CheckCount(actual, expected, count => count == expected, "have count", customMessage, actualExpression);

    /// <summary>
    /// Asserts that the enumerable contains more than the specified number of elements.
    /// </summary>
    public static void ShouldHaveCountGreaterThan<T>([NotNull] this IEnumerable<T>? actual, int expected, string? customMessage = null,
        [CallerArgumentExpression(nameof(actual))] string? actualExpression = null) =>
        CheckCount(actual, expected, count => count > expected, "have count greater than", customMessage, actualExpression);

    /// <summary>
    /// Asserts that the enumerable contains at least the specified number of elements.
    /// </summary>
    public static void ShouldHaveCountGreaterThanOrEqualTo<T>([NotNull] this IEnumerable<T>? actual, int expected, string? customMessage = null,
        [CallerArgumentExpression(nameof(actual))] string? actualExpression = null) =>
        CheckCount(actual, expected, count => count >= expected, "have count greater than or equal to", customMessage, actualExpression);

    /// <summary>
    /// Asserts that the enumerable contains fewer than the specified number of elements.
    /// </summary>
    public static void ShouldHaveCountLessThan<T>([NotNull] this IEnumerable<T>? actual, int expected, string? customMessage = null,
        [CallerArgumentExpression(nameof(actual))] string? actualExpression = null) =>
        CheckCount(actual, expected, count => count < expected, "have count less than", customMessage, actualExpression);

    /// <summary>
    /// Asserts that the enumerable contains at most the specified number of elements.
    /// </summary>
    public static void ShouldHaveCountLessThanOrEqualTo<T>([NotNull] this IEnumerable<T>? actual, int expected, string? customMessage = null,
        [CallerArgumentExpression(nameof(actual))] string? actualExpression = null) =>
        CheckCount(actual, expected, count => count <= expected, "have count less than or equal to", customMessage, actualExpression);

    private static void CheckCount<T>([NotNull] IEnumerable<T>? actual, int expected, Func<int, bool> predicate, string verb,
        string? customMessage, string? actualExpression)
    {
        if (actual == null)
            throw new ShouldAssertException(new ExpectedActualShouldlyMessage($"{verb} {expected}", "null", customMessage, actualExpression: actualExpression).ToString());

        var materialized = actual as ICollection<T> ?? actual.ToList();
        var count = materialized.Count;
        if (!predicate(count))
            throw new ShouldAssertException(new ExpectedActualShouldlyMessage($"{verb} {expected}", $"count {count}", customMessage, actualExpression: actualExpression).ToString());
    }
}
