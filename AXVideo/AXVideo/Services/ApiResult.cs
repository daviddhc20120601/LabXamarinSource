using Newtonsoft.Json;
using System;
using System.Net;

namespace AXVideo.Services
{
    public class ApiResult<T>
    {
        public T Result { get; set; }
        public BaseErrorDto ErrorDto { get; set; }
        public bool Success { get; set; }
        public bool UnAuthorizedRequest { get; set; }

        public async void GetExceptionMessage(Exception exception)
        {
            this.Success = false;
            if (exception == null)
                return;
            if (exception is Flurl.Http.FlurlHttpTimeoutException)
            {
                if (this.ErrorDto == null)
                    this.ErrorDto = new BaseErrorDto { ErrorMessage = "链接超时, 请稍后重试" };
                else
                    this.ErrorDto.ErrorMessage = "链接超时, 请稍后重试";
            }
            else if (exception is Flurl.Http.FlurlHttpException)
            {
                Flurl.Http.FlurlHttpException ex = exception as Flurl.Http.FlurlHttpException;
                if (ex == null)
                    return;
                switch (ex.Call.HttpStatus)
                {
                    case HttpStatusCode.NotFound:
                        if (this.ErrorDto == null)
                            this.ErrorDto = new BaseErrorDto { ErrorMessage = "内容加载失败, 请稍后重试" };
                        else
                            this.ErrorDto.ErrorMessage = "内容加载失败, 请稍后重试";
                        break;
                    case HttpStatusCode.Unauthorized:
                        if (this.ErrorDto == null)
                            this.ErrorDto = new BaseErrorDto { SignOut = true, ErrorMessage = "当前用户登录失效, 请重新登录" };
                        else
                        {
                            this.ErrorDto.SignOut = true;
                            this.ErrorDto.ErrorMessage = "当前用户登录失效, 请重新登录";
                        }
                        break;
                    case HttpStatusCode.Forbidden:
                        if (this.ErrorDto == null)
                            this.ErrorDto = new BaseErrorDto { AuthRequired = true, ErrorMessage = "当前用户登录失效, 请重新登录" };
                        else
                        {
                            this.ErrorDto.AuthRequired = true;
                            this.ErrorDto.ErrorMessage = "当前用户登录失效, 请重新登录";
                        }
                        break;
                    case HttpStatusCode.BadRequest:
                        var _content = await ex.Call.Response.Content.ReadAsStringAsync();
                        var _commonError = JsonConvert.DeserializeObject<ApiResult<string>>(_content);
                        if (this.ErrorDto == null)
                            this.ErrorDto = new BaseErrorDto { ErrorMessage = _commonError.Result };
                        else
                            this.ErrorDto.ErrorMessage = _commonError.Result;
                        break;
                    case HttpStatusCode.Conflict:
                        if (this.ErrorDto == null)
                            this.ErrorDto = new BaseErrorDto { ErrorMessage = "" };
                        else
                            this.ErrorDto.ErrorMessage = "";
                        break;
                    case HttpStatusCode.InternalServerError:
                        if (this.ErrorDto == null)
                            this.ErrorDto = new BaseErrorDto { ErrorMessage = "服务器异常, 请稍后重试" };
                        else
                            this.ErrorDto.ErrorMessage = "服务器异常, 请稍后重试";
                        break;
                    default:
                        if (this.ErrorDto == null)
                            this.ErrorDto = new BaseErrorDto { ErrorMessage = "未知异常, 请稍后重试" };
                        else
                            this.ErrorDto.ErrorMessage = "未知异常, 请稍后重试";
                        break;
                }
            }
            else
            {
                if (this.ErrorDto == null)
                    this.ErrorDto = new BaseErrorDto { ErrorMessage = "未知异常, 请稍后重试" };
                else
                    this.ErrorDto.ErrorMessage = "未知异常, 请稍后重试";
            }
        }
    }
}
