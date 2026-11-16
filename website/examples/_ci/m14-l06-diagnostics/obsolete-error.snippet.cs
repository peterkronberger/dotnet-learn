Console.WriteLine(Totals.Old(3));

static class Totals
{
    [Obsolete("Removed, use New instead.", error: true)]
    public static int Old(int n) => n * 2;

    public static int New(int n) => n * 2;
}
