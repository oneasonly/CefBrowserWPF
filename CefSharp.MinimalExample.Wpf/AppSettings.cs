using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CefSharp.MinimalExample.Wpf
{
    public static class AppSettings
    {
        public static readonly string urlMain = "localhost";
        //public static readonly string urlMain = @"C:\work\temp\index.html";
        public static readonly string urlShortMan = urlMain.Replace("http://", "").Replace(@"\", @"/");

        public static readonly List<string> UrlsAllowed = new List<string>();
        public static readonly List<string> BlockedContents = new List<string>();
        public static readonly List<string> UrlNeedZoom = new List<string>();
        public static readonly double ZoomRate = 2.2;

        static AppSettings()
        {
            UrlsAllowed.Add(urlShortMan);
            UrlsAllowed.Add($"82.209.205.124");
            UrlsAllowed.Add($"86.57.164.206");
            UrlsAllowed.Add($"178.124.203.59");
            UrlsAllowed.Add($"volrb");

            BlockedContents.Add($".pdf");
            BlockedContents.Add($".jpg");
            BlockedContents.Add($".png");
            BlockedContents.Add($"mailto");

            UrlNeedZoom.Add($"82.209.205.124");
            UrlNeedZoom.Add($"86.57.164.206");
            UrlNeedZoom.Add($"178.124.203.59");
        }
        
        public static bool isUrlAllowed(string argUrl) => TrueIfOneContains(argUrl, UrlsAllowed);
        public static bool isContainsBlockedContent(string argContent)=> TrueIfOneContains(argContent, BlockedContents);
        public static bool isUrlNeedZoom(string argUrl) => TrueIfOneContains(argUrl, UrlNeedZoom);

        private static bool TrueIfOneContains(string argSource, IList<string> argList)
        {
            foreach (var item in argList)
            {
                if (argSource.ToLower().Contains(item?.ToLower())) return true;
            }
            return false;
        }
    }
}
