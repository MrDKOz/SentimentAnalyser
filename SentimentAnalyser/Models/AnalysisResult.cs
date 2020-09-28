namespace SentimentAnalyser.Models
{
    public class AnalysisResult
    {
        public string Input { get; set; }
        public string Language { get; set; }
        public string Message { get; set; }
        public SentimentData SentimentData { get; set; }
    }

    public class SentimentData
    {
        public string Sentiment { get; set; }
        public double Positive { get; set; }
        public double Neutral { get; set; }
        public double Negative { get; set; }
    }
}