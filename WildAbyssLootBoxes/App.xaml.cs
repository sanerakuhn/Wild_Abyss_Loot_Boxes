using Microsoft.UI.Xaml.Controls;
using Windows.UI.ViewManagement;

namespace Wild_Abyss_Loot_Boxes
{
    public partial class App : Microsoft.Maui.Controls.Application
    {
        private readonly UISettings _uiSettings = new UISettings();

        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();

#if WINDOWS
            _uiSettings.ColorValuesChanged += OnColorValuesChanged;
            ApplyWindowsTheme();
#endif

        }

#if WINDOWS
        private void ApplyWindowsTheme()
        {
            var uiSettings = new UISettings();
            var theme = uiSettings.GetColorValue(UIColorType.Background);

            bool isDarkMode = theme == Microsoft.UI.Colors.Black;

            Microsoft.Maui.Handlers.WindowHandler.Mapper.AppendToMapping(nameof(IWindow), (handler, view) =>
            {
                var nativeWindow = handler.PlatformView;

                if (nativeWindow.Content is Panel rootPanel)
                {
                    rootPanel.Background = new Microsoft.UI.Xaml.Media.SolidColorBrush(
                        isDarkMode ? Microsoft.UI.ColorHelper.FromArgb(255, 30, 30, 30) : Microsoft.UI.Colors.White);
                }
            });
        }

        private void OnColorValuesChanged(UISettings sender, object args)
        {
            MainThread.BeginInvokeOnMainThread(ApplyWindowsTheme);
        }
#endif
    }
}
