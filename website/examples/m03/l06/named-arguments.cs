Console.WriteLine(Line("Failures", 7));
Console.WriteLine(Line(value: 7, label: "Failures"));
Console.WriteLine(Line("Rate", 50, unit: "%"));

string Line(string label, int value, string unit = "")
{
    return $"{label}: {value}{unit}";
}
