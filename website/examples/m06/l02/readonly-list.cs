Watchlist watchlist = new Watchlist();
watchlist.Users.Add("bob");
watchlist.Users.Add("dave");
Console.WriteLine(watchlist.Users.Count);

class Watchlist
{
    public readonly List<string> Users = [];
}
