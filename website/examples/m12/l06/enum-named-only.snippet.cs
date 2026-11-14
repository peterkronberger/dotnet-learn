Color color = (Color)7;
string name = color switch
{
    Color.Red => "red",
    Color.Green => "green",
    Color.Blue => "blue",
};
Console.WriteLine(name);

enum Color { Red, Green, Blue }
