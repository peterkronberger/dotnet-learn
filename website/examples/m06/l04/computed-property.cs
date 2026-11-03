Rectangle room = new Rectangle(4, 3);
Console.WriteLine(room.Area);

Rectangle hall = new Rectangle(10, 6);
Console.WriteLine(hall.Area);

class Rectangle
{
    private readonly int width;
    private readonly int height;

    public Rectangle(int width, int height)
    {
        this.width = width;
        this.height = height;
    }

    public int Area => width * height;
}
