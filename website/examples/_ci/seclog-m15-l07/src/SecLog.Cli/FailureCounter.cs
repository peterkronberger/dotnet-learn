// One counter for the whole provider (singleton): all runs add to it
sealed class FailureCounter
{
    private int total;

    public int Total => total;

    public void Add(int failures) => Interlocked.Add(ref total, failures);
}
