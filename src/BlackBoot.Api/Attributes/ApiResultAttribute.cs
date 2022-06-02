using BlackBoot.Shared.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;

namespace BlackBoot.Api.Attributes;

public class ApiResultAttribute : ActionFilterAttribute
{
    public override void OnResultExecuting(ResultExecutingContext context)
    {
        if (context.Result is OkObjectResult okObjectResult)
        {
            var apiResult = new ApiResult<object>(true, ApiResultStatusCode.Success, okObjectResult.Value);
            context.Result = new JsonResult(apiResult) { StatusCode = okObjectResult.StatusCode };
        }
        else if (context.Result is OkResult okResult)
        {
            var apiResult = new ApiResult(true, ApiResultStatusCode.Success);
            context.Result = new JsonResult(apiResult) { StatusCode = okResult.StatusCode };
        }
        //return BadRequest() method create an ObjectResult with StatusCode 400 in recent versions, So the following code has changed a bit.
        else if (context.Result is ObjectResult badRequestObjectResult && badRequestObjectResult.StatusCode == 400)
        {
            string message = null;
            switch (badRequestObjectResult.Value)
            {
                case ValidationProblemDetails validationProblemDetails:
                    var errorMessages = validationProblemDetails.Errors.Select(p => new { Key = p.Key, Value = p.Value }).Distinct();
                    message = JsonSerializer.Serialize(errorMessages);// string.Join(" | ", errorMessages);
                    break;
                case SerializableError errors:
                    var errorMessages2 = errors.Select(p => new { Key = p.Key, Value = p.Value }).Distinct();
                    message = JsonSerializer.Serialize(errorMessages2); //string.Join(" | ", errorMessages2);
                    break;
                case var value when value != null && !(value is ProblemDetails):
                    message = badRequestObjectResult.Value.ToString();
                    break;
            }

            var apiResult = new ApiResult(false, ApiResultStatusCode.BadRequest, message);
            context.Result = new JsonResult(apiResult) { StatusCode = badRequestObjectResult.StatusCode };
        }
        else if (context.Result is ObjectResult notFoundObjectResult && notFoundObjectResult.StatusCode == 404)
        {
            string message = null;
            if (notFoundObjectResult.Value != null && !(notFoundObjectResult.Value is ProblemDetails))
                message = notFoundObjectResult.Value.ToString();

            //var apiResult = new ApiResult<object>(false, ApiResultStatusCode.NotFound, notFoundObjectResult.Value);
            var apiResult = new ApiResult(false, ApiResultStatusCode.NotFound, message);
            context.Result = new JsonResult(apiResult) { StatusCode = notFoundObjectResult.StatusCode };
        }
        else if (context.Result is ContentResult contentResult)
        {
            var apiResult = new ApiResult(true, ApiResultStatusCode.Success, contentResult.Content);
            context.Result = new JsonResult(apiResult) { StatusCode = contentResult.StatusCode };
        }
        else if (context.Result is ObjectResult objectResult && objectResult.StatusCode == null && !(objectResult.Value is ApiResult))
        {
            var apiResult = new ApiResult<object>(true, ApiResultStatusCode.Success, objectResult.Value);
            context.Result = new JsonResult(apiResult) { StatusCode = objectResult.StatusCode };
        }

        base.OnResultExecuting(context);
    }
}
