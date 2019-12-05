using System;

namespace AXVideo.Models
{
    public class NewsListModel
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public string TitleText { get; set; }
        public DateTime DateText { get; set; }
        public bool IsActivity { get; set; }
        public string NewsImageUrl1 { get; set; }
        public string NewsImageUrl2 { get; set; }
        public string NewsImageUrl3 { get; set; }
    }
}
