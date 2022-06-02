using BlackBoot.Api.Attributes;

namespace BlackBoot.Api.Controllers;

[ApiController, Authorize, ApiResult]
[Route("[controller]/[action]")]
public class BaseController : ControllerBase
{
}
