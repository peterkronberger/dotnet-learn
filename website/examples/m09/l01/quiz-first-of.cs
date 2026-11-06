Console.WriteLine(FirstOf(["x", "y"], "-"));
Console.WriteLine(FirstOf<int>([], -1));
Console.WriteLine(FirstOf([3, 4], 0));

static T FirstOf<T>(List<T> items, T fallback)
{
    if (items.Count == 0)
    {
        return fallback;
    }
    return items[0];
}
