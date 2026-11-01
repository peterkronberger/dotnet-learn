string eventType = "login-failed";
int failed = 4;
Console.WriteLine($"Event: {eventType}, failed logins: {failed}");
// Step 1: replace false so that it checks for the event type login-failed
bool isFailure = false;
// Step 2: replace false so that it checks for at least 3 failures
bool isMany = false;
// Step 3: replace false so that both conditions must be true
bool suspicious = false;
Console.WriteLine($"Failure: {isFailure}, many: {isMany}");
Console.WriteLine($"Suspicious: {suspicious}");
