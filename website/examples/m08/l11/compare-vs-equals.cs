Player ann = new("ann", 10);
Player cy = new("cy", 10);
Console.WriteLine($"Same place in the order: {ann.CompareTo(cy) == 0}");
Console.WriteLine($"Equal: {ann.Equals(cy)}");

HashSet<Player> set = [ann, cy];
Console.WriteLine($"Set size: {set.Count}");

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
