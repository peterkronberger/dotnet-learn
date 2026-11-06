class Device
{
    private string secret = "hidden";
}

class Printer : Device
{
    public int SecretLength() => secret.Length;
}
