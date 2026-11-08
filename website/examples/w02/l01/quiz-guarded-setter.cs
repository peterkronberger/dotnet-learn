var incident = new Incident();
incident.Failures = 3;
incident.Failures = -2;
Console.WriteLine(incident.Failures);

class Incident
{
    private int failures;

    public int Failures
    {
        get => failures;
        set
        {
            if (value >= 0)
            {
                failures = value;
            }
        }
    }
}
