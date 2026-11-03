int? five = 5;
int? missing = null;
Console.WriteLine(five + 1);
Console.WriteLine(five + missing == null);
Console.WriteLine(five > missing);
Console.WriteLine(five < missing);
Console.WriteLine(five == missing);
Console.WriteLine(missing == null);
