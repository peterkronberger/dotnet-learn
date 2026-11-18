protected override async Task ExecuteAsync(CancellationToken stoppingToken)
{
    while (!stoppingToken.IsCancellationRequested)
    {
        Console.WriteLine("Checking the log source");
        await Task.Delay(1000, stoppingToken);
    }
}
