int failed = 4;
string eventType = "login-failed";
string user = "dave";
// 1. Both conditions must hold: a failed login with at least 3 failures
bool suspicious = eventType == "";
// 2. One condition is enough: the user is bob or dave
bool watched = user == "";
// 3. Negate a condition: fewer than 3 failures is quiet
bool quiet = failed > 0;
Console.WriteLine(suspicious);
Console.WriteLine(watched);
Console.WriteLine(quiet);
