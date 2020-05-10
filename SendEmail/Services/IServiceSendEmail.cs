using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SendEmail.Model;

namespace SendEmail.Services
{
    public interface IServiceSendEmail
    {
        /// <summary>
        /// Envia email de forma asyncrona
        /// </summary>
        /// <param name="configurationEmail">ConfigurationEmail</param>
        
        void SendEmailAsync(ConfigurationEmail email,ILogger logger);

        /// <summary>
        /// Envia email de forma asyncrona
        /// </summary>
        /// <param name="configurationEmail">ConfigurationEmail</param>
        /// <param name="semaphore">Semaphore externo para controle de threads</param>
        void SendEmailAsync(ConfigurationEmail email,ILogger logger,Semaphore semaphore = null);

    }
}