Player low = new("ann", 10);
Player high = new("bob", 30);
Player same = new("cy", 10);
Console.WriteLine(low.CompareTo(high) < 0);
Console.WriteLine(high.CompareTo(low) > 0);
Console.WriteLine(low.CompareTo(same) == 0);
Console.WriteLine(low.CompareTo(null) > 0);

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
