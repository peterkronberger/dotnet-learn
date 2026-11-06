class Device
{
    public virtual string Describe() => "Device";
}

class Printer : Device
{
    public sealed override string Describe() => "Printer";
}

class LabelPrinter : Printer
{
    public override string Describe() => "Label printer";
}
