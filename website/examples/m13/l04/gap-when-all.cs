// Fill the gaps: start both tasks first, then wait for both
Task<int> one = NumberAsync(1);
Task<int> two = NumberAsync(2);
int[] numbers = await Task.WhenAll(one, two);
Console.WriteLine($"Sum: {numbers.Sum()}");

async Task<int> NumberAsync(int value)
{
    await Task.Delay(10);
    return value;
}
