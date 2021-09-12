using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphQL.Common.Request;
using GraphQL.Client;
using System.Net;
using System.Xml;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using GraphQL.Common.Response;
using smashgg_to_liquipedia.Smashgg.Schema;
using smashgg_to_liquipedia.Smashgg;
using Esendex.TokenBucket;
using System.IO;

namespace smashgg_to_liquipedia
{
    class ApiQueries
    {
        private GraphQLClient graphQLClient;
        PlayerDatabase playerdb;
        Standardization standardization;

        private ITokenBucket bucket;
        private readonly IFormMain form;

        private const int PER_PAGE_SETS = 60;
        private const int PER_PAGE_SETS_DETAILED = 30;
        private const int PER_PAGE_ENTRANTS = 200;

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

            bucket = TokenBuckets.Construct().WithCapacity(50).WithFixedIntervalRefillStrategy(20, TimeSpan.FromSeconds(60)).Build();

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
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            

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
        private GraphQLResponse SendRequest(GraphQLRequest request)
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
                    foreach (var error in response.Errors)
                    {
                        WriteLineToLog(error.Message + "/" + error.Locations);
                    }
                }

                // Check for data
                if (response.Data != null)
                {
                    return response;
                }
                else
                {
                    WriteLineToLog("No data. Retrying...");
                    Task.Delay(5000).Wait();
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
                                phaseGroups(query: {
                                  page: 1
                                  perPage: 256
                                }) {
                                    nodes {
                                        id
                                        displayIdentifier
                                        state
                                        wave {
                                            id
                                            identifier
                                        }
                                    }
                                }
                            }
                            videogame{
                                id
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

            if (response != null && response.Data != null)
            {
                // Parse the json response
                tournament = JsonConvert.DeserializeObject<Tournament>(response.Data.tournament.ToString());
                tournament.slug = tournamentSlug;

                // For each event
                for (int i = 0; i < tournament.events.Count; i++)
                {
                    // For each phase in the event
                    for (int j = 0; j < tournament.events[i].phases.Count; j++)
                    {
                        // Only do additional work on phases that contain more than one phasegroup
                        if (tournament.events[i].phases[j].phasegroups.nodes.Count > 1)
                        {
                            // For each phasegroup in the phase
                            for (int k = 0; k < tournament.events[i].phases[j].phasegroups.nodes.Count; k++)
                            {
                                var currentPhaseGroup = tournament.events[i].phases[j].phasegroups.nodes[k];

                                // Generate waves for the phasegroup if relevant
                                currentPhaseGroup.GenerateWave();

                                // If the phasegroup has a wave, add it to the wave dictionary
                                if (currentPhaseGroup.WaveLetter != string.Empty && tournament.events[i].phases[j].waves.ContainsKey(currentPhaseGroup.WaveLetter))
                                {
                                    tournament.events[i].phases[j].waves[currentPhaseGroup.WaveLetter].Add(currentPhaseGroup);
                                }
                                else if (currentPhaseGroup.WaveLetter != string.Empty)
                                {
                                    tournament.events[i].phases[j].waves.Add(currentPhaseGroup.WaveLetter, new List<PhaseGroup>());
                                    tournament.events[i].phases[j].waves[currentPhaseGroup.WaveLetter].Add(currentPhaseGroup);
                                }
                                else
                                {
                                    break;
                                }
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

                        if (tournament.events[i].phases[j].phasegroups.nodes[0].identifierType == PhaseGroup.IdentiferType.WaveNumber)
                        {
                            tournament.events[i].phases[j].phasegroups.nodes = tournament.events[i].phases[j].phasegroups.nodes.OrderBy(q => q.WaveLetter).ThenBy(q => q.Number).ToList();
                        }
                        else if (tournament.events[i].phases[j].phasegroups.nodes[0].identifierType == PhaseGroup.IdentiferType.NumberOnly)
                        {
                            tournament.events[i].phases[j].phasegroups.nodes = tournament.events[i].phases[j].phasegroups.nodes.OrderBy(q => q.Number).ToList();
                        }
                    }
                }
            }
            else
            {
                WriteLineToLog("Could not retrieve any data. Check your token.");
            }

            return tournament;
        }

        public bool GetSets(int phaseGroupId, out List<Set> setList, bool includeDetails)
        {
            setList = new List<Set>();

            int page = 1;
            string setsWithoutDetails = @"
                query GetSets($phaseGroupId: ID!, $page: Int, $perPage: Int) {
                    phaseGroup(id: $phaseGroupId) {
                        sets(perPage:$perPage, page:$page, filters: {
                            showByes:true
                        }) {
                            pageInfo {
                                total
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
                }";

            string setsWithDetails = @"
                query GetSetsWithDetails($phaseGroupId: ID!, $page: Int, $perPage: Int) {
                    phaseGroup(id: $phaseGroupId) {
                        sets(perPage:$perPage, page:$page, filters: {
                            showByes:true
                        }) {
                            pageInfo {
                                total
                                totalPages
                            }
                            nodes {
                                id
                                round
                                displayScore
                                winnerId
                                state
                                wPlacement
                                lPlacement
                                slots {
                                    slotIndex
                                    entrant{
                                        id
                                    }
                                }
                                games {
                                    orderNum
                                    stage {
                                        id
                                    }
                                    winnerId
                                    selections {
                                        entrant{
                                            id
                                        }
                                        selectionType
                                        selectionValue
                                    }
                                }
                            }
                        }
                    }
                }";

            GraphQLRequest setsRequest = new GraphQLRequest();

            // Retrieve pages while there are more paginated sets
            PhaseGroup tempPhaseGroup = new PhaseGroup();
            do
            {
                if (includeDetails)
                {
                    setsRequest.Query = setsWithDetails;
                    setsRequest.OperationName = "GetSetsWithDetails";
                    setsRequest.Variables = new
                    {
                        phaseGroupId = phaseGroupId,
                        page = page,
                        perPage = PER_PAGE_SETS_DETAILED
                    };
                }
                else
                {
                    setsRequest.Query = setsWithoutDetails;
                    setsRequest.OperationName = "GetSets";
                    setsRequest.Variables = new
                    {
                        phaseGroupId = phaseGroupId,
                        page = page,
                        perPage = PER_PAGE_SETS
                    };
                }

                GraphQLResponse setsResponse = SendRequest(setsRequest);
                if (setsResponse != null)
                {
                    // Parse the json response
                    tempPhaseGroup = JsonConvert.DeserializeObject<PhaseGroup>(setsResponse.Data.phaseGroup.ToString());
                    setList.AddRange(tempPhaseGroup.sets.nodes);
                    WriteLineToLog("Got sets for " + phaseGroupId + " (pg " + page + " of " + tempPhaseGroup.sets.pageInfo.totalPages + ")");

                    // Increase the page count
                    page++;
                }
                else
                {
                    WriteLineToLog("Could not retrieve any data");
                    return false;
                }
            } while (page <= tempPhaseGroup.sets.pageInfo.totalPages);

            return true;
        }

        /// <summary>
        /// Gets entrants
        /// </summary>
        /// <param name="eventId">ID of the event to query</param>
        /// <param name="entrants">Entrants object for the event</param>
        public void GetEventEntrants(int eventId, out Dictionary<int,Entrant> entrants)
        {
            // Will contain the full entrants list
            List<Entrant> entrantList = new List<Entrant>();

            // First check to see if we have the data stored on disk
            string path = Path.Combine("EventEntrants", eventId.ToString() + ".txt");
            if (File.Exists(path))
            {
                // Determine if file is old
                DateTime creation = File.GetCreationTime(path);
                if (DateTime.Compare(creation, DateTime.Now.AddDays(-2)) < 0)
                {
                    // Delete file if it's old
                    File.Delete(path);
                }
                else
                {
                    // Read entrants from file
                    entrantList = FileIO.ReadFromJsonFile<List<Entrant>>(path);
                    WriteLineToLog("Read entrants from file");
                }
            }
            
            if (entrantList.Count == 0)
            {
                int page = 1;
                GraphQLRequest entrantsRequest = new GraphQLRequest
                {
                    Query = @"
                        query GetEventEntrants($eventId: ID!, $perPage: Int, $page: Int) {
                            event(id: $eventId) {
                                entrants (query:{perPage: $perPage, page: $page}) {
                                    pageInfo {
                                        total
                                        totalPages
                                    }
                                    nodes {
                                        id
                                        participants {
                                            gamerTag
                                            player {
                                              	id
                                            }
                                          	user {
                                              location {
                                                country
                                              }
                                            }
                                        }
                                    }
                                }
                            }
                        }",
                    OperationName = "GetEventEntrants"
                };

                // Retrieve pages while there are more paginated sets
                Event tempEvent = new Event();
                do
                {
                    entrantsRequest.Variables = new
                    {
                        eventId = eventId,
                        page = page,
                        perPage = PER_PAGE_ENTRANTS
                    };

                    GraphQLResponse entrantsResponse = SendRequest(entrantsRequest);
                    if (entrantsResponse != null)
                    {
                        // Parse the json response
                        tempEvent = JsonConvert.DeserializeObject<Event>(entrantsResponse.Data["event"].ToString());
                        entrantList.AddRange(tempEvent.entrants.nodes);
                        WriteLineToLog("Read page " + page + " of entrants");

                        // Increase the page count
                        page++;
                    }
                    else
                    {
                        WriteLineToLog("Could not retrieve any entrant data");
                        entrants = new Dictionary<int, Entrant>();
                        return;
                    }
                } while (page <= tempEvent.entrants.pageInfo.totalPages);

                // Save entrants to file
                if (entrantList.Count > 0)
                {
                    Directory.CreateDirectory(@"EventEntrants");
                    FileIO.WriteToJsonFile<List<Entrant>>(@"EventEntrants\" + eventId + @".txt", entrantList, false);
                }
            }

            // Convert country to abbreviations
            for (int i = 0; i < entrantList.Count; i++)
            {
                Standardization standards = new Standardization();
                for (int j = 0; j < entrantList[i].participants.Count; j++)
                {
                    entrantList[i].participants[j].user.location.country = standards.CountryAbbreviation(entrantList[i].participants[j].user.location.country);
                }
            }

            // Reformat list into dictionary
            entrants = entrantList.ToDictionary(x => x.id, x => x);
        }

        public List<Standing> GetStandings(int eventId, int lastPlacement)
        {
            GraphQLRequest standingsRequest = new GraphQLRequest
            {
                Query = @"
                    query GetStandings($eventId: ID, $lastPlacement: Int) {
                        event(id: $eventId) {
                            name
                            standings(query: {
                                page: 1
                                perPage: $lastPlacement
                            }) {
                                nodes{
                                  	stats {
                                      	score {
                                          	label
                                          	value
                                        }
                                    }
                                    placement
                                    entrant {
                                        id
                                        participants {
                                            gamerTag
                                            player {
                                              	id
                                            }
                                          	user {
                                              	location {
                                                  	country
                                                }
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

        public List<Seed> GetSeedStandings(int phaseGroupId)
        {
            GraphQLRequest pgStandingsRequest = new GraphQLRequest
            {
                Query = @"
                    query GetPhaseGroupStandings($phaseGroupId: ID) {
                        phaseGroup(id: $phaseGroupId) {
                            seeds(query: {
                                page: 1
                                perPage: 100
                            }) {
                                nodes{
                                  	entrant {
                                      	id
                                        name
                                    }
                                    placement
                                }
                            }
                        }
                    }",
                OperationName = "GetPhaseGroupStandings",
                Variables = new
                {
                    phaseGroupId = phaseGroupId
                }
            };

            GraphQLResponse response = SendRequest(pgStandingsRequest);
            PhaseGroup phaseGroupResults = new PhaseGroup();
            if (response != null)
            {
                // Parse the json response
                phaseGroupResults = JsonConvert.DeserializeObject<PhaseGroup>(response.Data.@phaseGroup.ToString());
            }
            else
            {
                WriteLineToLog("Could not retrieve any data");
            }

            // Standardize participant information
            foreach (Seed seed in phaseGroupResults.seeds.nodes)
            {
                if (seed.entrant != null)
                {
                    ParticipantStandardization(seed.entrant.participants);
                }
            }

            return phaseGroupResults.seeds.nodes;
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
                    if (participant.player.id == info.smashggID || participant.player.id == info.smashggID)
                    {
                        participant.gamerTag = info.name;
                        participant.gamerTag = info.name;
                        participant.user.location.country = info.flag;
                        break;
                    }
                }

                participant.user.location.country = standardization.CountryAbbreviation(participant.user.location.country);

                if (participant.player.id != 0)
                {
                    participant.player.id = participant.player.id;
                }
                else if (participant.player.id != 0)
                {
                    participant.player.id = participant.player.id;
                }

                if (participant.gamerTag != string.Empty)
                {
                    participant.gamerTag = participant.gamerTag;
                }
                else if (participant.gamerTag != string.Empty)
                {
                    participant.gamerTag = participant.gamerTag;
                }
            }
        }

        
        private void WriteLineToLog(string input)
        {
            form.Log += input + "\r\n";
        }
    }
}
