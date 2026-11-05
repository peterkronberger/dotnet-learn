// Give InvalidPortException a Port property that the constructor sets.
// Throw the exception with the port number and the message.
// Wanted output: Rejected port: 70000
try
{
    Check(70000);
}
catch (InvalidPortException ex)
{
    Console.WriteLine($"Rejected port: {ex.Port}");
}

void Check(int port)
{
    if (port > 65535)
    {
        throw new InvalidPortException("The port is too large.");
    }
}

class InvalidPortException : Exception
{
    public int Port { get; }

    public InvalidPortException(string message)
        : base(message)
    {
    }
}
