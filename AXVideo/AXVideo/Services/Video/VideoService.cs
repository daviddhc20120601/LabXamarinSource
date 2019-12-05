using AXVideo.Helpers;
using AXVideo.Services;
using Flurl;
using Flurl.Http;
using Polly;
using System;
using System.Net;
using System.Threading.Tasks;

[assembly: Xamarin.Forms.Dependency(typeof(VideoService))]
namespace AXVideo.Services
{
    public class VideoService : IVideoService
    {
        /// <summary>
        /// Get a specific playlist (api/v3/video/Playlists/{id}/Units)
        /// </summary>
        public async Task<ApiResult<PlaylistUnitsDto>> GetPlaylistAsync(int playlistId, int pageIndex)
        {
            ApiResult<PlaylistUnitsDto> items = null;
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
                                    return await cli.Request().AppendPathSegment($"api/v3/video/Playlists/{playlistId}/Units").SetQueryParams(new { PageIndex = pageIndex })
                                    .GetJsonAsync<ApiResult<PlaylistUnitsDto>>();
                                }
                            });
                return items;
            }
            catch (Exception e)
            {
                if (items == null)
                    items = new ApiResult<PlaylistUnitsDto>();
                items.GetExceptionMessage(e);
                return items;
            }
        }

        /// <summary>
        /// Get a specific video info (api/v3/video/Units/{id})
        /// </summary>
        public async Task<ApiResult<UnitDto>> GetVideoAsync(int unitId)
        {
            ApiResult<UnitDto> items = null;
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
                                    return await cli.Request().AppendPathSegment($"api/v3/video/Units/{unitId}").GetJsonAsync<ApiResult<UnitDto>>();
                                }
                            });
                return items;
            }
            catch (Exception e)
            {
                if (items == null)
                    items = new ApiResult<UnitDto>();
                items.GetExceptionMessage(e);
                return items;
            }
        }
    }
}
