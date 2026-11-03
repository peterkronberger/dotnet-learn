using System.Text;

var morning = new StringBuilder("morning:");
var evening = morning;
evening.Append(" 3 failures");
Console.WriteLine(morning);
Console.WriteLine(evening);
