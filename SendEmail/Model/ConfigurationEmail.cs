using System.Net.Mail;
using System.Text;

namespace SendEmail.Model
{
    public class ConfigurationEmail
    {
        public string[] To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        public class EmailSettings
        {
            public string From { get; internal set; }
            public string Alias { get; internal set; }
            public Encoding TextEncoding { get; internal set; }
            public bool IsBodyHtml { get; internal set; }
            public SmtpClient SmtpClient { get; internal set; }
        }
    }
}