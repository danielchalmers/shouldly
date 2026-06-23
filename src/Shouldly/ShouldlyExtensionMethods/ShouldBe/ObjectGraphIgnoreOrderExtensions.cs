using System.ComponentModel;

namespace Shouldly;

/// <summary>
/// Order-independent variant of <see cref="ObjectGraphTestExtensions.ShouldBeEquivalentTo(object, object, string, string)"/>.
/// Shouldly's default ShouldBeEquivalentTo compares enumerables strictly by index; FluentAssertions'
/// BeEquivalentTo is order-INDEPENDENT for collections by default. This overload restores that behaviour:
/// when ignoreOrder is true the top-level collections are matched as multisets, each actual element
/// paired with a distinct structurally-equivalent expected element.
/// </summary>
public static partial class ObjectGraphTestExtensions
{
    /// <summary>
    /// Asserts that an object is equivalent to another by deep member comparison, optionally ignoring
    /// the order of elements in the top-level collection.
    /// </summary>
    [RequiresUnreferencedCode("Walks the actual/expected object graph using reflection.")]
    public static void ShouldBeEquivalentTo(
        [NotNullIfNotNull(nameof(expected))] this object? actual,
        [NotNullIfNotNull(nameof(actual))] object? expected,
        bool ignoreOrder,
        string? customMessage = null,
        [CallerArgumentExpression(nameof(actual))] string? actualExpression = null)
    {
        if (!ignoreOrder)
        {
            actual.ShouldBeEquivalentTo(expected, customMessage, actualExpression);
            return;
        }

        if (actual is IEnumerable actualEnumerable and not string &&
            expected is IEnumerable expectedEnumerable and not string)
        {
            var actualList = actualEnumerable.Cast<object?>().ToList();
            var expectedList = expectedEnumerable.Cast<object?>().ToList();

            if (actualList.Count != expectedList.Count)
            {
                throw new ShouldAssertException(
                    new ExpectedActualShouldlyMessage($"equivalent collection of {expectedList.Count} item(s) (ignoring order)", actual, customMessage, actualExpression: actualExpression).ToString());
            }

            var matched = new bool[expectedList.Count];
            foreach (var actualItem in actualList)
            {
                var found = false;
                for (var j = 0; j < expectedList.Count; j++)
                {
                    if (matched[j])
                        continue;

                    if (AreEquivalent(actualItem, expectedList[j]))
                    {
                        matched[j] = true;
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    throw new ShouldAssertException(
                        new ExpectedActualShouldlyMessage("an equivalent element (ignoring order)", actualItem, customMessage, actualExpression: actualExpression).ToString());
                }
            }

            return;
        }

        // Not a pair of collections: order is meaningless, fall back to the regular deep compare.
        actual.ShouldBeEquivalentTo(expected, customMessage, actualExpression);
    }

    [RequiresUnreferencedCode("Walks the actual/expected object graph using reflection.")]
    private static bool AreEquivalent(object? actual, object? expected)
    {
        try
        {
            CompareObjects(actual, expected, new List<string>(), new Dictionary<object, IList<object?>>(), null);
            return true;
        }
        catch (ShouldAssertException)
        {
            return false;
        }
    }
}
