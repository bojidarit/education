namespace CefWebDemo
{
    using CefSharp;
    using CommonLib;
    using System;
    using System.Windows;
    using System.Windows.Input;

    public partial class CefWebWindow : Window
    {
        public CefWebWindow()
        {
            InitializeComponent();

            KeyUp += CefWebWindow_KeyUp;
        }


        #region Event Handlers

        private void CefWebWindow_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Application.Current.Shutdown();
            }
        }

        private void Browser_Initialized(object sender, EventArgs e)
        {
            var html = Common.GetHtmlFromResource("google-map-edge.html");
            LoadHtml(html);
        }

        #endregion


        #region Helpers

        private void LoadHtml(string html)
        {
            try
            {
                var url = @"http://test/page";

                Browser.LoadHtml(html, url);
                Browser.Address = url;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"{ex.GetType().FullName}{Environment.NewLine}" +
                    $"{ex.Message}{Environment.NewLine}" +
                    $"{ex.StackTrace}",
                    "LoadHTML()");
            }
        }

        #endregion
    }
}
