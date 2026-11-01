string eventType = "logout";
string other = "login-failed";
int failed = 2;
// 1. Add a section for "logout" that prints "Close session" and ends with break
switch (eventType)
{
    case "login-failed":
        Console.WriteLine("Count it");
        break;
    default:
        Console.WriteLine("Unknown");
        break;
}
// 2. Add a case guard so that the alert needs at least 5 failures
switch (other)
{
    case "login-failed":
        Console.WriteLine("Alert");
        break;
    default:
        Console.WriteLine("No alert");
        break;
}
Console.WriteLine(failed);
