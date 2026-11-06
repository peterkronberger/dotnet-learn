Func<int, string> label = Label;
Action<int> show = Show;

Console.WriteLine(label(3).Length);
show(label(3).Length);

static string Label(int number)
{
    return $"item-{number}";
}

static void Show(int number)
{
    Console.WriteLine($"Length {number}");
}
