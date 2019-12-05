using AXVideo.Helpers;
using AXVideo.Models;
using AXVideo.Services;
using AXVideo.Views;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AXVideo.ViewModels
{
    public class NewsViewModel : BaseViewModel
    {
        private int _pageNumber;
        private bool _noMoreData, _isAppearingLoad;
        private readonly INavigation _navigation;
        private readonly INewsService _newsService;

        private NewsListModel _selectedNewsItem;
        public NewsListModel SelectedNewsItem
        {
            get => _selectedNewsItem;
            set => SetProperty(ref _selectedNewsItem, value);
        }

        public ObservableCollection<NewsListModel> NewsItems { get; set; }

        public NewsViewModel(INavigation navigation)
        {
            _navigation = navigation;
            _newsService = DependencyService.Get<INewsService>();

            NewsItems = new ObservableCollection<NewsListModel>();
            RefreshCommand = new Command(Refresh);
            if (!ConnectivityHelper.IsConnected)
            {
                ConnectivityHelper.ShowToast("网络无法链接, 请稍后重试");
                return;
            }
            IsRefreshing = true;
            _ = GetNews();
        }

        private async Task GetNews()
        {
            try
            {
                var result = await _newsService.GetNewsListAsync(null, _pageNumber);
                if (result.Success)
                {
                    if (IsRefreshing)
                        NewsItems.Clear();

                    if (result.Result == null || result.Result.Count == 0)
                    {
                        FooterLabel = null;
                        return;
                    }
                    if (result.Result != null && result.Result.Count > 0)
                    {
                        if (result.Result.Count < 20)
                        {
                            _noMoreData = true;
                            FooterLabel = "已加载全部内容";
                        }
                        else
                        {
                            _noMoreData = false;
                            FooterLabel = "上拉加载更多内容";
                        }

                        foreach (var item in result.Result)
                        {
                            NewsListModel model = new NewsListModel()
                            {
                                Id = item.Id,
                                ImageUrl = item.CoverImageUrl,
                                TitleText = item.Title,
                                DateText = item.CreationTime.ToLocalTime()
                            };

                            if (item.PreviewImages?.Any() ?? false)
                            {
                                int length = item.PreviewImages.Length;
                                for (int i = 0; i < length; i++)
                                {
                                    item.PreviewImages[i] = item.PreviewImages[i].Replace("#forDropify=aaa.jpg", null);
                                }
                                if (item.PreviewImages.Length == 1)
                                {
                                    model.NewsImageUrl1 = item.PreviewImages[0];
                                }
                                else if (item.PreviewImages.Length == 2)
                                {
                                    model.NewsImageUrl1 = item.PreviewImages[0];
                                    model.NewsImageUrl2 = item.PreviewImages[1];
                                }
                                else
                                {
                                    model.NewsImageUrl1 = item.PreviewImages[0];
                                    model.NewsImageUrl2 = item.PreviewImages[1];
                                    model.NewsImageUrl3 = item.PreviewImages[2];
                                }
                            }
                            else if (!string.IsNullOrEmpty(item.CoverImageUrl))
                            {
                                model.NewsImageUrl1 = item.CoverImageUrl;
                            }
                            else
                            {
                                model.NewsImageUrl1 = "placeholder";
                            }
                            NewsItems.Add(model);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"##{ex}");
            }
            finally
            {
                IsRefreshing = false;
            }
        }

        private void Refresh()
        {
            _pageNumber = 0;
            _noMoreData = false;
            IsRefreshing = true;
            _ = GetNews();
        }

        // 跳转到新闻详情
        public Command ToDetailCommand => new Command(async () =>
        {
            if (SelectedNewsItem == null)
                return;
            await _navigation.PushAsync(new NewsDetailPage(new NewsDetailViewModel(SelectedNewsItem)));
            SelectedNewsItem = null;
        });

        // 加载更多
        public Command<NewsListModel> ItemAppearingCommand => new Command<NewsListModel>(async (paramter) =>
        {
            if (_noMoreData || _isAppearingLoad)
                return;
            var modelLast = NewsItems[NewsItems.Count - 1];
            if (modelLast.Id == paramter.Id)
            {
                _isAppearingLoad = true;
                FooterLabel = "加载中...";
                _pageNumber++;
                await GetNews();
                FooterLabel = "已加载全部内容";
                _isAppearingLoad = false;
            }
        });
    }
}
