int inner = 0;
for (int outer = 1; outer <= 3; outer++)
{
    for (int step = 1; step <= 2; step++)
    {
        inner++;
        Console.WriteLine($"outer {outer}, step {step}");
    }
}
Console.WriteLine($"inner block ran {inner} times");
