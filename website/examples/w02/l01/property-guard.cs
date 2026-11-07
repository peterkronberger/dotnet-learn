var incident = new Incident();
incident.Failures = 3;
incident.Failures = -2;
incident.Failures = 5;
Console.WriteLine(incident.Failures);
Console.WriteLine(incident.Rejected);

class Incident
{
    private int _failures;
    public int Rejected;

    public int Failures
    {
        get => _failures;
        set
        {
            if (value < 0)
            {
                Rejected++;
                return;
            }
            _failures = value;
        }
    }
}
