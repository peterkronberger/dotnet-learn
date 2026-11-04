using static System.Math;
using static Alerts.Rating;

Console.WriteLine(Max(3, 5));
Console.WriteLine(Describe(0));
Console.WriteLine(Describe(4));

namespace Alerts
{
    static class Rating
    {
        public static string Describe(int failures)
        {
            return failures == 0 ? "quiet" : "check the log";
        }
    }
}
