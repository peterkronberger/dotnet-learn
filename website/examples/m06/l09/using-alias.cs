using MailNotice = Alerts.Mail.Notice;
using SmsNotice = Alerts.Sms.Notice;

MailNotice mail = new MailNotice("Disk full");
SmsNotice sms = new SmsNotice("Disk full");
Console.WriteLine(mail.Describe());
Console.WriteLine(sms.Describe());

namespace Alerts.Mail
{
    class Notice(string text)
    {
        public string Describe() => $"mail: {text}";
    }
}

namespace Alerts.Sms
{
    class Notice(string text)
    {
        public string Describe() => $"sms: {text}";
    }
}
