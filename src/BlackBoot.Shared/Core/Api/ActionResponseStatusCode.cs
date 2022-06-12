using System.ComponentModel.DataAnnotations;

namespace BlackBoot.Shared.Core;

public enum ActionResponseStatusCode
{
    [Display(Name = "Success Action")]
    Success = 200,

    [Display(Name = "An error has been occured")]
    ServerError = 500,

    [Display(Name = "Parameters are invalid.")]
    BadRequest = 400,

    [Display(Name = "Not fount")]
    NotFound = 404,

    [Display(Name = "unauthorize user")]
    UnAuthorized = 401,

    [Display(Name = "the request has been banned.")]
    Forbidden = 403
}
