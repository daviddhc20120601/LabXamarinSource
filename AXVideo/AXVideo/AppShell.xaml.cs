using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AXVideo
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
        }

        protected override bool OnBackButtonPressed()
        {
            if (Navigation.NavigationStack.Count >= 2)
            {
                Navigation.PopAsync();
            }
            return true;
        }
    }
}