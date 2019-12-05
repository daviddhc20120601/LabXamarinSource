using System.Collections.ObjectModel;
using System.Threading.Tasks;
using AXVideo.Models;
using AXVideo.Services;
using AXVideo.Helpers;
using Xamarin.Forms;
using AXVideo.Views;

namespace AXVideo.ViewModels
{
    public class VideoViewModel : BaseViewModel
    {
        private int _pageNumber;
        private bool _noMoreData, _isAppearingLoad;
        private readonly INavigation _navigation;
        private readonly IVideoService _videoService;
        private readonly IAccountService _accountService;

        public Command ToDetailCommand { get; set; }

        private VideoListModel _selectedListItem;
        public VideoListModel SelectedListItem
        {
            get => _selectedListItem;
            set => SetProperty(ref _selectedListItem, value);
        }

        public ObservableCollection<VideoListModel> VideoItems { get; set; }

        public VideoViewModel(INavigation navigation)
        {
            _navigation = navigation;
            _videoService = DependencyService.Get<IVideoService>();
            _accountService = DependencyService.Get<IAccountService>();

            VideoItems = new ObservableCollection<VideoListModel>();

            ToDetailCommand = new Command(async () =>
            {
                if (SelectedListItem == null)
                    return;
                await _navigation.PushAsync(new VideoDetailPage(new VideoDetailViewModel(SelectedListItem)));
                SelectedListItem = null;
            });

            RefreshCommand = new Command(() =>
            {
                _pageNumber = 0;
                _noMoreData = false;
                IsRefreshing = true;
                _ = GetPlaylist();
            });

            _ = LoadData();
        }

        public async Task LoadData()
        {
            if (!ConnectivityHelper.IsConnected)
            {
                ConnectivityHelper.ShowToast("网络无法链接, 请稍后重试");
                return;
            }
            IsRefreshing = true;
            if (string.IsNullOrEmpty(AppSettings.AccountToken))
                await GetTenantToken();
            if (string.IsNullOrEmpty(AppSettings.AccountToken))
                return;
            await GetPlaylist();
        }

        //获取tenant toekn
        private async Task<bool> GetTenantToken()
        {
            var result = await _accountService.GetTenantTokenAsync();
            if (result.Success)
            {
                AppSettings.AccountToken = result.Result.Token;
                return true;
            }
            return false;
        }

        private async Task GetPlaylist()
        {
            try
            {
                ApiResult<PlaylistUnitsDto> result = await _videoService.GetPlaylistAsync(17, _pageNumber);
                if (result.Success)
                {
                    if (IsRefreshing)
                        VideoItems.Clear();

                    if (result.Result == null || result.Result.Units == null || result.Result.Units.Length == 0)
                    {
                        FooterLabel = null;
                        _noMoreData = true;
                        return;
                    }

                    if (result.Result?.Units?.Length > 0)
                    {
                        if (result.Result.Units.Length < 20)
                        {
                            _noMoreData = true;
                            FooterLabel = "已加载全部内容";
                        }
                        else
                        {
                            _noMoreData = false;
                            FooterLabel = "上拉加载更多内容";
                        }

                        foreach (Unit item in result.Result.Units)
                        {
                            VideoListModel model = new VideoListModel
                            {
                                Id = item.Id,
                                LivingTile = item.Name,
                                VideoImage = item.PosterURL,
                                UploadTime = item.Time,
                                VideoLength = item.Length.HasValue ? item.Length.Value.Hours > 0 ? string.Format("{0:hh\\:mm\\:ss}", item.Length) : string.Format("{0:mm\\:ss}", item.Length) : null
                            };
                            VideoItems.Add(model);
                        }
                    }
                    return;
                }
                if (result.ErrorDto.SignOut)
                    return;
            }
            finally
            {
                IsRefreshing = false;
            }
        }

        //加载更多
        public Command<VideoListModel> ItemAppearingCommand => new Command<VideoListModel>(async (paramter) =>
        {
            if (_noMoreData || _isAppearingLoad)
                return;
            var modelLast = VideoItems[VideoItems.Count - 1];
            if (modelLast.Id == paramter.Id)
            {
                _isAppearingLoad = true;
                FooterLabel = "加载中...";
                _pageNumber++;
                await GetPlaylist();
                FooterLabel = "已加载全部内容";
                _isAppearingLoad = false;
            }
        });
    }
}