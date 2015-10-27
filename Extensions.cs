using System;
using System.Text.RegularExpressions;

namespace BICSL
{
    public static class Extensions
    {
        public static int GetWordCount(this String str)
        {
            MatchCollection collection = Regex.Matches(str, @"[\S]+");
            return collection.Count;
        }
    }
}
