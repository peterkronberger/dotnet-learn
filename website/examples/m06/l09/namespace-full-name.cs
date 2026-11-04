Alerts.Mail.Notice notice = new Alerts.Mail.Notice("Disk full");
Console.WriteLine(notice.Text);
Console.WriteLine(typeof(Alerts.Mail.Notice).FullName);
Console.WriteLine(typeof(Console).FullName);

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
