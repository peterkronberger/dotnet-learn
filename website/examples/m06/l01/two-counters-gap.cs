// Goal: logins ends with 2 and logouts ends with 1.
Counter logins = new Counter();
Counter logouts = logins;

logins.Increment();
logins.Increment();
logouts.Increment();

Console.WriteLine($"logins: {logins.Value}");
Console.WriteLine($"logouts: {logouts.Value}");

class Counter
{
    public int Value;

    public void Increment()
    {
        Value++;
    }
}
