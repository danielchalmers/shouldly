namespace Shouldly.MessageGenerators;

abstract class ShouldlyMessageGenerator
{
    public abstract bool CanProcess(IShouldlyAssertionContext context);
    public abstract string GenerateErrorMessage(IShouldlyAssertionContext context);

    /// <summary>
    /// Looks up <paramref name="key"/> without requiring the actual to implement the non-generic
    /// <see cref="IDictionary"/>. Generic-only dictionaries (e.g. IReadOnlyDictionary&lt;,&gt;
    /// implementations or JsonObject) used to throw InvalidCastException here (#601). The
    /// <see cref="IDictionary"/> fast path keeps the original behaviour, including the dictionary's
    /// own key comparer; other types fall back to a linear scan over key/value pairs.
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