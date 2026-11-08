Action? log = Print;
log -= Print;

try
{
    log!();
}
catch (NullReferenceException ex)
{
    Console.WriteLine(ex.GetType().Name);
}

static void Print()
{
    Console.WriteLine("print");
}
