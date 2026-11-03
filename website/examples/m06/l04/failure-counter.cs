FailureCounter counter = new FailureCounter();
counter.Count = 3;
counter.Count = -2;
Console.WriteLine(counter.Count);

class FailureCounter
{
    private int count;

    public int Count
    {
        get { return count; }
        set
        {
            if (value >= 0)
            {
                count = value;
            }
        }
    }
}
