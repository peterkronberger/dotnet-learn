int failed = 4;
// Step 1: test the strictest condition first
if (failed >= 5)
{
    Console.WriteLine("Many failures");
}
// Step 2: test the next condition only if the first one was false
else if (failed >= 3)
{
    Console.WriteLine("Some failures");
}
// Step 3: the else block runs if no condition was true
else
{
    Console.WriteLine("Few failures");
}
