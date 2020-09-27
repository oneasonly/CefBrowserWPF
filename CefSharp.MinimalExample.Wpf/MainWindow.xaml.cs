using CefSharp.Wpf;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using MaterialDesignThemes.Wpf;
using System;
using System.Threading.Tasks;
using System.Windows.Media;
using Idle;
using CefSharp.MinimalExample.Wpf.HandlersCef;

namespace CefSharp.MinimalExample.Wpf
{
    public partial class MainWindow : Window
    {
        private IdleDetector idleDetector;
        private int idleTimedefault = 120; //таймаут бездействия в секундах
        public static bool IsShowKeyboard = false; //вкл/выкл экранной клавиатуры
        public MainWindow()
        {
            InitializeComponent();
            //this.Topmost = true;
            //this.WindowState = WindowState.Maximized;
            //this.WindowStyle = WindowStyle.None;
            idleDetector = new IdleDetector(this, idleTimedefault);
            idleDetector.IsIdle += OnIdle;
            //secondaryBrowser.Address = "тоже управление второым браузером"

            //сокрытие кнопок "назад, "домой" и т.д.
            //buttonBack.Visibility = Visibility.Collapsed;
            //buttonReload.Visibility = Visibility.Collapsed;
            //buttonHome.Visibility = Visibility.Collapsed;
        }

        private void OnIdle(object sender, EventArgs e) => GoHome();

        private void Browser_Initialized(object sender, System.EventArgs e)
        {
            Trace.WriteLine("MainWindow.Browser_Initialized()");
            Browser.Address = AppSettings.urlMain;
            
            var reqHandler = new RequestCefHandler();
            var loadHandler = new LoadCefHandler();

            loadHandler.LoadedEvent += OnUrlLoaded;
            loadHandler.LoadErrorEvent += (web, arg) => this.Dispatcher.Invoke(async() => 
            { 
                while (true) 
                {
                    Trace.WriteLine($"LoadErrorEvent()");
                    await Task.Delay(50); 
                    if (web.CanGoBack) web.Back(); 
                    break; 
                } 
            });

            Browser.LifeSpanHandler = new LifeSpanCefHandler();
            Browser.MenuHandler = new CustomMenuHandler();
            Browser.DialogHandler = new DialogCefHandler();
            Browser.RequestHandler = reqHandler;
            Browser.LoadHandler = loadHandler;            

            Browser.VirtualKeyboardRequested += Browser_VirtualKeyboardRequested;
        }

        private void OnUrlLoaded()
        {
            this.Dispatcher.Invoke(() =>
            {
                if (AppSettings.isUrlNeedZoom(Browser.Address)) Browser.SetZoomLevel(AppSettings.ZoomRate);

                if(AppSettings.UrlToShort(Browser.Address) == AppSettings.urlShortMain)
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
            if (IsShowKeyboard == false) return;
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
                try
                {
                    var js = await e.Browser.MainFrame.EvaluateScriptAsync(script);
                    double elemHeightPos = 0;
                    if (Double.TryParse(js?.Result?.ToString(), out elemHeightPos))
                    {
                        Trace.WriteLine($"OpenKeyboard(): js element pos={elemHeightPos}");
                        if (elemHeightPos > 425) dockArg = Dock.Top;
                    }
                }
                catch(Exception ex) 
                {
                    Trace.WriteLine($"Error with JS: {ex.Message}");
                }

                this.Dispatcher.Invoke(() =>
                {
                    if (dockArg == Dock.Top && KeyboardHost.IsBottomDrawerOpen || dockArg == Dock.Bottom && KeyboardHost.IsTopDrawerOpen)
                    { HideKeyboard(KeyboardHost); }
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
            this.Dispatcher.Invoke(() =>
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

        private void ReloadButton_Click(object sender, RoutedEventArgs e)
        {
            Browser.Reload(true);
        }
    }
}
