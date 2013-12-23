using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace TheExchange.MVC.Utilities
{
    public static class Phone
    {
        public static string RemoveNonDigits(string dirtyValue)
        {
            return Regex.Replace(dirtyValue, @"\D+", string.Empty);
        }
    }
}