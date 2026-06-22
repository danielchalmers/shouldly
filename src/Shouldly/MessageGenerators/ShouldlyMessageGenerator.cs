namespace Shouldly.MessageGenerators;

abstract class ShouldlyMessageGenerator
{
    public abstract bool CanProcess(IShouldlyAssertionContext context);
    public abstract string GenerateErrorMessage(IShouldlyAssertionContext context);

    /// <summary>
    /// Looks up <paramref name="key"/> in a dictionary actual without requiring it to implement
    /// the non-generic <see cref="IDictionary"/>. Types such as IHeaderDictionary,
    /// IReadOnlyDictionary&lt;,&gt; implementations, System.Text.Json's JsonObject and
    /// mocking-framework proxies implement only the generic dictionary interface (and therefore
    /// IEnumerable&lt;KeyValuePair&lt;,&gt;&gt;); casting them to IDictionary threw
    /// InvalidCastException while building the failure message (#601). The IDictionary fast path
    /// preserves the exact behaviour (including the dictionary's own key comparer) for the common
    /// case; only types that lack it fall back to a linear scan.
    /// </summary>
    [UnconditionalSuppressMessage("Trimming", "IL2075",
        Justification = "KeyValuePair<TKey, TValue> is a BCL type whose Key and Value properties define its public contract and are preserved by the trimmer.")]
    protected static (bool Exists, object? Value) LookupKey(object dictionary, object key)
    {
        if (dictionary is IDictionary nonGeneric)
        {
            return nonGeneric.Contains(key)
                ? (true, nonGeneric[key])
                : (false, null);
        }

        foreach (var entry in (IEnumerable)dictionary)
        {
            if (entry is null)
                continue;

            var entryType = entry.GetType();
            if (Equals(entryType.GetProperty("Key")?.GetValue(entry), key))
                return (true, entryType.GetProperty("Value")?.GetValue(entry));
        }

        return (false, null);
    }
}