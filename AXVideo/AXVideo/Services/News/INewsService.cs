using System.Threading.Tasks;
using System.Collections.Generic;

namespace AXVideo.Services
{
    public interface INewsService
    {
        /// <summary>
        /// Get news list
        /// </summary>
        Task<ApiResult<List<NewsListDto>>> GetNewsListAsync(string keyword, int pageNumber);

        /// <summary>
        /// Get a specific news info
        /// </summary>
        Task<ApiResult<NewsDto>> GetNewsAsync(int id);
    }
}
