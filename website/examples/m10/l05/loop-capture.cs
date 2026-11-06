List<Action> fromFor = [];
for (int i = 0; i < 3; i++)
{
    fromFor.Add(() => Console.WriteLine($"for: {i}"));
}

List<Action> fromForeach = [];
int[] numbers = [0, 1, 2];
foreach (int number in numbers)
{
    fromForeach.Add(() => Console.WriteLine($"foreach: {number}"));
}

foreach (Action action in fromFor)
{
    action();
}
foreach (Action action in fromForeach)
{
    action();
}
