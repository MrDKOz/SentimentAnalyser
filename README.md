# Hackathon

This project allows you to take written data, and then calculate the Sentiment, providing useful metrics to prioritise the given data as is needed. This was written for a Hackathon I did at my place of work based on a brief of providing a prototype utility that could take in text, analyse the sentiment, and output the sentiment of the given data.

This project is written in 2 parts, one part DLL that handles all of the analysis and returning of data, and then a "dumb" console app, this means you could take the DLL project, and throw it as is into another application and use it with very little work needed.  

## Setup

1. Download the repo

2. Rebuild (to grab the NuGet packages)

3. You're good to go!

## Features

* Choose between 2 methods of Sentiment Analysis:

*  [Azure Text Analytics](https://azure.microsoft.com/en-gb/services/cognitive-services/text-analytics/)* - Utilising the AI and Models built and updated by Microsoft, this is the most accurate of the two methods available. It does however come with a cost (depending on how many requests you are performing).

*  [VADER](https://github.com/codingupastorm/vadersharp) - A lexicon based Sentiment analyser, a fork of the original Python library, this is a purely offline analyser, while it is quicker, free, and doesn't require an external connection, it is not as accurate as the Azure method.

* Easy to Add More - The Analysers are easy to add or remove without breaking the entire thing.

* Choose your data source:

* Manually Enter a message - You can provide your own message for the utility to analyse

* Sample Data - The utility comes with an zipped dump of 10k Amazon reviews, you can either process the entire file, or process a subset.

* Custom Data - Run the analysis against your own data, files should be txt format, with one "message" per row.

* Twitter* - For a "random" set of data, you can utilise the Twitter integration, provide a search query, and the number of Tweets you wish to get, and it'll fetch relevant Tweets to your search query, this is useful for getting data that may be more positive/negative e.g. searching "complaint" will get a lot of negative Tweets, whereas "birthday" will fetch more positive Tweets.

* Voice (Default Mic)* - Once chosen, you will be prompted to speak, say as much as you wish, once you finish speaking it will save your response.

* As with the Analysers, adding or removing data sources is also easy and won't impact other parts of the utility


## Additional Configuration

  **Analysers**

* [Azure Text Analytics](https://azure.microsoft.com/en-gb/services/cognitive-services/text-analytics/) - To use this method to analyse the given input you will need to configure a Text Analytics Service on Azure ([documentation here](https://docs.microsoft.com/en-gb/azure/cognitive-services/text-analytics/)).
	* Once configured on Azure, be sure to enter the following details into the "AzureConfig.json" configuration file (these values can be found under the "Keys and Endpoint" section of your new Azure service):
		* **Credential** - This should the API key given to you via the Azure Portal
		* **Endpoint** - This should be the "Endpoint" given to you via the Azure Portal

**Input Sources**

* Twitter - To use this as your input source (allowing the app to search for Tweets and return them for analysis), you will need a Twitter Development account ([here](https://developer.twitter.com/en/apply-for-access)). Create a new app and then enter the following details into the "TwitterConfig.json" file:
	* **ApiKey** - This should be the API key given to you by Twitter
	* **EndPoint** - Leave as default unless you want mess around in the code and change the source
		* *Default - https://api.twitter.com/2/tweets/search/recent*

* Voice (Default Mic) - To use this as your input source, you must have a microphone attached to the device you're running the app on. You will also need to configure a Speech-To-Text service on Azure ([documentation here](https://docs.microsoft.com/en-us/azure/cognitive-services/speech-service/)).
	* Once configured on Azure, be sure to enter the following details into the "VoiceConfig.json" configuration file (these values can be found under the "Keys and Endpoint" section of your new Azure service):
		* **ApiKey** - This should be the API key given to you via the Azure Portal
		* **Location** - This should be the location value given to you via the Azure Portal