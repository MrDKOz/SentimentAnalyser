using Flurl.Http;
using System.Collections.Generic;
using System.Linq;
using TestConsole.Models;

namespace TestConsole.DataSources.Sources
{
    public class Twitter : IDataSource
    {
        private readonly string _endPoint;
        private readonly string _apiKey;

        public Twitter(TwitterConfig config)
        {
            _endPoint = config.Endpoint;
            _apiKey = config.ApiKey;
        }

        public List<string> FetchData(int batchSize = 5)
        {
            return SearchData("uk", batchSize);
        }

        public List<string> SearchData(string searchTerm, int batchSize = 5)
        {
            var returnData = new List<string>(batchSize);

            var tweets = _endPoint
                .WithOAuthBearerToken(_apiKey)
                .SetQueryParam("query", searchTerm)
                .SetQueryParam("max_results", batchSize)
                .GetJsonAsync<Tweet>().Result;

            returnData.AddRange(tweets.data.Select(t => t.text).ToList());

            return returnData;
        }
    }
}
