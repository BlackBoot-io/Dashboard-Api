using System.ComponentModel.DataAnnotations;

namespace BlackBoot.Shared.Core;

public enum ActionResponseStatusCode
{
    [Display(Name = "عملیات با موفقیت انجام شد")]
    Success = 200,

    [Display(Name = "خطایی در سرور رخ داده است")]
    ServerError = 500,

    [Display(Name = "پارامتر های ارسالی معتبر نیستند")]
    BadRequest = 400,

    [Display(Name = "یافت نشد")]
    NotFound = 404,

    [Display(Name = "خطای احراز هویت")]
    UnAuthorized = 401,

    [Display(Name = "درخواست کننده مجاز به انجام این درخواست نیست")]
    Forbidden = 403
    UnAuthorized = 401,

    [Display(Name = "خطای عدم دسترسی")]
    UnAuthorizedAccess = 403
}
