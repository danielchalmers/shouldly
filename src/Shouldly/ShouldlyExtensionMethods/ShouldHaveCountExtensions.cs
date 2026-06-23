using System.ComponentModel;
using NotNullAttribute = System.Diagnostics.CodeAnalysis.NotNullAttribute;

namespace Shouldly;

/// <summary>
/// Count assertions for enumerables, equivalent to <c>actual.Count().ShouldBe(n)</c> with a count-aware message.
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
        CheckCount(actual, expected, count => count == expected, customMessage, actualExpression);

    /// <summary>
    /// Asserts that the enumerable contains more than the specified number of elements.
    /// </summary>
    public static void ShouldHaveCountGreaterThan<T>([NotNull] this IEnumerable<T>? actual, int expected, string? customMessage = null,
        [CallerArgumentExpression(nameof(actual))] string? actualExpression = null) =>
        CheckCount(actual, expected, count => count > expected, customMessage, actualExpression);

    /// <summary>
    /// Asserts that the enumerable contains at least the specified number of elements.
    /// </summary>
    public static void ShouldHaveCountGreaterThanOrEqualTo<T>([NotNull] this IEnumerable<T>? actual, int expected, string? customMessage = null,
        [CallerArgumentExpression(nameof(actual))] string? actualExpression = null) =>
        CheckCount(actual, expected, count => count >= expected, customMessage, actualExpression);

    /// <summary>
    /// Asserts that the enumerable contains fewer than the specified number of elements.
    /// </summary>
    public static void ShouldHaveCountLessThan<T>([NotNull] this IEnumerable<T>? actual, int expected, string? customMessage = null,
        [CallerArgumentExpression(nameof(actual))] string? actualExpression = null) =>
        CheckCount(actual, expected, count => count < expected, customMessage, actualExpression);

    /// <summary>
    /// Asserts that the enumerable contains at most the specified number of elements.
    /// </summary>
    public static void ShouldHaveCountLessThanOrEqualTo<T>([NotNull] this IEnumerable<T>? actual, int expected, string? customMessage = null,
        [CallerArgumentExpression(nameof(actual))] string? actualExpression = null) =>
        CheckCount(actual, expected, count => count <= expected, customMessage, actualExpression);

    private static void CheckCount<T>([NotNull] IEnumerable<T>? actual, int expected, Func<int, bool> predicate,
        string? customMessage, string? actualExpression, [CallerMemberName] string shouldlyMethod = null!)
    {
        if (actual == null)
            throw new ShouldAssertException(new ExpectedActualShouldlyMessage(expected, null, customMessage, shouldlyMethod, actualExpression).ToString());

        var materialized = actual as IReadOnlyCollection<T> ?? actual.ToList();
        if (!predicate(materialized.Count))
            throw new ShouldAssertException(new ExpectedActualShouldlyMessage(expected, materialized, customMessage, shouldlyMethod, actualExpression).ToString());
    }
}
