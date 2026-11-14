// Wait for a Task<int> and use its result
int answer = await GetAnswerAsync();
Console.WriteLine($"Answer: {answer}");

async Task<int> GetAnswerAsync()
{
    await Task.Delay(10);
    return 42;
}
