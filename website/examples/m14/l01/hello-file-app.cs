#!/usr/bin/env dotnet
#:property TreatWarningsAsErrors=true
// One .cs file is the whole app: no project file needed.
Console.WriteLine("SecLog starts as a file-based app");
int failures = 5;
Console.WriteLine($"Failures: {failures}");
