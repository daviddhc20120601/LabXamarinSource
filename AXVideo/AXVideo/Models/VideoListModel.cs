using System;
using System.Collections.Generic;
using System.Text;

namespace AXVideo.Models
{
   public class VideoListModel
    {
        public int Id { get; set; }
        public string VideoImage { get; set; }
        public string VideoUrl { get; set; }
        public string VideoLength { get; set; }
        public string LivingTile { get; set; }
        public DateTime UploadTime { get; set; }
    }
}
