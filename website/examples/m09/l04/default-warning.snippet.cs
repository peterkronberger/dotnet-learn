static T First<T>(List<T> items)
{
    if (items.Count == 0)
    {
        return default;
    }
    return items[0];
}
