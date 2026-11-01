string eventType = "login-failed";
switch (eventType)
{
    case "login-failed":
        Console.WriteLine("Count the failure");
    case "logout":
        Console.WriteLine("No action");
        break;
}
