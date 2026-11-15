#define TRACE_STEPS
#define VERBOSE
#undef VERBOSE

Console.WriteLine("Begin");

#if VERBOSE
Console.WriteLine("Verbose");
#elif TRACE_STEPS
Console.WriteLine("Trace");
#endif

#if !VERBOSE
Console.WriteLine("Quiet");
#endif
