int[] a = [1, 2, 2, 3, 4];
int[] b = [3, 4, 4, 5];

Console.WriteLine($"Concat:    {string.Join(", ", a.Concat(b))}");
Console.WriteLine($"Distinct:  {string.Join(", ", a.Distinct())}");
Console.WriteLine($"Union:     {string.Join(", ", a.Union(b))}");
Console.WriteLine($"Intersect: {string.Join(", ", a.Intersect(b))}");
Console.WriteLine($"Except:    {string.Join(", ", a.Except(b))}");
