Console.WriteLine(Twice(3));

int Twice(int n) => n == 0 ? 0 : 2 + Twice(n - 1);
