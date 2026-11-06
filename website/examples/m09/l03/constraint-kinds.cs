Animal cat = new() { Name = "cat" };
Animal other = new() { Name = "cat" };

Console.WriteLine(Shout(cat));
Console.WriteLine(Same(cat, other));
Console.WriteLine(Same(cat, cat));
Console.WriteLine(Show(42));
Console.WriteLine(Make<Animal>().Name);
Console.WriteLine(Make<List<int>>().Count);

static string Shout<T>(T animal) where T : Animal
{
    return animal.Name.ToUpper();
}

static bool Same<T>(T a, T b) where T : class
{
    return a == b;
}

static string Show<T>(T value) where T : struct
{
    return $"struct: {value}";
}

static T Make<T>() where T : new()
{
    return new T();
}

class Animal
{
    public string Name { get; set; } = "unknown";
}
