var counts = CountEvents();
Console.WriteLine($"{counts.Failed} of {counts.Total}");

(int Failed, int Total) CountEvents()
{
    return (0, 0);
}
