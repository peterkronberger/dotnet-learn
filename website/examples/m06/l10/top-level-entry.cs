// A program with top-level statements: the compiler generates the entry point method
Console.WriteLine($"Arguments: {args.Length}");
Console.WriteLine(args.GetType());

if (args.Length == 0)
{
    Console.WriteLine("No arguments given");
}
