namespace WebBrowserDemo
{
    using CommonLib;
    using System.Windows;
    using System.Windows.Input;

    public partial class WebBrowserWindow : Window
    {
        public WebBrowserWindow()
        {
            InitializeComponent();

            KeyUp += MainWindow_KeyUp;

            var html = Common.GetHtmlFromResource("google-map-edge.html");

            webBrowser.NavigateToString(html);
        }


        #region Event Handlers

        private void MainWindow_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Application.Current.Shutdown();
            }
        }

        #endregion
    }
}
