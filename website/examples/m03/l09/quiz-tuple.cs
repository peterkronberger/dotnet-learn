var (low, high) = Order(9, 2);
Console.WriteLine($"{low} {high}");

(int, int) Order(int a, int b) => a < b ? (a, b) : (b, a);
