using CefSharp.Wpf;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using MaterialDesignThemes.Wpf;
using System;
using System.Threading.Tasks;
using System.Windows.Media;
using Idle;

namespace CefSharp.MinimalExample.Wpf
{
    public partial class MainWindow : Window
    {
        private IdleDetector idleDetector;
        private int idleTimedefault = 120;
        public MainWindow()
        {
            InitializeComponent();
            //this.Topmost = true;
            this.WindowState = WindowState.Maximized;
            this.WindowStyle = WindowStyle.None;
            idleDetector = new IdleDetector(this, idleTimedefault);
            idleDetector.IsIdle += OnIdle;
        }

        private void OnIdle(object sender, EventArgs e) => GoHome();

        private void Browser_Initialized(object sender, System.EventArgs e)
        {
            Trace.WriteLine("MainWindow.Browser_Initialized()");
            Browser.Address = AppSettings.urlMain;
            var reqHandler = new cefReqHandler();
            var loadHandler = new cefLoadHandler();

            loadHandler.LoadedEvent += OnUrlLoaded;


            Browser.LifeSpanHandler = new cefSpanHand();
            Browser.MenuHandler = new CustomMenuHandler();
            Browser.RequestHandler = reqHandler;
            Browser.LoadHandler = loadHandler;

            Browser.VirtualKeyboardRequested += Browser_VirtualKeyboardRequested;


            //CefSettings settings = new CefSettings();
            //settings.CefCommandLineArgs.Add("disable-usb-keyboard-detect", "1");
            //Cef.Initialize(settings);
        }

        private void OnUrlLoaded()
        {
            this.Dispatcher.Invoke(() =>
            {
                if (AppSettings.isUrlNeedZoom(Browser.Address)) Browser.SetZoomLevel(AppSettings.ZoomRate);

                if(Browser.Address.Replace(@"\","/").Contains(AppSettings.urlMain.Replace(@"\", "/")))
                {
                    buttonBack.IsEnabled = false;
                }
                else
                {
                    buttonBack.IsEnabled = true;
                }
            });
        }

        private void Browser_VirtualKeyboardRequested(object sender, VirtualKeyboardRequestedEventArgs e)
        {
            if (e.TextInputMode == Enums.TextInputMode.None) HideKeyboard(sender);
            else OpenKeyboard(e);
        }

        private async Task OpenKeyboard(VirtualKeyboardRequestedEventArgs e)
        {
            try
            {
                double kbHeight = 400;
                Dock dockArg = Dock.Bottom;
                string script = @"( () => { var element = document.activeElement; return element.getBoundingClientRect().bottom ; } )();"; //@"document.activeElement";
                var js = await e.Browser.MainFrame.EvaluateScriptAsync(script);
                double elemHeightPos = 0;
                if (Double.TryParse(js?.Result?.ToString(), out elemHeightPos))
                {
                    Trace.WriteLine($"OpenKeyboard(): js element pos={elemHeightPos}");
                    if (elemHeightPos > 400) dockArg = Dock.Top;
                }

                this.Dispatcher.Invoke(() =>
                {

                    BotFullKeyboard.SetEngLang(false).Height = kbHeight;
                    TopFullKeyboard.SetEngLang(false).Height = kbHeight;
                    DrawerHost.OpenDrawerCommand.Execute(dockArg, KeyboardHost);

                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Browser_VirtualKeyboardRequested() {ex.Message}");
            }
        }

        private void HideKeyboard(object sender)
        {
            this.Dispatcher.InvokeAsync(() =>
            {
                try
                {
                    DrawerHost.CloseDrawerCommand.Execute(null, sender as FrameworkElement);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Browser_VirtualKeyboardRequested() {ex.Message}");
                }
            });

        }

        private void Browser_Loaded(object sender, RoutedEventArgs e)
        {
            Trace.WriteLine("MainWindow.Browser_Loaded()");

            //Browser.Address = StaticData.urlMain;


        }


        private async Task GoHome()
        {
            Browser.Stop();
            Browser.Address = AppSettings.urlMain;
        }



        #region buttons clicks
        private void buttonBack_Click(object sender, RoutedEventArgs e)
        {
            if (Browser.CanGoBack) Browser.Back();
        }
        private async void buttonHome_Click(object sender, RoutedEventArgs e)
        {
            await GoHome();
            HideKeyboard(sender);
        }
        private void ButtonCloseKeyboard_Click(object sender, RoutedEventArgs e)
        {
            HideKeyboard(sender);
        }
        #endregion
    }
}
