// A constant gets its value once, at compile time
const int secondsPerMinute = 60;
const int minutesPerHour = 60;
// A constant expression may use literals and other constants
const int secondsPerHour = secondsPerMinute * minutesPerHour;
Console.WriteLine(secondsPerMinute);
Console.WriteLine(secondsPerHour);
// A normal variable can change, a constant cannot
int attempts = 3;
attempts = attempts + 1;
Console.WriteLine(attempts);
Console.WriteLine(secondsPerHour * 2);
