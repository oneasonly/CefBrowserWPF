using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CefSharp.MinimalExample.Wpf
{
    public class cefLoadHandler : ILoadHandler
    {
        public event Action LoadedEvent = ()=>{};
        public void OnFrameLoadEnd(IWebBrowser chromiumWebBrowser, FrameLoadEndEventArgs frameLoadEndArgs) 
        {
            if (frameLoadEndArgs.Frame.IsMain) LoadedEvent();            
        }
        public void OnFrameLoadStart(IWebBrowser chromiumWebBrowser, FrameLoadStartEventArgs frameLoadStartArgs)
        {
            //frameLoadStartArgs.Frame.
        }
        public void OnLoadError(IWebBrowser chromiumWebBrowser, LoadErrorEventArgs loadErrorArgs) {}
        public void OnLoadingStateChange(IWebBrowser chromiumWebBrowser, LoadingStateChangedEventArgs loadingStateChangedArgs) 
        {
            //Trace.WriteLine($" isLoading={loadingStateChangedArgs.IsLoading}; canBack={loadingStateChangedArgs.CanGoBack}");
            //if (loadingStateChangedArgs.IsLoading == false) LoadedEvent();
        }
    }
}
