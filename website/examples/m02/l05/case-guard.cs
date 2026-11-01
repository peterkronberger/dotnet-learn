string eventType = "login-failed";
int failed = 6;
switch (eventType)
{
    case "login-failed" when failed >= 5:
        Console.WriteLine("Raise an alert");
        break;
    case "login-failed":
        Console.WriteLine("Count the failure");
        break;
    default:
        Console.WriteLine("Ignore");
        break;
}
