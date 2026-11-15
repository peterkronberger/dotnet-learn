using System.Text;
using System.Globalization;
using SecLog.Model;
using System.Collections.Concurrent;
using System.Threading.Channels;
Console.WriteLine("SecLog project profile");
Console.WriteLine("Purpose: find suspicious logins in made-up login logs");
Console.WriteLine("Language: C#");
Console.WriteLine("Platform: .NET");
Console.WriteLine("Variant: .NET (Linux server and Windows laptops)");
Console.WriteLine("Version: .NET 11 (STS)");
Console.WriteLine("End of support: 2028-11-14");
Console.WriteLine("Execution model: Native AOT");
// Print the build configuration: the compiler keeps only one of the two lines
#if DEBUG
Console.WriteLine("Build: Debug");
#else
Console.WriteLine("Build: Release");
#endif
Console.WriteLine();
