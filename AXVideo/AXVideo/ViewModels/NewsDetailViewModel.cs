using AXVideo.Helpers;
using AXVideo.Models;
using AXVideo.Services;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AXVideo.ViewModels
{
    public class NewsDetailViewModel : BaseViewModel
    {
        private readonly NewsListModel _newsItem;
        private readonly INewsService _newsService;
        private readonly string imgCss = "img{display:inline;height:auto;max-width:100%;}";

        private string _newsContent;
        public string NewsContent
        {
            get => _newsContent;
            set => SetProperty(ref _newsContent, value);
        }

        public NewsDetailViewModel(NewsListModel model)
        {
            _newsItem = model;
            _newsService = DependencyService.Get<INewsService>();

            if (!ConnectivityHelper.IsConnected)
            {
                ConnectivityHelper.ShowToast("网络无法链接, 请稍后重试");
                return;
            }
            Acr.UserDialogs.UserDialogs.Instance.ShowLoading("加载中");
            _ = GetNewsDatails();
        }

        /// <summary>
        /// 获取新闻详情数据
        /// </summary>
        /// <returns></returns>
        public async Task GetNewsDatails()
        {
            try
            {
                ApiResult<NewsDto> result = await _newsService.GetNewsAsync(_newsItem.Id);
                if (result != null && result.Success)
                {
                    if (result.Result == null)
                        return;
                    Title = result.Result.Title;
                    //适配图片
                    NewsContent = $"<html><head><style>{imgCss}</style></head><body>{result.Result.Content}</body></html>";
                }
                else
                {
                    if (result.ErrorDto.SignOut)
                    {
                    }
                }
            }
            finally
            {
                Acr.UserDialogs.UserDialogs.Instance.HideLoading();
            }
        }
    }
}
