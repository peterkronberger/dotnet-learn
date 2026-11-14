// The loop body changes one shared variable: a race condition.
int[] numbers = [1, 2, 3, 4, 5, 6, 7, 8];
int total = 0;

Parallel.ForEach(numbers, n =>
{
    total += n;
});

Console.WriteLine(total);
