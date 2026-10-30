string text = "3";
// 1. Text plus number joins the characters
Console.WriteLine(text + 4);
// 2. Parse turns the text into a number first
Console.WriteLine(int.Parse(text) + 4);
// 3. double.Parse works the same way
double price = double.Parse("19.99");
Console.WriteLine(price * 2);
