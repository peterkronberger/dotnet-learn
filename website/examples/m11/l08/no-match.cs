int[] numbers = [4, 8, 15, 16, 23, 42];

Try("First above 100", () => numbers.First(n => n > 100));
Try("Last above 100", () => numbers.Last(n => n > 100));
Try("Single above 100", () => numbers.Single(n => n > 100));
Try("Single above 10", () => numbers.Single(n => n > 10));
Try("ElementAt 10", () => numbers.ElementAt(10));
Try("First of empty", () => new int[0].First());

static void Try<T>(string name, Func<T> operation)
{
    try
    {
        Console.WriteLine($"{name}: {operation()}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"{name}: {ex.GetType().Name}");
    }
}
