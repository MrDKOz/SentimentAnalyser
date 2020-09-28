using System;
using System.Collections.Generic;
using NUnit.Framework;
using SentimentAnalyser.Analysis;
using SentimentAnalyser.Analysis.Analysers;

namespace Tests
{
    public class VaderAnalysisTests
    {
        private IAnalyser _analyser;

        [SetUp]
        public void Setup()
        {
            _analyser = new Vader();
        }

        [Test]
        public void VaderAnalysis_GivenMessagesAreNegative()
        {
            var negativeMessages = new List<string>
            {
                "This was not what I hoped.",
                "I don't like this at all.",
                "Would not recommend, this is awful."
            };

            var results = _analyser.Analyse(negativeMessages);

            foreach (var result in results)
            {
                Assert.AreEqual(result.SentimentData.Sentiment, "Negative");
            }
        }

        [Test]
        public void VaderAnalysis_GivenMessagesAreNeutral()
        {
            var neutralMessages = new List<string>
            {
                "Hello, my name is bot.",
                "I bought one of these.",
                "I have got one of these."
            };

            var results = _analyser.Analyse(neutralMessages);

            foreach (var result in results)
            {
                Assert.AreEqual(result.SentimentData.Sentiment, "Neutral");
            }
        }

        [Test]
        public void VaderAnalysis_GivenMessagesArePositive()
        {
            var positiveMessages = new List<string>
            {
                "Would recommend! 5 stars.",
                "Love it, it's my favourite.",
                "I've told everyone to get one, it's amazing!"
            };

            var results = _analyser.Analyse(positiveMessages);

            foreach (var result in results)
            {
                Assert.AreEqual(result.SentimentData.Sentiment, "Positive");
            }
        }

        [Test]
        public void VaderAnalysis_LongMessagesAreProcessed()
        {
            bool processed;
            var longMessages = new List<string>
            {
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit. In erat nibh, semper ut feugiat ac, venenatis at ante. Nunc vestibulum, leo quis maximus rutrum, neque nisl mollis purus, eget pharetra augue arcu vel erat. Quisque eu leo sit amet risus sollicitudin pharetra id at nibh. Etiam fringilla pharetra commodo. Nam faucibus sagittis mi, et finibus odio gravida rhoncus. Suspendisse ut consequat ipsum. Nunc vel erat a neque bibendum commodo. Sed nibh neque, consequat vel ultricies nec, convallis a urna. Pellentesque porttitor diam nec ligula placerat luctus vitae sit amet sem. In hac habitasse platea dictumst. Ut imperdiet nunc elit, non ultrices tortor condimentum in. Nulla ut pretium sem. Vivamus quis mattis nisi, mollis commodo est. Pellentesque sed urna vel nunc malesuada ullamcorper. Aliquam lobortis eu ex eget ultricies. Mauris lectus orci, consectetur et nisi et, viverra placerat dolor.",
                "Maecenas justo lectus, ullamcorper et diam at, ultricies sollicitudin felis. Duis aliquam, leo a gravida cursus, ipsum mauris pharetra enim, in vestibulum tellus neque et est. Sed pulvinar ac ex at aliquet. Aenean placerat magna nec venenatis varius. Donec euismod congue mauris vitae fringilla. Donec ut tortor augue. Suspendisse facilisis augue nisi, in sollicitudin arcu volutpat id. In aliquet lacinia urna at consectetur. Donec eget massa nec elit porttitor porta. Integer rhoncus elementum magna. Ut eu elit vehicula, finibus justo at, tristique magna. Mauris rutrum vestibulum magna, a aliquam mauris vulputate iaculis. Praesent rutrum eleifend felis, nec imperdiet dui. Pellentesque commodo lacus a tellus eleifend, in finibus libero tempus. Pellentesque commodo, sapien varius pretium varius, velit nisi suscipit dolor, at mollis quam felis id sapien. Nulla venenatis iaculis dolor, sed bibendum nibh efficitur luctus.",
            };

            try
            {
                _ = _analyser.Analyse(longMessages);

                processed = true;
            }
            catch (Exception)
            {
                processed = false;
            }

            Assert.IsTrue(processed);
        }
    }
}