using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace smashgg_api
{
    class smashgg
    {
        static int PLAYER_BYE = -1;

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
        /// Appends entrants from the json input into entrantList
        /// </summary>
        /// <param name="input">json of the entrants token</param>
        /// <param name="entrantList">List of entrants to be outputted to</param>
        public void GetEntrants(JToken input, ref Dictionary<int, Entrant> entrantList)
        {
            // Add bye info
            entrantList.Add(-1, new Entrant(new Player("Bye", string.Empty)));

            // Divide input into manageable chunks
            foreach (JToken entrant in input.Children())
            {
                // Get player ID
                if (entrant[SmashggStrings.ID].IsNullOrEmpty()) { continue; }
                int id = GetIntParameter(entrant, SmashggStrings.ID);

                // Get participant IDs
                SortedList<int, Player> pIds = new SortedList<int, smashgg_api.Player>();
                foreach (int participant in entrant[SmashggStrings.ParticipantIds])
                {
                    Player newPlayer = new Player();
                    pIds.Add(participant, newPlayer);
                }

                foreach (KeyValuePair<int, Player> participant in pIds) 
                {
                    // Get player ID based off participant ID
                    int playerId = entrant.SelectToken(SmashggStrings.PlayerIds + "." + participant.Key).Value<int>();

                    // Select player token based off player ID
                    JToken playerInfo = entrant.SelectToken("mutations.players" + "." + playerId);

                    // Get player tag
                    pIds[participant.Key].name = playerInfo[SmashggStrings.Gamertag].Value<string>();

                    // Get player country. Leave empty if null
                    if (!playerInfo[SmashggStrings.Country].IsNullOrEmpty())
                    {
                        pIds[participant.Key].country = CountryAbbreviation(playerInfo[SmashggStrings.Country].Value<string>());
                    }
                    else
                    {
                        pIds[participant.Key].country = string.Empty;
                    }
                }

                Entrant newEntrant = new Entrant(pIds.Values.ToList<Player>());
                entrantList.Add(id, newEntrant);
            }
        }

        /// <summary>
        /// Appends sets from the json input into setList
        /// </summary>
        /// <param name="input">json of the sets token</param>
        /// <param name="entrantList">List of sets to be outputted to</param>
        public void GetSets(JToken input, ref List<Set> setList)
        {
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
                    newSet.entrantID1 = PLAYER_BYE;
                }

                newSet.entrantID2 = GetIntParameter(set, SmashggStrings.Entrant2Id);
                if (newSet.entrantID2 == -99)
                {
                    newSet.entrantID2 = PLAYER_BYE;
                }

                // Get match data
                newSet.entrant1wins = GetIntParameter(set, SmashggStrings.Entrant1Score);
                newSet.entrant2wins = GetIntParameter(set, SmashggStrings.Entrant2Score);
                newSet.winner = GetIntParameter(set, SmashggStrings.Winner);
                newSet.state = GetIntParameter(set, SmashggStrings.State);

                // Bracket rank
                newSet.wPlacement = GetIntParameter(set, SmashggStrings.wPlace);
                newSet.lPlacement = GetIntParameter(set, SmashggStrings.lPlace);

                // Round and match identifiers
                newSet.originalRound = GetIntParameter(set, SmashggStrings.OriginalRound);
                newSet.displayRound = GetIntParameter(set, SmashggStrings.DisplayRound);
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

                setList.Add(newSet);
            }
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
                return token[param].Value<int>();
            }
            else
            {
                return -99;
            }
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
