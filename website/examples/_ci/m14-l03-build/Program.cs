#define FILE_SYMBOL

// Tiny app for the build and run options of lesson m14-l03.
#if DEBUG
Console.WriteLine("Build: Debug");
#else
Console.WriteLine("Build: Release");
#endif

#if AUDIT
Console.WriteLine("Symbol AUDIT: set in the project file");
#endif

#if FILE_SYMBOL
Console.WriteLine("Symbol FILE_SYMBOL: set with #define");
#endif
