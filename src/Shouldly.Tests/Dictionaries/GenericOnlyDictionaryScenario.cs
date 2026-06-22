using System.Collections;

namespace Shouldly.Tests.Dictionaries;

// Reproduces #601: building the failure message cast the actual to the non-generic
// System.Collections.IDictionary, which threw InvalidCastException for dictionaries that
// implement only IDictionary<TKey,TValue> (e.g. IHeaderDictionary, System.Text.Json's
// JsonObject, or a mocking-framework proxy).
public class GenericOnlyDictionaryScenario
{
    private static GenericOnlyDictionary<string, string> StringDictionary() =>
        new(new Dictionary<string, string> { ["Foo"] = "Bar" });

    [Fact]
    public void ContainKeyAndValueWhenValueDiffersShouldFail()
    {
        Verify.ShouldFail(() =>
            StringDictionary().ShouldContainKeyAndValue("Foo", "WrongValue", "Some additional context"));
    }

    [Fact]
    public void ContainKeyAndValueWhenKeyMissingShouldFail()
    {
        Verify.ShouldFail(() =>
            StringDictionary().ShouldContainKeyAndValue("Missing", "Bar", "Some additional context"));
    }

    [Fact]
    public void NotContainValueForKeyWhenPresentShouldFail()
    {
        Verify.ShouldFail(() =>
            StringDictionary().ShouldNotContainValueForKey("Foo", "Bar", "Some additional context"));
    }

    [Fact]
    public void ShouldPass()
    {
        StringDictionary().ShouldContainKeyAndValue("Foo", "Bar");
        StringDictionary().ShouldNotContainValueForKey("Foo", "NotBar");
    }
}

/// <summary>
/// A dictionary that implements <see cref="IDictionary{TKey,TValue}"/> (and therefore
/// <see cref="IEnumerable{T}"/> of key/value pairs) but deliberately NOT the non-generic
/// <see cref="IDictionary"/>, mirroring types such as IHeaderDictionary or JsonObject.
/// </summary>
public sealed class GenericOnlyDictionary<TKey, TValue>(IDictionary<TKey, TValue> items)
    : IDictionary<TKey, TValue>
    where TKey : notnull
{
    private readonly Dictionary<TKey, TValue> _inner = new(items);

    public TValue this[TKey key] { get => _inner[key]; set => _inner[key] = value; }
    public ICollection<TKey> Keys => _inner.Keys;
    public ICollection<TValue> Values => _inner.Values;
    public int Count => _inner.Count;
    public bool IsReadOnly => false;
    public void Add(TKey key, TValue value) => _inner.Add(key, value);
    public void Add(KeyValuePair<TKey, TValue> item) => ((IDictionary<TKey, TValue>)_inner).Add(item);
    public void Clear() => _inner.Clear();
    public bool Contains(KeyValuePair<TKey, TValue> item) => ((IDictionary<TKey, TValue>)_inner).Contains(item);
    public bool ContainsKey(TKey key) => _inner.ContainsKey(key);
    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) => ((IDictionary<TKey, TValue>)_inner).CopyTo(array, arrayIndex);
    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _inner.GetEnumerator();
    public bool Remove(TKey key) => _inner.Remove(key);
    public bool Remove(KeyValuePair<TKey, TValue> item) => ((IDictionary<TKey, TValue>)_inner).Remove(item);
    public bool TryGetValue(TKey key, out TValue value) => _inner.TryGetValue(key, out value!);
    IEnumerator IEnumerable.GetEnumerator() => _inner.GetEnumerator();
}

#if NET9_0_OR_GREATER
// The IReadOnlyDictionary<,> overloads (net9+) are the worst case for #601: a read-only
// dictionary never implements the non-generic IDictionary, so message generation used to
// throw for every such type (the existing tests only passed because they used a backing
// Dictionary<,>, which does implement IDictionary).
public class ReadOnlyOnlyDictionaryScenario
{
    private static ReadOnlyOnlyDictionary<string, string> StringDictionary() =>
        new(new Dictionary<string, string> { ["Foo"] = "Bar" });

    [Fact]
    public void ContainKeyAndValueWhenValueDiffersShouldFail()
    {
        Verify.ShouldFail(() =>
            StringDictionary().ShouldContainKeyAndValue("Foo", "WrongValue", "Some additional context"));
    }

    [Fact]
    public void NotContainValueForKeyWhenPresentShouldFail()
    {
        Verify.ShouldFail(() =>
            StringDictionary().ShouldNotContainValueForKey("Foo", "Bar", "Some additional context"));
    }

    [Fact]
    public void ShouldPass()
    {
        StringDictionary().ShouldContainKeyAndValue("Foo", "Bar");
        StringDictionary().ShouldNotContainValueForKey("Foo", "NotBar");
    }
}

/// <summary>
/// A dictionary that implements only <see cref="IReadOnlyDictionary{TKey,TValue}"/>, never the
/// non-generic <see cref="IDictionary"/> (which read-only dictionaries cannot).
/// </summary>
public sealed class ReadOnlyOnlyDictionary<TKey, TValue>(IReadOnlyDictionary<TKey, TValue> items)
    : IReadOnlyDictionary<TKey, TValue>
    where TKey : notnull
{
    private readonly Dictionary<TKey, TValue> _inner = items.ToDictionary(x => x.Key, x => x.Value);

    public TValue this[TKey key] => _inner[key];
    public IEnumerable<TKey> Keys => _inner.Keys;
    public IEnumerable<TValue> Values => _inner.Values;
    public int Count => _inner.Count;
    public bool ContainsKey(TKey key) => _inner.ContainsKey(key);
    public bool TryGetValue(TKey key, out TValue value) => _inner.TryGetValue(key, out value!);
    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _inner.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => _inner.GetEnumerator();
}
#endif
