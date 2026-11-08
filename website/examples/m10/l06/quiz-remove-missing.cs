Action? log = A;
log += B;
log -= C;
log?.Invoke();

static void A()
{
    Console.WriteLine("A");
}

static void B()
{
    Console.WriteLine("B");
}

static void C()
{
    Console.WriteLine("C");
}
