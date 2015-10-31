using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace BICSL
{
    public enum ResponseFormat
    {
        json,
        html,
        text
    }

    static public class MeatGrinder
    {
        const string BASE_BACON_API_URI = "https://baconipsum.com/api/";

        public static string GetParagraphs(int paragraphCount, ResponseFormat format, bool startWithLorem = false, bool allMeat = false)
        {
            var url = GetApiURL(format, allMeat, paragraphCount: paragraphCount, startWithLorem: startWithLorem);
            return RequestIpsum(url);
        }

        /// <summary>
        /// Get sentences
        /// </summary>
        /// <param name="sentenceCount">Number of sentences</param>
        /// <param name="allMeat">Meat only or meat mixed with miscellaneous ‘lorem ipsum’ filler.</param>
        /// <param name="startWithLorem">Start the first paragraph with ‘Bacon ipsum dolor...’.</param>
        /// <returns></returns>
        public static string GetSentences(int sentenceCount, ResponseFormat format, bool startWithLorem = false, bool allMeat = false)
        {
            var sentences = GetApiURL(format, allMeat, sentenceCount: sentenceCount, startWithLorem: startWithLorem);
            return RequestIpsum(sentences);
        }

        /// <summary>
        /// Get sentences
        /// </summary>
        /// <param name="wordCount">Number of sentences</param>
        /// <param name="allMeat">Meat only or meat mixed with miscellaneous ‘lorem ipsum’ filler.</param>
        /// <param name="startWithLorem">Start the first paragraph with ‘Bacon ipsum dolor...’.</param>
        /// <returns></returns>
        public static List<string> GetWords(int wordCount, ResponseFormat format, bool startWithLorem = false, bool allMeat = false)
        {
            var text = GetParagraphs(2, format, startWithLorem, allMeat);

            var chars = text.ToCharArray();
            List<char> nonWordChars = new List<char> { '"', '.', '?', ';', ':', ',', '\n', ' ', '\0' };
            List<string> words = new List<string>();
            StringBuilder currWord = new StringBuilder();

            foreach (char c in chars)
            {
                if (nonWordChars.Contains(c))
                {
                    words.Add(currWord.ToString());

                    if (words.Count >= wordCount)
                        break;
                    else
                        currWord = new StringBuilder();
                }
                else
                {
                    currWord.Append(c);
                }

            }
            return words;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        static private string RequestIpsum(string url)
        {
            string retval = null;

            WebRequest wrURL = WebRequest.Create(url);
            Stream responseStream = wrURL.GetResponse().GetResponseStream();
            StreamReader streamReader = new StreamReader(responseStream);

            retval = streamReader.ReadToEnd();

            return retval;
        }

        /// <summary>
        /// Builds the url to request ipsum
        /// </summary>
        /// <param name="allMeat">Meat only or meat mixed with miscellaneous ‘lorem ipsum’ filler.</param>
        /// <param name="sentenceCount">Number of sentences (this overrides paragraphs)</param>
        /// <param name="paragraphCount">Number of paragraphs, defaults to 5.</param>
        /// <param name="loremStart">Start the first paragraph with ‘Bacon ipsum dolor...’.</param>
        /// <returns></returns>
        static private string GetApiURL(ResponseFormat format, bool allMeat, int sentenceCount = 0, int paragraphCount = 1, bool startWithLorem = true)
        {
            StringBuilder requestUrl = new StringBuilder();
            requestUrl.Append(BASE_BACON_API_URI);
            requestUrl.AppendFormat("?type={0}", allMeat ? "all-meat" : "meat-and-filler");

            if (sentenceCount > 0)
                requestUrl.AppendFormat("&sentences={0}", sentenceCount.ToString());
            else
                requestUrl.AppendFormat("&paras={0}", paragraphCount.ToString());

            if (!startWithLorem)
                requestUrl.Append("&start-with-lorem=0");

            requestUrl.AppendFormat("&format={0}", format.ToString());

            return requestUrl.ToString();
        }
    }
}
