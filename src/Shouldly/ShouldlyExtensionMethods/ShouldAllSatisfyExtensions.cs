using System.ComponentModel;
using JetBrains.Annotations;
using NotNullAttribute = System.Diagnostics.CodeAnalysis.NotNullAttribute;

namespace Shouldly;

/// <summary>
/// Asserts that every element of an enumerable satisfies an assertion action. Unlike
/// <see cref="ShouldBeEnumerableTestExtensions.ShouldAllBe{T}"/> (which takes a boolean predicate),
/// this runs arbitrary Shouldly assertions per element and aggregates every failure into one report.
/// Mirrors FluentAssertions' AllSatisfy.
/// </summary>
[ShouldlyMethods]
[EditorBrowsable(EditorBrowsableState.Never)]
public static partial class ShouldAllSatisfyExtensions
{
    /// <summary>
    /// Asserts that every element satisfies the supplied assertion action, reporting every failure at once.
    /// </summary>
    public static void ShouldAllSatisfy<T>([NotNull] this IEnumerable<T>? actual, [InstantHandle] Action<T> elementAssertion, string? customMessage = null,
        [CallerArgumentExpression(nameof(actual))] string? actualExpression = null)
    {
        if (actual == null)
            throw new ShouldAssertException(new ExpectedShouldlyMessage(actual, customMessage, actualExpression: actualExpression).ToString());

        var failures = new List<string>();
        var index = 0;
        foreach (var item in actual)
        {
            try
            {
                elementAssertion(item);
            }
            catch (Exception ex)
            {
                failures.Add($"  [{index}] {ex.Message}");
            }

            index++;
        }

        if (failures.Count > 0)
        {
            var detail = $"all elements to satisfy the assertion, but {failures.Count} did not:{Environment.NewLine}{string.Join(Environment.NewLine, failures)}";
            throw new ShouldAssertException(new ExpectedActualShouldlyMessage(detail, actual, customMessage, actualExpression: actualExpression).ToString());
        }
    }
}
