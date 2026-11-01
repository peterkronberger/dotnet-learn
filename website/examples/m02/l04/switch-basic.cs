string eventType = "logout";
string action = eventType switch
{
    "login-failed" => "count it",
    "login-success" => "welcome",
    "logout" => "close session",
    _ => "unknown event type"
};
Console.WriteLine(action);
