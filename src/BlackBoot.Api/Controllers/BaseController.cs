using BlackBoot.Api.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlackBoot.Api.Controllers;

[ApiController, Authorize, ApiResult]
[Route("[controller]/[action]")]
public class BaseController : ControllerBase
{
}
