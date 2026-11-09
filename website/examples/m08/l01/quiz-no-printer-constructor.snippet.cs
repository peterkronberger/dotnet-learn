Printer printer = new Printer("office printer");

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
}
