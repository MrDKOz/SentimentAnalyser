using System.Collections.Generic;
using SentimentAnalyser.Models;

namespace SentimentAnalyser.Analysis
{
    public interface IAnalyser
    {
        /// <summary>
        /// Analyse the given data.
        /// </summary>
        /// <param name="input">The data to analyse.</param>
        /// <returns>A list of results after analysis has been completed.</returns>
        List<AnalysisResult> Analyse(List<string> input);
    }
}