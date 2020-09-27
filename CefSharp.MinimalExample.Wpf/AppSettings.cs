using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CefSharp.MinimalExample.Wpf
{
    public static class AppSettings
    {
        public static readonly string urlMain = @"https://www.ibank.belapb.by";
        //public static readonly string urlMain = @"http://localhost";        
        public static readonly double ZoomRate = 2.2; //коэффициент увеличения для адрессов в "UrlNeedZoom"

        #region not editable fields
        public static readonly string urlShortMain = UrlToShort(urlMain);
        public static readonly List<string> UrlsAllowed = new List<string>(); //открываются только те адреса, в которых имееются строки из данного списка
        public static readonly List<string> BlockedContents = new List<string>(); //блочим все что имеется данные строки
        public static readonly List<string> UrlNeedZoom = new List<string>(); //адреса нуждающиеся в увеличении масштаба
        #endregion

        static AppSettings()
        {            
            UrlsAllowed.Add(@"ibank.belapb.by");
            UrlsAllowed.Add(@"raschet.by");
            //UrlsAllowed.Add(urlShortMain);

            BlockedContents.Add($".pdf");
            BlockedContents.Add($".jpg");
            BlockedContents.Add($".png");
            BlockedContents.Add($"mailto");

            //UrlNeedZoom.Add($"82.209.205.124"); //адреса нуждающиеся в увеличении масштаба
        }

        public static string UrlToShort(string argUrl) => argUrl?.ToLower()?.Replace("http://", "")?.Replace("https://", "")?.Replace(@"\", @"/")?.Trim('/');
        public static bool isUrlAllowed(string argUrl) => UrlsAllowed.Count==0 || TrueIfOneContains(argUrl, UrlsAllowed);
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
