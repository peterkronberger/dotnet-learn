var total = 0;
var limit = 4;

for (var i = 1; i <= limit; i++)
{
    if (i % 2 == 0)
    {
        total += i * 10;
    }
    else
    {
        total += i;
    }

    Console.WriteLine($"i = {i}, total = {total}");
}

Console.WriteLine($"Result: {total}");
