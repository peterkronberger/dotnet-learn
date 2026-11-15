int[] numbers = [0, 1, 2];

var forActions = new List<Action>();
for (int i = 0; i < 3; i++)
{
    forActions.Add(() => Console.WriteLine(i));
}

var foreachActions = new List<Action>();
foreach (int n in numbers)
{
    foreachActions.Add(() => Console.WriteLine(n));
}

foreach (Action action in forActions)
{
    action();
}

foreach (Action action in foreachActions)
{
    action();
}
