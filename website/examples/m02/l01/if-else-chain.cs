int attempts = 4;
// 1. Test the strictest condition first
if (attempts >= 5)
{
    Console.WriteLine("Lock the account");
}
// 2. Test the next condition only if the first was false
else if (attempts >= 3)
{
    Console.WriteLine("Show a warning");
}
// 3. Run this block if no condition was true
else
{
    Console.WriteLine("Everything is fine");
}
Console.WriteLine("Check finished");
