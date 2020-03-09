using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace smashgg_to_liquipedia
{
    class smashgg
    {
        public enum State { Unknown, NotStarted, Pending, Completed };

        Dictionary<string, string> flagList = new Dictionary<string, string>();

        /// <summary>
        /// Constructor
        /// </summary>
        public smashgg()
        {
            // Populate flag abbreviation list
            using (StreamReader file = new StreamReader("Flag List.csv"))
            {
                while (!file.EndOfStream)
                {
                    string line = file.ReadLine();
                    string[] text = line.Split(',');

                    if (text.Length >= 2)
                    {
                        flagList.Add(text[0].ToLower(), text[1].ToLower());
                    }
                }
            }
        }

        #region Public Methods
        /// <summary>
        /// Appends players from the json input into playerList
        /// </summary>
        /// <param name="input">json of the player token</param>
        /// <param name="playerList">List of players to be outputted to</param>
        /// <returns>Returns true if successful, false otherwise</returns>
        public bool GetPlayers(JToken input, ref Dictionary<int, Player> playerList, PlayerDatabase playerdb)
        {
            if (input == null) return false;

            // Add bye info
            playerList.Add(-1, new Player(0, "Bye", string.Empty));

            // Divide input into manageable chunks
            foreach (JToken player in input.Children())
            {
                // Get entrant ID
                if (player[SmashggStrings.ID].IsNullOrEmpty()) { continue; }
                int id = GetIntParameter(player, SmashggStrings.ID);
                string name = string.Empty;
                string country = string.Empty;


                // Check the AKA database to see if this player is in it. Fill information if they are found
                bool foundInDatabase = false;
                foreach (PlayerInfo info in playerdb.players)
                {
                    if (id == info.smashggID)
                    {
                        name = info.name;
                        country = info.flag;
                        foundInDatabase = true;
                        break;
                    }
                }
                if (!foundInDatabase)
                {
                    // Get player tag
                    name = player.SelectToken(SmashggStrings.Gamertag).Value<string>();

                    // Get player country
                    if (!player.SelectToken(SmashggStrings.Country).IsNullOrEmpty())
                    {
                        country = player.SelectToken(SmashggStrings.Country).Value<string>();
                    }

                    // Get player country. Leave empty if null
                    if (country != null && country != string.Empty)
                    {
                       country = CountryAbbreviation(country);
                    }
                    else
                    {
                        country = string.Empty;
                    }
                }

                name = name.Replace("|", "{{!}}");
                Player newPlayer = new Player(id, name, country);
                playerList.Add(id, newPlayer);
            }

            return true;
        }


        /// <summary>
        /// Appends entrants from the json input into entrantList
        /// </summary>
        /// <param name="input">json of the entrants token</param>
        /// <param name="entrantList">List of entrants to be outputted to</param>
        /// <returns>Returns true if successful, false otherwise</returns>
        public bool GetEntrants(JToken input, ref Dictionary<int, Entrant> entrantList, ref Dictionary<int, Player> playerList, PlayerDatabase playerdb, Entrant.EntrantType assumedType)
        {
            if (input == null) return false;
            
            // Add bye info
            entrantList.Add(-1, new Entrant(new Player(0, "Bye", string.Empty)));

            // Divide input into manageable chunks
            foreach (JToken entrant in input.Children())
            {
                // Get entrant ID
                if (entrant[SmashggStrings.ID].IsNullOrEmpty()) { continue; }
                int id = GetIntParameter(entrant, SmashggStrings.ID);
                string name = entrant.SelectToken(SmashggStrings.Name).Value<string>();

                // Get participant IDs
                SortedList<int, Player> partIds = new SortedList<int, Player>();
                foreach (int participant in entrant[SmashggStrings.ParticipantIds])
                {
                    Player newPlayer = new Player();
                    partIds.Add(participant, newPlayer);
                }

                foreach (KeyValuePair<int, Player> participant in partIds) 
                {
                    // Get player ID based off participant ID
                    participant.Value.playerID = entrant.SelectToken(SmashggStrings.PlayerIds + "." + participant.Key).Value<int>();

                    if (playerList.ContainsKey(participant.Value.playerID))
                    {
                        participant.Value.name = playerList[participant.Value.playerID].name;
                        participant.Value.country = playerList[participant.Value.playerID].country;
                    }
                    else
                    {
                        // Check the AKA database to see if this player is in it. Fill information if they are found
                        bool foundInDatabase = false;
                        foreach (PlayerInfo info in playerdb.players)
                        {
                            if (participant.Value.playerID == info.smashggID)
                            {
                                participant.Value.name = info.name;
                                participant.Value.country = info.flag;
                                foundInDatabase = true;
                                break;
                            }
                        }
                        if (!foundInDatabase)
                        {
                            // Select player token based off player ID
                            JToken playerInfo = entrant.SelectToken("mutations.players" + "." + participant.Value.playerID);

                            // Get player tag
                            partIds[participant.Key].name = playerInfo[SmashggStrings.Gamertag].Value<string>();
                            partIds[participant.Key].name = partIds[participant.Key].name.Replace("|", "{{!}}");

                            // Get player country. Leave empty if null
                            if (!playerInfo[SmashggStrings.Country].IsNullOrEmpty())
                            {
                                partIds[participant.Key].country = CountryAbbreviation(playerInfo[SmashggStrings.Country].Value<string>());
                            }
                            else
                            {
                                partIds[participant.Key].country = string.Empty;
                            }
                        }
                    }
                }

                Entrant newEntrant = new Entrant(partIds.Values.ToList<Player>());
                newEntrant.Name = name;
                entrantList.Add(id, newEntrant);
            }

            return true;
        }

        /// <summary>
        /// Appends sets from the json input into setList
        /// </summary>
        /// <param name="input">json of the sets token</param>
        /// <param name="entrantList">List of sets to be outputted to</param>
        /// <param name="parseMatchDetails">If true, parse match details where available</param>
        /// <returns>Returns true if successful, false otherwise</returns>
        public bool GetSets(JToken input, ref List<Set> setList, bool parseMatchDetails)
        {
            if (input == null) return false;
            
            // Get set data
            List<int> matchCountWinners = new List<int>();
            List<int> matchCountLosers = new List<int>();
            foreach (JToken set in input.Children())
            {
                Set newSet = new Set();

                // Get the entrant IDs. Set the entrant as a bye if it is null.
                newSet.entrantID1 = GetIntParameter(set, SmashggStrings.Entrant1Id);
                if (newSet.entrantID1 == -99)
                {
                    newSet.entrantID1 = Consts.PLAYER_BYE;
                }

                newSet.entrantID2 = GetIntParameter(set, SmashggStrings.Entrant2Id);
                if (newSet.entrantID2 == -99)
                {
                    newSet.entrantID2 = Consts.PLAYER_BYE;
                }

                // Get match data
                newSet.entrant1wins = GetIntParameter(set, SmashggStrings.Entrant1Score);
                newSet.entrant2wins = GetIntParameter(set, SmashggStrings.Entrant2Score);
                newSet.winner = GetIntParameter(set, SmashggStrings.Winner);
                newSet.State = (State)GetIntParameter(set, SmashggStrings.State);

                if (!set[SmashggStrings.IsGF].IsNullOrEmpty())
                {
                    newSet.isGF = set[SmashggStrings.IsGF].Value<bool>();
                }
                else
                {
                    newSet.isGF = false;
                }
                
                // Bracket rank
                newSet.wPlacement = GetIntParameter(set, SmashggStrings.wPlace);
                newSet.lPlacement = GetIntParameter(set, SmashggStrings.lPlace);

                // Round and match identifiers
                newSet.id = GetIntParameter(set, SmashggStrings.ID);
                newSet.originalRound = GetIntParameter(set, SmashggStrings.OriginalRound);
                newSet.displayRound = GetIntParameter(set, SmashggStrings.DisplayRound);
                newSet.entrant1PrereqId = GetIntParameter(set, SmashggStrings.Entrant1PrereqId);
                newSet.entrant2PrereqId = GetIntParameter(set, SmashggStrings.Entrant2PrereqId);

                // Progression identifiers
                newSet.wProgressingPhaseGroupId = GetIntParameter(set, SmashggStrings.wProgressingPhaseGroup);
                newSet.lProgressingPhaseGroupId = GetIntParameter(set, SmashggStrings.lProgressingPhaseGroup);

                // Get game ID
                newSet.gameId = GetIntParameter(set, SmashggStrings.GameId);

                int round = Math.Abs(newSet.originalRound);

                if (newSet.originalRound == -99)
                {
                    continue;
                }
                else if (newSet.originalRound > 0)
                {
                    while (round > matchCountWinners.Count)
                    {
                        matchCountWinners.Add(0);
                    }

                    matchCountWinners[round - 1]++;
                    newSet.match = matchCountWinners[round - 1];
                }
                else if (newSet.originalRound < 0)
                {
                    while (round > matchCountLosers.Count)
                    {
                        matchCountLosers.Add(0);
                    }

                    matchCountLosers[round - 1]++;
                    newSet.match = matchCountLosers[round - 1];
                }

                // Make sure match details list is populated
                JToken sourceGames = set.SelectToken(SmashggStrings.Games);
                if (sourceGames != null)
                {
                    List<Game> gameList = new List<Game>();

                    foreach (JToken gameData in sourceGames)
                    {
                        Game newGame = new Game();

                        // Get entrant 1 character
                        JToken charData = gameData.SelectToken(SmashggStrings.Selections + "." + newSet.entrantID1.ToString() + "." + SmashggStrings.Character);

                        if (charData != null)
                        {
                            JToken[] charArray = charData.ToArray<JToken>();
                            if (charArray[0].SelectToken(SmashggStrings.SelectionType).Value<string>() == "character")
                            {
                                newGame.entrant1p1char = GetIntParameter(charArray[0], SmashggStrings.SelectionValue);
                            }
                        }

                        // Get entrant 2 character
                        charData = gameData.SelectToken(SmashggStrings.Selections + "." + newSet.entrantID2.ToString() + "." + SmashggStrings.Character);

                        if (charData != null)
                        {
                            JToken[] charArray = charData.ToArray<JToken>();
                            if (charArray[0].SelectToken(SmashggStrings.SelectionType).Value<string>() == "character")
                            {
                                newGame.entrant2p1char = GetIntParameter(charArray[0], SmashggStrings.SelectionValue);
                            }
                        }

                        newGame.winner = GetIntParameter(gameData, SmashggStrings.Winner);
                        newGame.stage = GetIntParameter(gameData, SmashggStrings.Stage);
                        newGame.gameOrder = gameData.SelectToken(SmashggStrings.GameOrder).Value<string>();

                        newGame.entrant1p1stocks = GetIntParameter(gameData, SmashggStrings.Entrant1P1stocks);
                        newGame.entrant1p2stocks = GetIntParameter(gameData, SmashggStrings.Entrant1P2stocks);
                        newGame.entrant2p1stocks = GetIntParameter(gameData, SmashggStrings.Entrant2P1stocks);
                        newGame.entrant2p2stocks = GetIntParameter(gameData, SmashggStrings.Entrant2P2stocks);

                        gameList.Add(newGame);
                    }

                    // If no characters are set per game and an overall character is set, assume all games are the same character
                    bool nochars = false;
                    if (newSet.entrant1chars != null && newSet.entrant1chars.Count == 1)
                    {
                        for (int i = 0; i < gameList.Count; i++)
                        {
                            if (gameList[i].entrant1p1char != -99)
                            {
                                break;
                            }
                        }

                        nochars = true;
                    }

                    if (nochars)
                    {
                        for (int i = 0; i < gameList.Count; i++)
                        {
                            gameList[i].entrant1p1char = newSet.entrant1chars[0];
                        }
                    }

                    // Do the same for player 2
                    nochars = false;
                    if (newSet.entrant2chars != null && newSet.entrant2chars.Count == 1)
                    {
                        for (int i = 0; i < gameList.Count; i++)
                        {
                            if (gameList[i].entrant2p1char != -99)
                            {
                                break;
                            }
                        }

                        nochars = true;
                    }

                    if (nochars)
                    {
                        for (int i = 0; i < gameList.Count; i++)
                        {
                            gameList[i].entrant2p1char = newSet.entrant2chars[0];
                        }
                    }

                    newSet.games = gameList;
                }

                setList.Add(newSet);
            }

            return true;
        }

        /// <summary>
        /// Appends sets from the json input into setList
        /// </summary>
        /// <param name="input">json of the sets token</param>
        /// <param name="entrantList">List of sets to be outputted to</param>
        /// <param name="parseMatchDetails">If true, parse match details where available</param>
        /// <returns>Returns true if successful, false otherwise</returns>
        public bool GetRank(JToken input, Dictionary<int, Entrant> entrantList)
        {
            if (input == null) return false;

            // Get rank data for each entrant
            foreach (int entrantId in entrantList.Keys)
            {
                foreach (JToken entry in input.Children())
                {
                    if (GetIntParameter(entry, SmashggStrings.EntrantId) == entrantId)
                    {
                        entrantList[entrantId].Placement = GetIntParameter(entry, SmashggStrings.Placement);
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Returns the state of the retrieved phase group
        /// </summary>
        /// <param name="input">JSON token</param>
        /// <returns>State of the set</returns>
        public State GetPhaseGroupState(JToken input)
        {
            return (State)GetIntParameter(input, SmashggStrings.State);
        }

        /// <summary>
        /// Returns an integer from the specified parameter, or -99 on a null entry
        /// </summary>
        /// <param name="token">json input</param>
        /// <param name="param">Requested parameter</param>
        /// <returns>Integer value of param, or -99 for null</returns>
        public int GetIntParameter(JToken token, string param)
        {
            if (!token[param].IsNullOrEmpty())
            {
                if (token[param].Type == JTokenType.Integer)
                {
                    return token[param].Value<int>();
                }
            }

            return Consts.UNKNOWN;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Takes an input country and abbreviates it to two letters
        /// </summary>
        /// <param name="country">Input country</param>
        /// <returns>Two letter abbreviation of the coutnry, or string.Empty</returns>
        private string CountryAbbreviation(string country)
        {
            string lcCountry = country.ToLower();

            // Assume a two-letter country is already an abbreviation
            if (lcCountry.Length == 2)
            {
                return lcCountry;
            }

            // Look in the dictionary for the country
            if (flagList.ContainsKey(lcCountry))
            {
                return flagList[lcCountry];
            }
            else
            {
                return string.Empty;
            }
        }
        #endregion
    }
}
