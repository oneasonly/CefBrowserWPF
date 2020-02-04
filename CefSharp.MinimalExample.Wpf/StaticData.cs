using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CefSharp.MinimalExample.Wpf
{
    public static class StaticData
    {
        //public static readonly string urlMain = "localhost";
        public static readonly string urlMain = @"C:\work\temp\index.html"; 
        public static readonly string urlHome = urlMain.Replace("http://", "").Replace(@"\", @"/");
    }
}
