using System.Collections;

namespace Shouldly.Tests.Dictionaries;

// Reproduces #601: the failure message cast the actual to non-generic IDictionary, throwing
// InvalidCastException for dictionaries that implement only IDictionary<TKey,TValue>.
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

// Implements only IDictionary<TKey,TValue>, deliberately not the non-generic IDictionary.
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
// A read-only dictionary can never implement the non-generic IDictionary, so #601 used to
// throw for every IReadOnlyDictionary<,> (net9+ overloads).
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

// Implements only IReadOnlyDictionary<TKey,TValue>, never the non-generic IDictionary.
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
