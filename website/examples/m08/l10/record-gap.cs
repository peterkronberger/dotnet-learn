Login first = new("alice", true);
Login second = new("alice", true);
// Task 1: print whether first and second are equal, using ==
Console.WriteLine(false);
// Task 2: deconstruct first into who and ok (replace the two lines)
string who = "";
bool ok = false;
Console.WriteLine($"{who}: {ok}");

record Login(string User, bool Success);
