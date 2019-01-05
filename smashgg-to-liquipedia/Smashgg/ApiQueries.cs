using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphQL.Common.Request;
using GraphQL.Client;
using System.Net.Http.Headers;
using System.Xml;
using Newtonsoft.Json.Linq;

namespace smashgg_to_liquipedia
{
    class ApiQueries
    {
        GraphQLClient graphQLClient;

        // Variables are not yet set in these requests
        GraphQLRequest eventsRequest = new GraphQLRequest
        {
            Query = @"
                query GetEvents($slug: String) {
                    tournament(slug: $slug) {
                        events {
                            id
                            name
                            numEntrants
                            videogame{
                                name
                            }
                        }
                    }
                }",
            OperationName = "GetEvents"
        };

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="token">API token</param>
        /// <param name="success">Indicates if the contructor initialized successfully</param>
        public ApiQueries(string token, out bool success)
        {
            // Get the endpoint
            string endpoint = string.Empty;
            using (XmlReader reader = XmlReader.Create("Configuration.xml"))
            {
                reader.MoveToContent();
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            if (reader.Name == "endpoint")
                            {
                                reader.Read();
                                endpoint = reader.Value;
                            }
                            break;
                    }
                }
            }
            if (endpoint == string.Empty)
            {
                success = false;
                return;
            }

            // Make a new client, set the endpoint, set the token
            graphQLClient = new GraphQLClient(endpoint);
            graphQLClient.DefaultRequestHeaders.Add("Authorization", $"Bearer " + token);

            // Constructor finished succesfully
            success = true;
        }
        
        /// <summary>
        /// Sends an API request to smash.gg
        /// Variables should be attached to the request beforehand
        /// </summary>
        /// <param name="tournamentSlug">Tournament slug</param>
        /// <param name="request">GraphQL request</param>
        /// <returns>JSON data</returns>
        public async Task<dynamic> SendRequest(string tournamentSlug, GraphQLRequest request)
        {
            eventsRequest.Variables = new { slug = tournamentSlug };
            var graphQLResponse = await graphQLClient.PostAsync(eventsRequest);

            return (graphQLResponse.Data);
        }
    }
}
