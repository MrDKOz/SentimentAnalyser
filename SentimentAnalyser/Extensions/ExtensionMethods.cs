using Azure.AI.TextAnalytics;
using System.Collections.Generic;

namespace SentimentAnalyser.Extensions
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Takes the given double, and returns the appropriate sentiment.
        /// </summary>
        /// <param name="dbl">The compounded score.</param>
        /// <returns>The sentiment the double sat with.</returns>
        public static string Sentiment(this double dbl)
        {
            var result = TextSentiment.Positive;

            if (dbl > -0.05 && dbl < 0.05)
                result = TextSentiment.Neutral;
            else if (dbl <= -0.05)
                result = TextSentiment.Negative;

            return result.ToString();
        }

        private static readonly List<string> SupportedLanguages = new List<string>
        {
            "zh-hans", // Chinese (simplified)
            "zh",      // Chinese (simplified)
            "zh-hant", // Chinese (traditional)
            "en",      // English
            "fr",      // French
            "de",      // German
            "it",      // Italian
            "ja",      // Japanese
            "ko",      // Korean
            "no",      // Norwegian
            "pt",      // Portuguese
            "pt-PT",   // Portuguese
            "es",      // Spanish
            "tr"       // Turkish
        };

        /// <summary>
        /// Is the given language code supported?.
        /// </summary>
        /// <param name="str">The language code.</param>
        /// <returns>Whether the language is supported.</returns>
        public static bool SupportedLanguage(this string str)
        {
            return SupportedLanguages.Contains(str);
        }
    }
}
