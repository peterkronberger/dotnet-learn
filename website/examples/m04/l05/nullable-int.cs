int? failures = null;
Console.WriteLine(failures.HasValue);
failures = 5;
Console.WriteLine(failures.HasValue);
Console.WriteLine(failures.Value);
int? none = null;
Console.WriteLine(none.GetValueOrDefault());
