List<Func<int>> forLambdas = [];
for (int i = 0; i < 3; i++)
{
    forLambdas.Add(() => i);
}

List<Func<int>> foreachLambdas = [];
foreach (int number in new[] { 0, 1, 2 })
{
    foreachLambdas.Add(() => number);
}

foreach (Func<int> lambda in forLambdas)
{
    Console.WriteLine(lambda());
}

foreach (Func<int> lambda in foreachLambdas)
{
    Console.WriteLine(lambda());
}
