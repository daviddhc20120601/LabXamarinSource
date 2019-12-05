using AXVideo.Helpers;
using AXVideo.Services;
using Flurl.Http;
using Polly;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

[assembly: Xamarin.Forms.Dependency(typeof(NewsService))]
namespace AXVideo.Services
{
    public class NewsService : INewsService
    {
        /// <summary>
        /// Get news list (api/news/search)
        /// </summary>
        public async Task<ApiResult<List<NewsListDto>>> GetNewsListAsync(string keyword, int pageNumber)
        {
            ApiResult<List<NewsListDto>> items = null;
            try
            {
                items = await Policy
                            .Handle<FlurlHttpException>(r => r.Call.HttpStatus == HttpStatusCode.Unauthorized)
                            .RetryAsync(1, async (exception, retryCount) =>
                            {
                                await ApiHelper.CheckAuthRequired();
                            })
                            .ExecuteAsync(async () =>
                            {
                                using (var cli = new FlurlClient(AppConstants.Base_API_Url).WithOAuthBearerToken(AppSettings.AccountToken).WithTimeout(30))
                                {
                                    return await cli.Request().AppendPathSegment($"api/news/search").SetQueryParams(new
                                    {
                                        Keyword = keyword,
                                        PageIndex = pageNumber
                                    }).GetJsonAsync<ApiResult<List<NewsListDto>>>();
                                }
                            });
                return items;
            }
            catch (Exception e)
            {
                if (items == null)
                    items = new ApiResult<List<NewsListDto>>();
                items.GetExceptionMessage(e);
                return items;
            }
        }

        /// <summary>
        /// Get a specific news info (api/news/{id})
        /// </summary>
        public async Task<ApiResult<NewsDto>> GetNewsAsync(int id)
        {
            ApiResult<NewsDto> items = null;
            try
            {
                items = await Policy
                            .Handle<FlurlHttpException>(r => r.Call.HttpStatus == HttpStatusCode.Unauthorized)
                            .RetryAsync(2, async (exception, retryCount) =>
                            {
                                await ApiHelper.CheckAuthRequired();
                            })
                            .ExecuteAsync(async () =>
                            {
                                using (var cli = new FlurlClient(AppConstants.Base_API_Url).WithOAuthBearerToken(AppSettings.AccountToken).WithTimeout(30))
                                {
                                    return await cli.Request().AppendPathSegment($"api/news/{id}")
                                    .GetJsonAsync<ApiResult<NewsDto>>();
                                }
                            });
                return items;
            }
            catch (Exception e)
            {
                if (items == null)
                    items = new ApiResult<NewsDto>();
                items.GetExceptionMessage(e);
                return items;
            }
        }
    }
}
