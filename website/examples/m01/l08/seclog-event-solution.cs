Console.WriteLine("SecLog project profile");
Console.WriteLine("Purpose: find suspicious logins in made-up login logs");
Console.WriteLine("Language: C#");
Console.WriteLine("Platform: .NET");
Console.WriteLine("Variant: .NET (Linux server and Windows laptops)");
Console.WriteLine("Version: .NET 10 (LTS)");
Console.WriteLine("End of support: 2028-11-14");
Console.WriteLine("Execution model: Native AOT");
Console.WriteLine();
// Print the heading and the first event of the sample log
Console.WriteLine("First event:");
Console.WriteLine("2026-03-02T08:00:12Z login-success user=alice ip=192.0.2.10");
// Store the user, the IP address and the event type of sample log line 2
string user = "bob";
string ip = "203.0.113.7";
string eventType = "login-failed";
Console.Write("User: ");
Console.WriteLine(user);
Console.Write("IP: ");
Console.WriteLine(ip);
Console.Write("Event type: ");
Console.WriteLine(eventType);
// Compute the failure rate of the sample log in percent
const int events = 14;
const int failures = 7;
var failureRate = (failures * 100.0) / events;
Console.Write("Failure rate (%): ");
Console.WriteLine(failureRate);
// Count the failed logins of bob (sample log lines 2, 3, 5, 6 and 7)
var bobFailures = 0;
bobFailures++;
bobFailures++;
bobFailures++;
bobFailures++;
bobFailures++;
Console.Write("Failed logins of bob: ");
Console.WriteLine(bobFailures);
// Compute the share of bob in all failures in percent, rounded to 2 places
var bobShare = Math.Round(bobFailures * 100.0 / failures, 2);
Console.Write("Share of bob (%): ");
Console.WriteLine(bobShare);
// Print the event of sample line 2 as one log line, built from the variables
string timestamp = "2026-03-02T08:01:40Z";
Console.WriteLine($"{timestamp} {eventType} user={user} ip={ip}");
// Print the failure rate with one decimal place
Console.WriteLine($"Failure rate: {failureRate:F1} %");
// Print the share of successful logins (5 of 14) as a whole percent number
const int successes = 5;
var successRate = successes * 100.0 / events;
Console.WriteLine($"Success share, cut off (%): {(int)successRate}");
Console.WriteLine($"Success share, rounded (%): {(int)Math.Round(successRate)}");
// Read the number of failed logins from text and compute the failure rate
string failedText = "7";
int failedFromText = int.Parse(failedText);
double rateFromText = failedFromText * 100.0 / events;
Console.WriteLine($"Failure rate from text (%): {rateFromText}");
// Check a second text with TryParse and print the result
string badText = "seven";
bool parsed = int.TryParse(badText, out int badNumber);
Console.WriteLine($"Second text worked: {parsed}");
Console.WriteLine($"Second text number: {badNumber}");
