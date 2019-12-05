using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using AXVideo.ViewModels;

namespace AXVideo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VideoPage : ContentPage
    {
        public VideoPage()
        {
            InitializeComponent();
            BindingContext = new VideoViewModel(Navigation);
        }
    }
}