static IEnumerable<int> Numbers()
{
    for (int i = 1; i <= 5; i++)
    {
        if (i == 3)
        {
            yield break;
        }
        yield return i;
    }
}

Console.WriteLine(string.Join(", ", Numbers()));
