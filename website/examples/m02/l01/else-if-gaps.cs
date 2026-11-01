int failed = 2;
Console.WriteLine($"Failed logins: {failed}");
// Step 1: replace false so that "many" means five or more failures
bool many = false;
// Step 2: replace false so that "some" means one or more failures
bool some = false;
if (many)
{
    Console.WriteLine("Many failures");
}
else if (some)
{
    Console.WriteLine("Some failures");
}
else
{
    Console.WriteLine("No failures");
}
