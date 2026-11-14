// A counter that several tasks use at the same time.
class Counter
{
    private volatile int _count;

    public void Add()
    {
        _count++;
    }

    public int Count => _count;
}
