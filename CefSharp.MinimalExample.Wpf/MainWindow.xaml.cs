using CefSharp.Wpf;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using MaterialDesignThemes.Wpf;
using System;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CefSharp.MinimalExample.Wpf
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //this.Topmost = true;
            //this.WindowState = WindowState.Maximized;
            //this.WindowStyle = WindowStyle.None;
        }

        private void Browser_Initialized(object sender, System.EventArgs e)
        {
            Trace.WriteLine("MainWindow.Browser_Initialized()");
            Browser.Address = StaticData.urlMain;
            var reqHandler = new cefReqHandler();
            //reqHandler.OnZoomEvent += () => { this.Dispatcher.Invoke(() => Browser.ZoomLevel = 3); };
            //reqHandler.DefaultZoomEvent += () => { this.Dispatcher.Invoke(() => Browser.ZoomLevel = 0); };
            Browser.RequestHandler = reqHandler;
            Browser.LifeSpanHandler = new cefSpanHand();
            Browser.MenuHandler = new CustomMenuHandler();             

            Browser.VirtualKeyboardRequested += Browser_VirtualKeyboardRequested;


            //CefSettings settings = new CefSettings();
            //settings.CefCommandLineArgs.Add("disable-usb-keyboard-detect", "1");
            //Cef.Initialize(settings);
        }

        private void Browser_VirtualKeyboardRequested(object sender, VirtualKeyboardRequestedEventArgs e)
        {
            double kbHeight = 400;
            Dock dockArg = Dock.Bottom;            
            this.Dispatcher.Invoke(() =>
            {
                try
                {
                    BotFullKeyboard.SetEngLang(false).Height = kbHeight;
                    DrawerHost.OpenDrawerCommand.Execute(dockArg, KeyboardHost);                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Browser_VirtualKeyboardRequested() {ex.Message}");
                }
            });
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DrawerHost.CloseDrawerCommand.Execute(null, sender as FrameworkElement);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Browser_VirtualKeyboardRequested() {ex.Message}");
            }
        }

        private void Browser_Loaded(object sender, RoutedEventArgs e)
        {
            Trace.WriteLine("MainWindow.Browser_Loaded()");

            //Browser.Address = StaticData.urlMain;


        }

        private async void buttonHome_Click(object sender, RoutedEventArgs e)
        {
            Browser.Stop();
            while (Browser.CanGoBack)
            {
                if(!Browser.IsLoading)
                {
                    Browser.Back();                    
                }
                await Task.Delay(50);
            }
        }
    }
}
