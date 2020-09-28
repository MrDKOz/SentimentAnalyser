using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestConsole.Models;

namespace TestConsole.DataSources.Sources
{
    public class Voice : IDataSource
    {
        private readonly SpeechConfig _speechConfig;

        public Voice(VoiceConfig config)
        {
            _speechConfig = SpeechConfig.FromSubscription(config.ApiKey, config.Location);
        }

        public List<string> FetchData(int batchSize = 5)
        {
            string result;

            do
            {
                result = DetectSpeech().Result;
            } while (string.IsNullOrEmpty(result));

            return new List<string>
            {
                result
            };
        }

        private async Task<string> DetectSpeech()
        {
            Console.WriteLine("Start talking, detection stops when it detects silence...");

            var result = string.Empty;

            using var audioConfig = AudioConfig.FromDefaultMicrophoneInput();
            using var recogniser = new SpeechRecognizer(_speechConfig, audioConfig);

            var detection = await recogniser.RecognizeOnceAsync();

            switch (detection.Reason)
            {
                case ResultReason.RecognizedSpeech:
                    result = detection.Text;
                    break;
                case ResultReason.NoMatch:
                    Console.WriteLine("Failed to detect matches, are you speaking English?");
                    break;
            }

            return result;
        }

        public List<string> SearchData(string searchTerm, int batchSize = 5)
        {
            throw new NotImplementedException();
        }
    }
}
