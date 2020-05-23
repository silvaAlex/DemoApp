using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Mail;
using System.Threading;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SendEmail.Model;

namespace SendEmail.Services
{
    public class AuthMessageSender : IServiceSendEmail
    {
        /// <summary>
        ///
        /// </summary>
        struct EmailStruct
        {
            public string To;
            public string Subject;
            public string Body;
        }

        /// <summary>
        /// Semaforo binario para execução de um envio por vez
        /// </summary>
        private Semaphore _semaphore = new Semaphore(1, 1);
        readonly ConfigurationEmail.EmailSettings _emailSettings;
        private ILogger _logger;

        /// <summary>
        /// Contador de token (garante o uso único)
        /// </summary>
        private static readonly IList<string> sendTokens = new List<string>();

        /// <summary>
        /// Estrutura temporaria para envio de email
        /// </summary>
        public AuthMessageSender(IOptions<ConfigurationEmail.EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public void SendEmailAsync(ConfigurationEmail email, ILogger logger)
        {
            _logger = logger;
            Execute(email);
        }

        public void SendEmailAsync(ConfigurationEmail email, ILogger logger, Semaphore semaphore = null)
        {
            _semaphore = semaphore;
            _logger = logger;
            Execute(email);
        }

        void Execute(ConfigurationEmail email)
        {
            foreach (string to in email.To)
            {
                ThreadExecuteAsync(new EmailStruct()
                {
                    To = to,
                    Subject = email.Subject,
                    Body = email.Body
                });
            }
        }

        /// <summary>
        /// Thread para envio de email
        /// </summary>
        /// <param name="emailStruct">EmailStruct</param>s
        private void ThreadExecuteAsync(EmailStruct email)
        {
            new Thread(() =>
            {
                SmtpClient smtpClient = _emailSettings.SmtpClient;
                MailAddress from = new MailAddress(_emailSettings.From, _emailSettings.Alias, _emailSettings.TextEncoding);
                MailAddress to = new MailAddress(email.To);
                MailMessage mailMessage = new MailMessage(from, to);
                smtpClient.SendCompleted += new SendCompletedEventHandler(EmailCallbackAsync);

                mailMessage.SubjectEncoding = _emailSettings.TextEncoding;
                mailMessage.BodyEncoding = _emailSettings.TextEncoding;
                mailMessage.IsBodyHtml = _emailSettings.IsBodyHtml;
                mailMessage.Subject = email.Subject;
                mailMessage.Body = email.Body;

                string token = email.To;
                _semaphore.WaitOne();
                smtpClient.SendAsync(mailMessage, token);

            }).Start();
        }

        /// <summary>
        /// Callback para release do semaforo
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">AsyncCompletedEventArgs</param>
        private void EmailCallbackAsync(object sender, AsyncCompletedEventArgs e)
        {
            string userState = (string)e.UserState;

            if (!sendTokens.Contains(userState))
            {
                _semaphore.Release();
                if (e.Cancelled)
                {
                    _logger.LogWarning($"cancelado o envio para {(string)e.UserState}");
                }
                else if (e.Error != null)
                {
                    _logger.LogError((string)e.UserState, e.Error);
                }
                sendTokens.Add(userState);
            }
        }
    }
}