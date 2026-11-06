// Task 1: add Beep to the delegate with += (after PrintDot)
// Task 2: invoke it once, print the number of targets, then remove PrintDot
Action signal = PrintDot;
signal();

static void PrintDot()
{
    Console.WriteLine(".");
}

static void Beep()
{
    Console.WriteLine("beep");
}
