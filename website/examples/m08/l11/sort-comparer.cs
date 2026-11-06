List<Player> players = [new("bob", 30), new("ann", 10), new("dee", 20)];
players.Sort();
Console.WriteLine($"By score: {Names(players)}");
players.Sort(new NameComparer());
Console.WriteLine($"By name: {Names(players)}");

string Names(List<Player> list)
{
    List<string> names = [];
    foreach (Player player in list)
    {
        names.Add(player.Name);
    }
    return string.Join(", ", names);
}

class Player : IComparable<Player>
{
    public string Name { get; }
    public int Score { get; }

    public Player(string name, int score)
    {
        Name = name;
        Score = score;
    }

    public int CompareTo(Player? other)
    {
        if (other is null)
        {
            return 1;
        }
        return Score.CompareTo(other.Score);
    }
}

class NameComparer : IComparer<Player>
{
    public int Compare(Player? x, Player? y)
    {
        if (ReferenceEquals(x, y))
        {
            return 0;
        }
        if (x is null)
        {
            return -1;
        }
        if (y is null)
        {
            return 1;
        }
        return string.CompareOrdinal(x.Name, y.Name);
    }
}
