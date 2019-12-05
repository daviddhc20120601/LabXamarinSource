using System.Threading.Tasks;

namespace AXVideo.Services
{
    public interface IAccountService
    {      
        /// <summary>
        /// Get tenant token to request auth/api/...
        /// </summary>
        Task<ApiResult<TokenDto>> GetTenantTokenAsync();
    }
}
