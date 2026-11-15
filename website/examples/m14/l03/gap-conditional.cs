#define EXTRA

Console.WriteLine("Start");

#if EXTRA
Console.WriteLine("Extra output");
#else
Console.WriteLine("No extra output");
#endif

Console.WriteLine("End");
