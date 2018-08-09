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

            Console.WriteLine("===== MS_CA_TEXT_ANALYSIS ======");
            Console.WriteLine("==== KEY PHRASE & SENTIMENT ====");
            Console.WriteLine("Type 'quit' to exit application");

            string text;

            do
            {
                Console.WriteLine("\nText to Analyze:");
                text = Console.ReadLine();

               //** GATHER (POST)
                //Key Phrase(s)
                KeyPhraseBatchResult result_k = client.KeyPhrasesAsync(new MultiLanguageBatchInput(
                            new List<MultiLanguageInput>()
                            {
                          new MultiLanguageInput("en", "3", text),
                            })).Result;

                //Sentiment
                SentimentBatchResult result_s = client.SentimentAsync(new MultiLanguageBatchInput(
                            new List<MultiLanguageInput>()
                            {
                          new MultiLanguageInput("en", "3", text),
                            })).Result;

                //Entities
                EntitiesBatchResult result_e = client.EntitiesAsync(new MultiLanguageBatchInput(
                            new List<MultiLanguageInput>()
                            {
                          new MultiLanguageInput("en", "3", text),
                            })).Result;

               //** PRINT (RESPONSE)
                //Sentiment
                foreach (var document in result_s.Documents)
                {
                    Console.WriteLine("\n Sentiment: ");
                    Console.WriteLine("\t " + Math.Round(document.Score.Value, 3, MidpointRounding.AwayFromZero));
                }

                //Key Phrase(s) S
                foreach (var document in result_k.Documents)
                {
                     Console.WriteLine("\n Key Phrases:");
                     foreach (string keyphrase in document.KeyPhrases)
                     {
                        Console.WriteLine("\t " + keyphrase);
                     }
                }

                //Entities
                foreach (var document in result_e.Documents)
                {
                    Console.WriteLine("\n Entities:");
                    foreach (var entity in document.Entities)
                    {
                        Console.WriteLine("\t " + entity.Name + " (" + entity.WikipediaUrl + ")" );
                    }
                }

            } while (text != "quit");
        }
    }
}