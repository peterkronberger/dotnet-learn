// 1. Declare a constant for the number of seconds in a minute
const int secondsPerMinute = 60;
// 2. Declare a constant that is computed from the first one (60 * 60 = 3600)
const int secondsPerHour = 0;
// 3. Let the compiler infer the type of the variable from the value 90
var seconds = 0;
// 4. Compute how many full minutes fit into the seconds
var minutes = seconds / secondsPerMinute;
Console.WriteLine(secondsPerHour);
Console.WriteLine(minutes);
