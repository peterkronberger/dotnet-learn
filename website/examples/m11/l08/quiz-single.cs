string[] names = ["bob", "amy", "bob"];

try
{
    Console.WriteLine(names.Single(n => n == "amy"));
    Console.WriteLine(names.Single(n => n == "bob"));
}
catch (InvalidOperationException)
{
    Console.WriteLine("error");
}
