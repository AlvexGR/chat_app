using ChatApp.Apis.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Apis.Controllers
{
    [ApiController]
    [ServiceFilter(typeof(AuthFilter))]
    public class BaseController : ControllerBase
    {
        protected readonly IHttpContextAccessor HttpContextAccessor;

        public BaseController(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }
    }
}
