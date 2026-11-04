// Add a using directive here, then shorten the names below

Alerts.Mail.Notice first = new Alerts.Mail.Notice("Disk full");
Alerts.Mail.Notice second = new Alerts.Mail.Notice("Many failed logins");
Console.WriteLine(first.Text);
Console.WriteLine(second.Text);

namespace Alerts.Mail
{
    class Notice
    {
        public string Text { get; }

        public Notice(string text)
        {
            Text = text;
        }
    }
}
