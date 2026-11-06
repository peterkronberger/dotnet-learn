Device device = new Device("router");

abstract class Device
{
    public string Name { get; }

    protected Device(string name)
    {
        Name = name;
    }

    public abstract string Describe();
}
