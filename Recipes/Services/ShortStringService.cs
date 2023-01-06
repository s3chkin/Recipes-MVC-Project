﻿namespace Recipes.Services
{
    public class ShortStringService : IShortStringService
    {
        //string GetShort(string str, int maxLen);

        public string GetShort(string str, int maxLen)
        {
            if (str == null)
            {
                return str;
            }
            if (str.Length <= maxLen)
            {
                return str;
            }
            return str.Substring(0, maxLen) + "...";
        }
    }
}
