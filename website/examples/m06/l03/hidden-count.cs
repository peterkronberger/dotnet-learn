FailureCounter counter = new FailureCounter();
bool first = counter.Add(3);
bool second = counter.Add(-2);

Console.WriteLine($"{first} {second}");
Console.WriteLine(counter.Current());

class FailureCounter
{
    private int count;

    public bool Add(int number)
    {
        if (number < 0)
        {
            return false;
        }
        count += number;
        return true;
    }

    public int Current()
    {
        return count;
    }
}
