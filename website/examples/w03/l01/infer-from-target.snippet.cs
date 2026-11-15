Console.WriteLine(Largest(3, 9));
List<int> numbers = Create();

static T Largest<T>(T a, T b) where T : IComparable<T> => a.CompareTo(b) >= 0 ? a : b;
static List<T> Create<T>() => new List<T>();
