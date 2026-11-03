// @fold-start
using System.Text;
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
// Read the user from the first input line, store the IP address and the event type
var user = Console.ReadLine();
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
// Read the number of failed logins from the second input line, compute the rate
var failedText = Console.ReadLine();
int.TryParse(failedText, out int failedFromInput);
double rateFromInput = failedFromInput * 100.0 / events;
Console.WriteLine($"Failure rate from input (%): {rateFromInput:F1}");
// Check a second text with TryParse and print the result
string badText = "seven";
bool parsed = int.TryParse(badText, out int badNumber);
Console.WriteLine($"Second text worked: {parsed}");
Console.WriteLine($"Second text number: {badNumber}");
// Print an alert if the number of failed logins from the input is high
if (failedFromInput >= 5)
{
    Console.WriteLine("Alert: many failed logins");
}
else
{
    Console.WriteLine("No alert");
}
// Rate the event as suspicious with a method, for two event types
bool suspicious = IsSuspicious(eventType, failedFromInput);
Console.WriteLine($"Suspicious: {suspicious}");
Console.WriteLine($"Suspicious for login-success: {IsSuspicious("login-success", failedFromInput)}");
// Print the number of failures with the singular or plural word
Console.WriteLine($"{failedFromInput} failed {AttemptWord(failedFromInput)}");
Console.WriteLine($"{bobFailures} failed {AttemptWord(bobFailures)}");
// Rate the severity of the failures with a method
string severity = RateSeverity(failedFromInput);
Console.WriteLine($"Severity: {severity}");
Console.WriteLine($"Severity of bob: {RateSeverity(bobFailures)}");
// Choose an action for the event type with a switch statement
switch (eventType)
{
    case "login-failed":
        Console.WriteLine("Action: count the failure");
        break;
    case "login-success":
    case "logout":
        Console.WriteLine("Action: none");
        break;
    default:
        Console.WriteLine("Action: unknown event type");
        break;
}
// Count the failure streak: read event types while they are login-failed
var nextEvent = Console.ReadLine();
int streak = 0;
while (nextEvent == "login-failed")
{
    streak++;
    nextEvent = Console.ReadLine();
}
Console.WriteLine($"Failure streak: {streak}");
// Print one line per failed login of bob with a for loop
for (int attempt = 1; attempt <= bobFailures; attempt++)
{
    Console.WriteLine($"Attempt {attempt}");
}
// Count the dots in the IP address with a foreach loop
int dots = 0;
foreach (char symbol in ip)
{
    if (symbol == '.')
    {
        dots++;
    }
}
Console.WriteLine($"Dots in IP: {dots}");
// Describe the IP address with a method that uses a static local function
Console.WriteLine(DescribeIp(ip));
// Build the date part of the timestamp: stop at the letter T
string datePart = "";
foreach (char symbol in timestamp)
{
    if (symbol == 'T')
    {
        break;
    }
    datePart += symbol;
}
Console.WriteLine($"Date: {datePart}");
// Count the digits of the IP address: skip the dots
int digits = 0;
foreach (char symbol in ip)
{
    if (symbol == '.')
    {
        continue;
    }
    digits++;
}
Console.WriteLine($"Digits in IP: {digits}");
// Print the counts and the rate with the overloaded Report.Line
Console.WriteLine(Report.Line("Failures", failures));
Console.WriteLine(Report.Line("Failure rate (%)", failureRate));
// Format log lines with a method that has an optional parameter
Console.WriteLine(FormatEvent(timestamp, eventType, "bob", ip));
Console.WriteLine(FormatEvent(user: "carol", eventType: "logout", timestamp: "2026-03-02T08:42:11Z"));
// Count the failed logins among the 14 event types of the sample log
Console.WriteLine($"Failed in sample: {CountFailed("login-success", "login-failed", "login-failed", "login-success", "login-failed", "login-failed", "login-failed", "login-success", "login-failed", "login-success", "logout", "login-failed", "login-success", "logout")}");
// Read the user from sample line 2, and check a text that is not a log line
string sampleLine = "2026-03-02T08:01:40Z login-failed user=bob ip=203.0.113.7";
bool lineOk = TryReadUser(sampleLine, out string readUser);
if (lineOk)
{
    Console.WriteLine($"User from line: {readUser}");
}
bool brokenOk = TryReadUser("broken line", out string brokenUser);
Console.WriteLine($"Line ok: {brokenOk}");
// Read user and IP address together from sample line 2 and split the tuple
var (lineUser, lineIp) = ReadUserAndIp(sampleLine);
Console.WriteLine($"User and IP: {lineUser} {lineIp}");
// Print a bar chart of the failed logins per user of the sample log
for (int userNumber = 1; userNumber <= 4; userNumber++)
{
    string name = userNumber switch
    {
        1 => "alice",
        2 => "bob",
        3 => "carol",
        _ => "dave",
    };
    int count = userNumber switch
    {
        1 => 1,
        2 => 5,
        3 => 0,
        _ => 1,
    };
    Console.WriteLine($"{name} |{Bar(count)}");
}
// Print the bar of all failures of the sample log
Console.WriteLine($"all |{Bar(failures)}");
// @fold-end
// Collect the findings in one StringBuilder
var findings = new StringBuilder();
var sameFindings = findings;
AddFinding(sameFindings, "bob: 5 failed attempts");
AddFinding(findings, "alice: 1 failed attempt");
Console.WriteLine($"Findings: {findings}");
// @fold-start
// Find the user of a line, or null if it has none
string? foundUser = FindUser(sampleLine);
if (foundUser != null)
{
    Console.WriteLine($"User of sample line: {foundUser}");
}
Console.WriteLine($"User of broken line is missing: {FindUser("broken line") == null}");
// Read a number of failures, or null if the text is not a number
int? inputFailures = FailuresOrNull(failedText);
Console.WriteLine($"Failures from input: {inputFailures}");
Console.WriteLine($"Number in \"many\": {FailuresOrNull("many").HasValue}");
Console.WriteLine($"Sum with a missing number is null: {FailuresOrNull(failedText) + FailuresOrNull("many") == null}");
// Handle missing values with null-safe operators
string shownUser = FindUser("broken line") ?? "unknown";
Console.WriteLine($"Shown user: {shownUser}");
int? userLength = FindUser(sampleLine)?.Length;
Console.WriteLine($"Length of user: {userLength}");
string? lastIp = null;
lastIp ??= "unknown";
Console.WriteLine($"Last IP: {lastIp}");
// @fold-end
// Clean up a user name: string methods return a new string
string rawUser = "  Bob ";
string cleanUser = rawUser.Trim().ToLower();
Console.WriteLine($"Raw user: [{rawUser}]");
Console.WriteLine($"Clean user: [{cleanUser}]");
// Compare user names: == and Equals compare the text of strings
Console.WriteLine($"Is bob: {cleanUser == "bob"}");
Console.WriteLine($"Equals bob: {cleanUser.Equals("bob")}");
Console.WriteLine($"Same findings object: {object.ReferenceEquals(findings, sameFindings)}");

// @fold-start
// Rate the severity of a number of failures (the thresholds are made up)
string RateSeverity(int failures)
{
    return failures switch
    {
        0 => "none",
        < 3 => "low",
        < 5 => "medium",
        _ => "high",
    };
}

// Choose the singular or plural word for a number of attempts
string AttemptWord(int count) => count == 1 ? "attempt" : "attempts";

// Rate an event as suspicious: a failed login and at least 3 failures
bool IsSuspicious(string eventType, int failures)
{
    return eventType == "login-failed" && failures >= 3;
}

// Describe an IP address: the dots are counted by a static local function
string DescribeIp(string ip)
{
    int dotCount = CountChar(ip, '.');
    return $"{ip} has {dotCount} dots";

    static int CountChar(string text, char wanted)
    {
        int found = 0;
        foreach (char symbol in text)
        {
            if (symbol == wanted)
            {
                found++;
            }
        }
        return found;
    }
}

// Build a log line: the IP address is optional and defaults to "unknown"
string FormatEvent(string timestamp, string eventType, string user, string ip = "unknown")
{
    return $"{timestamp} {eventType} user={user} ip={ip}";
}

// Count the login-failed entries among any number of event types
int CountFailed(params string[] eventTypes)
{
    int failed = 0;
    foreach (string kind in eventTypes)
    {
        if (kind == "login-failed")
        {
            failed++;
        }
    }
    return failed;
}

// Read the user name after "user=" from a log line, or report that it is missing
bool TryReadUser(string line, out string userName)
{
    userName = "";
    string word = "";
    bool found = false;
    foreach (char symbol in line)
    {
        if (found)
        {
            if (symbol == ' ')
            {
                break;
            }
            userName += symbol;
        }
        else if (symbol == ' ')
        {
            word = "";
        }
        else
        {
            word += symbol;
            if (word == "user=")
            {
                found = true;
            }
        }
    }
    return found;
}

// Read the user name and the IP address of a log line and return both
(string User, string Ip) ReadUserAndIp(string line)
{
    string userName = "";
    string ipText = "";
    string field = "";
    string word = "";
    foreach (char symbol in line)
    {
        if (symbol == ' ')
        {
            field = "";
            word = "";
        }
        else if (field == "user")
        {
            userName += symbol;
        }
        else if (field == "ip")
        {
            ipText += symbol;
        }
        else
        {
            word += symbol;
            if (word == "user=")
            {
                field = "user";
            }
            else if (word == "ip=")
            {
                field = "ip";
            }
        }
    }
    return (userName, ipText);
}

// Build a bar of "#" characters with recursion: one "#" per count
string Bar(int count)
{
    if (count == 0)
    {
        return "";
    }
    return "#" + Bar(count - 1);
}

// Add one finding and a separator to the findings
void AddFinding(StringBuilder findings, string text)
{
    findings.Append(text);
    findings.Append("; ");
}

// Find the user name of a log line, or return null if the line has none
string? FindUser(string line)
{
    if (TryReadUser(line, out string userName))
    {
        return userName;
    }
    return null;
}

// Read a number of failures from a text, or return null if it is not a number
int? FailuresOrNull(string? text)
{
    if (int.TryParse(text, out int number))
    {
        return number;
    }
    return null;
}

// Format a report line: the overloads choose the number format
static class Report
{
    public static string Line(string label, int value)
    {
        return $"{label}: {value}";
    }

    public static string Line(string label, double value)
    {
        return $"{label}: {value:F1}";
    }
}
// @fold-end
