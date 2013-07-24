using System;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
namespace AspNetUI.Support
{
    public static class StringOperations
    {
        /// <summary>
        /// Returns trimmed string if contains word size greater than longWordSize
        /// </summary>
        /// <param name="stringToTrim"></param>
        /// <param name="longWordSize"></param>
        /// <returns>string</returns>
        public static string TrimAfterLongWord(string stringToTrim, int longWordSize = 75)
        {
                if (String.IsNullOrEmpty(stringToTrim) || longWordSize <= 0)
                {
                    return stringToTrim;
                }

                string[] stringParts = stringToTrim.Split(new char[]{' ', '-'});
                var stringbuilder = new StringBuilder();

                foreach (var stringPart in stringParts)
                {
                    if (stringPart.Length > longWordSize)
                    {
                        stringbuilder.Append(stringPart.Substring(0, longWordSize) + "...");
                        return stringbuilder.ToString();
                    }
                    
                    stringbuilder.Append(stringPart);
                }

            return stringbuilder.ToString();
        }

        public static string TrimAfter(string stringToTrim, int size = 75)
        {
                if (String.IsNullOrEmpty(stringToTrim) || size <= 0)
                {
                    return stringToTrim;
                }

                if (stringToTrim.Length > size)
                {
                    return stringToTrim.Substring(0, size) + "...";
                }

            return stringToTrim;
        }

        public static string GetOptimalStringSize(string stringToTrim, Font font, int MaxWidth) {
            Graphics g = Graphics.FromImage(new Bitmap(10, 10));
            
            int L = 1, H = 1000, M = 0;
            for (int i = 0; i < 12; ++i) {
                M = (L + H) / 2;
                SizeF sz = g.MeasureString(TrimAfterLongWord(stringToTrim, M), font);
                if ((int)sz.Width > MaxWidth)
                {
                    H = M - 1;
                }
                else {
                    L = M;
                }
            }
            return TrimAfterLongWord(stringToTrim, M);
        }

        public static string GetRandomStringWord(int length)
        {
            return Guid.NewGuid().ToString("N").Substring(0, length);
        }

        public static string RemoveColonFromEnd(string data)
        {
            data = data.TrimEnd();
            int colonIndex = data.LastIndexOf(":");
            data = colonIndex == data.Length - 1 ? data.Remove(colonIndex) : data;

            return data;
        }

        public static string RemoveHtmlTags(string data)
        {
            return Regex.Replace(data, "<.*?>", string.Empty);
        }

        public static string GetRelatedName(string data)
        {
            return RemoveColonFromEnd(RemoveHtmlTags(data));
        }

        public static string ReplaceNullOrWhiteSpace(string source, string replaceString)
        {
            if (string.IsNullOrWhiteSpace(source))
                return replaceString;
            return source;
        }
    }
}