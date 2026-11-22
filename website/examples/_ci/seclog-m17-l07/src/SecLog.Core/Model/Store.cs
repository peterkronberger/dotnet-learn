namespace SecLog.Model;

/// <summary>Stores items of one type in the order they were added.</summary>
/// <typeparam name="T">The type of the stored items.</typeparam>
public class Store<T> : IStore<T>
{
    private readonly List<T> items = [];

    /// <summary>The number of stored items.</summary>
    public int Count => items.Count;

    /// <summary>The stored items, in the order they were added.</summary>
    public IReadOnlyList<T> Items => items;

    /// <summary>Adds an item at the end.</summary>
    /// <param name="item">The item to store.</param>
    public void Add(T item)
    {
        items.Add(item);
    }

    /// <summary>Returns the item at a place (the first place is 0).</summary>
    /// <param name="index">The place of the item.</param>
    /// <returns>The stored item.</returns>
    public T Get(int index)
    {
        return items[index];
    }

    /// <summary>Returns the item at a place, or the default value.</summary>
    /// <param name="index">The place of the item.</param>
    /// <returns>The item, or the default value of T if there is none.</returns>
    public T? GetOrDefault(int index)
    {
        if (index < 0 || index >= items.Count)
        {
            return default;
        }
        return items[index];
    }

    /// <summary>Tries to read the item at a place.</summary>
    /// <param name="index">The place of the item.</param>
    /// <param name="item">The item, or the default value of T.</param>
    /// <returns>True if the place exists.</returns>
    public bool TryGet(int index, out T? item)
    {
        if (index < 0 || index >= items.Count)
        {
            item = default;
            return false;
        }
        item = items[index];
        return true;
    }
}
