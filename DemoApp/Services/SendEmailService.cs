using Microsoft.Extensions.Logging;
using SendEmail.Services;
using SendEmail.Model;

namespace DemoApp.Services
{
    public class SendEmailService
    {
        ILogger<SendEmailService> _logger;
        IServiceSendEmail _serviceSendEmail;

        public SendEmailService(ILogger<SendEmailService> logger)
        {
            _logger = logger;
        }

        public void SendEmailAsync(ConfigurationEmail model)
        {
            _serviceSendEmail.SendEmailAsync(model,_logger);
        }
    }
}
