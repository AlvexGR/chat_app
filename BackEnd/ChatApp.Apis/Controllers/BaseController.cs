using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ChatApp.Apis.Controllers
{
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected ILogger Logger { get; }

        public BaseController(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(typeof(BaseController));
        }
    }
}
