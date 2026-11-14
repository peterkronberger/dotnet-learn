Console.WriteLine("Start");
SendAlert("bob");
Console.WriteLine("After the call");

async void SendAlert(string user)
{
    Console.WriteLine($"Sending alert for {user}");
    await Task.Delay(10);
    Console.WriteLine($"Alert for {user} sent");
}
