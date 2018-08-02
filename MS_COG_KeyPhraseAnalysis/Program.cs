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
            Console.WriteLine("===== KEY-PHRASE EXTRACTION ======");
            Console.WriteLine("Type 'quit' to exit application");

            string text;

            do
            {
                Console.WriteLine("\nText to Analyze:");
                text = Console.ReadLine();

                KeyPhraseBatchResult result2 = client.KeyPhrasesAsync(new MultiLanguageBatchInput(
                            new List<MultiLanguageInput>()
                            {
                          new MultiLanguageInput("en", "3", text),
                            })).Result;

                // Printing keyphrases
                foreach (var document in result2.Documents)
                {
                    Console.WriteLine("\n Key phrases:");

                    foreach (string keyphrase in document.KeyPhrases)
                    {
                        Console.WriteLine("\t " + keyphrase);
                    }

                    Console.WriteLine("\n===End of Result===");
                }

            } while (text != "quit");
        }
    }
}