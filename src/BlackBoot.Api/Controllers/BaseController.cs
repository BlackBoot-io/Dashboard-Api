using BlackBoot.Api.Filters;

namespace BlackBoot.Api.Controllers;

[ApiController, Authorize, ApiResult]//, IdentityMapperFilter]
[Route("[controller]/[action]")]
public class BaseController : ControllerBase
{
}
