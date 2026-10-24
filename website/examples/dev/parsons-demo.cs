var scores = new[] { 80, 90, 100 };
var total = 0;
foreach (var score in scores)
{
    total += score;
}
var average = total / scores.Length;
Console.WriteLine($"Average: {average}");
