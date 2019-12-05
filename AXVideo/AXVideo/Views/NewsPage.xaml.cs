using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using AXVideo.ViewModels;

namespace AXVideo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewsPage : ContentPage
    {
        public NewsPage()
        {
            InitializeComponent();
            BindingContext = new NewsViewModel(Navigation);
        }
    }
}