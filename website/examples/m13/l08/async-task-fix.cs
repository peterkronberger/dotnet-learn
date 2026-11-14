// Predict the output: the same method as async Task, awaited by the caller
Console.WriteLine("Start");
await SendAlertAsync("bob");
Console.WriteLine("After the call");

try
{
    await SendAlertAsync("");
}
catch (ArgumentException ex)
{
    Console.WriteLine($"Caught: {ex.GetType().Name}");
}

async Task SendAlertAsync(string user)
{
    if (user == "")
    {
        throw new ArgumentException("user is empty");
    }
    Console.WriteLine($"Sending alert for {user}");
    await Task.Delay(10);
    Console.WriteLine($"Alert for {user} sent");
}
