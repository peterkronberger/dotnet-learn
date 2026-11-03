Watchlist watchlist = new Watchlist();
watchlist.Users.Add("bob");
watchlist.Users = [];

class Watchlist
{
    public readonly List<string> Users = [];
}
