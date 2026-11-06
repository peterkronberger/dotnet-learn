Gate gate = new();
gate.Opened += (sender, e) => Console.WriteLine($"first {e.Number}");
gate.Opened += (sender, e) => Console.WriteLine($"second {e.Number}");
gate.Open(5);

sealed class OpenedEventArgs : EventArgs
{
    public OpenedEventArgs(int number)
    {
        Number = number;
    }

    public int Number { get; }
}

sealed class Gate
{
    public event EventHandler<OpenedEventArgs>? Opened;

    public void Open(int number)
    {
        Console.WriteLine("open");
        OnOpened(new OpenedEventArgs(number));
        Console.WriteLine("done");
    }

    private void OnOpened(OpenedEventArgs e)
    {
        Opened?.Invoke(this, e);
    }
}
