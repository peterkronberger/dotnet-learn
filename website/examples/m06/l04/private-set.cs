Visits visits = new Visits();
visits.Register();
visits.Register();
Console.WriteLine(visits.Total);

class Visits
{
    private int total;

    public int Total
    {
        get { return total; }
        private set { total = value; }
    }

    public void Register()
    {
        Total = Total + 1;
    }
}
