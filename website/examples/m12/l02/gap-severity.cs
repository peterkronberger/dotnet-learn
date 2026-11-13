// Rate a number of failures with relational and logical patterns
static string Rate(int failures) => failures switch
{
    0 => "none",
    >= 1 and < 3 => "low",
    >= 3 and <= 5 => "medium",
    _ => "high",
};

int[] samples = [0, 2, 4, 9];
foreach (int sample in samples)
{
    Console.WriteLine($"{sample}: {Rate(sample)}");
}
