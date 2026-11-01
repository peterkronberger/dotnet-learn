string eventType = "login-failed";
int failed = 6;
switch (eventType)
{
    case "login-failed" when failed >= 5:
        Console.WriteLine("alert");
        break;
    case "login-failed":
        Console.WriteLine("count it");
        break;
    default:
        Console.WriteLine("ignore");
        break;
}
