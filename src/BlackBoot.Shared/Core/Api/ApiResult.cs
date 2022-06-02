using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace BlackBoot.Shared.Core;


public interface IApiResult
{
    public bool IsSuccess { get; set; }
    public ApiResultStatusCode StatusCode { get; set; }
    public string Message { get; set; }
}
public interface IApiResult<TData> : IApiResult where TData : class
{
    public TData Data { get; set; }
}



public class ApiResult : IApiResult
{


    public ApiResult(bool isSuccess, ApiResultStatusCode statusCode, string message = null)
    {
        IsSuccess = isSuccess;
        StatusCode = statusCode;
        Message = message ?? GetDisplayName(statusCode);
    }

    public bool IsSuccess { get; set; }
    public ApiResultStatusCode StatusCode { get; set; }
    public string Message { get; set; }
    #region Implicit Operators
    public static implicit operator ApiResult(OkResult result)
    {
        return new ApiResult(true, ApiResultStatusCode.Success);
    }
    public static implicit operator ApiResult(BadRequestResult result)
    {
        return new ApiResult(false, ApiResultStatusCode.BadRequest);
    }
    public static implicit operator ApiResult(BadRequestObjectResult result)
    {
        var message = result.Value?.ToString();
        if (result.Value is SerializableError errors)
        {
            var errorMessages = errors.SelectMany(p => (string[])p.Value).Distinct();
            message = string.Join(" | ", errorMessages);
        }
        return new ApiResult(false, ApiResultStatusCode.BadRequest, message);
    }

    public static implicit operator ApiResult(ContentResult result)
    {
        return new ApiResult(true, ApiResultStatusCode.Success, result.Content);
    }

    public static implicit operator ApiResult(NotFoundResult result)
    {
        return new ApiResult(false, ApiResultStatusCode.NotFound);
    }
    #endregion

    public string GetDisplayName(Enum value)
    {
        //Assert.NotNull(value, nameof(value));

        var attribute = value.GetType().GetField(value.ToString())
            .GetCustomAttributes<DisplayAttribute>(false).FirstOrDefault();

        if (attribute == null)
            return value.ToString();

        var propValue = attribute.GetType().GetProperty("Name").GetValue(attribute, null);
        return propValue.ToString();
    }
}
public class ApiResult<TData> : ApiResult, IApiResult<TData> where TData : class
{
    public TData Data { get; set; }

    public ApiResult(bool isSuccess, ApiResultStatusCode statusCode, TData data, string message = null)
        : base(isSuccess, statusCode, message)
    {
        Data = data;
    }

    #region Implicit Operators
    public static implicit operator ApiResult<TData>(TData data)
    {
        return new ApiResult<TData>(true, ApiResultStatusCode.Success, data);
    }

    public static implicit operator ApiResult<TData>(OkResult result)
    {
        return new ApiResult<TData>(true, ApiResultStatusCode.Success, null);
    }

    public static implicit operator ApiResult<TData>(OkObjectResult result)
    {
        return new ApiResult<TData>(true, ApiResultStatusCode.Success, (TData)result.Value);
    }

    public static implicit operator ApiResult<TData>(BadRequestResult result)
    {
        return new ApiResult<TData>(false, ApiResultStatusCode.BadRequest, null);
    }

    public static implicit operator ApiResult<TData>(BadRequestObjectResult result)
    {
        var message = result.Value?.ToString();
        if (result.Value is SerializableError errors)
        {
            var errorMessages = errors.SelectMany(p => (string[])p.Value).Distinct();
            message = string.Join(" | ", errorMessages);
        }
        return new ApiResult<TData>(false, ApiResultStatusCode.BadRequest, null, message);
    }

    public static implicit operator ApiResult<TData>(ContentResult result)
    {
        return new ApiResult<TData>(true, ApiResultStatusCode.Success, null, result.Content);
    }

    public static implicit operator ApiResult<TData>(NotFoundResult result)
    {
        return new ApiResult<TData>(false, ApiResultStatusCode.NotFound, null);
    }

    public static implicit operator ApiResult<TData>(NotFoundObjectResult result)
    {
        return new ApiResult<TData>(false, ApiResultStatusCode.NotFound, (TData)result.Value);
    }
    #endregion
}