using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CefSharp.MinimalExample.Wpf
{
    public class cefFocusHandler : IFocusHandler
    {
        public event Action OnTakeFocusEvent = ()=> { };
        public void OnGotFocus(IWebBrowser chromiumWebBrowser, IBrowser browser) { }
        public bool OnSetFocus(IWebBrowser chromiumWebBrowser, IBrowser browser, CefFocusSource source) => false;
        public void OnTakeFocus(IWebBrowser chromiumWebBrowser, IBrowser browser, bool next)
        {
            OnTakeFocusEvent();
        }
    }
}
