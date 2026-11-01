int failed = 1;
switch (failed)
{
    case 0:
    case 1:
    case 2:
        Console.WriteLine("few");
        break;
    case > 10:
        Console.WriteLine("very many");
        break;
    default:
        Console.WriteLine("some");
        break;
}
