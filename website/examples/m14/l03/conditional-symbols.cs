#define AUDIT
#define FAST
#undef FAST

#if FAST
Console.WriteLine("Mode: fast");
#elif AUDIT
Console.WriteLine("Mode: audit");
#else
Console.WriteLine("Mode: normal");
#endif

#if AUDIT && !FAST
Console.WriteLine("Audit without fast mode");
#endif

#if FAST || SPEED
Console.WriteLine("Fast or speed");
#endif
