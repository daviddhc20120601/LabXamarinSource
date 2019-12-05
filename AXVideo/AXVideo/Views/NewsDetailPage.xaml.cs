using AXVideo.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AXVideo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewsDetailPage : ContentPage
    {
        public NewsDetailPage(NewsDetailViewModel viewModel)
        {
            InitializeComponent();
            Shell.SetTabBarIsVisible(this, false);
            BindingContext = viewModel;
            if (Device.RuntimePlatform == Device.iOS)
                Shell.SetBackButtonBehavior(this, new BackButtonBehavior()
                {
                    IconOverride = "back",
                    Command = new Command(() =>
                    {
                        Navigation.PopAsync();
                    })
                });
        }
    }
}