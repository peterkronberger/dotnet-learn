FailureCounter counter = new FailureCounter();
counter.count = -5;

class FailureCounter
{
    private int count;

    public int Current()
    {
        return count;
    }
}
