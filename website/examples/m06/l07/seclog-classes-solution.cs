using System.Text;
using System.Globalization;
// @fold-start
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
// Collect the findings in one StringBuilder
var findings = new StringBuilder();
var sameFindings = findings;
AddFinding(sameFindings, "bob: 5 failed attempts");
AddFinding(findings, "alice: 1 failed attempt");
Console.WriteLine($"Findings: {findings}");
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
// Clean up a user name: string methods return a new string
string rawUser = "  Bob ";
string cleanUser = rawUser.Trim().ToLower();
Console.WriteLine($"Raw user: [{rawUser}]");
Console.WriteLine($"Clean user: [{cleanUser}]");
// Compare user names: == and Equals compare the text of strings
Console.WriteLine($"Is bob: {cleanUser == "bob"}");
Console.WriteLine($"Equals bob: {cleanUser.Equals("bob")}");
Console.WriteLine($"Same findings object: {object.ReferenceEquals(findings, sameFindings)}");
// Hold the whole sample log in an array
string[] sampleLog =
{
    "2026-03-02T08:00:12Z login-success user=alice ip=192.0.2.10",
    "2026-03-02T08:01:40Z login-failed user=bob ip=203.0.113.7",
    "2026-03-02T08:02:05Z login-failed user=bob ip=203.0.113.7",
    "2026-03-02T08:03:31Z login-success user=carol ip=198.51.100.23",
    "2026-03-02T08:04:02Z login-failed user=bob ip=203.0.113.7",
    "2026-03-02T08:05:15Z login-failed user=bob ip=203.0.113.7",
    "2026-03-02T08:06:48Z login-failed user=bob ip=203.0.113.7",
    "2026-03-02T08:07:20Z login-success user=dave ip=198.51.100.8",
    "2026-03-02T08:15:03Z login-failed user=alice ip=192.0.2.10",
    "2026-03-02T08:15:30Z login-success user=alice ip=192.0.2.10",
    "2026-03-02T08:42:11Z logout user=carol ip=198.51.100.23",
    "2026-03-02T09:10:09Z login-failed user=dave ip=198.51.100.8",
    "2026-03-02T09:10:41Z login-success user=dave ip=198.51.100.8",
    "2026-03-02T09:30:00Z logout user=alice ip=192.0.2.10",
};
Console.WriteLine($"Lines in the sample log: {sampleLog.Length}");
Console.WriteLine($"First line: {sampleLog[0]}");
Console.WriteLine($"Last line: {sampleLog[sampleLog.Length - 1]}");
Console.WriteLine($"User of line 2: {FindUser(sampleLog[1])}");
// Print a table of failed logins per user and hour
string[] users = { "alice", "bob", "carol", "dave" };
int[,] failuresPerHour =
{
    { 1, 0 },
    { 5, 0 },
    { 0, 0 },
    { 0, 1 },
};
for (int row = 0; row < failuresPerHour.GetLength(0); row++)
{
    Console.Write(users[row]);
    for (int column = 0; column < failuresPerHour.GetLength(1); column++)
    {
        Console.Write($" | hour {8 + column}: {failuresPerHour[row, column]}");
    }
    Console.WriteLine();
}
// Show parts of the sample log with ranges
Console.WriteLine($"Last line again: {sampleLog[^1]}");
string[] attack = sampleLog[1..7];
Console.WriteLine($"Lines 2 to 7: {attack.Length}");
foreach (string attackLine in attack)
{
    Console.WriteLine(attackLine);
}
string[] lastThree = sampleLog[^3..];
Console.WriteLine($"Last three lines: {lastThree.Length}");
Console.WriteLine($"Date of line 1: {sampleLog[0][..10]}");
// Collect the failed lines of the sample log in a list
List<string> failedLines = new List<string>();
foreach (string line in sampleLog)
{
    if (line.Contains("login-failed"))
    {
        failedLines.Add(line);
    }
}
Console.WriteLine($"Failed lines: {failedLines.Count}");
Console.WriteLine($"First failed line: {failedLines[0]}");
// Keep only the failed lines of bob
List<string> bobLines = new List<string>(failedLines);
for (int i = bobLines.Count - 1; i >= 0; i--)
{
    if (FindUser(bobLines[i]) != "bob")
    {
        bobLines.RemoveAt(i);
    }
}
Console.WriteLine($"Failed lines of bob: {bobLines.Count}");
Console.WriteLine($"Failed lines in total: {failedLines.Count}");
// Count the failed logins per user with a dictionary
Dictionary<string, int> failuresPerUser = new Dictionary<string, int>();
foreach (string failedLine in failedLines)
{
    string failedUser = FindUser(failedLine) ?? "unknown";
    failuresPerUser.TryGetValue(failedUser, out int count);
    failuresPerUser[failedUser] = count + 1;
}
foreach (string name in users)
{
    failuresPerUser.TryGetValue(name, out int total);
    Console.WriteLine($"{name}: {total}");
}
// List the distinct IP addresses of the sample log
HashSet<string> distinctIps = new HashSet<string>();
int repeatedIps = 0;
foreach (string logLine in sampleLog)
{
    if (!distinctIps.Add(ReadUserAndIp(logLine).Ip))
    {
        repeatedIps++;
    }
}
Console.WriteLine($"Distinct IP addresses: {distinctIps.Count}");
Console.WriteLine($"Repeated IP addresses: {repeatedIps}");
Console.WriteLine($"Contains 203.0.113.7: {distinctIps.Contains("203.0.113.7")}");
foreach (string address in distinctIps)
{
    Console.WriteLine(address);
}
// Build a watch list with collection expressions
string[] watchList = ["bob"];
List<string> extendedWatchList = [.. watchList, "dave"];
foreach (string watched in extendedWatchList)
{
    failuresPerUser.TryGetValue(watched, out int watchedCount);
    Console.WriteLine($"Watch {watched}: {watchedCount}");
}
// Print one alert per failed login of bob, in order
PrintAlerts(bobLines);
// Count the events per type with Split
Dictionary<string, int> eventsPerType = new Dictionary<string, int>();
foreach (string entry in sampleLog)
{
    string type = EventTypeOf(entry);
    eventsPerType.TryGetValue(type, out int typeCount);
    eventsPerType[type] = typeCount + 1;
}
string[] eventTypes = ["login-success", "login-failed", "logout"];
foreach (string kind in eventTypes)
{
    Console.WriteLine($"{kind}: {eventsPerType[kind]}");
}
string watchText = string.Join(", ", extendedWatchList);
Console.WriteLine($"Watch list: {watchText}");
bool sameType = string.Equals(
    EventTypeOf(sampleLog[1]), "LOGIN-FAILED", StringComparison.OrdinalIgnoreCase);
Console.WriteLine($"Line 2 is LOGIN-FAILED, ignoring case: {sameType}");
// Find 5 failed logins within 10 minutes
List<DateTimeOffset> bobTimes = new List<DateTimeOffset>();
foreach (string bobLine in bobLines)
{
    bobTimes.Add(TimestampOf(bobLine));
}
TimeSpan window = TimeSpan.FromMinutes(10);
for (int first = 0; first + 4 < bobTimes.Count; first++)
{
    TimeSpan span = bobTimes[first + 4] - bobTimes[first];
    if (span <= window)
    {
        string startText = bobTimes[first].ToString(
            "HH:mm:ss", CultureInfo.InvariantCulture);
        string endText = bobTimes[first + 4].ToString(
            "HH:mm:ss", CultureInfo.InvariantCulture);
        Console.WriteLine(
            $"bob: 5 failed logins within 10 minutes ({startText} to {endText})");
        Console.WriteLine($"Time span: {span}");
        break;
    }
}
// Model one event of the sample log as an object
LogEvent sampleEvent = new LogEvent(
    TimestampOf(sampleLog[1]),
    EventTypeOf(sampleLog[1]),
    FindUser(sampleLog[1]));
Console.WriteLine(sampleEvent.Describe());
// Turn every line of the sample log into a LogEvent
List<LogEvent> logEvents = [];
foreach (string rawLine in sampleLog)
{
    logEvents.Add(new(
        TimestampOf(rawLine), EventTypeOf(rawLine), FindUser(rawLine)));
}
Console.WriteLine($"Events: {logEvents.Count}");
Console.WriteLine(logEvents[logEvents.Count - 1].Describe());
// @fold-end
// Collect the failed logins of bob in an incident
Incident bobIncident = new Incident("bob") { Source = "sample log" };
// @fold-start
int rejected = 0;
foreach (LogEvent logEvent in logEvents)
{
    if (!bobIncident.Add(logEvent))
    {
        rejected++;
    }
}
Console.WriteLine(bobIncident.Summary());
Console.WriteLine($"Rejected events: {rejected}");
// Change the status of the incident
bobIncident.Status = "investigating";
Console.WriteLine($"Status: {bobIncident.Status}");
bobIncident.Status = "unknown";
Console.WriteLine($"Status: {bobIncident.Status}");
// @fold-end
// Open an incident for dave with an object initializer
Incident daveIncident = new("dave")
{
    Status = "investigating",
    Source = "sample log",
};
// @fold-start
foreach (LogEvent logEvent in logEvents)
{
    daveIncident.Add(logEvent);
}
Console.WriteLine(daveIncident.Summary());
Console.WriteLine($"Status: {daveIncident.Status}");
// @fold-end
// Print the source of each incident
Console.WriteLine($"Source of bob: {bobIncident.Source}");
Console.WriteLine($"Source of dave: {daveIncident.Source}");
// Print the user of each incident through the User property
Console.WriteLine($"User of bob: {bobIncident.User}");
Console.WriteLine($"User of dave: {daveIncident.User}");

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

// Print one alert per line: a queue gives the lines back in arrival order
void PrintAlerts(IReadOnlyList<string> lines)
{
    Queue<string> queue = new Queue<string>();
    foreach (string line in lines)
    {
        queue.Enqueue(line);
    }
    while (queue.Count > 0)
    {
        string next = queue.Dequeue();
        Console.WriteLine($"Alert: {next[..20]}");
    }
}

// Read the event type: the second part of a line, split at the spaces
string EventTypeOf(string line) => line.Split(' ')[1];

// Read the timestamp: the first part of a line, parsed as a point in time
DateTimeOffset TimestampOf(string line) =>
    DateTimeOffset.Parse(line.Split(' ')[0], CultureInfo.InvariantCulture);

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

// @fold-start
// Describe one event of the sample log as an object
class LogEvent
{
    public DateTimeOffset Timestamp { get; }
    public string EventType { get; }
    public string? User { get; }

    public LogEvent(DateTimeOffset timestamp, string eventType, string? user)
    {
        this.Timestamp = timestamp;
        this.EventType = eventType;
        this.User = user;
    }

    public string Describe()
    {
        string time = Timestamp.ToString(
            "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
        return $"{time} {EventType} {User ?? "unknown"}";
    }
}
// @fold-end

/// <summary>The failed logins of one user.</summary>
/// <param name="user">The name of the user.</param>
class Incident(string user)
{
    private readonly List<LogEvent> failures = [];

    /// <summary>The name of the user of this incident.</summary>
    public string User => user;

    /// <summary>Where the incident was found. Set once, at creation.</summary>
    public required string Source { get; init; }

    // @fold-start
    /// <summary>Adds a failed login of this user.</summary>
    /// <param name="logEvent">The event to add.</param>
    /// <returns>
    /// <c>true</c> if the event was added, <c>false</c> if it is not a
    /// failed login of this user.
    /// </returns>
    public bool Add(LogEvent logEvent)
    {
        bool failed = string.Equals(
            logEvent.EventType, "login-failed", StringComparison.Ordinal);
        bool sameUser = string.Equals(
            logEvent.User, user, StringComparison.Ordinal);
        if (!failed || !sameUser)
        {
            return false;
        }
        failures.Add(logEvent);
        return true;
    }

    /// <summary>The number of failed logins in this incident.</summary>
    public int FailureCount => failures.Count;
    // @fold-end

    // @fold-start
    /// <summary>
    /// The state of the incident: open, investigating or closed.
    /// Any other value is ignored.
    /// </summary>
    public string Status
    {
        get => field;
        set
        {
            bool known =
                string.Equals(value, "open", StringComparison.Ordinal) ||
                string.Equals(value, "investigating", StringComparison.Ordinal) ||
                string.Equals(value, "closed", StringComparison.Ordinal);
            if (known)
            {
                field = value;
            }
        }
    } = "open";
    // @fold-end

    /// <summary>Describes this incident in one line.</summary>
    /// <returns>
    /// A line such as <c>Incident bob: 5 failed logins (sample log)</c>.
    /// </returns>
    public string Summary()
    {
        return $"Incident {user}: {FailureCount} failed logins ({Source})";
    }
}
