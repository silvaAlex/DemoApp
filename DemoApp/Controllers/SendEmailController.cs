using Microsoft.AspNetCore.Mvc;
using DemoApp.Services;
using System.Threading.Tasks;
using SendEmail.Model;

namespace DemoApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SendEmailController : ControllerBase
    {
        private SendEmailService _emailService;

        public SendEmailController(SendEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpGet]
        public string Get() => "OK";

        [HttpPost("sendEmail")]
        public void SendEmail(ConfigurationEmail email)
        {
            _emailService.SendEmailAsync(email);
        } 
    }
}
