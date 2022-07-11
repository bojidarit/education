namespace WebView2Demo
{
    using CommonLib;
    using System.Windows;
    using System.Windows.Input;

    // One must install WebView2 Runtime on host machine to use it.
    // Source: https://developer.microsoft.com/en-us/microsoft-edge/webview2/
    public partial class WebView2Window : Window
    {
        public WebView2Window()
        {
            InitializeComponent();

            KeyUp += WebView2Window_KeyUp;

            InitAsync();
        }


        #region Event helpers

        private void WebView2Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Application.Current.Shutdown();
            }
        }

        #endregion


        #region Helpers

        private async void InitAsync()
        {
            await webView.EnsureCoreWebView2Async(null);

            var html = Common.GetHtmlFromResource("google-map-edge.html");
            webView.NavigateToString(html);
        }

        #endregion
    }
}
