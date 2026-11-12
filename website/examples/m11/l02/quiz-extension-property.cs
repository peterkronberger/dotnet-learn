Box box = new() { Width = 3, Height = 4 };
Console.WriteLine(box.Area);
box.Width = 10;
Console.WriteLine(box.Area);

class Box
{
    public int Width { get; set; }
    public int Height { get; set; }
}

static class BoxExtensions
{
    extension(Box box)
    {
        public int Area => box.Width * box.Height;
    }
}
