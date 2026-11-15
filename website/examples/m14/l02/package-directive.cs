#!/usr/bin/env dotnet
#:package Newtonsoft.Json@13.0.3
using Newtonsoft.Json;

// The package adds the JsonConvert class.
var row = new { User = "bob", Failures = 5 };
Console.WriteLine(JsonConvert.SerializeObject(row));
