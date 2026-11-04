using Alerts.Mail;
using Alerts.Sms;

Notice notice = new Notice("Disk full");

namespace Alerts.Mail
{
    class Notice(string text) { }
}

namespace Alerts.Sms
{
    class Notice(string text) { }
}
