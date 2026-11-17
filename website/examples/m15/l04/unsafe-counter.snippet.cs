class FailureCounter
{
    private int total;

    public int Total => total;

    // Not thread safe: two workers can read the same value and both
    // write the same result, so one increment is lost
    public void Add() => total++;
}
