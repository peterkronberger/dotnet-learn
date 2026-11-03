// Failed logins per user: alice, bob, carol, dave
int[] failures = { 1, 5, 0, 1 };
Console.WriteLine(failures[0]);
Console.WriteLine(failures.Length);
failures[2] = 4;
Console.WriteLine(failures[2]);
Console.WriteLine(failures[failures.Length - 1]);
