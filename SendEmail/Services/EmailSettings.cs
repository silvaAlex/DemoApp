using System.Net.Mail;
using System.Text;

namespace SendEmail.Services
{
    public class EmailSettings
    {
        public string From { get; internal set; }
        public string Alias { get; internal set; }
        public Encoding TextEncoding { get; internal set; }
        public bool IsBodyHtml { get; internal set; }
        public SmtpClient SmtpClient { get; internal set; }
    }
}