int failed = 4;
string eventType = "login-failed";
// 1. Compare numbers: print the text if failed is at least 3
if (failed > 100)
{
    Console.WriteLine("At least 3 failures");
}
// 2. Compare strings: print the text if the event type is login-failed
if (eventType == "")
{
    Console.WriteLine("Failure event");
}
// 3. Choose with else: more than 4 failures is "many"
if (failed > 0)
{
    Console.WriteLine("Many");
}
else
{
    Console.WriteLine("Not many");
}
