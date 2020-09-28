using Azure.AI.TextAnalytics;
using SentimentAnalyser.Models;
using System.Collections.Generic;
using Azure;
using SentimentAnalyser.Extensions;

namespace SentimentAnalyser.Analysis.Analysers
{
    /// <summary>
    /// Performs Sentiment analysis using Microsoft Azures Text Analytics tool.
    /// https://azure.microsoft.com/en-gb/services/cognitive-services/text-analytics/
    ///
    /// This uses AI and Models created for the purpose of Sentiment analysis that are
    /// hosted on Microsoft's Azure Platform, this requires and internet connection.
    /// </summary>
    public class AzureTextAnalytics : IAnalyser
    {
        private readonly TextAnalyticsClient _client;

        public AzureTextAnalytics(AzureConfig config)
        {
            _client = new TextAnalyticsClient(config.Endpoint, new AzureKeyCredential(config.Credential));
        }

        /// <summary>
        /// Analyse the given data.
        /// </summary>
        /// <param name="input">The data to analyse.</param>
        /// <returns>
        /// A list of results after analysis has been completed.
        /// </returns>
        public List<AnalysisResult> Analyse(List<string> input)
        {
            var results = new List<AnalysisResult>();

            foreach (var datum in input)
            {
                var language = Language(datum);
                var sentiment = Sentiment(datum, language.Iso6391Name);

                var result = new AnalysisResult
                {
                    Input = datum,
                    Language = language.Name
                };

                if (sentiment != null)
                    result.SentimentData = new SentimentData
                    {
                        Sentiment = sentiment.Sentiment.ToString(),
                        Positive = sentiment.ConfidenceScores.Positive,
                        Neutral = sentiment.ConfidenceScores.Neutral,
                        Negative = sentiment.ConfidenceScores.Negative
                    };
                else
                    result.Message = "Sentiment was not analysed, as the given string was in an unsupported language.";

                results.Add(result);
            }

            return results;
        }

        /// <summary>
        /// Detects the language of the given input, if we fetch a supported language
        /// we can provide this to the sentiment analysis to help it along.
        /// </summary>
        /// <param name="input">The string to determine the language of.</param>
        /// <returns>The details of the detected language.</returns>
        public DetectedLanguage Language(string input)
        {
            return _client.DetectLanguage(input);
        }

        /// <summary>
        /// Uses the black magic of Azure to calculate the sentiment of the given string.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="languageHint">A hint as to what language the string is in.</param>
        /// <returns>The results of the Sentiment analysis.</returns>
        public DocumentSentiment Sentiment(string input, string languageHint)
        {
            DocumentSentiment result = null;

            // If we don't have the supported language, the analysis won't be accurate. Skip it.
            if (languageHint.SupportedLanguage())
                result = _client.AnalyzeSentiment(input, languageHint).Value;

            return result;
        }
    }
}