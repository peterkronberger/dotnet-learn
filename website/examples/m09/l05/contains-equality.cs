List<Pair> pairs = [new Pair(1, 2)];
Console.WriteLine(pairs.Contains(new Pair(1, 2)));

List<Plain> plains = [new Plain { A = 1, B = 2 }];
Console.WriteLine(plains.Contains(new Plain { A = 1, B = 2 }));

record Pair(int A, int B);

class Plain
{
    public int A { get; set; }
    public int B { get; set; }
}
