namespace WebBrowserDemo
{
    using System.Windows;
    using System.Windows.Input;

    public partial class WebBrowserWindow : Window
    {
        private string dummyHtml = "<html><body><h1>test</h1></body></html>";

        public WebBrowserWindow()
        {
            InitializeComponent();

            KeyUp += MainWindow_KeyUp;
        }


        #region Event Handlers

        private void MainWindow_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Application.Current.Shutdown();
            }
        }

        private void WebBrowser_Loaded(object sender, RoutedEventArgs e)
        {
            // NOT Working in this control
            //var html = Common.GetHtmlFromResource("google-map-ie10.html");

            WebBrowser.NavigateToString(dummyHtml);
        }

        #endregion
    }
}
