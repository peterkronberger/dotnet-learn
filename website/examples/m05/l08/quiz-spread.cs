int[] first = [1, 2];
int[] second = [3];
List<int> all = [.. second, 0, .. first, .. second];

Console.WriteLine(all.Count);
Console.WriteLine(all[0]);
Console.WriteLine(all[all.Count - 1]);
Console.WriteLine(first.Length);
