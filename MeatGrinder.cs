using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace BICSL
{
    public enum ResponseFormat
    {
        Json,
        Html,
        Text
    }

    public static class MeatGrinder
    {
        const string BaseBaconApiUri = "https://baconipsum.com/api/";

        public static string GetParagraphs(int paragraphCount, ResponseFormat format, bool startWithLorem = false, bool allMeat = false)
        {
            var url = GetApiUrl(format, allMeat, paragraphCount: paragraphCount, startWithLorem: startWithLorem);
            return RequestIpsum(url);
        }

        /// <summary>
        /// Get sentences
        /// </summary>
        /// <param name="sentenceCount">Number of sentences</param>
        /// <param name="format">The format.</param>
        /// <param name="startWithLorem">Start the first paragraph with 'Bacon ipsum dolor...'.</param>
        /// <param name="allMeat">Meat only or meat mixed with miscellaneous "lorem ipsum" filler.</param>
        /// <returns>System.String.</returns>
        public static string GetSentences(int sentenceCount, ResponseFormat format, bool startWithLorem = false, bool allMeat = false)
        {
            var sentences = GetApiUrl(format, allMeat, sentenceCount: sentenceCount, startWithLorem: startWithLorem);
            return RequestIpsum(sentences);
        }

        /// <summary>
        /// Get sentences
        /// </summary>
        /// <param name="wordCount">Number of sentences</param>
        /// <param name="format">The format.</param>
        /// <param name="startWithLorem">Start the first paragraph with 'Bacon ipsum dolor...'.</param>
        /// <param name="allMeat">Meat only or meat mixed with miscellaneous 'lorem ipsum' filler.</param>
        /// <returns>List&lt;System.String&gt;.</returns>
        public static List<string> GetWords(int wordCount, ResponseFormat format, bool startWithLorem = false, bool allMeat = false)
        {
            var text = GetParagraphs(2, format, startWithLorem, allMeat);

            var chars = text.ToCharArray();
            var nonWordChars = new List<char> { '"', '.', '?', ';', ':', ',', '\n', ' ', '\0' };
            var words = new List<string>();
            var currWord = new StringBuilder();

            foreach (var c in chars)
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

        private static string RequestIpsum(string url)
        {
            var wrUrl = WebRequest.Create(url);
            var responseStream = wrUrl.GetResponse().GetResponseStream();
            if (responseStream != null){
                var streamReader = new StreamReader(responseStream);

                var retval = streamReader.ReadToEnd();

                return retval;
            }
            return string.Empty;
        }

        /// <summary>
        /// Builds the URL to request ipsum
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="allMeat">Meat only or meat mixed with miscellaneous 'lorem ipsum' filler.</param>
        /// <param name="sentenceCount">Number of sentences (this overrides paragraphs)</param>
        /// <param name="paragraphCount">Number of paragraphs, defaults to 5.</param>
        /// <param name="startWithLorem">if set to <c>true</c> [start with lorem].</param>
        /// <returns>System.String.</returns>
        private static string GetApiUrl(ResponseFormat format, bool allMeat, int sentenceCount = 0, int paragraphCount = 1, bool startWithLorem = true)
        {
            var requestUrl = new StringBuilder();
            requestUrl.Append(BaseBaconApiUri);
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
