List<Func<int>> reads = [];
int value = 1;
reads.Add(() => value);
value = 2;
reads.Add(() => value);
value = 3;

foreach (Func<int> read in reads)
{
    Console.WriteLine(read());
}
