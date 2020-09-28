using SentimentAnalyser.Analysis;
using SentimentAnalyser.Analysis.Analysers;
using SentimentAnalyser.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SentimentAnalyser
{
    public class Analyse
    {
        private readonly IAnalyser _analyser;

        /// <summary>
        /// Sets up the analyser to we can get straight to using it.
        /// </summary>
        /// <param name="analyser">The analyser to setup for use.</param>
        /// <param name="azureConfig">(Optional) Azure configuration details.</param>
        public Analyse(Analyser analyser, AzureConfig azureConfig = null)
        {
            try
            {
                _analyser = analyser switch
                {
                    Analyser.Azure => new AzureTextAnalytics(azureConfig),
                    Analyser.Vader => new Vader()
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// Passes the data to to the configured analyser.
        /// </summary>
        /// <param name="input">A list of strings to analyse.</param>
        /// <returns>A list containing all of the results from the tests.</returns>
        public List<AnalysisResult> AnalyseInput(List<string> input)
        {
            var results = new List<AnalysisResult>();

            if (input.Any())
                results.AddRange(_analyser.Analyse(input));

            return results;
        }
    }
}
