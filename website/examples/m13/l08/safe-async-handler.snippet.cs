// An async void handler is allowed because EventHandler returns void.
// Catch every exception inside it: nobody else can.
alarm.Raised += async (sender, e) =>
{
    try
    {
        await SendAsync();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Sending failed: {ex.GetType().Name}");
    }
};
