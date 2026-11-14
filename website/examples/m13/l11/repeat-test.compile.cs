// A test that repeats the racy counter 100 times and expects 2000 each time.
int wrong = 0;

for (int run = 0; run < 100; run++)
{
    int count = 0;
    Action addThousand = () =>
    {
        for (int i = 0; i < 1000; i++)
        {
            count++;
        }
    };
    await Task.WhenAll(Task.Run(addThousand), Task.Run(addThousand));
    if (count != 2000)
    {
        wrong++;
    }
}
Console.WriteLine($"Wrong runs: {wrong}");
