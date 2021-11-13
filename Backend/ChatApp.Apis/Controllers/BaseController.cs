using ChatApp.Apis.Filters;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Apis.Controllers
{
    [ApiController]
    [ServiceFilter(typeof(AuthFilter))]
    public class BaseController : ControllerBase
    {
    }
}
