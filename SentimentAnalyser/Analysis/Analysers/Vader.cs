using SentimentAnalyser.Extensions;
using SentimentAnalyser.Models;
using System.Collections.Generic;
using VaderSharp;

namespace SentimentAnalyser.Analysis.Analysers
{
    /// <summary>
    /// Performs Sentiment analysis using the Vader Sentiment Analysis tool.
    /// https://github.com/codingupastorm/vadersharp
    ///
    /// This is a Lexicon based approach, and does not require an internet connection.
    /// </summary>
    public class Vader : IAnalyser
    {
        private readonly SentimentIntensityAnalyzer _analyser = new SentimentIntensityAnalyzer();

        /// <summary>
        /// Loop through the given data, analysing the input, and building
        /// a list of result object.
        /// </summary>
        /// <param name="input">The data to analyse.</param>
        /// <returns>
        /// A list of results after analysis has been completed.
        /// </returns>
        public List<AnalysisResult> Analyse(List<string> input)
        {
            var results = new List<AnalysisResult>(input.Count);

            foreach (var datum in input)
            {
                var sentiment = Sentiment(datum);

                results.Add(new AnalysisResult
                {
                    Input = datum,
                    Language = "Unsupported on Vader",
                    SentimentData = new SentimentData
                    {
                        Sentiment = sentiment.Compound.Sentiment(),
                        Positive = sentiment.Positive,
                        Neutral = sentiment.Neutral,
                        Negative = sentiment.Negative
                    }
                });
            }

            return results;
        }

        /// <summary>
        /// Calculates the Sentiment of the given string,
        /// and returns the polarity score.
        /// </summary>
        /// <param name="datum">The string to analyse</param>
        /// <returns>The results of the analysis.</returns>
        private SentimentAnalysisResults Sentiment(string datum)
        {
            return _analyser.PolarityScores(datum);
        }
    }
}