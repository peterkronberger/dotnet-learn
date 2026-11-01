int count = 0;
bool a = true || ++count > 0;
bool b = false | ++count > 0;
bool c = true | ++count > 0;
Console.WriteLine(count);
Console.WriteLine(a && b && c);
