Action steps = () => Console.WriteLine("step 1");
steps += () =>
{
    throw new InvalidOperationException("step 2 failed");
};
steps += () => Console.WriteLine("step 3");

try
{
    steps();
}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"caught: {ex.Message}");
}
