using AXVideo.Helpers;
using AXVideo.Models;
using AXVideo.Services;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AXVideo.ViewModels
{
    public class VideoDetailViewModel : BaseViewModel
    {
        private readonly VideoListModel _videoModel;
        private readonly IVideoService _videoService;

        private string _videoImage;
        public string VideoImage
        {
            get => _videoImage;
            set => SetProperty(ref _videoImage, value);
        }

        private DateTime _uploadTime;
        public DateTime UploadTime
        {
            get => _uploadTime;
            set => SetProperty(ref _uploadTime, value);
        }

        private string _videoTitle;
        public string VideoTile
        {
            get => _videoTitle;
            set => SetProperty(ref _videoTitle, value);
        }

        private string _description;
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        private string _videoUrl;
        public string VideoUrl
        {
            get => _videoUrl;
            set => SetProperty(ref _videoUrl, value);
        }

        private bool _isRunning;
        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        public VideoDetailViewModel(VideoListModel model)
        {
            _videoModel = model;
            VideoImage = model.VideoImage;
            _videoService = DependencyService.Get<IVideoService>();

            if (!ConnectivityHelper.IsConnected)
            {
                ConnectivityHelper.ShowToast("网络无法链接, 请稍后重试");
                return;
            }
            IsRunning = true;
            _ = GetVideoDetails();
        }

        /// <summary>
        /// 获取视频详情信息
        /// </summary>
        /// <returns></returns>
        public async Task GetVideoDetails()
        {
            try
            {
                ApiResult<UnitDto> result = await _videoService.GetVideoAsync(_videoModel.Id);
                if (result != null && result.Success)
                {
                    if (result.Result == null)
                        return;
                    VideoUrl = result.Result.Playback.AmshlsV3URL;
                    VideoTile = result.Result.Name;
                    UploadTime = result.Result.Time;
                    Description = result.Result.Description;
                    return;
                }
                if (result.ErrorDto.SignOut)
                    return;
            }
            finally
            {
                Acr.UserDialogs.UserDialogs.Instance.HideLoading();
            }
        }
    }
}
