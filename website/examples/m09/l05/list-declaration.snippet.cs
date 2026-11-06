// Shortened from the reference page of List<T>
public class List<T> : IList<T>, ICollection<T>, IEnumerable<T>,
    IReadOnlyList<T>, IReadOnlyCollection<T> { /* … */ }

// Shortened from the reference page of Dictionary<TKey,TValue>
public class Dictionary<TKey, TValue> :
    IDictionary<TKey, TValue>, IReadOnlyDictionary<TKey, TValue>
    where TKey : notnull { /* … */ }
