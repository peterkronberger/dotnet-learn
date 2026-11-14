// CPU-bound work goes to a thread pool thread.
Task<int> pending = Task.Run(() => CountPrimesBelow(20_000));

// The caller can do other things here, then it collects the result.
int count = await pending;
Console.WriteLine($"Primes below 20000: {count}");

static int CountPrimesBelow(int limit)
{
    int count = 0;
    for (int n = 2; n < limit; n++)
    {
        bool isPrime = true;
        for (int d = 2; d * d <= n; d++)
        {
            if (n % d == 0)
            {
                isPrime = false;
                break;
            }
        }

        if (isPrime)
        {
            count++;
        }
    }

    return count;
}
