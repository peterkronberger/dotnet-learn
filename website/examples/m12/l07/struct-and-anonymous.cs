// with works for a struct and for an anonymous type, too
Reading before = new() { Sensor = "door", Value = 3 };
Reading after = before with { Value = 4 };
Console.WriteLine($"{before.Value} {after.Value}");

var alert = new { User = "bob", Failures = 5 };
var worse = alert with { Failures = 6 };
Console.WriteLine(alert);
Console.WriteLine(worse);

struct Reading
{
    public string Sensor { get; set; }
    public int Value { get; set; }
}
