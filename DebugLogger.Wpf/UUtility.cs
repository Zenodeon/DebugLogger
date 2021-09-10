using System;
using System.Collections.Generic;
using System.Text;

namespace DebugLogger.Wpf
{
    public static class UUtility
    {
        public static string KFormat(this int num)
        {          
            if (num >= 1000000)
                return (num / 1000000D).ToString("0.#") + "M";

            if (num >= 1000)
                return (num / 1000D).ToString("0.#") + "K";

            return num + "";
        }
    }
}