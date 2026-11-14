// The switch statement accepts the same patterns
foreach (int failures in new[] { 0, 2, 6 })
{
    switch (failures)
    {
        case 0:
            Console.WriteLine("none");
            break;
        case < 5:
            Console.WriteLine("some");
            break;
        case var n when n >= 5:
            Console.WriteLine("many");
            break;
    }
}
