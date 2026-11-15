using System.Runtime.CompilerServices;

// Tiny app for the publish options of lesson m14-l04.
Console.WriteLine("Hello from the published app.");

// False under Native AOT: there is no JIT compiler at run time.
bool jit = RuntimeFeature.IsDynamicCodeSupported;
Console.WriteLine($"JIT available: {jit}");
