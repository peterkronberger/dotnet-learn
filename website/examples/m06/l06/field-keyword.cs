Alert alert = new Alert();
Console.WriteLine(alert.Level);
alert.Level = 3;
Console.WriteLine(alert.Level);
alert.Level = 9;
Console.WriteLine(alert.Level);
alert.Level = 0;
Console.WriteLine(alert.Level);

class Alert
{
    public int Level
    {
        get => field;
        set
        {
            if (value >= 1 && value <= 3)
            {
                field = value;
            }
        }
    } = 1;
}
