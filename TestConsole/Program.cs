using Newtonsoft.Json;
using SentimentAnalyser;
using SentimentAnalyser.Analysis;
using SentimentAnalyser.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TestConsole.DataSources;
using TestConsole.DataSources.Sources;
using TestConsole.Models;

namespace TestConsole
{
    internal class Program
    {
        private static Analyse _analyser;
        private static IDataSource _dataSource;

        private static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;

            do
            {
                Console.Clear();
                Console.WriteLine("==============================");
                Console.WriteLine("|     Sentiment Analyser     |");
                Console.WriteLine("|  Welcome, wonderful judges |");
                Console.WriteLine("==============================");
                Console.WriteLine("");

                // Configure analyser
                AnalyserOptions();

                // Ascertain the source, and fetch data
                var data = SourceOptions();

                // Analyse the data
                HandleAnalysis(data);
            } while (GoAgain());
        }

        /// <summary>
        /// Displays the options of which Analyser to use.
        /// </summary>
        private static void AnalyserOptions()
        {
            Console.WriteLine("What method of analysis should be used?");
            Console.WriteLine("1) Azure Text Analytics");
            Console.WriteLine("2) VADER");

            ConfigureAnalyser(ChoiceHelper.MenuChoice(2));
        }

        /// <summary>
        /// Configures the chosen Analyser.
        /// </summary>
        /// <param name="choice">The choice of Analyser to use.</param>
        /// <returns>True/False as to whether a valid choice was made.</returns>
        private static void ConfigureAnalyser(int choice)
        {
            var chosenAnalyser = string.Empty;

            switch (choice)
            {
                case 1:
                    chosenAnalyser = "Azure Text Analytics";
                    var azureConfig = Configuration.FetchConfig<AzureConfig>();
                    _analyser = new Analyse(Analyser.Azure, azureConfig);
                    break;
                case 2:
                    chosenAnalyser = "Vader";
                    _analyser = new Analyse(Analyser.Vader);
                    break;
            }

            Console.WriteLine("");
            Console.WriteLine($"+ Using {chosenAnalyser} for analysis +");
            Console.WriteLine("");
        }

        /// <summary>
        /// Displays the options of where the data can come from.
        /// </summary>
        private static List<string> SourceOptions()
        {
            Console.WriteLine("Where will the data to be analysed come from?");
            Console.WriteLine("1) Manually entered message");
            Console.WriteLine("2) Sample Data");
            Console.WriteLine("3) Custom DataFile");
            Console.WriteLine("4) Twitter Query");
            Console.WriteLine("5) Speech (Default Mic)");

            return HandleSource(ChoiceHelper.MenuChoice(5));
        }

        /// <summary>
        /// Handles the source of the data, and operations relating to fetching the data.
        /// </summary>
        /// <param name="choice">The choice of source.</param>
        /// <returns>The records fetched from the chosen source.</returns>
        private static List<string> HandleSource(int choice)
        {
            var chosenSource = string.Empty;
            dynamic config;
            var inputs = new List<string>();

            switch (choice)
            {
                case 1: // Manually entered
                    chosenSource = "Manual Entry";

                    inputs.Add(ChoiceHelper.TextInput("Enter the message to be analysed and hit enter:"));
                    break;
                case 2: // Sample data
                    chosenSource = "Sample Data";

                    _dataSource = new TextFile("sample-data.zip");
                    
                    inputs.AddRange(_dataSource.FetchData(
                        ChoiceHelper.NumericalInput("How many records from the sample data should we analyse? [1-10,000]:", 1, 10000)));
                    break;
                case 3: // Custom data
                    chosenSource = "Custom Data";

                    _dataSource = new TextFile(ChoiceHelper.FilePathInput("File path of your text file (.txt file, 1 entry per line):"));

                    inputs.AddRange(_dataSource.FetchData(
                        ChoiceHelper.NumericalInput("How many records from the data should we analyse? [1-10,000]:", 1, 10000)));
                    break;
                case 4: // Twitter query
                    chosenSource = "Twitter";

                    config = Configuration.FetchConfig<TwitterConfig>();
                    _dataSource = new Twitter(config);

                    inputs.AddRange(_dataSource.SearchData(
                        ChoiceHelper.TextInput("Enter a search term to search for on Twitter:"),
                        ChoiceHelper.NumericalInput("How many Tweets should we fetch? [10-100]", 10, 100)));
                    break;
                case 5: // Speech (Default Mic)
                    chosenSource = "Speech";

                    config = Configuration.FetchConfig<VoiceConfig>();
                    _dataSource = new Voice(config);

                    inputs.AddRange(_dataSource.FetchData());
                    break;
            }

            Console.WriteLine("");
            Console.WriteLine($"+ Using {chosenSource} as a source for the data +");
            Console.WriteLine("");

            return inputs;
        }

        /// <summary>
        /// Handles the analysis of the given data.
        /// </summary>
        /// <param name="inputs">The inputs to analyse.</param>
        private static void HandleAnalysis(List<string> inputs)
        {
            Console.WriteLine("");
            Console.WriteLine($"+ Analysing {inputs.Count} entries, please wait... +");
            Console.WriteLine("");

            var results = _analyser.AnalyseInput(inputs);

            HandleOutput(results);
        }

        /// <summary>
        /// Handles the output of the results
        /// </summary>
        /// <param name="results">The results to output in the chosen form.</param>
        private static void HandleOutput(IReadOnlyCollection<AnalysisResult> results)
        {
            Console.WriteLine("Analysis Complete! How do you want to output your results?");
            Console.WriteLine("1) Console output");
            Console.WriteLine("2) JSON File (Sensible Option for large result sets)");

            var choice = ChoiceHelper.MenuChoice(2);

            switch (choice)
            {
                case 1:

                    Console.Clear();
                    Console.WriteLine("=====================================");
                    Console.WriteLine($"Displaying {results.Count} result(s)");
                    Console.WriteLine("=====================================");
                    Console.WriteLine("");

                    foreach (var result in results)
                    {
                        Console.WriteLine("==========");
                        Console.WriteLine($"Input:     {result.Input}");
                        Console.WriteLine($"Language:  {result.Language}");

                        Console.WriteLine(result.SentimentData == null
                            ? $"Message:   {result.Message}"
                            : $"Sentiment: {result.SentimentData.Sentiment} (+{result.SentimentData.Positive}, ={result.SentimentData.Neutral}, -{result.SentimentData.Negative})");

                        Console.WriteLine("==========");
                    }
                    break;
                case 2:
                    var outputDir = ChoiceHelper.DirectoryPath(@"Which directory should the file be saved to? (e.g. E:\Output):");

                    Directory.CreateDirectory(outputDir);

                    var json = JsonConvert.SerializeObject(results, Formatting.Indented);
                    var fileOutput = Path.Combine(outputDir, $"{DateTime.Now:yyyyMMddHHmmss}-SentimentOutput.json");
                    File.WriteAllText(fileOutput, json);

                    Console.Clear();
                    Console.WriteLine($"+ File written to: {fileOutput} +");

                    break;
            }
        }

        /// <summary>
        /// Shall we go for another run, or exit now?.
        /// </summary>
        /// <returns>True/False as to whether we should carry on.</returns>
        private static bool GoAgain()
        {
            Console.WriteLine("");
            Console.WriteLine("Want to do some more testing, or just exit?");
            Console.WriteLine("1) Let's go again");
            Console.WriteLine("2) Exit");

            return ChoiceHelper.MenuChoice(2) == 1;
        }
    }
}
