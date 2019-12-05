using System;
using System.Collections.Generic;
using System.Text;

namespace AXVideo.Services
{
    #region /api/news/search
    /// <summary>
    /// /api/news/search Dto
    /// </summary>
    public class NewsListDto
    {
        public string Title { get; set; }
        public string CoverImageUrl { get; set; }
        public DateTime CreationTime { get; set; }
        public int Id { get; set; }
        public string[] PreviewImages { get; set; }
    }
    #endregion

    #region /api/news/{id}
    /// <summary>
    /// /api/news/{id} Dto
    /// </summary>
    public class NewsDto
    {
        public string Title { get; set; }
        public string CoverImageUrl { get; set; }
        public DateTime CreationTime { get; set; }
        public string Content { get; set; }
        public string WebUrl { get; set; }
        public int Id { get; set; }
        public int Comments { get; set; }
    }
    #endregion
}
