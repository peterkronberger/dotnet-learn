class Device
{
    public string Name { get; }

    public Device(string name)
    {
        Name = name;
    }
}

class Printer : Device
{
    public int PagesPerMinute { get; }

    public Printer(int pagesPerMinute)
    {
        PagesPerMinute = pagesPerMinute;
    }
}
