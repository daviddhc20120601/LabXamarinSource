using AXVideo.Services;
using Flurl;
using Flurl.Http;
using Polly;
using System;
using System.Net;
using System.Threading.Tasks;

[assembly: Xamarin.Forms.Dependency(typeof(AccountService))]
namespace AXVideo.Services
{
    public class AccountService : IAccountService
    {
        /// <summary>
        /// Generate TenantLevelToken (api/token/TenantLevelToken)
        /// </summary>
        public async Task<ApiResult<TokenDto>> GetTenantTokenAsync()
        {
            ApiResult<TokenDto> items = null;
            try
            {
                items = await Policy
                            .Handle<FlurlHttpTimeoutException>()
                            .Or<FlurlHttpException>(r => r.Call.HttpStatus == HttpStatusCode.Unauthorized)
                            .WaitAndRetryAsync(2, retryAttempt =>
                            TimeSpan.FromSeconds(2))
                            .ExecuteAsync(async () =>
                            {
                                using (var cli = new FlurlClient(AppConstants.Base_API_Url).WithTimeout(30))
                                {
                                    return await cli.Request().AppendPathSegment($"api/token/TenantLevelToken")
                                    .PostJsonAsync(new
                                    {
                                        tenancyName = AppConstants.TenancyName,
                                        tenantAppCode = AppConstants.TenantAppCode,
                                        tenantAppKey = AppConstants.TenantAppKey
                                    }).ReceiveJson<ApiResult<TokenDto>>();
                                }
                            });
                return items;
            }
            catch (Exception e)
            {
                if (items == null)
                    items = new ApiResult<TokenDto>();
                items.GetExceptionMessage(e);
                return items;
            }
        }
    }
}
