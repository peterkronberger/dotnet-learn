// Read the parts of a split line with a list pattern
string[] parts = "08:00 login-failed bob".Split(' ');
if (parts is [var time, "login-failed", var user])
{
    Console.WriteLine($"{user} failed at {time}");
}

// The first two parts are enough: skip the rest
string[] longer = "08:01 logout bob 203.0.113.7".Split(' ');
if (longer is [_, "logout", ..])
{
    Console.WriteLine("logout");
}
