// An entry point that returns an int: the value is the exit code
class Program
{
    static int Main(string[] args)
    {
        string logName = args.Length > 0 ? args[0] : "sample log";
        Console.WriteLine($"Analyzed log: {logName}");
        return 0;
    }
}
