List<Player> players = [new("bob", 30), new("ann", 10), new("dee", 20)];
players.Sort();
foreach (Player player in players)
{
    Console.WriteLine($"{player.Name}: {player.Score}");
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
