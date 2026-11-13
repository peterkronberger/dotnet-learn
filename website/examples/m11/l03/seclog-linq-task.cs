using System.Text;
using System.Globalization;
using SecLog.Model;
// @fold-start
Console.WriteLine("SecLog project profile");
Console.WriteLine("Purpose: find suspicious logins in made-up login logs");
Console.WriteLine("Language: C#");
Console.WriteLine("Platform: .NET");
Console.WriteLine("Variant: .NET (Linux server and Windows laptops)");
Console.WriteLine("Version: .NET 11 (STS)");
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
        LogEvent goodEvent = ParseLine(candidates[number - 1], number);
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
                ParseLine(fileLine, fileLineNumber);
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
FailedLoginRule failedRule = new(5);
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
Console.WriteLine($"Event type: {ParseEventType(EventTypeOf(sampleLog[1]))}");
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
LogEvent eventA = ParseLine(sampleLog[1], 2);
LogEvent eventB = ParseLine(sampleLog[1], 2);
Console.WriteLine($"Same reference with ==: {eventA == eventB}");
Console.WriteLine($"Same values with Equals: {eventA.Equals(eventB)}");
HashSet<LogEvent> eventSet = [eventA, eventB, ParseLine(sampleLog[2], 3)];
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
    typeValues.Add(ParseEventType(EventTypeOf(rawType)));
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
IncidentMonitor monitor = new(detector);
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
// @fold-end
// End with exit code 0 (success); later steps add their blocks before this line
return 0;

// @fold-start
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

// Turn one log line into a LogEvent, or reject it with an own exception
LogEvent ParseLine(string line, int lineNumber)
{
    string[] parts = line.Split(' ');
    if (parts.Length != 4)
    {
        throw new InvalidLogLineException(
            lineNumber, $"expected 4 parts, found {parts.Length}");
    }
    DateTimeOffset time;
    try
    {
        time = TimestampOf(line);
    }
    catch (FormatException ex)
    {
        throw new InvalidLogLineException(lineNumber, "invalid timestamp", ex);
    }
    string kind = EventTypeOf(line);
    bool known =
        string.Equals(kind, "login-success", StringComparison.Ordinal) ||
        string.Equals(kind, "login-failed", StringComparison.Ordinal) ||
        string.Equals(kind, "logout", StringComparison.Ordinal);
    if (!known)
    {
        throw new InvalidLogLineException(
            lineNumber, $"unknown event type {kind}");
    }
    return new LogEvent(time, kind, FindUser(line));
}

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

// Turn the text of a log line into an event type: unknown text is an error
EventType ParseEventType(string text)
{
    return text switch
    {
        "login-success" => EventType.LoginSuccess,
        "login-failed" => EventType.LoginFailed,
        "logout" => EventType.Logout,
        _ => throw new ArgumentException($"Unknown event type: {text}"),
    };
}

// Rate the severity of a number of failures as an enum value
Severity SeverityOf(int failures)
{
    return failures switch
    {
        0 => Severity.None,
        < 3 => Severity.Low,
        < 5 => Severity.Medium,
        _ => Severity.High,
    };
}

// Convert a LogEvent into the compact record form
SecurityEvent ToSecurityEvent(LogEvent logEvent)
{
    return new SecurityEvent(
        logEvent.Timestamp, ParseEventType(logEvent.EventType), logEvent.User);
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

// @fold-end
// Print a title and one line per item, for any collection
static void PrintEach<T>(string title, IEnumerable<T> items)
{
    Console.WriteLine(title);
    foreach (T item in items)
    {
        Console.WriteLine($"- {item}");
    }
}

// @fold-start
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

namespace SecLog.Model
{
    // @fold-start
    // Describe one event of the sample log as an object
    class LogEvent : IEquatable<LogEvent>
    {
        public DateTimeOffset Timestamp { get; }
        public string EventType { get; }
        public string? User { get; }

        public LogEvent(DateTimeOffset timestamp, string eventType, string? user)
        {
            ArgumentException.ThrowIfNullOrEmpty(eventType);
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

        /// <summary>Two events are equal if all three values are equal.</summary>
        public bool Equals(LogEvent? other)
        {
            return other is not null &&
                Timestamp == other.Timestamp &&
                string.Equals(EventType, other.EventType,
                    StringComparison.Ordinal) &&
                string.Equals(User, other.User, StringComparison.Ordinal);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as LogEvent);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Timestamp, EventType, User);
        }

        public override string ToString()
        {
            return Describe();
        }
    }

    /// <summary>The failed logins of one user.</summary>
    /// <param name="user">The name of the user.</param>
    class Incident(string user)
    {
        private static int nextNumber;
        private readonly List<LogEvent> failures = [];

        /// <summary>The number of this incident. Every new one gets the next.</summary>
        public int Number { get; } = ++nextNumber;

        /// <summary>The number of incidents created so far.</summary>
        public static int Count => nextNumber;

        /// <summary>The name of the user of this incident.</summary>
        public string User => user;

        /// <summary>Where the incident was found. Set once, at creation.</summary>
        public required string Source { get; init; }

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

        /// <summary>
        /// The state of the incident: open, investigating or closed.
        /// Any other value throws an ArgumentException.
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
                if (!known)
                {
                    throw new ArgumentException($"Unknown status: {value}");
                }
                field = value;
            }
        } = "open";

        /// <summary>Describes this incident in one line.</summary>
        /// <returns>
        /// A line such as <c>Incident #1 bob: 5 failed logins (sample log)</c>.
        /// </returns>
        public string Summary()
        {
            return $"Incident #{Number} {user}: {FailureCount} failed logins ({Source})";
        }
    }

    /// <summary>A log line that cannot be turned into a LogEvent.</summary>
    class InvalidLogLineException : Exception
    {
        /// <summary>The number of the rejected line (the first line is 1).</summary>
        public int LineNumber { get; }

        /// <summary>Creates the exception for a rejected line.</summary>
        /// <param name="lineNumber">The number of the line.</param>
        /// <param name="message">The broken rule.</param>
        public InvalidLogLineException(int lineNumber, string message)
            : base(message)
        {
            LineNumber = lineNumber;
        }

        /// <summary>Creates the exception with the error that caused it.</summary>
        /// <param name="lineNumber">The number of the line.</param>
        /// <param name="message">The broken rule.</param>
        /// <param name="innerException">The error that caused this one.</param>
        public InvalidLogLineException(
            int lineNumber, string message, Exception innerException)
            : base(message, innerException)
        {
            LineNumber = lineNumber;
        }
    }

    /// <summary>The contract of every detection rule.</summary>
    interface IDetectionRule
    {
        /// <summary>The short name of the rule.</summary>
        string Name { get; }

        /// <summary>Decides whether an incident matches the rule.</summary>
        bool IsMatch(Incident incident);
    }

    /// <summary>A rule that decides whether an incident needs attention.</summary>
    abstract class DetectionRule : IDetectionRule
    {
        /// <summary>The short name of the rule.</summary>
        public string Name { get; }

        /// <summary>Creates a rule with a name.</summary>
        /// <param name="name">The short name of the rule.</param>
        protected DetectionRule(string name)
        {
            Name = name;
        }

        /// <summary>Describes the rule in one line.</summary>
        /// <returns>A line such as <c>Rule: failed logins</c>.</returns>
        public virtual string Describe()
        {
            return $"Rule: {Name}";
        }

        /// <summary>The text of a rule is its description.</summary>
        public override string ToString()
        {
            return Describe();
        }

        /// <summary>Decides whether an incident matches the rule.</summary>
        /// <param name="incident">The incident to check.</param>
        /// <returns><c>true</c> if the rule matches the incident.</returns>
        public abstract bool IsMatch(Incident incident);
    }

    /// <summary>A rule that counts failed logins against a threshold.</summary>
    sealed class FailedLoginRule : DetectionRule
    {
        /// <summary>The number of failed logins that triggers the rule.</summary>
        public int Threshold { get; }

        /// <summary>Creates the rule with its threshold.</summary>
        /// <param name="threshold">The number of failed logins.</param>
        public FailedLoginRule(int threshold) : base("failed logins")
        {
            Threshold = threshold;
        }

        /// <summary>Adds the threshold to the description.</summary>
        public override string Describe()
        {
            return base.Describe() + $" (threshold {Threshold})";
        }

        /// <summary>Matches if the incident has enough failed logins.</summary>
        public override bool IsMatch(Incident incident)
        {
            return incident.FailureCount >= Threshold;
        }
    }

    /// <summary>A rule that watches one user.</summary>
    sealed class WatchListRule : DetectionRule
    {
        /// <summary>The name of the watched user.</summary>
        public string WatchedUser { get; }

        /// <summary>Creates the rule for one user.</summary>
        /// <param name="watchedUser">The name of the watched user.</param>
        public WatchListRule(string watchedUser) : base("watch list")
        {
            WatchedUser = watchedUser;
        }

        /// <summary>Adds the watched user to the description.</summary>
        public override string Describe()
        {
            return base.Describe() + $" ({WatchedUser})";
        }

        /// <summary>Matches if the incident belongs to the watched user.</summary>
        public override bool IsMatch(Incident incident)
        {
            return string.Equals(
                incident.User, WatchedUser, StringComparison.Ordinal);
        }
    }

    /// <summary>A rule that matches incidents under investigation.</summary>
    sealed class StatusRule : IDetectionRule
    {
        /// <summary>The short name of the rule.</summary>
        public string Name => "status investigating";

        /// <summary>Matches if the incident is under investigation.</summary>
        public bool IsMatch(Incident incident)
        {
            return string.Equals(
                incident.Status, "investigating", StringComparison.Ordinal);
        }
    }

    /// <summary>Holds exchangeable rules and counts how many match.</summary>
    sealed class Detector
    {
        /// <summary>Creates a detector with its rules.</summary>
        /// <param name="rules">The rules to check.</param>
        public Detector(List<IDetectionRule> rules)
        {
            Rules = rules;
        }

        /// <summary>The rules of the detector.</summary>
        public IReadOnlyList<IDetectionRule> Rules { get; }

        /// <summary>Counts the rules that match an incident.</summary>
        /// <param name="incident">The incident to check.</param>
        /// <returns>The number of matching rules.</returns>
        public int CountMatches(Incident incident)
        {
            int count = 0;
            foreach (IDetectionRule rule in Rules)
            {
                if (rule.IsMatch(incident))
                {
                    count++;
                }
            }
            return count;
        }
    }

    /// <summary>The kind of a log event.</summary>
    enum EventType
    {
        LoginSuccess,
        LoginFailed,
        Logout,
    }

    /// <summary>How serious a number of failures is.</summary>
    enum Severity
    {
        None,
        Low,
        Medium,
        High,
    }

    /// <summary>A period of time that starts at a given moment.</summary>
    readonly struct TimeWindow
    {
        /// <summary>Creates a window from its start and its length.</summary>
        /// <param name="start">The first moment of the window.</param>
        /// <param name="length">How long the window lasts.</param>
        public TimeWindow(DateTimeOffset start, TimeSpan length)
        {
            Start = start;
            Length = length;
        }

        /// <summary>The first moment of the window.</summary>
        public DateTimeOffset Start { get; }

        /// <summary>How long the window lasts.</summary>
        public TimeSpan Length { get; }

        /// <summary>The last moment of the window.</summary>
        public DateTimeOffset End => Start + Length;

        /// <summary>Checks if a moment lies in the window.</summary>
        /// <param name="time">The moment to check.</param>
        public bool Contains(DateTimeOffset time)
        {
            return time >= Start && time <= End;
        }
    }

    /// <summary>A log event in compact form. Equal if all values are equal.</summary>
    /// <param name="Timestamp">When the event happened.</param>
    /// <param name="Type">The kind of the event.</param>
    /// <param name="User">The user of the event, if the line has one.</param>
    sealed record SecurityEvent(DateTimeOffset Timestamp, EventType Type, string? User);

    /// <summary>Orders incidents: most failed logins first, then by user.</summary>
    sealed class IncidentComparer : IComparer<Incident>
    {
        /// <summary>Compares two incidents for sorting.</summary>
        /// <param name="x">The first incident.</param>
        /// <param name="y">The second incident.</param>
        /// <returns>
        /// A negative number if <paramref name="x"/> comes first, zero if
        /// both have the same place, a positive number if it comes last.
        /// </returns>
        public int Compare(Incident? x, Incident? y)
        {
            if (ReferenceEquals(x, y))
            {
                return 0;
            }
            if (x is null)
            {
                return -1;
            }
            if (y is null)
            {
                return 1;
            }
            int byFailures = y.FailureCount.CompareTo(x.FailureCount);
            if (byFailures != 0)
            {
                return byFailures;
            }
            return string.CompareOrdinal(x.User, y.User);
        }
    }

    /// <summary>A store of items of one type.</summary>
    /// <typeparam name="T">The type of the stored items.</typeparam>
    interface IStore<T>
    {
        /// <summary>Adds an item at the end.</summary>
        /// <param name="item">The item to store.</param>
        void Add(T item);

        /// <summary>The number of stored items.</summary>
        int Count { get; }

        /// <summary>The stored items, in the order they were added.</summary>
        IReadOnlyList<T> Items { get; }
    }

    /// <summary>Stores items of one type in the order they were added.</summary>
    /// <typeparam name="T">The type of the stored items.</typeparam>
    class Store<T> : IStore<T>
    {
        private readonly List<T> items = [];

        /// <summary>The number of stored items.</summary>
        public int Count => items.Count;

        /// <summary>The stored items, in the order they were added.</summary>
        public IReadOnlyList<T> Items => items;

        /// <summary>Adds an item at the end.</summary>
        /// <param name="item">The item to store.</param>
        public void Add(T item)
        {
            items.Add(item);
        }

        /// <summary>Returns the item at a place (the first place is 0).</summary>
        /// <param name="index">The place of the item.</param>
        /// <returns>The stored item.</returns>
        public T Get(int index)
        {
            return items[index];
        }

        /// <summary>Returns the item at a place, or the default value.</summary>
        /// <param name="index">The place of the item.</param>
        /// <returns>The item, or the default value of T if there is none.</returns>
        public T? GetOrDefault(int index)
        {
            if (index < 0 || index >= items.Count)
            {
                return default;
            }
            return items[index];
        }

        /// <summary>Tries to read the item at a place.</summary>
        /// <param name="index">The place of the item.</param>
        /// <param name="item">The item, or the default value of T.</param>
        /// <returns>True if the place exists.</returns>
        public bool TryGet(int index, out T? item)
        {
            if (index < 0 || index >= items.Count)
            {
                item = default;
                return false;
            }
            item = items[index];
            return true;
        }
    }

    /// <summary>Counts how often each key was added.</summary>
    /// <typeparam name="TKey">The type of the keys. It must be sortable.</typeparam>
    class Counter<TKey> where TKey : notnull, IComparable<TKey>
    {
        private readonly Dictionary<TKey, int> counts = [];

        /// <summary>Counts one more occurrence of a key.</summary>
        /// <param name="key">The key to count.</param>
        public void Add(TKey key)
        {
            counts.TryGetValue(key, out int current);
            counts[key] = current + 1;
        }

        /// <summary>Returns how often a key was added (0 if never).</summary>
        /// <param name="key">The key to look up.</param>
        /// <returns>The count of the key.</returns>
        public int CountOf(TKey key)
        {
            if (counts.TryGetValue(key, out int current))
            {
                return current;
            }
            return 0;
        }

        /// <summary>Returns a sorted copy of all keys.</summary>
        /// <returns>The keys in ascending order.</returns>
        public List<TKey> SortedKeys()
        {
            List<TKey> keys = new(counts.Keys);
            keys.Sort();
            return keys;
        }
    }

    /// <summary>A method that decides whether an incident matches.</summary>
    /// <param name="incident">The incident to check.</param>
    /// <returns><c>true</c> if the incident matches the check.</returns>
    delegate bool IncidentCheck(Incident incident);

    /// <summary>A rule that gets its check as a function.</summary>
    sealed class FuncRule : IDetectionRule
    {
        private readonly Func<Incident, bool> check;

        /// <summary>Creates a rule from a name and a check.</summary>
        /// <param name="name">The short name of the rule.</param>
        /// <param name="check">Decides whether an incident matches.</param>
        public FuncRule(string name, Func<Incident, bool> check)
        {
            Name = name;
            this.check = check;
        }

        /// <summary>The short name of the rule.</summary>
        public string Name { get; }

        /// <summary>Matches if the check returns true.</summary>
        public bool IsMatch(Incident incident)
        {
            return check(incident);
        }
    }

    /// <summary>The data of the IncidentFound event.</summary>
    sealed class IncidentFoundEventArgs : EventArgs
    {
        /// <summary>Creates the event data.</summary>
        /// <param name="incident">The incident that was found.</param>
        /// <param name="matches">The number of rules that match it.</param>
        public IncidentFoundEventArgs(Incident incident, int matches)
        {
            Incident = incident;
            Matches = matches;
        }

        /// <summary>The incident that was found.</summary>
        public Incident Incident { get; }

        /// <summary>The number of rules that match the incident.</summary>
        public int Matches { get; }
    }

    /// <summary>Checks incidents with a detector and raises an event for each match.</summary>
    sealed class IncidentMonitor
    {
        private readonly Detector detector;

        /// <summary>Creates a monitor that uses a detector.</summary>
        /// <param name="detector">Counts the rules that match an incident.</param>
        public IncidentMonitor(Detector detector)
        {
            this.detector = detector;
        }

        /// <summary>Raised for every incident that at least one rule matches.</summary>
        public event EventHandler<IncidentFoundEventArgs>? IncidentFound;

        /// <summary>Checks the incidents and raises the event for each match.</summary>
        /// <param name="items">The incidents to check.</param>
        public void Check(IEnumerable<Incident> items)
        {
            foreach (Incident incident in items)
            {
                int matches = detector.CountMatches(incident);
                if (matches > 0)
                {
                    OnIncidentFound(new IncidentFoundEventArgs(incident, matches));
                }
            }
        }

        /// <summary>Raises the IncidentFound event.</summary>
        /// <param name="e">The data of the event.</param>
        private void OnIncidentFound(IncidentFoundEventArgs e)
        {
            IncidentFound?.Invoke(this, e);
        }
    }

    /// <summary>Extension methods that ask a log event a question.</summary>
    static class LogEventExtensions
    {
        /// <summary>Checks whether the event is a failed login.</summary>
        /// <param name="logEvent">The event to check.</param>
        public static bool IsFailure(this LogEvent logEvent)
        {
            return logEvent.EventType == "login-failed";
        }

        /// <summary>Checks whether the event belongs to a user.</summary>
        /// <param name="logEvent">The event to check.</param>
        /// <param name="user">The name of the user.</param>
        public static bool IsFrom(this LogEvent logEvent, string user)
        {
            return logEvent.User == user;
        }
    }

    /// <summary>Extension members that ask an incident a question.</summary>
    static class IncidentExtensions
    {
        extension(Incident incident)
        {
            /// <summary>An incident is severe from 5 failed logins.</summary>
            public bool IsSevere => incident.FailureCount >= 5;
        }

        extension(Incident)
        {
            /// <summary>The header line of an incident table.</summary>
            public static string TableHeader => "Number | User | Failures | Severe";
        }
    }
    // @fold-end
}
