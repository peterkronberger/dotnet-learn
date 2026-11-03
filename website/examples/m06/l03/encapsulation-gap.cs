// Goal: Add(-2) is rejected, so the count stays 3.
FailureCounter counter = new FailureCounter();
Console.WriteLine(counter.Add(3));
Console.WriteLine(counter.Add(-2));
Console.WriteLine(counter.Current());

class FailureCounter
{
    private int count;

    public bool Add(int number)
    {
        count += number;
        return true;
    }

    public int Current()
    {
        return count;
    }
}
