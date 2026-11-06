Tag a = new("ab");
Tag b = new("cd");
Console.WriteLine(a.GetHashCode() == b.GetHashCode());
Console.WriteLine(a.Equals(b));

HashSet<Tag> tags = [a, b, new Tag("ab")];
Console.WriteLine(tags.Count);

class Tag(string name)
{
    public string Name => name;

    public override bool Equals(object? obj)
    {
        return obj is Tag other && Name == other.Name;
    }

    public override int GetHashCode()
    {
        return Name.Length;
    }
}
