using AXVideo.Helpers;
using System;

namespace AXVideo.Services
{
    /// <summary>
    /// api/videoV3/Playlists/All Dto
    /// </summary>
    public class Playlist
    {
        public bool IsFeatured { get; set; }
        public string Name { get; set; }
        public bool IsHomeRecommended { get; set; }
        public string UniqueCode { get; set; }
        public string CoverImageURL { get; set; }
        public int Id { get; set; }
    }

    /// <summary>
    /// api/videoV3/Playlists/{id}/Units Dto
    /// </summary>
    public class PlaylistUnitsDto
    {
        public Playlist Playlist { get; set; }
        public Unit[] Units { get; set; }
    }

    /// <summary>
    /// api/videoV3/Units/{id} Dto
    /// </summary>
    public class UnitDto
    {
        public Playback Playback { get; set; }
        public int[] Categories { get; set; }
        public int[] Playlists { get; set; }
        public int[] Series { get; set; }
        public string ShareURL { get; set; }
        public string VmapAddress { get; set; }
        public bool? Favorited { get; set; }
        public bool? Liked { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PosterURL { get; set; }
        public bool IsFeatured { get; set; }
        public TimeSpan Length { get; set; }
        public int WatchTimes { get; set; }
        public DateTime Time { get; set; }
        public int CommentCounts { get; set; }
        public int LikeCounts { get; set; }
        public int ByFavoriteCounts { get; set; }
        public string PreviewPlayUrl { get; set; }
        public int Id { get; set; }
    }

    #region Public Dto
    /// <summary>
    /// Unit info
    /// </summary>
    public class Unit
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string PosterURL { get; set; }
        public bool IsFeatured { get; set; }
        public TimeSpan? Length { get; set; }
        public int WatchTimes { get; set; }
        public DateTime Time { get; set; }
        public int CommentCounts { get; set; }
        public int LikeCounts { get; set; }
        public int ByFavoriteCounts { get; set; }
        public string PreviewPlayUrl { get; set; }
        public int Id { get; set; }
    }

    /// <summary>
    /// Playback info
    /// </summary>
    public class Playback
    {
        public string AmshlsV3URL { get; set; }
    }
    #endregion
}
