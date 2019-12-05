using AXVideo.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Octane.Xamarin.Forms.VideoPlayer.Constants;

namespace AXVideo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VideoDetailPage : ContentPage
    {
        private readonly VideoDetailViewModel viewModel;

        public VideoDetailPage(VideoDetailViewModel viewModel)
        {
            InitializeComponent();
            Shell.SetTabBarIsVisible(this, false);
            BindingContext = this.viewModel = viewModel;

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

        private void VideoPlayer_PlayerStateChanged(object sender, Octane.Xamarin.Forms.VideoPlayer.Events.VideoPlayerStateChangedEventArgs e)
        {
            if (e.CurrentState == PlayerState.Playing)
            {
                viewModel.IsRunning = false;
            }
        }
    }
}