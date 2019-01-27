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
using smashgg_to_liquipedia.Smashgg;
using Esendex.TokenBucket;
using System.Threading;

namespace smashgg_to_liquipedia
{
    class ApiQueries
    {
        private GraphQLClient graphQLClient;
        PlayerDatabase playerdb;
        Standardization standardization;

        private ITokenBucket bucket;
        private readonly IFormMain form;

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
        /// <param name="form">Form interface for logging</param>
        /// <param name="playerdb">Player database object for standardizing player tags and flags</param>
        public ApiQueries(string token, PlayerDatabase playerdb, IFormMain form, out bool success)
        {
            standardization = new Smashgg.Standardization();
            this.playerdb = playerdb;
            this.form = form;

            bucket = TokenBuckets.Construct().WithCapacity(10).WithFixedIntervalRefillStrategy(10, TimeSpan.FromSeconds(10.5)).Build();

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

        public void SetToken(string token)
        {
            graphQLClient.DefaultRequestHeaders.Clear();
            graphQLClient.DefaultRequestHeaders.Add("Authorization", $"Bearer " + token);
        }

        public void SetEndpoint(string endpoint)
        {
            graphQLClient.EndPoint = new System.Uri(endpoint);
        }

        /// <summary>
        /// Sends an API request to smash.gg
        /// Variables should be attached to the request beforehand
        /// </summary>
        /// <param name="request">GraphQL request</param>
        /// <returns>JSON data</returns>
        public GraphQLResponse SendRequest(GraphQLRequest request)
        {
            int tries = 3;

            GraphQLResponse response;

            // Check for data
            while (tries > 0)
            {
                // Consume a token and send the api query
                bucket.Consume(1);
                response = graphQLClient.PostAsync(request).Result;

                // Write errors
                if (response.Errors != null)
                {
                    WriteLineToLog(response.Errors.ToString());
                }

                // Check for data
                if (response.Data != null)
                {
                    return response;
                }
                else
                {
                    WriteLineToLog("No data. Retrying...");
                    Thread.Sleep(2500);
                    tries--;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets a list of events from the tournament slug
        /// </summary>
        /// <param name="tournamentSlug">Tournament slug</param>
        /// <param name="errors">Errors</param>
        /// <returns>Tournament object with all events</returns>
        public Tournament GetEvents(string tournamentSlug)
        {
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

            
            GraphQLResponse response = SendRequest(eventsRequest);
            Tournament tournament = new Tournament();

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
                                tournament.events[i].phases[j].waves.ElementAt(k).Value.Sort((group1, group2) => group1.Number.CompareTo(group2.Number));
                            }
                        }

                        if (tournament.events[i].phases[j].phasegroups[0].identifierType == PhaseGroup.IdentiferType.WaveNumber)
                        {
                            tournament.events[i].phases[j].phasegroups = tournament.events[i].phases[j].phasegroups.OrderBy(q => q.Wave).ThenBy(q => q.Number).ToList();
                        }
                        else if (tournament.events[i].phases[j].phasegroups[0].identifierType == PhaseGroup.IdentiferType.NumberOnly)
                        {
                            tournament.events[i].phases[j].phasegroups = tournament.events[i].phases[j].phasegroups.OrderBy(q => q.Number).ToList();
                        }
                    }
                }
            }
            else
            {
                WriteLineToLog("Could not retrieve any data");
            }

            return tournament;
        }

        public void GetSets(int phaseGroupId, out List<Seed> seedList, out List<Set> setList)
        {
            seedList = new List<Seed>();
            setList = new List<Set>();

            int page = 1;
            GraphQLRequest setsRequest = new GraphQLRequest
            {
                Query = @"
                query GetSets($phaseGroupId: Int, $page: Int) {
                    phaseGroup(id: $phaseGroupId) {
                        paginatedSets(perPage:60, page:$page) {
                            pageInfo {
                                page
                                totalPages
                            }
                            nodes {
                                id
                                round
                                displayScore
                                winnerId
                                state
                                slots {
                                    entrant{
                                        id
                                    }
                                }
                            }
                        }
                    }
                }",
                OperationName = "GetSets"                
            };

            // Retrieve pages while there are more paginated sets
            PhaseGroup tempPhaseGroup = new PhaseGroup();
            do
            {
                setsRequest.Variables = new
                {
                    phaseGroupId = phaseGroupId,
                    page = page
                };

                GraphQLResponse setsResponse = SendRequest(setsRequest);
                if (setsResponse != null)
                {
                    // Parse the json response
                    tempPhaseGroup = JsonConvert.DeserializeObject<PhaseGroup>(setsResponse.Data.phaseGroup.ToString());
                    setList.AddRange(tempPhaseGroup.paginatedSets.nodes);

                    // Increase the page count
                    page++;
                }
                else
                {
                    WriteLineToLog("Could not retrieve any data");
                    return;
                }
            } while (page <= tempPhaseGroup.paginatedSets.pageInfo.totalPages);

            GraphQLRequest seedsRequest = new GraphQLRequest
            {
                Query = @"
                query GetSeeds($phaseGroupId: Int) {
                    phaseGroup(id: $phaseGroupId) {
    					seeds {
                          placement
                          entrant {
                            id
                            participants {
                              playerId
                              gamerTag
                              player {
                                country
                              }
                            }
                          }
                        }
                    }
                }",
                OperationName = "GetSeeds",
                Variables = new
                {
                    phaseGroupId = phaseGroupId
                }
            };

            GraphQLResponse seedsResponse = SendRequest(seedsRequest);
            if (seedsResponse != null)
            {
                // Parse the json response
                tempPhaseGroup = JsonConvert.DeserializeObject<PhaseGroup>(seedsResponse.Data.phaseGroup.ToString());
                seedList = tempPhaseGroup.seeds;

                // Standardize participant information
                foreach (Seed seed in seedList)
                {
                    ParticipantStandardization(seed.entrant.participants);
                }
            }
            else
            {
                WriteLineToLog("Could not retrieve any data");
            }
        }

        public List<Standing> GetStandings(int eventId, int lastPlacement)
        {
            GraphQLRequest standingsRequest = new GraphQLRequest
            {
                Query = @"
                query GetStandings($eventId: Int, $lastPlacement: Int) {
                    event(id: $eventId) {
                        name
                        standings(query: {
                            page: 1
                            perPage: $lastPlacement
                        }) {
                            nodes{
                                standing
                                entrant {
                                    id
                                    participants {
                                        playerId
                                        gamerTag
                                        player {
                                            country
                                        }
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

            GraphQLResponse response = SendRequest(standingsRequest);
            Event eventResults = new Event();
            if (response != null)
            {
                // Parse the json response
                eventResults = JsonConvert.DeserializeObject<Event>(response.Data.@event.ToString());
            }
            else
            {
                WriteLineToLog("Could not retrieve any data");
            }

            // Standardize participant information
            foreach (Standing ranking in eventResults.standings.nodes)
            {
                ParticipantStandardization(ranking.entrant.participants);
            }

            return eventResults.standings.nodes;
        }

        /// <summary>
        /// Standarize information in participant fields including tag (matched against smash.gg player ids) and country (abbreviated to 2 letter code)
        /// </summary>
        /// <param name="participants">List of participants</param>
        private void ParticipantStandardization(List<Participant> participants)
        {
            foreach (Participant participant in participants)
            {
                foreach (PlayerInfo info in playerdb.players)
                {
                    if (participant.playerId == info.smashggID || participant.player.id == info.smashggID)
                    {
                        participant.gamerTag = info.name;
                        participant.player.gamerTag = info.name;
                        participant.player.country = info.flag;
                        break;
                    }
                }

                participant.player.country = standardization.CountryAbbreviation(participant.player.country);

                if (participant.playerId != 0)
                {
                    participant.player.id = participant.playerId;
                }
                else if (participant.player.id != 0)
                {
                    participant.playerId = participant.player.id;
                }

                if (participant.gamerTag != string.Empty)
                {
                    participant.player.gamerTag = participant.gamerTag;
                }
                else if (participant.player.gamerTag != string.Empty)
                {
                    participant.gamerTag = participant.player.gamerTag;
                }
            }
        }

        
        private void WriteLineToLog(string input)
        {
            form.Log += input + "\r\n";
        }
    }
}
