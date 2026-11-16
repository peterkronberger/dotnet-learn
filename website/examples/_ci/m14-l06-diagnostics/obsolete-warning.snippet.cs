Console.WriteLine(Totals.Old(3));

static class Totals
{
    [Obsolete("Use New instead.")]
    public static int Old(int n) => n * 2;

    public static int New(int n) => n * 2;
}
