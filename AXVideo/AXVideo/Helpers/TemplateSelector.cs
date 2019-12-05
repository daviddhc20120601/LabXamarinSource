using AXVideo.Models;
using Xamarin.Forms;

namespace AXVideo.Helpers
{
    public class NewsTemplateSelector : DataTemplateSelector
    {
        public DataTemplate OneImageTemplate { get; set; }
        public DataTemplate TwoImageTemplate { get; set; }
        public DataTemplate ThreeImageTemplate { get; set; }
        public DataTemplate TextTemplate { get; set; }
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var news = (NewsListModel)item;

            if (!string.IsNullOrEmpty(news.NewsImageUrl1) && !string.IsNullOrEmpty(news.NewsImageUrl2) && !string.IsNullOrEmpty(news.NewsImageUrl3))
                return ThreeImageTemplate;

            if (!string.IsNullOrEmpty(news.NewsImageUrl1) && !string.IsNullOrEmpty(news.NewsImageUrl2))
                return TwoImageTemplate;

            if (!string.IsNullOrEmpty(news.NewsImageUrl1))
                return OneImageTemplate;

            return TextTemplate;
        }
    }
}
