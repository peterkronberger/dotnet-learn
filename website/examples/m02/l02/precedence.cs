bool failed = true;
bool fromBob = false;
bool locked = false;
Console.WriteLine(failed || fromBob && locked);
Console.WriteLine((failed || fromBob) && locked);
Console.WriteLine(!failed || fromBob);
