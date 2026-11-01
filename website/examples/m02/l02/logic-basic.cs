string eventType = "login-failed";
int failed = 4;
bool isFailure = eventType == "login-failed";
bool many = failed >= 5;
Console.WriteLine(isFailure && many);
Console.WriteLine(isFailure || many);
Console.WriteLine(!many);
Console.WriteLine(isFailure ^ many);
