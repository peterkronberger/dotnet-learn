int[] empty = [];
string[] noWords = [];

Console.WriteLine($"Count: {empty.Count()}");
Console.WriteLine($"Sum: {empty.Sum()}");
Try("Average", () => empty.Average());
Try("Min", () => empty.Min());
Try("Max", () => empty.Max());
Try("MaxBy of ints", () => empty.MaxBy(n => n));
Try("MaxBy of strings", () => noWords.MaxBy(w => w.Length));

int?[] noValues = [];
Console.WriteLine($"Average of int?: {noValues.Average() is null}");

static void Try<T>(string name, Func<T> operation)
{
    try
    {
        Console.WriteLine($"{name}: {operation()}");
    }
    catch (InvalidOperationException ex)
    {
        Console.WriteLine($"{name}: {ex.GetType().Name}");
    }
}
