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
using Newtonsoft.Json;
using GraphQL.Common.Response;
using smashgg_to_liquipedia.Smashgg.Schema;

namespace smashgg_to_liquipedia
{
    class ApiQueries
    {
        private GraphQLClient graphQLClient;
        System.Timers.Timer timer = new System.Timers.Timer(1000);

        #region Smash.gg Enumerations
        public enum ActivityState
        {
            CREATED,    // Activity is created
            ACTIVE,     // Activity is active or in progress
            COMPLETED,  // Activity is done
            READY,      // Activity is ready to be started
            INVALID,    // Activity is invalid
            CALLED,     // Activity, like a set, has been called to start
            QUEUED      // Activity is queued to run
        }

        public enum TournamentPaginationSort
        {
            startAt,
            endAt,
            eventRegistrationClosesAt
        }
        #endregion

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
        public async Task<GraphQLResponse> SendRequest(GraphQLRequest request)
        {
            var graphQLResponse = await graphQLClient.PostAsync(request);
            return (graphQLResponse.Data);
        }

        /// <summary>
        /// Gets a list of events from the tournament slug
        /// </summary>
        /// <param name="tournamentSlug">Tournament slug</param>
        /// <param name="errors">Errors</param>
        /// <returns>Tournament object with all events</returns>
        public Tournament GetEvents(string tournamentSlug, out string errors)
        {
            errors = string.Empty;

            GraphQLRequest eventsRequest = new GraphQLRequest
            {
                Query = @"
                query GetEvents($slug: String) {
                    tournament(slug: $slug) {
                        events {
                            id
                            name
                            numEntrants
                            type
                            state
                            phases {
                                id
                                name
                            }
                            phaseGroups {
                                id
                                displayIdentifier
                                state
                                phaseId
                                waveId
                            }
                            videogame{
                                name
                            }
                        }
                    }
                }",
                OperationName = "GetEvents",
                Variables = new
                {
                    slug = tournamentSlug
                }
            };

            GraphQLResponse response = graphQLClient.PostAsync(eventsRequest).Result;
            Tournament tournament = new Tournament();
            if  (response.Errors != null)
            {
                // Return the errors
                errors = response.Errors.ToString();
            }
            else
            {
                if (response.Data != null)
                {
                    // Parse the json response
                    tournament = JsonConvert.DeserializeObject<Tournament>(response.Data.tournament.ToString());
                    tournament.slug = tournamentSlug;

                    // For each event
                    for (int i = 0; i < tournament.events.Count; i++)
                    {
                        // For each phasegroup in the event
                        for (int j = 0; j < tournament.events[i].phaseGroups.Count; j++)
                        {
                            // Generate waves for the phasegroup if relevant
                            tournament.events[i].phaseGroups[j].GenerateWave();

                            // For each phase in the event
                            for (int k = 0; k < tournament.events[i].phases.Count; k++)
                            {
                                // Check if the phasegroup phase is equal to the selected phase and add it if true
                                if (tournament.events[i].phases[k].id == tournament.events[i].phaseGroups[j].phaseId)
                                {
                                    tournament.events[i].phases[k].phasegroups.Add(tournament.events[i].phaseGroups[j]);

                                    // If the phasegroup has a wave, add it to the wave dictionary
                                    if (tournament.events[i].phaseGroups[j].Wave != string.Empty && tournament.events[i].phases[k].waves.ContainsKey(tournament.events[i].phaseGroups[j].Wave))
                                    {
                                        tournament.events[i].phases[k].waves[tournament.events[i].phaseGroups[j].Wave].Add(tournament.events[i].phaseGroups[j]);
                                    }
                                    else if (tournament.events[i].phaseGroups[j].Wave != string.Empty)
                                    {
                                        tournament.events[i].phases[k].waves.Add(tournament.events[i].phaseGroups[j].Wave, new List<PhaseGroup>());
                                        tournament.events[i].phases[k].waves[tournament.events[i].phaseGroups[j].Wave].Add(tournament.events[i].phaseGroups[j]);
                                    }
                                    
                                    break;
                                }
                            }
                        }

                        // Sort the phasegroups into a logical order
                        
                        for (int j=0; j< tournament.events[i].phases.Count; j++)
                        {
                            // Sort the phasegroups within the waves
                            if (tournament.events[i].phases[j].waves.Count > 0)
                            {
                                for (int k = 0; k < tournament.events[i].phases[j].waves.Count; k++)
                                {
                                    tournament.events[i].phases[j].waves.ElementAt(k).Value.Sort((group1, group2) => group1.displayIdentifier.CompareTo(group2.displayIdentifier));
                                }
                            }

                            tournament.events[i].phases[j].phasegroups.Sort((group1,group2) => group1.displayIdentifier.CompareTo(group2.displayIdentifier));
                        }
                    }
                }
                else
                {
                    // Return the errors
                    errors = "Could not retrieve any data";
                }
            }

            return tournament;
        }

        public Tournament GetSets(int phaseGroupId, out string errors)
        {
            errors = string.Empty;

            GraphQLRequest eventsRequest = new GraphQLRequest
            {
                Query = @"
                query GetSets($phaseGroupId: Int) {
                    phaseGroup(id: $phaseGroupId) {
                        sets{
                          round
                          displayScore
                          winnerId                          
                          slots {
                            entrant {
                              id
                              name
                            }
                          }
                        }
                    }
                }",
                OperationName = "GetSets",
                Variables = new
                {
                    phaseGroupId = phaseGroupId
                }
            };

            GraphQLResponse response = graphQLClient.PostAsync(eventsRequest).Result;
            Tournament tournament = new Tournament();
            if (response.Errors != null)
            {
                // Return the errors
                errors = response.Errors.ToString();
            }
            else
            {
                if (response.Data != null)
                {
                    // Parse the json response
                    tournament = JsonConvert.DeserializeObject<Tournament>(response.Data.tournament.ToString());
                    tournament.slug = tournamentSlug;

                    // For each event
                    for (int i = 0; i < tournament.events.Count; i++)
                    {
                        // For each phasegroup in the event
                        for (int j = 0; j < tournament.events[i].phaseGroups.Count; j++)
                        {
                            // Generate waves for the phasegroup if relevant
                            tournament.events[i].phaseGroups[j].GenerateWave();

                            // For each phase in the event
                            for (int k = 0; k < tournament.events[i].phases.Count; k++)
                            {
                                // Check if the phasegroup phase is equal to the selected phase and add it if true
                                if (tournament.events[i].phases[k].id == tournament.events[i].phaseGroups[j].phaseId)
                                {
                                    tournament.events[i].phases[k].phasegroups.Add(tournament.events[i].phaseGroups[j]);

                                    // If the phasegroup has a wave, add it to the wave dictionary
                                    if (tournament.events[i].phaseGroups[j].Wave != string.Empty && tournament.events[i].phases[k].waves.ContainsKey(tournament.events[i].phaseGroups[j].Wave))
                                    {
                                        tournament.events[i].phases[k].waves[tournament.events[i].phaseGroups[j].Wave].Add(tournament.events[i].phaseGroups[j]);
                                    }
                                    else if (tournament.events[i].phaseGroups[j].Wave != string.Empty)
                                    {
                                        tournament.events[i].phases[k].waves.Add(tournament.events[i].phaseGroups[j].Wave, new List<PhaseGroup>());
                                        tournament.events[i].phases[k].waves[tournament.events[i].phaseGroups[j].Wave].Add(tournament.events[i].phaseGroups[j]);
                                    }

                                    break;
                                }
                            }
                        }

                        // Sort the phasegroups into a logical order

                        for (int j = 0; j < tournament.events[i].phases.Count; j++)
                        {
                            // Sort the phasegroups within the waves
                            if (tournament.events[i].phases[j].waves.Count > 0)
                            {
                                for (int k = 0; k < tournament.events[i].phases[j].waves.Count; k++)
                                {
                                    tournament.events[i].phases[j].waves.ElementAt(k).Value.Sort((group1, group2) => group1.displayIdentifier.CompareTo(group2.displayIdentifier));
                                }
                            }

                            tournament.events[i].phases[j].phasegroups.Sort((group1, group2) => group1.displayIdentifier.CompareTo(group2.displayIdentifier));
                        }
                    }
                }
                else
                {
                    // Return the errors
                    errors = "Could not retrieve any data";
                }
            }

            return tournament;
        }

        public List<Standing> GetStandings(int eventId, int lastPlacement, out string errors)
        {
            errors = string.Empty;

            GraphQLRequest eventsRequest = new GraphQLRequest
            {
                Query = @"
                query GetStandings($eventId: Int, $lastPlacement: Int) {
                    event(id: $eventId) {
                        name
                        standings(query: {
                            page: 1
                            perPage: $lastplacement
                        }) {
                            nodes{
                                standing
                                entrant {
                                    participants {
                                        playerId
                                        gamerTag
                                    }
                                }
                            }
                        }
                    }
                }",
                OperationName = "GetStandings",
                Variables = new
                {
                    eventId = eventId,
                    lastPlacement = lastPlacement
                }
            };

            GraphQLResponse response = graphQLClient.PostAsync(eventsRequest).Result;
            Event eventResults = new Event();
            if (response.Errors != null)
            {
                // Return the errors
                errors = response.Errors.ToString();
            }
            else
            {
                if (response.Data != null)
                {
                    // Parse the json response
                    eventResults = JsonConvert.DeserializeObject<Event>(response.Data.ToString());
                }
                else
                {
                    // Return the errors
                    errors = "Could not retrieve any data";
                }
            }

            return eventResults.standings;
        }
    }
}
