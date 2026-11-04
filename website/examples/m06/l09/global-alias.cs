Alerts.Printer.Show("Disk full");

namespace Alerts.System
{
    class Marker { }
}

namespace Alerts
{
    static class Printer
    {
        public static void Show(string text)
        {
            global::System.Console.WriteLine(text);
        }
    }
}
