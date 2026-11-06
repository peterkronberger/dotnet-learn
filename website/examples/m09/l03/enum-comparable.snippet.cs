Counter<Level> levels = new();

enum Level { Low, High }

class Counter<TKey> where TKey : notnull, IComparable<TKey>
{
}
