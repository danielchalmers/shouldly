namespace Shouldly;

/// <summary>
/// Order-independent variant of ShouldBeEquivalentTo. Shouldly's default compares enumerables strictly by
/// index; FluentAssertions' BeEquivalentTo is order-INDEPENDENT for collections by default. With
/// <see cref="EquivalencyOptions.IgnoreOrder"/> the top-level collections are matched as multisets: each
/// actual element is paired with a distinct structurally-equivalent expected element (a maximum bipartite
/// matching, so duplicates and ambiguous equivalences are handled correctly).
/// </summary>
public static partial class ObjectGraphTestExtensions
{
    /// <summary>
    /// Asserts that an object is equivalent to another by deep member comparison, ignoring the order of
    /// elements in the top-level collection. Convenience for <c>ShouldBeEquivalentTo(expected, new EquivalencyOptions { IgnoreOrder = ignoreOrder })</c>.
    /// </summary>
    [RequiresUnreferencedCode("Walks the actual/expected object graph using reflection.")]
    public static void ShouldBeEquivalentTo(
        [NotNullIfNotNull(nameof(expected))] this object? actual,
        [NotNullIfNotNull(nameof(actual))] object? expected,
        bool ignoreOrder,
        string? customMessage = null,
        [CallerArgumentExpression(nameof(actual))] string? actualExpression = null) =>
        actual.ShouldBeEquivalentTo(expected, new EquivalencyOptions { IgnoreOrder = ignoreOrder }, customMessage, actualExpression);

    /// <summary>
    /// Asserts that an object is equivalent to another by deep member comparison, honouring the supplied options.
    /// </summary>
    [RequiresUnreferencedCode("Walks the actual/expected object graph using reflection.")]
    public static void ShouldBeEquivalentTo(
        [NotNullIfNotNull(nameof(expected))] this object? actual,
        [NotNullIfNotNull(nameof(actual))] object? expected,
        EquivalencyOptions options,
        string? customMessage = null,
        [CallerArgumentExpression(nameof(actual))] string? actualExpression = null)
    {
        if (!options.IgnoreOrder)
        {
            actual.ShouldBeEquivalentTo(expected, customMessage, actualExpression);
            return;
        }

        if (actual is IEnumerable actualEnumerable and not string &&
            expected is IEnumerable expectedEnumerable and not string)
        {
            var actualList = actualEnumerable.Cast<object?>().ToList();
            var expectedList = expectedEnumerable.Cast<object?>().ToList();

            if (actualList.Count != expectedList.Count || !HasPerfectMatching(actualList, expectedList))
            {
                throw new ShouldAssertException(
                    new ExpectedActualShouldlyMessage(expectedList, actualList, customMessage, actualExpression: actualExpression).ToString());
            }

            return;
        }

        // Not a pair of collections: order is meaningless, fall back to the regular deep compare.
        actual.ShouldBeEquivalentTo(expected, customMessage, actualExpression);
    }

    // Each actual element must pair with a distinct equivalent expected element. Because the two lists are
    // the same length, a perfect matching exists iff they are equivalent as multisets. Solved with Kuhn's
    // augmenting-path algorithm (O(V*E)); collection sizes in tests are small.
    [RequiresUnreferencedCode("Walks the actual/expected object graph using reflection.")]
    private static bool HasPerfectMatching(List<object?> actualList, List<object?> expectedList)
    {
        var n = actualList.Count;
        var candidates = new List<int>[n];
        for (var i = 0; i < n; i++)
        {
            candidates[i] = [];
            for (var j = 0; j < n; j++)
            {
                if (AreEquivalent(actualList[i], expectedList[j]))
                    candidates[i].Add(j);
            }
        }

        var expectedMatchedToActual = new int[n];
        for (var k = 0; k < n; k++)
            expectedMatchedToActual[k] = -1;

        var matched = 0;
        for (var i = 0; i < n; i++)
        {
            var seen = new bool[n];
            if (TryAssign(i, candidates, expectedMatchedToActual, seen))
                matched++;
        }

        return matched == n;
    }

    private static bool TryAssign(int actualIndex, List<int>[] candidates, int[] expectedMatchedToActual, bool[] seen)
    {
        foreach (var expectedIndex in candidates[actualIndex])
        {
            if (seen[expectedIndex])
                continue;
            seen[expectedIndex] = true;

            if (expectedMatchedToActual[expectedIndex] == -1 ||
                TryAssign(expectedMatchedToActual[expectedIndex], candidates, expectedMatchedToActual, seen))
            {
                expectedMatchedToActual[expectedIndex] = actualIndex;
                return true;
            }
        }

        return false;
    }

    [RequiresUnreferencedCode("Walks the actual/expected object graph using reflection.")]
    private static bool AreEquivalent(object? actual, object? expected)
    {
        try
        {
            // We only use the deep compare as a boolean probe; its message is never surfaced, so opt into
            // the stack-walk fallback rather than tripping the CallerArgumentExpression guard.
            using (ShouldlyConfiguration.AllowStackWalking())
                CompareObjects(actual, expected, new List<string>(), new Dictionary<object, IList<object?>>(), null);
            return true;
        }
        catch (ShouldAssertException)
        {
            return false;
        }
    }
}
