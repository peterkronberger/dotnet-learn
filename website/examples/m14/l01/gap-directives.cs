#!/usr/bin/env dotnet
#:property TreatWarningsAsErrors=true
#:include include-helper.snippet.cs
#nullable disable
string note = null;
#nullable enable
Console.WriteLine(FailureText.For(3));
Console.WriteLine($"note is null: {note is null}");
