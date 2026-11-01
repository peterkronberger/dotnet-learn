string eventType = "logout";
switch (eventType)
{
    case "login-failed":
        Console.WriteLine("Count the failure");
        break;
    case "login-success":
    case "logout":
        Console.WriteLine("No action");
        break;
    default:
        Console.WriteLine("Unknown event type");
        break;
}
Console.WriteLine("Done");
