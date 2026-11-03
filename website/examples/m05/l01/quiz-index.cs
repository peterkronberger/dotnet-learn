int[] counts = { 3, 1, 4, 1, 5 };
Console.WriteLine(counts[counts.Length - 1]);
Console.WriteLine(counts[1] + counts[3]);
counts[0] = counts[4];
Console.WriteLine(counts[0]);
