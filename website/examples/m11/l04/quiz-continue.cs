static IEnumerable<int> Numbers()
{
    yield return 1;
    yield return 2;
    yield return 3;
}

Console.WriteLine(string.Join(", ", Numbers()));
