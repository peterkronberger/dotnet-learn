Pair first = new(1, 2);
Pair second = first;
// Task 1: set the property A of second to 7
Console.WriteLine($"{first.A} {second.A}");
// Task 2: set third to the default value of Pair
Pair third = new(9, 9);
Console.WriteLine($"{third.A} {third.B}");

struct Pair
{
    public int A { get; set; }
    public int B { get; set; }

    public Pair(int a, int b)
    {
        A = a;
        B = b;
    }
}
