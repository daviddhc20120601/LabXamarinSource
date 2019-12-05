using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AXVideo.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public ICommand OpenWebCommand { get; }

        public AboutViewModel()
        {
            OpenWebCommand = new Command(() => Launcher.OpenAsync(new Uri("https://www.bopoda.cn")));
        }
    }
}