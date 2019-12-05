using Acr.UserDialogs;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace AXVideo.Helpers
{
    public class ConnectivityHelper
    {
        public static bool IsConnected
        {
            get
            {
                return CrossConnectivity.Current.IsConnected;
            }
        }

        public static void ShowToast(string message, ToastPosition position = ToastPosition.Top)
        {
            ToastConfig toastConfig = new ToastConfig(message)
            {
                Position = position
            };
            toastConfig.SetDuration(3000);
            toastConfig.SetBackgroundColor((Color)App.Current.Resources["MainColor"]);

            UserDialogs.Instance.Toast(toastConfig);
        }
    }
}
