// 1. Read the line: it is text
var text = Console.ReadLine();
// 2. Text plus 1 appends a character
Console.WriteLine(text + 1);
// 3. Convert the text to a number, then add 1
bool ok = int.TryParse(text, out int number);
Console.WriteLine(ok);
Console.WriteLine(number + 1);
