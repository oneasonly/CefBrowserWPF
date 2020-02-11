using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CefSharp.MinimalExample.Wpf.HandlersCef
{
    public class DialogCefHandler : IDialogHandler
    {
        public bool OnFileDialog(IWebBrowser chromiumWebBrowser, IBrowser browser
            , CefFileDialogMode mode, CefFileDialogFlags flags
            , string title, string defaultFilePath
            , List<string> acceptFilters, int selectedAcceptFilter
            , IFileDialogCallback callback)
        {            
            return true;
        }
    }
}
