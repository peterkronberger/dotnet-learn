string eventType = "logout";
int failed = 4;
// 1. Constant patterns: add an arm for "logout" with the result "close session"
string action = eventType switch
{
    "login-failed" => "count it",
    _ => "unknown"
};
// 2. Relational pattern: add an arm for fewer than 5 with the result "medium"
string severity = failed switch
{
    0 => "none",
    < 3 => "low",
    _ => "high"
};
Console.WriteLine(action);
Console.WriteLine(severity);
