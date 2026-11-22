using System.Text;
using System.Globalization;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.EntityFrameworkCore;
using SecLog.Data;
using SecLog.Data.Entities;
using SecLog.Model;
using System.Collections.Concurrent;
using System.Threading.Channels;
Console.WriteLine("SecLog project profile");
Console.WriteLine("Purpose: find suspicious logins in made-up login logs");
Console.WriteLine("Language: C#");
Console.WriteLine("Platform: .NET");
Console.WriteLine("Variant: .NET (Linux server and Windows laptops)");
Console.WriteLine("Version: .NET 11 (STS)");
Console.WriteLine("End of support: 2028-11-14");
Console.WriteLine("Execution model: Native AOT");
// Print the build configuration: the compiler keeps only one of the two lines
#if DEBUG
Console.WriteLine("Build: Debug");
#else
Console.WriteLine("Build: Release");
#endif
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
// Create the generic host builder: configuration, logging and the container in one
HostApplicationBuilder builder = Host.CreateApplicationBuilder(
    new HostApplicationBuilderSettings
    {
        Args = args,
        ContentRootPath = AppContext.BaseDirectory,
    });
// Read the thresholds of the brute-force rule: the environment beats appsettings.json
IConfiguration configuration = builder.Configuration;
DetectionOptions detection = configuration
    .GetSection(DetectionOptions.SectionName).Get<DetectionOptions>() ?? new();
int maxFailures = detection.MaxFailures;
int windowMinutes = detection.WindowMinutes;
Console.WriteLine(
    $"Detection settings: {maxFailures} failures in {windowMinutes} minutes");
TimeSpan window = TimeSpan.FromMinutes(windowMinutes);
for (int first = 0; first + maxFailures - 1 < bobTimes.Count; first++)
{
    TimeSpan span = bobTimes[first + maxFailures - 1] - bobTimes[first];
    if (span <= window)
    {
        string startText = bobTimes[first].ToString(
            "HH:mm:ss", CultureInfo.InvariantCulture);
        string endText = bobTimes[first + maxFailures - 1].ToString(
            "HH:mm:ss", CultureInfo.InvariantCulture);
        Console.WriteLine(
            $"bob: {maxFailures} failed logins within {windowMinutes} minutes "
            + $"({startText} to {endText})");
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
// Collect the failed logins of bob in an incident
Incident bobIncident = new Incident("bob") { Source = "sample log" };
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
try
{
    bobIncident.Status = "unknown";
}
catch (ArgumentException ex)
{
    Console.WriteLine(ex.Message);
}
Console.WriteLine($"Status: {bobIncident.Status}");
// Open an incident for dave with an object initializer
Incident daveIncident = new("dave")
{
    Status = "investigating",
    Source = "sample log",
};
foreach (LogEvent logEvent in logEvents)
{
    daveIncident.Add(logEvent);
}
Console.WriteLine(daveIncident.Summary());
Console.WriteLine($"Status: {daveIncident.Status}");
// Print the source of each incident
Console.WriteLine($"Source of bob: {bobIncident.Source}");
Console.WriteLine($"Source of dave: {daveIncident.Source}");
// Print the user of each incident through the User property
Console.WriteLine($"User of bob: {bobIncident.User}");
Console.WriteLine($"User of dave: {daveIncident.User}");
// Count the incidents with a static property
Console.WriteLine($"Incidents: {Incident.Count}");
// Print the full name of the LogEvent type
Console.WriteLine(typeof(LogEvent).FullName);
// Read the name of the log from the command-line arguments
string logName = args.Length > 0 ? args[0] : "sample log";
Console.WriteLine($"Analyzed log: {logName}");
// Parse a failure count with int.Parse and catch the error
try
{
    int goodCount = int.Parse("7");
    Console.WriteLine($"Failure count: {goodCount}");
    int badCount = int.Parse(badText);
    Console.WriteLine($"Failure count: {badCount}");
}
catch (FormatException ex)
{
    Console.WriteLine($"Not a number: {badText}");
    Console.WriteLine(ex.Message);
}
// Read sample lines by number and handle wrong numbers
string[] lineNumbers = ["2", "15", "two"];
foreach (string numberText in lineNumbers)
{
    try
    {
        int number = int.Parse(numberText);
        Console.WriteLine(sampleLog[number - 1]);
    }
    catch (IndexOutOfRangeException)
    {
        Console.WriteLine($"No line {numberText} in the sample log");
    }
    catch (FormatException)
    {
        Console.WriteLine($"Not a line number: {numberText}");
    }
}
// Count the processed lines with finally
string[] checkedLines = [sampleLog[1], "broken line"];
int processed = 0;
int rejectedLines = 0;
foreach (string checkedLine in checkedLines)
{
    try
    {
        TimestampOf(checkedLine);
    }
    catch (FormatException)
    {
        rejectedLines++;
        Console.WriteLine($"Rejected: {checkedLine}");
    }
    finally
    {
        processed++;
    }
}
Console.WriteLine($"Processed: {processed}, rejected: {rejectedLines}");
// Reject an event without event type
try
{
    LogEvent noType = new(sampleEvent.Timestamp, "", null);
    Console.WriteLine(noType.Describe());
}
catch (ArgumentException ex)
{
    Console.WriteLine($"Rejected event: {ex.Message}");
}
// Reject invalid log lines with an own exception
string[] candidates =
[
    sampleLog[1],
    "broken line",
    "2026-13-02T08:01:40Z login-failed user=bob ip=203.0.113.7",
];
int accepted = 0;
for (int number = 1; number <= candidates.Length; number++)
{
    try
    {
        LogEvent goodEvent = LogParser.ParseLine(candidates[number - 1], number);
        Console.WriteLine(goodEvent.Describe());
        accepted++;
    }
    catch (InvalidLogLineException ex)
    {
        Console.WriteLine($"Line {ex.LineNumber} rejected: {ex.Message}");
        if (ex.InnerException != null)
        {
            Console.WriteLine($"Cause: {ex.InnerException.GetType().Name}");
        }
    }
}
Console.WriteLine($"Accepted lines: {accepted}");
// Count invalid lines of a mixed log without exceptions
string[] mixedLog = [candidates[0], candidates[1], candidates[2], sampleLog[13]];
int goodLines = 0;
int badLines = 0;
foreach (string mixedLine in mixedLog)
{
    if (TryParseLine(mixedLine, out LogEvent? parsedEvent))
    {
        goodLines++;
    }
    else
    {
        badLines++;
    }
}
Console.WriteLine($"Good lines: {goodLines}, bad lines: {badLines}");
// Read the sample log line by line with a StringReader
string logText = string.Join("\n", sampleLog);
int linesRead = 0;
int eventsParsed = 0;
using (StringReader reader = new(logText))
{
    string? readLine = reader.ReadLine();
    while (readLine != null)
    {
        linesRead++;
        if (TryParseLine(readLine, out LogEvent? readEvent))
        {
            eventsParsed++;
        }
        readLine = reader.ReadLine();
    }
}
Console.WriteLine($"Lines read: {linesRead}, events: {eventsParsed}");
// Write the sample log to a file and read it back
DirectoryInfo logDirectory = Directory.CreateTempSubdirectory("seclog-");
try
{
    string logPath = Path.Combine(logDirectory.FullName, "sample.log");
    File.WriteAllLines(logPath, sampleLog);
    int fileEvents = 0;
    int fileLineNumber = 0;
    using (StreamReader fileReader = new(logPath))
    {
        string? fileLine = fileReader.ReadLine();
        while (fileLine != null)
        {
            fileLineNumber++;
            try
            {
                LogParser.ParseLine(fileLine, fileLineNumber);
                fileEvents++;
            }
            catch (InvalidLogLineException ex)
            {
                Console.WriteLine($"Line {ex.LineNumber} rejected: {ex.Message}");
            }
            fileLine = fileReader.ReadLine();
        }
    }
    Console.WriteLine($"Events from {Path.GetFileName(logPath)}: {fileEvents}");
    string missingPath = Path.Combine(logDirectory.FullName, "missing.log");
    try
    {
        File.ReadAllLines(missingPath);
    }
    catch (FileNotFoundException)
    {
        Console.WriteLine($"File not found: {Path.GetFileName(missingPath)}");
    }
}
finally
{
    Directory.Delete(logDirectory.FullName, true);
}
// Create a detection rule that inherits from DetectionRule
FailedLoginRule failedRule = new(Options.Create(detection));
Console.WriteLine(failedRule.Describe());
Console.WriteLine($"Threshold: {failedRule.Threshold}");
DetectionRule baseRule = failedRule;
Console.WriteLine(baseRule.Name);
// Describe a rule through the base type
Console.WriteLine(baseRule.Describe());
Console.WriteLine($"As text: {baseRule}");
// Check the incidents with every detection rule
List<DetectionRule> rules = [failedRule, new WatchListRule("dave")];
List<Incident> incidents = [bobIncident, daveIncident];
foreach (DetectionRule rule in rules)
{
    foreach (Incident incident in incidents)
    {
        Console.WriteLine($"{rule.Name} / {incident.User}: {rule.IsMatch(incident)}");
    }
}
// Check that the rule classes are sealed
Console.WriteLine($"FailedLoginRule sealed: {typeof(FailedLoginRule).IsSealed}");
Console.WriteLine($"WatchListRule sealed: {typeof(WatchListRule).IsSealed}");
Console.WriteLine($"DetectionRule sealed: {typeof(DetectionRule).IsSealed}");
// Combine exchangeable rules in a detector
Detector detector = new(
    [failedRule, new WatchListRule("dave"), new StatusRule()]);
foreach (Incident incident in incidents)
{
    int matches = detector.CountMatches(incident);
    Console.WriteLine(
        $"{incident.User}: {matches} of {detector.Rules.Count} rules match");
}
// Find the threshold rules among the rules of the detector
int fromBaseClass = 0;
foreach (IDetectionRule rule in detector.Rules)
{
    if (rule is FailedLoginRule thresholdRule)
    {
        Console.WriteLine(
            $"{thresholdRule.Name}: threshold {thresholdRule.Threshold}");
    }
    DetectionRule? fromBase = rule as DetectionRule;
    if (fromBase != null)
    {
        fromBaseClass++;
    }
}
Console.WriteLine(
    $"Rules from DetectionRule: {fromBaseClass} of {detector.Rules.Count}");
// Use enums for the event type and the severity
Console.WriteLine($"Event type: {LogParser.ParseEventType(EventTypeOf(sampleLog[1]))}");
Console.WriteLine($"Severity of bob: {SeverityOf(bobIncident.FailureCount)}");
Console.WriteLine($"Severity of dave: {SeverityOf(daveIncident.FailureCount)}");
Console.WriteLine($"Number of High: {(int)Severity.High}");
bool mediumOk = Enum.TryParse("Medium", out Severity parsedSeverity);
Console.WriteLine($"Medium parsed: {mediumOk} ({parsedSeverity})");
bool criticalOk = Enum.TryParse("Critical", out Severity unknownSeverity);
Console.WriteLine($"Critical parsed: {criticalOk} ({unknownSeverity})");
// Describe the detection window with a struct
TimeWindow bobWindow = new(bobTimes[0], window);
int inWindow = 0;
foreach (DateTimeOffset bobTime in bobTimes)
{
    if (bobWindow.Contains(bobTime))
    {
        inWindow++;
    }
}
Console.WriteLine($"In the window: {inWindow} of {bobTimes.Count}");
TimeWindow windowCopy = bobWindow;
Console.WriteLine($"Copy has the same start: {windowCopy.Start == bobWindow.Start}");
// Compare two events by their values
LogEvent eventA = LogParser.ParseLine(sampleLog[1], 2);
LogEvent eventB = LogParser.ParseLine(sampleLog[1], 2);
Console.WriteLine($"Same reference with ==: {eventA == eventB}");
Console.WriteLine($"Same values with Equals: {eventA.Equals(eventB)}");
HashSet<LogEvent> eventSet = [eventA, eventB, LogParser.ParseLine(sampleLog[2], 3)];
Console.WriteLine($"Distinct events: {eventSet.Count}");
// Store events as records
SecurityEvent recordA = ToSecurityEvent(logEvents[1]);
SecurityEvent recordB = ToSecurityEvent(logEvents[1]);
Console.WriteLine($"As record: {recordA}");
Console.WriteLine($"Records equal with ==: {recordA == recordB}");
var (recordTime, recordType, who) = recordA;
Console.WriteLine($"{who}: {recordType}");
// Sort the incidents: most failed logins first
List<Incident> sortedIncidents = [daveIncident, bobIncident];
sortedIncidents.Sort(new IncidentComparer());
foreach (Incident sorted in sortedIncidents)
{
    Console.WriteLine(sorted.Summary());
}
Console.WriteLine(
    $"Severity of the first: {SeverityOf(sortedIncidents[0].FailureCount)}");
// Count values with one generic method
List<string> typeNames = [];
List<EventType> typeValues = [];
foreach (string rawType in sampleLog)
{
    typeNames.Add(EventTypeOf(rawType));
    typeValues.Add(LogParser.ParseEventType(EventTypeOf(rawType)));
}
Console.WriteLine(
    $"login-failed texts: {CountOf<string>(typeNames, "login-failed")}");
Console.WriteLine(
    $"LoginFailed values: {CountOf(typeValues, EventType.LoginFailed)}");
Console.WriteLine($"Same events: {CountOf(logEvents, logEvents[1])}");
// Store events, incidents and rules in one generic store
Store<LogEvent> eventStore = new();
Store<Incident> incidentStore = new();
Store<IDetectionRule> ruleStore = new();
foreach (LogEvent stored in logEvents)
{
    eventStore.Add(stored);
}
incidentStore.Add(bobIncident);
incidentStore.Add(daveIncident);
foreach (IDetectionRule storedRule in detector.Rules)
{
    ruleStore.Add(storedRule);
}
Console.WriteLine($"Stored events: {eventStore.Count}");
Console.WriteLine($"Stored incidents: {incidentStore.Count}");
Console.WriteLine($"Stored rules: {ruleStore.Count}");
Console.WriteLine($"Last event: {eventStore.Get(eventStore.Count - 1).Describe()}");
Console.WriteLine($"First incident: {incidentStore.Items[0].Summary()}");
Console.WriteLine($"First rule: {ruleStore.Get(0).Name}");
// Count failures per user and per IP address with one generic counter
Counter<string> userCounter = new();
Counter<string> ipCounter = new();
foreach (string countedLine in sampleLog)
{
    if (EventTypeOf(countedLine) == "login-failed")
    {
        var (countedUser, countedIp) = ReadUserAndIp(countedLine);
        userCounter.Add(countedUser);
        ipCounter.Add(countedIp);
    }
}
foreach (string key in userCounter.SortedKeys())
{
    Console.WriteLine($"{key}: {userCounter.CountOf(key)}");
}
foreach (string key in ipCounter.SortedKeys())
{
    Console.WriteLine($"{key}: {ipCounter.CountOf(key)}");
}
Console.WriteLine($"Failures of carol: {userCounter.CountOf("carol")}");
// Read from the store safely
Console.WriteLine(
    $"Event 99: {eventStore.GetOrDefault(99)?.Describe() ?? "(none)"}");
Store<int> failureCounts = new();
foreach (string counted in userCounter.SortedKeys())
{
    failureCounts.Add(userCounter.CountOf(counted));
}
Console.WriteLine($"Count 99: {failureCounts.GetOrDefault(99)}");
bool foundIncidents = failureCounts.TryGet(1, out int secondCount);
Console.WriteLine($"TryGet 1: {foundIncidents}, {secondCount}");
bool foundEvent = eventStore.TryGet(99, out LogEvent? missing);
Console.WriteLine($"TryGet 99: {foundEvent}, null: {missing is null}");
// Print any collection with one generic method
PrintEach("Watch list", extendedWatchList);
string[] firstLines = sampleLog[..2];
PrintEach("First lines", firstLines);
List<string> sortedIps = [.. distinctIps];
sortedIps.Sort();
PrintEach("Distinct IPs", sortedIps);
PrintEach("Failure counts", failureCounts.Items);
// Use the store through its interface and read events as objects
IStore<SecurityEvent> securityEvents = new Store<SecurityEvent>();
securityEvents.Add(ToSecurityEvent(logEvents[0]));
securityEvents.Add(ToSecurityEvent(logEvents[1]));
Console.WriteLine($"Interface count: {securityEvents.Count}");
PrintEach<object>("Events as objects", securityEvents.Items);
// Check incidents with methods stored in a delegate
IncidentCheck manyFailures = HasManyFailures;
Console.WriteLine($"bob has many failures: {manyFailures(bobIncident)}");
Console.WriteLine($"dave has many failures: {manyFailures(daveIncident)}");
Console.WriteLine(
    $"Many failures: {CountMatching(incidents, HasManyFailures)}");
Console.WriteLine(
    $"Under investigation: {CountMatching(incidents, IsUnderInvestigation)}");
// Plug in detection rules as Func values
FuncRule manyFailuresRule = new("many failures", HasManyFailures);
ReportMatches(incidents, manyFailuresRule, Console.WriteLine);
List<IDetectionRule> mixedRules =
[
    manyFailuresRule,
    new FuncRule("under investigation", IsUnderInvestigation),
    new WatchListRule("dave"),
    new StatusRule(),
];
Detector mixedDetector = new(mixedRules);
foreach (Incident incident in incidents)
{
    Console.WriteLine(
        $"{incident.User}: {mixedDetector.CountMatches(incident)} matches");
}
// Write detection rules as lambdas
FuncRule failureRule = new(
    "many failures", incident => incident.FailureCount >= 5);
FuncRule daveRule = new(
    "user dave",
    incident => string.Equals(incident.User, "dave", StringComparison.Ordinal));
Detector lambdaDetector = new([failureRule, daveRule, new StatusRule()]);
foreach (Incident incident in incidents)
{
    Console.WriteLine(
        $"{incident.User}: {lambdaDetector.CountMatches(incident)} lambda matches");
}
List<Incident> ranked = new(incidents);
ranked.Sort(Comparer<Incident>.Create((a, b) =>
{
    int byCount = b.FailureCount.CompareTo(a.FailureCount);
    return byCount != 0 ? byCount : string.CompareOrdinal(a.User, b.User);
}));
foreach (Incident item in ranked)
{
    Console.WriteLine(item.Summary());
}
// Format alerts with a lambda that has a default value
var formatAlert = (Incident incident, string level = "ALERT") =>
    $"[{level}] {incident.Summary()}";
Console.WriteLine(formatAlert(bobIncident));
Console.WriteLine(formatAlert(daveIncident, "INFO"));
// Build threshold rules from captured values
FuncRule atLeast2 = new("at least 2", FailuresAtLeast(2));
FuncRule atLeast5 = new("at least 5", FailuresAtLeast(5));
Console.WriteLine($"{atLeast2.Name} / dave: {atLeast2.IsMatch(daveIncident)}");
Console.WriteLine($"{atLeast2.Name} / bob: {atLeast2.IsMatch(bobIncident)}");
Console.WriteLine($"{atLeast5.Name} / dave: {atLeast5.IsMatch(daveIncident)}");
Console.WriteLine($"{atLeast5.Name} / bob: {atLeast5.IsMatch(bobIncident)}");
// Use a static lambda for a rule that needs no captured variable
FuncRule noFailures = new(
    "no failures", static incident => incident.FailureCount == 0);
Console.WriteLine($"{noFailures.Name} / dave: {noFailures.IsMatch(daveIncident)}");
// Count the checks of a rule in a captured local variable
int checkedCount = 0;
FuncRule counting = new("counting", incident =>
{
    checkedCount++;
    return incident.FailureCount > 0;
});
foreach (Incident incident in incidents)
{
    counting.IsMatch(incident);
}
Console.WriteLine($"Checked incidents: {checkedCount}");
// Send one incident to several alert methods
int alertCount = 0;
Action<Incident>? alerts = PrintAlert;
alerts += PrintSeverity;
alerts += incident => alertCount++;
alerts(bobIncident);
Console.WriteLine($"Targets: {alerts.GetInvocationList().Length}");
alerts -= PrintSeverity;
alerts?.Invoke(daveIncident);
Console.WriteLine($"Targets: {alerts?.GetInvocationList().Length}");
Console.WriteLine($"Alerts counted: {alertCount}");
// Raise an event when an incident is found
IncidentMonitor monitor = new(detector, NullLogger<IncidentMonitor>.Instance);
int incidentsSeen = 0;
EventHandler<IncidentFoundEventArgs> report = (sender, e) =>
    Console.WriteLine($"{e.Incident.Summary()} ({e.Matches} rules)");
monitor.IncidentFound += report;
monitor.IncidentFound += (sender, e) => incidentsSeen++;
monitor.Check(incidents);
Console.WriteLine($"Found: {incidentsSeen}");
monitor.IncidentFound -= report;
monitor.Check(incidents);
Console.WriteLine($"Found: {incidentsSeen}");
// Ask log events with extension methods
int failedEvents = 0;
foreach (LogEvent logEvent in logEvents)
{
    if (logEvent.IsFailure())
    {
        failedEvents++;
    }
}
Console.WriteLine($"Failed events: {failedEvents}");
Console.WriteLine($"Event 2 is from bob: {logEvents[1].IsFrom("bob")}");
Console.WriteLine($"Static form: {LogEventExtensions.IsFrom(logEvents[1], "bob")}");
// Read incident facts through extension members
Console.WriteLine(Incident.TableHeader);
foreach (Incident row in incidents)
{
    Console.WriteLine(
        $"{row.Number} | {row.User} | {row.FailureCount} | {row.IsSevere}");
}
// Analyze the log with Where, Select and OrderBy
List<string> failedSummaries = logEvents
    .Where(e => e.IsFailure())
    .OrderByDescending(e => e.Timestamp)
    .Select(e => e.Describe())
    .ToList();
PrintEach("Failed logins, newest first", failedSummaries);
List<string> firstEvents = logEvents
    .Where(e => e.User is not null)
    .OrderBy(e => e.User, StringComparer.Ordinal)
    .ThenBy(e => e.Timestamp)
    .Take(3)
    .Select(e => e.Describe())
    .ToList();
PrintEach("First 3 events by user", firstEvents);
Console.WriteLine($"Events in the log: {logEvents.Count}");
// Read the lines of one user with an iterator
foreach (string line in LinesOfUser(sampleLog, "bob", 3))
{
    Console.WriteLine(line);
}
PrintEach("Lines of alice", LinesOfUser(sampleLog, "alice", 10));
// See when a LINQ query runs
List<string> liveLog = [.. sampleLog[..4]];
var failedQuery = liveLog.Where(
    line => string.Equals(EventTypeOf(line), "login-failed", StringComparison.Ordinal));
Console.WriteLine($"Failed logins in the query: {failedQuery.Count()}");
liveLog.Add(sampleLog[4]);
Console.WriteLine($"Failed logins after adding a line: {failedQuery.Count()}");
List<string> snapshot = failedQuery.ToList();
liveLog.Add(sampleLog[5]);
Console.WriteLine($"Failed logins in the snapshot: {snapshot.Count}");
Console.WriteLine($"Failed logins in the query now: {failedQuery.Count()}");
// Project log events into anonymous objects
var failureRows = logEvents
    .Where(e => e.IsFailure())
    .Select(e => new { e.User, e.Timestamp.Hour });
foreach (var row in failureRows)
{
    Console.WriteLine(row);
}
var rowA = new { User = "bob", Hour = 8 };
var rowB = new { User = "bob", Hour = 8 };
Console.WriteLine($"Same rows: {rowA.Equals(rowB)}");
// Count failures per hour and find the top IP addresses
foreach (HourCount hourCount in LogAnalysis.FailuresPerHour(logEvents))
{
    Console.WriteLine($"{hourCount.Hour:D2}:00 {hourCount.Count} failures");
}
var topIps = sampleLog
    .Select(line => ReadUserAndIp(line).Ip)
    .GroupBy(ip => ip)
    .OrderByDescending(g => g.Count())
    .ThenBy(g => g.Key, StringComparer.Ordinal)
    .Take(3);
foreach (var ipGroup in topIps)
{
    Console.WriteLine($"{ipGroup.Key} {ipGroup.Count()}");
}
Console.WriteLine(incidents.MaxBy(i => i.FailureCount)?.Summary());
// Rate reported messages as possible phishing
string[] reportedMessages =
[
    "Your account is locked. Confirm your password now at http://example.com/verify or it will be deleted.",
    "Urgent: unusual sign-in. Open http://example.org/secure and enter your login data within 24 hours.",
    "You won a prize! Click http://example.com/claim to get your reward today.",
    "The team meeting moves to 10:00 tomorrow. The agenda is in the shared folder.",
    "Reminder: the quarterly report is due on Friday. Please send your part to the team lead.",
    "Thanks for your help with the server update. The maintenance window ended without problems.",
];
string[] signalWords =
    ["password", "login", "urgent", "locked", "prize", "click", "verify"];
for (int number = 1; number <= reportedMessages.Length; number++)
{
    bool phishing = IsPossiblePhishing(reportedMessages[number - 1], signalWords);
    string rating = phishing ? "possible phishing" : "harmless";
    Console.WriteLine($"Message {number}: {rating}");
}
Console.WriteLine($"All messages have text: {reportedMessages.All(m => m.Length > 0)}");
string firstHarmless = reportedMessages.First(m => !IsPossiblePhishing(m, signalWords));
Console.WriteLine($"First harmless message: {Array.IndexOf(reportedMessages, firstHarmless) + 1}");
Console.WriteLine(
    logEvents.FirstOrDefault(e => e.IsFrom("bob") && e.EventType == "logout")?.Describe()
    ?? "no logout of bob");
// Join incidents with log events and compare user sets
var eventsPerIncident = incidents.GroupJoin(
    logEvents,
    incident => incident.User,
    logEvent => logEvent.User,
    (incident, events) => $"{incident.User} {events.Count()}");
PrintEach("Log events per incident", eventsPerIncident);
var failedUsers = logEvents
    .Where(e => e.IsFailure() && e.User is not null)
    .Select(e => e.User)
    .Distinct();
var successUsers = logEvents
    .Where(e => e.EventType == "login-success" && e.User is not null)
    .Select(e => e.User)
    .Distinct();
PrintEach("Failed and logged in", failedUsers.Intersect(successUsers));
PrintEach("Failed, never logged in", failedUsers.Except(successUsers));
PrintEach("All users with a login", failedUsers.Union(successUsers));
// Count failures per hour with query syntax
var queryRows =
    from e in logEvents
    where e.IsFailure()
    group e by e.Timestamp.Hour into hourGroup
    orderby hourGroup.Key
    select new { Hour = hourGroup.Key, Count = hourGroup.Count() };
foreach (var row in queryRows)
{
    Console.WriteLine($"{row.Hour:D2}:00 {row.Count} failures");
}
var methodRows = logEvents
    .Where(e => e.IsFailure())
    .GroupBy(e => e.Timestamp.Hour)
    .OrderBy(g => g.Key)
    .Select(g => new { Hour = g.Key, Count = g.Count() });
Console.WriteLine($"Same rows as method syntax: {queryRows.SequenceEqual(methodRows)}");
// Describe mixed items with type patterns
object?[] mixedItems =
[
    logEvents[0],
    ToSecurityEvent(logEvents[1]),
    sampleLog[2],
    null,
    42,
];
foreach (object? mixed in mixedItems)
{
    Console.WriteLine(DescribeItem(mixed));
}
Console.WriteLine($"mixedItems[2] is string: {mixedItems[2] is string}");
Console.WriteLine($"mixedItems[3] is string: {mixedItems[3] is string}");
// Check log lines with logical patterns
int morningFailures = 0;
int otherThanSuccess = 0;
foreach (string logLine in sampleLog)
{
    if (IsMorningFailure(logLine))
    {
        morningFailures++;
    }
    if (EventTypeOf(logLine) is not ("login-success" or "logout"))
    {
        otherThanSuccess++;
    }
}
Console.WriteLine($"Failures between 08:00 and 08:59: {morningFailures}");
Console.WriteLine($"Lines that are neither success nor logout: {otherThanSuccess}");
// Find log events with property patterns
int bobLoginFailures = 0;
int nineOclock = 0;
foreach (LogEvent matchedEvent in logEvents)
{
    if (matchedEvent is { EventType: "login-failed", User: "bob" })
    {
        bobLoginFailures++;
    }
    if (matchedEvent is { Timestamp.Hour: 9 })
    {
        nineOclock++;
    }
}
Console.WriteLine($"Failed logins of bob: {bobLoginFailures}");
Console.WriteLine($"Events in the hour 09:00 to 09:59: {nineOclock}");
foreach (Incident severeIncident in incidents)
{
    if (severeIncident is { FailureCount: >= 5 })
    {
        Console.WriteLine(severeIncident.Summary());
    }
}
// Split events with Deconstruct and positional patterns
var (splitTime, splitType, splitUser) = logEvents[1];
Console.WriteLine($"Time: {splitTime:yyyy-MM-dd HH:mm}");
Console.WriteLine($"Type: {splitType}");
Console.WriteLine($"User: {splitUser}");
int fromServer = 0;
foreach (string serverLine in sampleLog)
{
    if (ReadUserAndIp(serverLine) is (_, "203.0.113.7"))
    {
        fromServer++;
    }
}
Console.WriteLine($"Lines from 203.0.113.7: {fromServer}");
int failedWithUser = 0;
foreach (LogEvent checkedEvent in logEvents)
{
    if (ToSecurityEvent(checkedEvent) is (_, EventType.LoginFailed, var caller)
        && caller is not null)
    {
        failedWithUser++;
    }
}
Console.WriteLine($"Failed logins with a user: {failedWithUser}");
// Parse log lines with list patterns
int parsedCount = 0;
foreach (string rawText in sampleLog)
{
    if (LogParser.ParseWithPatterns(rawText) is not null)
    {
        parsedCount++;
    }
}
Console.WriteLine($"Lines parsed with list patterns: {parsedCount}");
string[] brokenLines =
[
    "2026-03-02T08:00:12Z login-success",
    "2026-03-02T08:00:12Z reboot user=alice ip=192.0.2.10",
];
foreach (string brokenLine in brokenLines)
{
    SecurityEvent? result = LogParser.ParseWithPatterns(brokenLine);
    Console.WriteLine(result is null ? "invalid" : "valid");
}
// Choose an action per event with a switch expression
foreach (LogEvent actionEvent in logEvents)
{
    if (actionEvent.User == "bob")
    {
        int userFailures = logEvents.Count(other =>
            other.User == actionEvent.User && other.EventType == "login-failed");
        string action = ActionFor(ToSecurityEvent(actionEvent), userFailures);
        Console.WriteLine($"{actionEvent.EventType} of bob: {action}");
    }
}
// Update incident states with with expressions
IncidentState opened = StateOf(bobIncident) with { Status = "open" };
IncidentState investigating = opened with { Status = "investigating" };
IncidentState done = investigating with { Status = "closed" };
Console.WriteLine(opened);
Console.WriteLine(investigating);
Console.WriteLine(done);
Console.WriteLine($"Status of the first state: {opened.Status}");
Console.WriteLine($"Same state: {opened == investigating}");
Console.WriteLine($"Same after an empty with: {opened == opened with { }}");
// Parse lines into a union of event or error
Console.WriteLine(Describe(LogParser.Parse(sampleLog[1])));
Console.WriteLine(Describe(LogParser.Parse(brokenLines[0])));
Console.WriteLine(Describe(LogParser.Parse(brokenLines[1])));
// Describe incident statuses with a closed hierarchy
IncidentStatus[] statuses =
[
    new OpenStatus(),
    new InvestigatingStatus("analyst-1"),
    new ResolvedStatus("false alarm"),
];
foreach (IncidentStatus shownStatus in statuses)
{
    Console.WriteLine(StatusLine(shownStatus));
}
// Choose between a new rule and a new switch
foreach (IncidentStatus statusToCheck in statuses)
{
    Console.WriteLine($"Needs attention ({statusToCheck.GetType().Name}): {NeedsAttention(statusToCheck)}");
}
FuncRule moreThan3 = new("more than 3 failures", FailuresAtLeast(4));
Detector extendedDetector = new([.. detector.Rules, moreThan3]);
foreach (Incident candidate in incidents)
{
    Console.WriteLine(
        $"{candidate.User}: {extendedDetector.CountMatches(candidate)} of {extendedDetector.Rules.Count} rules match");
}
// Simulate two log sources in memory
Dictionary<string, string[]> logSources = new()
{
    ["web"] = sampleLog[..7],
    ["vpn"] = sampleLog[7..],
};
// Load a log source asynchronously
string[] webLines = await LoadSourceAsync("web");
Console.WriteLine($"Loaded web: {webLines.Length} lines");
// Watch the order of steps in an async call
Task<int> pending = CountFailuresAsync("vpn");
Console.WriteLine($"Call returned, completed: {pending.IsCompleted}");
int vpnFailures = await pending;
Console.WriteLine($"Failures in vpn: {vpnFailures}");
// Catch the error of a failed source at the await
try
{
    string[] backupLines = await LoadSourceAsync("backup");
    Console.WriteLine($"Loaded backup: {backupLines.Length} lines");
}
catch (KeyNotFoundException ex)
{
    Console.WriteLine($"Source backup failed: {ex.GetType().Name}");
}
Task<string[]> failed = LoadSourceAsync("backup");
try
{
    await failed;
}
catch (KeyNotFoundException)
{
    Console.WriteLine($"Faulted: {failed.IsFaulted}");
}
// Load all sources at the same time
string[][] all = await Task.WhenAll(LoadSourceAsync("web"), LoadSourceAsync("vpn"));
Console.WriteLine($"Sources loaded: {all.Length}, lines: {all.Sum(source => source.Length)}");
// Limit the waiting time of a slow source
try
{
    await LoadSlowSourceAsync().WaitAsync(TimeSpan.FromMilliseconds(50));
}
catch (TimeoutException)
{
    Console.WriteLine("Slow source: timeout");
}
// Cancel loading the sources
using CancellationTokenSource cancelSource = new();
cancelSource.Cancel();
try
{
    await LoadSourceAsync("web", cancelSource.Token);
}
catch (OperationCanceledException ex)
{
    Console.WriteLine($"Loading web: canceled ({ex.GetType().Name})");
}
using CancellationTokenSource slowSource = new();
slowSource.CancelAfter(TimeSpan.FromMilliseconds(50));
try
{
    await LoadSlowSourceAsync(slowSource.Token);
}
catch (OperationCanceledException)
{
    Console.WriteLine("Slow source: canceled");
}
// Read the sources line by line as async streams
int streamFailures = 0;
foreach (string sourceName in new[] { "web", "vpn" })
{
    await foreach (string line in ReadSourceLinesAsync(sourceName))
    {
        if (line.Contains("login-failed"))
        {
            streamFailures++;
        }
    }
}
Console.WriteLine($"Failures in all sources: {streamFailures}");
// Composition root: the container puts the readers together
IServiceCollection services = builder.Services;
services.AddSingleton<IReportWriter, ConsoleReportWriter>();
services.AddSingleton<LogReaderFactory>();
// Lifetimes: shared state is a singleton, one run is scoped, a helper is transient
services.AddSingleton(TimeProvider.System);
services.AddSingleton<FailureCounter>();
services.AddScoped<AnalysisRun>();
services.AddTransient<FailureMatcher>();
// Logging: the minimum level decides which messages are written, the provider where
builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(LogLevel.Information);
builder.Logging.AddFilter("Microsoft.Hosting", LogLevel.Warning);
// The context from the container logs every SQL command: keep the output stable
builder.Logging.AddFilter("Microsoft.EntityFrameworkCore", LogLevel.Warning);
builder.Logging.AddProvider(new ConsoleLoggerProvider());
services.AddSingleton(detector);
services.AddTransient<IncidentMonitor>();
services.AddSingleton<LineValidator>();
// Options: bind the section to the options class and check its attributes
services.AddOptions<DetectionOptions>()
    .Bind(configuration.GetSection(DetectionOptions.SectionName))
    .ValidateDataAnnotations()
    .ValidateOnStart();
// Validation finds lifetime mistakes when the host builds the provider
builder.ConfigureContainer(new DefaultServiceProviderFactory(
    new ServiceProviderOptions { ValidateScopes = true, ValidateOnBuild = true }));
// The database context comes from the container: one scoped instance per scope
services.AddDbContext<SecLogDbContext>(options => options.UseSqlite(
    configuration.GetConnectionString("SecLog")));
// The analysis runs in a background service that stops the host when it is done
services.AddHostedService(provider => new AnalysisService(
    stoppingToken => RunAnalysisAsync(provider, stoppingToken),
    provider.GetRequiredService<IHostApplicationLifetime>()));
using IHost host = builder.Build();
await host.RunAsync();
// End with exit code 0 (success); later steps add their blocks before this line
return 0;

// Run the whole analysis with the services of the host
async Task RunAnalysisAsync(
    IServiceProvider provider, CancellationToken stoppingToken)
{
    LogReaderFactory readers = provider.GetRequiredService<LogReaderFactory>();
    // One analysis run of both sources inside one scope
    using (IServiceScope analysisScope = provider.CreateScope())
    {
        AnalysisRun analysis = analysisScope.ServiceProvider.GetRequiredService<AnalysisRun>();
        analysis.Analyze(logSources["web"]);
        analysis.Analyze(logSources["vpn"]);
    }
    // Reject invalid lines: the validator logs every line it rejects
    LineValidator validator = provider.GetRequiredService<LineValidator>();
    ILogger runLogger = provider.GetRequiredService<ILoggerFactory>().CreateLogger("SecLog.Run");
    using (runLogger.BeginScope("File {FileName}", "sample.log"))
    {
        validator.TryParse(sampleLog[1], 2);
        validator.TryParse(brokenLines[0], 3);
        validator.TryParse(brokenLines[1], 4);
        runLogger.LogInformation("Validation finished");
    }
    LogSourceReader webReader = readers.Create("web", logSources["web"]);
    LogSourceReader vpnReader = readers.Create("vpn", logSources["vpn"]);
    // Release a log source reader asynchronously
    await using (LogSourceReader reader = webReader)
    {
        Console.WriteLine($"Failures in {reader.Name}: {reader.CountFailures()}");
    }
    {
        await using LogSourceReader reader = vpnReader;
        Console.WriteLine($"Failures in {reader.Name}: {reader.CountFailures()}");
        Console.WriteLine("End of the block");
    }
    Console.WriteLine("After the blocks");
    // Start async work from an event handler safely
    List<Task> notifications = [];
    IncidentMonitor notifier = provider.GetRequiredService<IncidentMonitor>();
    notifier.IncidentFound += (sender, e) => notifications.Add(NotifyAsync(e.Incident));
    notifier.Check(incidents);
    await Task.WhenAll(notifications);
    Console.WriteLine($"Notifications sent: {notifications.Count}");
    // Count failures on the thread pool
    int[] perSource = await Task.WhenAll(
        Task.Run(() => CountFailuresIn(logSources["web"])),
        Task.Run(() => CountFailuresIn(logSources["vpn"])));
    Console.WriteLine($"Failures per source: {string.Join(", ", perSource)}");
    // Count failures of all sources with a shared counter and a lock
    Lock failureLock = new();
    int sharedFailures = 0;
    List<Task> workers = [];
    foreach (string[] lines in logSources.Values)
    {
        workers.Add(Task.Run(() =>
        {
            foreach (string line in lines)
            {
                if (line.Contains("login-failed"))
                {
                    lock (failureLock)
                    {
                        sharedFailures++;
                    }
                }
            }
        }));
    }
    await Task.WhenAll(workers);
    Console.WriteLine($"Shared failure count: {sharedFailures}");
    // Count failures with an atomic operation
    int atomicFailures = 0;
    List<Task> atomicWorkers = [];
    foreach (string[] lines in logSources.Values)
    {
        atomicWorkers.Add(Task.Run(() =>
        {
            foreach (string line in lines)
            {
                if (line.Contains("login-failed"))
                {
                    Interlocked.Increment(ref atomicFailures);
                }
            }
        }));
    }
    await Task.WhenAll(atomicWorkers);
    Console.WriteLine($"Atomic failure count: {atomicFailures}");
    // Count failures per user with a concurrent dictionary
    ConcurrentDictionary<string, int> concurrentFailures = new();
    List<Task> userWorkers = [];
    foreach (string[] lines in logSources.Values)
    {
        userWorkers.Add(Task.Run(() =>
        {
            foreach (string line in lines)
            {
                string? failedUser = FindUser(line);
                if (failedUser is not null && line.Contains("login-failed"))
                {
                    concurrentFailures.AddOrUpdate(failedUser, 1, (_, count) => count + 1);
                }
            }
        }));
    }
    await Task.WhenAll(userWorkers);
    foreach (KeyValuePair<string, int> pair in concurrentFailures.OrderBy(p => p.Key))
    {
        Console.WriteLine($"{pair.Key}: {pair.Value}");
    }
    // Rate all log lines in parallel with PLINQ
    int parallelFailures = sampleLog.AsParallel().Count(row => row.Contains("login-failed"));
    Console.WriteLine($"Failures (PLINQ): {parallelFailures}");
    ConcurrentDictionary<string, int> failuresPerSource = new();
    Parallel.ForEach(logSources, source =>
    {
        failuresPerSource[source.Key] = CountFailuresIn(source.Value);
    });
    foreach (KeyValuePair<string, int> pair in failuresPerSource.OrderBy(p => p.Key))
    {
        Console.WriteLine($"{pair.Key}: {pair.Value}");
    }
    // Analyze many log files with workers and a channel
    DirectoryInfo workDirectory = Directory.CreateTempSubdirectory("seclog-");
    try
    {
        string[][] fileParts = [sampleLog[0..4], sampleLog[4..8], sampleLog[8..11], sampleLog[11..]];
        List<string> fileNames = [];
        for (int partIndex = 0; partIndex < fileParts.Length; partIndex++)
        {
            string partName = $"part-{partIndex + 1}.log";
            File.WriteAllLines(Path.Combine(workDirectory.FullName, partName), fileParts[partIndex]);
            fileNames.Add(partName);
        }
        Channel<string> fileQueue = Channel.CreateBounded<string>(2);
        ConcurrentDictionary<string, int> failuresPerFile = new();
        using CancellationTokenSource workersSource = new();
        Task fileProducer = Task.Run(async () =>
        {
            foreach (string queuedName in fileNames)
            {
                await fileQueue.Writer.WriteAsync(queuedName, workersSource.Token);
            }
            fileQueue.Writer.Complete();
        });
        List<Task> fileWorkers = [];
        for (int workerIndex = 0; workerIndex < 2; workerIndex++)
        {
            fileWorkers.Add(Task.Run(async () =>
            {
                await foreach (string nextName in fileQueue.Reader.ReadAllAsync(workersSource.Token))
                {
                    string[] fileLines = await File.ReadAllLinesAsync(
                        Path.Combine(workDirectory.FullName, nextName), workersSource.Token);
                    failuresPerFile[nextName] = CountFailuresIn(fileLines);
                }
            }));
        }
        await fileProducer;
        await Task.WhenAll(fileWorkers);
        foreach (KeyValuePair<string, int> filePair in failuresPerFile.OrderBy(p => p.Key))
        {
            Console.WriteLine($"{filePair.Key}: {filePair.Value}");
        }
        Console.WriteLine($"Total failures in files: {failuresPerFile.Values.Sum()}");
    }
    finally
    {
        Directory.Delete(workDirectory.FullName, true);
    }
    // Report the analysis steps: StepLog prints the name of the calling method
    AnalysisSteps.CountFailures(sampleLog);
    AnalysisSteps.CountUsers(sampleLog);
    // Check if the incident of bob is still active: the clock is a parameter
    DateTimeOffset lastSeen = bobIncident.LastFailure!.Value;
    TimeSpan activeWindow = TimeSpan.FromMinutes(10);
    IncidentActivity soon = new(
        new FixedTimeProvider(lastSeen.AddMinutes(3)), activeWindow);
    IncidentActivity later = new(
        new FixedTimeProvider(lastSeen.AddMinutes(11)), activeWindow);
    Console.WriteLine($"Active after 3 minutes: {soon.IsActive(bobIncident)}");
    Console.WriteLine($"Active after 11 minutes: {later.IsActive(bobIncident)}");
    // Store the incidents in the SQLite database and read them back
    Console.WriteLine("== Database ==");
    // One scope is one unit of work: it gets its own context and disposes it
    await using (AsyncServiceScope dbScope = provider.CreateAsyncScope())
    {
        SecLogDbContext db = dbScope.ServiceProvider
            .GetRequiredService<SecLogDbContext>();
        // Apply the migrations that the database does not have yet
        await db.Database.MigrateAsync(stoppingToken);
        foreach (string migration in
            await db.Database.GetAppliedMigrationsAsync(stoppingToken))
        {
            Console.WriteLine($"Applied migration: {migration[(migration.IndexOf('_') + 1)..]}");
        }
        db.Incidents.AddRange(incidents.Select(StoredIncident.From));
        await db.SaveChangesAsync(stoppingToken);
        List<StoredIncident> stored = await db.Incidents
            .OrderBy(i => i.Id)
            .ToListAsync(stoppingToken);
        Console.WriteLine($"Stored incidents: {stored.Count}");
        foreach (StoredIncident row in stored)
        {
            Console.WriteLine(
                $"#{row.Id} {row.User}: {row.FailureCount} failed logins, {row.Status} ({row.Source})");
        }
        // Count the stored events per incident: the database does the counting
        var eventCounts = await db.Incidents
            .OrderBy(i => i.Id)
            .Select(i => new { i.User, Count = i.Events.Count })
            .ToListAsync(stoppingToken);
        foreach (var eventCount in eventCounts)
        {
            Console.WriteLine($"Stored events of {eventCount.User}: {eventCount.Count}");
        }
        // Ask the database: critical incidents, most failures first
        // (the filter is an expression tree, so the database filters)
        var severe = db.Incidents
            .Where(IncidentFilters.AtLeastFailures(maxFailures))
            .OrderByDescending(i => i.FailureCount)
            .Select(i => new { i.User, i.FailureCount });
        Console.WriteLine(severe.ToQueryString());
        foreach (var row in await severe.ToListAsync(stoppingToken))
        {
            Console.WriteLine($"Severe: {row.User} {row.FailureCount}");
        }
        // Load every incident with its failed logins in one query
        var withEvents = db.Incidents
            .Include(i => i.Events)
            .OrderBy(i => i.Id);
        Console.WriteLine(withEvents.ToQueryString());
        foreach (StoredIncident row in await withEvents.ToListAsync(stoppingToken))
        {
            Console.WriteLine($"Failures of {row.User}:");
            foreach (StoredEvent failure in row.Events.OrderBy(e => e.Timestamp))
            {
                Console.WriteLine($"  {failure.Timestamp:yyyy-MM-dd'T'HH:mm:ss'Z'}");
            }
        }
        // Store a new open incident, then set it to investigating
        db.Incidents.Add(new StoredIncident
        {
            User = "carol",
            Source = "sample log",
            Status = "open",
            FailureCount = 1,
        });
        await db.SaveChangesAsync(stoppingToken);
        StoredIncident carol = await db.Incidents
            .SingleAsync(i => i.User == "carol", stoppingToken);
        string before = db.Entry(carol).State.ToString();
        carol.Status = "investigating";
        string changed = db.Entry(carol).State.ToString();
        int written = await db.SaveChangesAsync(stoppingToken);
        string after = db.Entry(carol).State.ToString();
        Console.WriteLine($"State: {before} → {changed} → {after}");
        Console.WriteLine($"Rows written: {written}");
        // The report only reads, so it needs no tracking
        await foreach (StoredIncident row in db.Incidents
            .AsNoTracking()
            .OrderBy(i => i.Id)
            .AsAsyncEnumerable()
            .WithCancellation(stoppingToken))
        {
            Console.WriteLine($"Report: #{row.Id} {row.User} {row.Status}");
        }
        // Search the incidents of a user name that comes from outside:
        // FromSql sends the value as a parameter, it never becomes SQL text
        string searchUser = configuration["Search:User"] ?? "";
        var searchResult = db.Incidents
            .FromSql($"SELECT * FROM Incidents WHERE User = {searchUser}")
            .OrderBy(i => i.Id);
        Console.WriteLine(searchResult.ToQueryString());
        foreach (StoredIncident row in await searchResult.ToListAsync(stoppingToken))
        {
            Console.WriteLine($"Found: #{row.Id} {row.User} {row.FailureCount}");
        }
        // Print the stored incidents as JSON, in the shape the API will use
        Console.WriteLine("== JSON ==");
        List<IncidentSummary> summaries = (await db.Incidents
            .AsNoTracking()
            .OrderBy(i => i.Id)
            .ToListAsync(stoppingToken))
            .Select(IncidentSummary.From)
            .ToList();
        JsonSerializerOptions jsonOptions =
            new(JsonSerializerDefaults.Web) { WriteIndented = true };
        Console.WriteLine(JsonSerializer.Serialize(summaries, jsonOptions));
    }
}

// Print one alert line for an incident
static void PrintAlert(Incident incident)
{
    Console.WriteLine($"ALERT: {incident.Summary()}");
}

// Print the severity of an incident
void PrintSeverity(Incident incident)
{
    Console.WriteLine($"Severity: {SeverityOf(incident.FailureCount)}");
}

// Build a check that captures the threshold: every call has its own copy
static Func<Incident, bool> FailuresAtLeast(int threshold)
{
    return incident => incident.FailureCount >= threshold;
}

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

// Turn one log line into a LogEvent without exceptions: false if it is invalid
bool TryParseLine(string line, out LogEvent? logEvent)
{
    logEvent = null;
    string[] parts = line.Split(' ');
    if (parts.Length != 4)
    {
        return false;
    }
    if (!DateTimeOffset.TryParse(
            parts[0], CultureInfo.InvariantCulture, DateTimeStyles.None,
            out DateTimeOffset time))
    {
        return false;
    }
    string kind = parts[1];
    bool known =
        string.Equals(kind, "login-success", StringComparison.Ordinal) ||
        string.Equals(kind, "login-failed", StringComparison.Ordinal) ||
        string.Equals(kind, "logout", StringComparison.Ordinal);
    if (!known)
    {
        return false;
    }
    logEvent = new LogEvent(time, kind, FindUser(line));
    return true;
}

// Rate the severity of a number of failures as an enum value
Severity SeverityOf(int failures)
{
    return SeverityRating.For(failures);
}

// Convert a LogEvent into the compact record form
SecurityEvent ToSecurityEvent(LogEvent logEvent)
{
    EventType type = LogParser.ParseEventType(logEvent.EventType);
    return new SecurityEvent(logEvent.Timestamp, type, logEvent.User);
}

// Count the items that are equal to a value, for any item type
static int CountOf<T>(IReadOnlyList<T> items, T value)
{
    int count = 0;
    foreach (T item in items)
    {
        if (Equals(item, value))
        {
            count++;
        }
    }
    return count;
}

// Print a title and one line per item, for any collection
static void PrintEach<T>(string title, IEnumerable<T> items)
{
    Console.WriteLine(title);
    foreach (T item in items)
    {
        Console.WriteLine($"- {item}");
    }
}

// Check one incident: five or more failed logins
static bool HasManyFailures(Incident incident)
{
    return incident.FailureCount >= 5;
}

// Check one incident: the status is "investigating"
static bool IsUnderInvestigation(Incident incident)
{
    return string.Equals(
        incident.Status, "investigating", StringComparison.Ordinal);
}

// Count the incidents for which a check returns true
static int CountMatching(IReadOnlyList<Incident> items, IncidentCheck check)
{
    int count = 0;
    foreach (Incident item in items)
    {
        if (check(item))
        {
            count++;
        }
    }
    return count;
}

// Report one line for each incident that matches a rule
static void ReportMatches(
    IReadOnlyList<Incident> items, IDetectionRule rule, Action<string> report)
{
    foreach (Incident item in items)
    {
        if (rule.IsMatch(item))
        {
            report($"{rule.Name}: {item.Summary()}");
        }
    }
}

// Give the lines of one user, at most maxCount of them
IEnumerable<string> LinesOfUser(
    IEnumerable<string> lines, string user, int maxCount)
{
    int given = 0;
    foreach (string line in lines)
    {
        if (string.Equals(
            ReadUserAndIp(line).User, user, StringComparison.Ordinal))
        {
            yield return line;
            given++;
            if (given == maxCount)
            {
                yield break;
            }
        }
    }
}

// Rate a reported message: a plain http:// link and at least one signal word
bool IsPossiblePhishing(string message, string[] signalWords)
{
    bool hasLink = message.Contains("http://", StringComparison.Ordinal);
    return hasLink && signalWords.Any(word =>
        message.Contains(word, StringComparison.OrdinalIgnoreCase));
}

// Describe one item of any type: each switch arm has a type pattern
string DescribeItem(object? item) => item switch
{
    null => "missing",
    LogEvent e => $"log event: {e.Describe()}",
    SecurityEvent s => $"security event of {s.User ?? "unknown"}",
    string line => $"raw line: {line}",
    _ => "unknown item",
};

// A failed login in the hour from 08:00 to 08:59 (timestamps are UTC)
bool IsMorningFailure(string line) =>
    EventTypeOf(line) is "login-failed"
    && TimestampOf(line).Hour is >= 8 and < 9;

// Choose an action for an event: the first matching arm wins
string ActionFor(SecurityEvent securityEvent, int failures) =>
    (securityEvent.Type, failures) switch
    {
        (EventType.LoginFailed, >= 5) => "lock account",
        (EventType.LoginFailed, _) => "watch",
        (EventType.LoginSuccess, _) when securityEvent.User is null
            => "check user",
        (EventType.LoginSuccess or EventType.Logout, _) => "none",
        _ => "unknown",
    };

// Build the state of an incident
IncidentState StateOf(Incident incident) =>
    new(incident.Number, incident.User, incident.FailureCount,
        SeverityOf(incident.FailureCount), incident.Status);

// Describe the result: one arm per case type, no discard arm
string Describe(ParseResult result) => result switch
{
    SecurityEvent securityEvent =>
        $"event: {securityEvent.Type} of {securityEvent.User ?? "unknown"}",
    LineError error => $"error: {error.Reason}",
};

// Describe a status: one arm per direct derived type, no discard arm
string StatusLine(IncidentStatus status) => status switch
{
    OpenStatus => "open: waiting for an analyst",
    InvestigatingStatus investigating => $"investigating: {investigating.Analyst}",
    ResolvedStatus resolved => $"resolved: {resolved.Note}",
};

// Decide if a status needs attention: a new operation, the status types stay unchanged
bool NeedsAttention(IncidentStatus status) => status switch
{
    OpenStatus => true,
    InvestigatingStatus => false,
    ResolvedStatus => false,
};

// Load one log source: the wait is simulated with a short delay
async Task<string[]> LoadSourceAsync(string name, CancellationToken token = default)
{
    await Task.Delay(10, token);
    return logSources[name];
}

// Count the failed logins of a source: the first line runs before the await
async Task<int> CountFailuresAsync(string name)
{
    Console.WriteLine($"Loading {name}");
    string[] lines = await LoadSourceAsync(name);
    return lines.Count(line => line.Contains("login-failed"));
}

// Load a slow source: the delay is much longer than the timeout
async Task<string[]> LoadSlowSourceAsync(CancellationToken token = default)
{
    await Task.Delay(5000, token);
    return [];
}

// Read the lines of a source one by one: each line arrives after a short wait
async IAsyncEnumerable<string> ReadSourceLinesAsync(string name,
    [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken token = default)
{
    foreach (string line in logSources[name])
    {
        await Task.Delay(1, token);
        yield return line;
    }
}

// Notify about an incident: the delay stands for sending a message
async Task NotifyAsync(Incident incident)
{
    await Task.Delay(1);
}

// Count the failed logins in some lines: CPU-bound work, no waiting
int CountFailuresIn(string[] lines)
{
    return lines.Count(line => line.Contains("login-failed"));
}
