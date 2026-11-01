int checks = 0;
bool ready = false;
bool done = true;
bool andResult = ready && (++checks > 0);
Console.WriteLine($"&& result: {andResult}, checks: {checks}");
bool orResult = done || (++checks > 0);
Console.WriteLine($"|| result: {orResult}, checks: {checks}");
bool bitResult = ready & (++checks > 0);
Console.WriteLine($"& result: {bitResult}, checks: {checks}");
