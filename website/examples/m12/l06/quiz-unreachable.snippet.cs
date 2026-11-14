string eventType = "logout";
string action = eventType switch
{
    "login-failed" => "watch",
    "login-success" or "logout" => "none",
    "logout" => "close session",
    _ => "unknown",
};
Console.WriteLine(action);
