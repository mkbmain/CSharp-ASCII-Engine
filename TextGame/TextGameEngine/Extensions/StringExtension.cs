using System;
using System.Linq;

namespace TextGameEngine.Extensions
{
    public static class StringExtension
    {
        public static char FirstNoWhiteSpaceChar(this string item)
        {
            if(item == null){throw  new ArgumentNullException();}
            return item.Trim().FirstOrDefault();
        }
    }
}