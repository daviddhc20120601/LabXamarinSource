using AXVideo.Services;
using System.Threading.Tasks;

namespace AXVideo.Helpers
{
    public class ApiHelper
    {
        public static async Task<bool> CheckAuthRequired()
        {
            ApiResult<TokenDto> result = await Xamarin.Forms.DependencyService.Get<IAccountService>().GetTenantTokenAsync();
            if (result.Success)
            {
                if (result.Result.InstantToken)
                    return true;
                AppSettings.AccountToken = result.Result.Token;
                return false;
            }
            return false;
        }
    }
}
