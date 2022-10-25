using System;
using UnityEngine;

namespace Utils.Extensions
{
    public static class StringExtensions
    {
        public static string WithColor(this string self, Color color)
        {
            return $"<color=#{ColorUtility.ToHtmlStringRGBA(color)}>{self}</color>";
        }
        
        public static string GetLastUntilOrEmpty(this string text, string stopAt = ":")
        {
            if (!string.IsNullOrWhiteSpace(text))
            {
                int charLocation = text.IndexOf(stopAt, StringComparison.Ordinal);

                if (charLocation < text.Length)
                {
                    return text.Substring(charLocation, text.Length - charLocation);
                }
            }

            return String.Empty;
        }
    }
}
