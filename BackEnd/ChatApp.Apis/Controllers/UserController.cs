using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ChatApp.Apis.Controllers
{
    [Route("api/users")]
    public class UserController : BaseController
    {
        public UserController(ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
        }

        [HttpGet]
        [Route("test-logging")]
        public async Task<bool> TestLogging()
        {
            for (var i = 0; i < 1000000; i++)
            {
                Logger.LogInformation($"Logging at index: {i}");
                await Task.Delay(500);
                Logger.LogError($"Testing logging error at index: {i}");

                if (i % 10 != 0) continue;

                Logger.LogWarning($"Warning at index: {i}");
                await Task.Delay(1000);
            }

            return true;
        }
    }
}
