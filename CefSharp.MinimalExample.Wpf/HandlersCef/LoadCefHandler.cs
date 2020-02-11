using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CefSharp.MinimalExample.Wpf.HandlersCef
{
    public class LoadCefHandler : ILoadHandler
    {
        public event Action LoadedEvent = ()=>{};
        public event Action<IWebBrowser, LoadErrorEventArgs> LoadErrorEvent = (s,arg) => { };
        public void OnFrameLoadEnd(IWebBrowser chromiumWebBrowser, FrameLoadEndEventArgs frameLoadEndArgs) 
        {
            Trace.WriteLine($"OnFrameLoadEnd() isMain={frameLoadEndArgs.Frame.IsMain}; url={frameLoadEndArgs.Frame.Url}");
            if (frameLoadEndArgs.Frame.IsMain) LoadedEvent();            
        }
        public void OnFrameLoadStart(IWebBrowser chromiumWebBrowser, FrameLoadStartEventArgs frameLoadStartArgs)
        {
            //frameLoadStartArgs.Frame.
        }
        public void OnLoadError(IWebBrowser chromiumWebBrowser, LoadErrorEventArgs loadErrorArgs) 
        {
            if(AppSettings.isUrlAllowed(loadErrorArgs.FailedUrl) && !AppSettings.isContainsBlockedContent(loadErrorArgs.FailedUrl))
            {
                Trace.WriteLine($"OnLoadError() error={loadErrorArgs.ErrorCode} {loadErrorArgs.ErrorText}; url={loadErrorArgs.FailedUrl}");
                LoadErrorEvent(chromiumWebBrowser, loadErrorArgs);
            }
        }
        public void OnLoadingStateChange(IWebBrowser chromiumWebBrowser, LoadingStateChangedEventArgs loadingStateChangedArgs) 
        {
            //Trace.WriteLine($" isLoading={loadingStateChangedArgs.IsLoading}; canBack={loadingStateChangedArgs.CanGoBack}");
            //if (loadingStateChangedArgs.IsLoading == false) LoadedEvent();
        }
    }
}
