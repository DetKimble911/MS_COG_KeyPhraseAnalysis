using System;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics.Models;
using System.Collections.Generic;
using Microsoft.Rest;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        /// <summary>
        /// Container for subscription credentials. Make sure to enter your valid key.
        /// </summary>
        class ApiKeyServiceClientCredentials : ServiceClientCredentials
        {
            public override Task ProcessHttpRequestAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                request.Headers.Add("Ocp-Apim-Subscription-Key", "b83e2504e983497e80bf44e7f9f8cf16");
                return base.ProcessHttpRequestAsync(request, cancellationToken);
            }
        }

        static void Main(string[] args)
        {

            // Create a client.
            TextAnalyticsAPI client = new TextAnalyticsAPI(new ApiKeyServiceClientCredentials())
            {
                AzureRegion = AzureRegions.Eastus
            };

            Console.OutputEncoding = System.Text.Encoding.UTF8;


            // Getting key-phrases
            Console.WriteLine("===== MS_CA_TEXT_ANALYSIS ======");
            Console.WriteLine("==== KEY PHRASE & SENTIMENT ====");
            Console.WriteLine("Type 'quit' to exit application");

            string text;

            do
            {
                Console.WriteLine("\nText to Analyze:");
                text = Console.ReadLine();

                //** GATHER **
                //Key Phrase(s)
                KeyPhraseBatchResult result2 = client.KeyPhrasesAsync(new MultiLanguageBatchInput(
                            new List<MultiLanguageInput>()
                            {
                          new MultiLanguageInput("en", "3", text),
                            })).Result;

                //Sentiment
                SentimentBatchResult result3 = client.SentimentAsync(new MultiLanguageBatchInput(
                            new List<MultiLanguageInput>()
                            {
                          new MultiLanguageInput("en", "3", text),
                            })).Result;

                //** PRINT **
                //Key Phrase(s)
                foreach (var document in result3.Documents)
                {
                    Console.WriteLine("\n Sentiment: ");
                    Console.WriteLine("\t " + Math.Round(document.Score.Value, 3, MidpointRounding.AwayFromZero));
                }

                //Sentiment
                foreach (var document in result2.Documents)
                {
                     Console.WriteLine("\n Key Phrases:");
                     foreach (string keyphrase in document.KeyPhrases)
                     {
                        Console.WriteLine("\t " + keyphrase);
                     }
                } 

            } while (text != "quit");
        }
    }
}