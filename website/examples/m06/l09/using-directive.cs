using Alerts.Mail;

Notice notice = new Notice("Disk full");
Console.WriteLine(notice.Text);
Console.WriteLine(typeof(Notice).FullName);

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
