namespace SecLog.Model;

/// <summary>A store of items of one type.</summary>
/// <typeparam name="T">The type of the stored items.</typeparam>
interface IStore<T>
{
    /// <summary>Adds an item at the end.</summary>
    /// <param name="item">The item to store.</param>
    void Add(T item);

    /// <summary>The number of stored items.</summary>
    int Count { get; }

    /// <summary>The stored items, in the order they were added.</summary>
    IReadOnlyList<T> Items { get; }
}
