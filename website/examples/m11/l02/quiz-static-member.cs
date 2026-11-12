Console.WriteLine(Box.Corners);
Console.WriteLine(new Box().Describe());

class Box
{
}

static class BoxExtensions
{
    extension(Box)
    {
        public static int Corners => 4;
    }

    extension(Box box)
    {
        public string Describe()
        {
            return $"box with {Box.Corners} corners";
        }
    }
}
