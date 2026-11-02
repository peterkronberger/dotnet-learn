Console.WriteLine(Count("a", "b", "c"));
Console.WriteLine(Count());

int Count(params string[] words)
{
    int found = 0;
    foreach (string word in words)
    {
        found++;
    }
    return found;
}
