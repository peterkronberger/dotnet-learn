namespace SecLog.Model;

/// <summary>Counts how often each key was added.</summary>
/// <typeparam name="TKey">The type of the keys. It must be sortable.</typeparam>
public class Counter<TKey> where TKey : notnull, IComparable<TKey>
{
    private readonly Dictionary<TKey, int> counts = [];

    /// <summary>Counts one more occurrence of a key.</summary>
    /// <param name="key">The key to count.</param>
    public void Add(TKey key)
    {
        counts.TryGetValue(key, out int current);
        counts[key] = current + 1;
    }

    /// <summary>Returns how often a key was added (0 if never).</summary>
    /// <param name="key">The key to look up.</param>
    /// <returns>The count of the key.</returns>
    public int CountOf(TKey key)
    {
        if (counts.TryGetValue(key, out int current))
        {
            return current;
        }
        return 0;
    }

    /// <summary>Returns a sorted copy of all keys.</summary>
    /// <returns>The keys in ascending order.</returns>
    public List<TKey> SortedKeys()
    {
        List<TKey> keys = new(counts.Keys);
        keys.Sort();
        return keys;
    }
}
