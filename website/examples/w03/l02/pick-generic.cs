List<string> users = ["ana", "ben"];
List<int> empty = [];

Console.WriteLine(Pick(users, "nobody"));
Console.WriteLine(Pick(empty, -1));

static T Pick<T>(List<T> items, T fallback)
{
    if (items.Count == 0)
    {
        return fallback;
    }
    return items[0];
}
