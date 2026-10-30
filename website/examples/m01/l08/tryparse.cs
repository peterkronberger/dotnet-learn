// 1. Valid text
bool okGood = int.TryParse("42", out int good);
Console.WriteLine(okGood);
Console.WriteLine(good);
// 2. Text with a decimal point
bool okDecimal = int.TryParse("3.5", out int decimalNumber);
Console.WriteLine(okDecimal);
Console.WriteLine(decimalNumber);
// 3. Text with letters
bool okWord = int.TryParse("seven", out int word);
Console.WriteLine(okWord);
Console.WriteLine(word);
