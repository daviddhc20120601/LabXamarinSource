using System.Threading.Tasks;

namespace AXVideo.Services
{
    public interface IVideoService
    {
        /// <summary>
        /// Get a specific playlist
        /// </summary>
        Task<ApiResult<PlaylistUnitsDto>> GetPlaylistAsync(int playlistId, int pageIndex);

        /// <summary>
        /// Get a specific video info
        /// </summary>
        Task<ApiResult<UnitDto>> GetVideoAsync(int unitId);
    }
}
