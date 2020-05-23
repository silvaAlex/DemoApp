using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SendEmail.Model;
using SendEmail.Services;

namespace DemoApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SendEmailController : ControllerBase
    {
        private IServiceSendEmail _emailService;
        private readonly ILogger _logger;

        public SendEmailController(IServiceSendEmail emailService, ILogger<SendEmailController> logger)
        {
            _emailService = emailService;
            _logger = logger;
        }

        [HttpGet]
        public string Get() => "OK";

        [HttpPost("sendemail")]
        public void SendEmail(ConfigurationEmail email)
        {
            _emailService.SendEmailAsync(email, _logger);
        }
    }
}
