if (args.Length == 0)
{
    Console.WriteLine("Usage: pass the name of a log");
    return 1;
}

Console.WriteLine($"Analyzed log: {args[0]}");
return 0;
