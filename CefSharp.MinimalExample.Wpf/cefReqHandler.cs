using CefSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CefSharp.MinimalExample.Wpf
{
    public class cefReqHandler : IRequestHandler
    {
        public event Action OnZoomEvent = ()=> { };
        public event Action DefaultZoomEvent = () => { };
        public bool OnBeforeBrowse(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, bool userGesture, bool isRedirect)
        {
            bool isZoom = request.Url.Contains("82.209.205.124:8080") || request.Url.Contains("82.209.205.124:8080") || request.Url.Contains("82.209.205.124:8080");
            if (isZoom)
            {
                OnZoomEvent();
            }
            else DefaultZoomEvent();

            bool isAddressCheck = !request.Url.Contains(StaticData.urlHome) && !request.Url.Contains("82.209.205.124:8080") && !request.Url.Contains("82.209.205.124:8080") && !request.Url.Contains("82.209.205.124:8080");
            bool isFileCheck = request.Url.Contains(".pdf") || request.Url.Contains(".jpg") || request.Url.Contains(".JPG") || request.Url.Contains(".png");
            if (isAddressCheck || isFileCheck)
            {
                browser.StopLoad();
            }

            return false;
        }

        public bool OnQuotaRequest(IWebBrowser chromiumWebBrowser, IBrowser browser, string originUrl, long newSize, IRequestCallback callback)
        {
            callback.Dispose();
            return false;
        }

        #region not used
        public bool GetAuthCredentials(IWebBrowser chromiumWebBrowser, IBrowser browser, string originUrl, bool isProxy, string host, int port, string realm, string scheme, IAuthCallback callback) => false;
        public IResourceRequestHandler GetResourceRequestHandler(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, bool isNavigation, bool isDownload, string requestInitiator, ref bool disableDefaultHandling) => null;        
        public bool OnCertificateError(IWebBrowser chromiumWebBrowser, IBrowser browser, CefErrorCode errorCode, string requestUrl, ISslInfo sslInfo, IRequestCallback callback) => false;
        public bool OnOpenUrlFromTab(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, string targetUrl, WindowOpenDisposition targetDisposition, bool userGesture) => false;
        public void OnPluginCrashed(IWebBrowser chromiumWebBrowser, IBrowser browser, string pluginPath){}        
        public void OnRenderProcessTerminated(IWebBrowser chromiumWebBrowser, IBrowser browser, CefTerminationStatus status) { }
        public void OnRenderViewReady(IWebBrowser chromiumWebBrowser, IBrowser browser) { }
        public bool OnSelectClientCertificate(IWebBrowser chromiumWebBrowser, IBrowser browser, bool isProxy, string host, int port, X509Certificate2Collection certificates, ISelectClientCertificateCallback callback) => false;
        #endregion
    }
}
