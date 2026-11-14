// Two tasks each add 1000 to a shared counter. The result is not always 2000.
int count = 0;

Task first = Task.Run(() =>
{
    for (int i = 0; i < 1000; i++)
    {
        count++;
    }
});
Task second = Task.Run(() =>
{
    for (int i = 0; i < 1000; i++)
    {
        count++;
    }
});

await Task.WhenAll(first, second);
Console.WriteLine($"Count: {count}");
