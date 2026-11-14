try
{
    SendAlert("bob");
}
catch (InvalidOperationException)
{
    Console.WriteLine("Caught");
}

async void SendAlert(string user)
{
    await Task.Delay(10);
    throw new InvalidOperationException("sending failed");
}
