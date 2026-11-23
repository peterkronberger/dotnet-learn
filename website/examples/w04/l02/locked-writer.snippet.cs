// Several tasks call Add. Another task reads Total at the same time.
class FailureCounter
{
    private readonly Lock _gate = new();
    private int _count;

    public void Add()
    {
        lock (_gate)
        {
            _count++;
        }
    }

    public int Total => _count;
}
