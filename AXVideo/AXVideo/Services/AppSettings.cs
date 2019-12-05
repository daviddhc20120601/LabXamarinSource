using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace AXVideo.Services
{
    public class AppSettings
    {
        private static ISettings Settings => CrossSettings.Current;

        public static string AccountToken
        {
            get => Settings.GetValueOrDefault(nameof(AccountToken), string.Empty);
            set => Settings.AddOrUpdateValue(nameof(AccountToken), value);
        }
    }
}
