Volume volume = new Volume();
volume.Level = 7;
volume.Level = 12;
volume.Level = 4;
Console.WriteLine(volume.Level);

class Volume
{
    private int level;

    public int Level
    {
        get { return level; }
        set
        {
            if (value <= 10)
            {
                level = value;
            }
        }
    }
}
