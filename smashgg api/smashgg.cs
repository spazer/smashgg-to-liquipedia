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
        // Set parameters
        static string GG_ENTRANT1 = "entrant1Id";
        static string GG_ENTRANT2 = "entrant2Id";
        static string GG_WPLACEMENT = "wPlacement";
        static string GG_LPLACEMENT = "lPlacement";
        static string GG_ROUND = "round";
        static string GG_ORIGINALROUND = "originalRound";
        static string GG_ENTRANT1SCORE = "entrant1Score";
        static string GG_ENTRANT2SCORE = "entrant2Score";
        static string GG_WINNERID = "winnerId";
        static string GG_ENTRANT1TYPE = "entrant1PrereqType";
        static string GG_ENTRANT2TYPE = "entrant2PrereqType";
        static string GG_STATE = "state";

        // Player parameters
        static string GG_GAMERTAG = "gamerTag";
        static string GG_COUNTRY = "country";
        static string GG_PLAYERS = "players";

        // Misc
        static int PLAYER_BYE = -1;

        Dictionary<string, string> flagList = new Dictionary<string, string>();

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
        public string ExpandOnly(string input, string title, int startPos, out int endPos)
        {
            endPos = 0;
            int bracketLevel = 0;
            char openBracket;
            char closeBracket;

            // Find the beginning of the desired expand
            if (input.IndexOf(title + "[", startPos) != -1)
            {
                openBracket = '[';
                closeBracket = ']';

                startPos = input.IndexOf(title + "[", startPos) + title.Length + 1;
            }
            else if (input.IndexOf(title + "{", startPos) != -1)
            {
                openBracket = '{';
                closeBracket = '}';

                startPos = input.IndexOf(title + "{", startPos) + title.Length + 1;
            }
            else
            {
                endPos = input.Length;
                return string.Empty;
            }

            endPos = startPos;
            bracketLevel = 1;

            // Iterate through the text until the whole expand is acquired
            do
            {
                int nextOpen = input.IndexOf(openBracket, endPos);
                int nextClose = input.IndexOf(closeBracket, endPos);

                // Bracket level cannot be zero if there are no more close brackets
                if (nextClose == -1)
                {
                    endPos = input.Length;
                    return string.Empty;
                }

                // Increase the bracket level for each additional open bracket. Subtract a level per close bracket.
                if (nextOpen < nextClose && nextOpen != -1)
                {
                    bracketLevel++;
                    endPos = nextOpen + 1;
                }
                else
                {
                    bracketLevel--;
                    endPos = nextClose + 1;
                }
            } while (bracketLevel > 0);

            // Get whatever's in the brackets
            string output = input.Substring(startPos, endPos - startPos - 1);

            // If all we get are empty brackets, go deeper via recursion. If the end of input is reached, string.empty will be returned.
            if (output == string.Empty)
            {
                output = ExpandOnly(input, title, endPos, out endPos);
            }

            return output;
        }

        public string SplitExpand(string input, int startPos, out int endPos)
        {
            int bracketLevel = 0;
            endPos = startPos;

            do
            {
                int nextOpen = input.IndexOf('{', endPos);
                int nextClose = input.IndexOf('}', endPos);

                // Bracket level cannot be zero if there are no more brackets
                if (nextClose == -1)
                {
                    endPos = input.Length;
                    return string.Empty;
                }

                // Increase the bracket level for each additional open bracket. Subtract a level per close bracket.
                if (nextOpen < nextClose && nextOpen != -1)
                {
                    bracketLevel++;
                    endPos = nextOpen + 1;
                }
                else
                {
                    bracketLevel--;
                    endPos = nextClose + 1;
                }
            } while (bracketLevel > 0);

            // Return whatever's in the brackets
            return input.Substring(startPos, endPos - startPos);
        }

        public void GetEntrants(JToken input, ref Dictionary<int, Player> entrantList)
        {
            // Add bye info
            entrantList.Add(-1, new Player("Bye", string.Empty));

            // Divide input into manageable chunks
            foreach (JToken entrant in input.Children())
            {
                Player newPlayer = new Player();

                // Get player ID
                if (entrant[SmashggStrings.ID].IsNullOrEmpty()) { continue; }
                int id = entrant[SmashggStrings.ID].Value<int>();

                var players = entrant.SelectToken("mutations.players");
                if (players.Type == JTokenType.Object)
                {
                    // Get player tag
                    newPlayer.name = players.First.First[GG_GAMERTAG].Value<string>();

                    // Get player country
                    if (!players.First.First[GG_COUNTRY].IsNullOrEmpty())
                    {
                        newPlayer.country = CountryAbbreviation(players.First.First[GG_COUNTRY].Value<string>());

                    }
                }
                else if (players.Type == JTokenType.Array)
                {
                    // Not implemented
                }
                else
                {
                    
                }

                entrantList.Add(id, newPlayer);
            }
        }

        public void GetDoublesEntrants(string input, ref Dictionary<int, Team> teamList)
        {
            List<string> rawEntrantData = new List<string>();
            int endPos = 0;
            string temp;

            // Add bye info
            teamList.Add(-1, new Team(new Player("Bye", string.Empty), new Player("Bye", string.Empty)));

            // Divide input into manageable chunks
            do
            {
                temp = SplitExpand(input, endPos, out endPos);
                if (temp != string.Empty)
                {
                    rawEntrantData.Add(temp);
                }
            } while (temp != string.Empty);

            foreach (string entrant in rawEntrantData)
            {
                Team newTeam = new Team();

                // Get team ID
                int id = GetIntParameter(entrant, SmashggStrings.ID);
                if (id == -99) continue;

                // Get team players
                Player player1 = new Player();
                Player player2 = new Player();

                // Seperate player data
                string[] rawPlayerData = new string[2];
                temp = ExpandOnly(entrant, GG_PLAYERS, 0, out endPos);
                rawPlayerData[0] = SplitExpand(temp, 0, out endPos);
                rawPlayerData[1] = SplitExpand(temp, endPos, out endPos);

                // Get player tags
                player1.name = GetStringParameter(rawPlayerData[0], GG_GAMERTAG);
                player1.name = System.Text.RegularExpressions.Regex.Unescape(player1.name);
                player2.name = GetStringParameter(rawPlayerData[1], GG_GAMERTAG);
                player2.name = System.Text.RegularExpressions.Regex.Unescape(player2.name);

                // Get player countries
                player1.country = GetStringParameter(rawPlayerData[0], GG_COUNTRY);
                player1.country = CountryAbbreviation(player1.country);
                player2.country = GetStringParameter(rawPlayerData[1], GG_COUNTRY);
                player2.country = CountryAbbreviation(player2.country);

                // Add team
                teamList.Add(id, new Team(player1, player2));
            }
        }

        public void GetSets(JToken input, ref List<Set> setList)
        {
            // Get set data
            List<int> matchCountWinners = new List<int>();
            List<int> matchCountLosers = new List<int>();
            foreach (JToken set in input.Children())
            {
                Set newSet = new Set();

                // Get the entrant IDs. Set the entrant as a bye if it is null.
                newSet.entrantID1 = GetIntParameter(set, GG_ENTRANT1);
                if (newSet.entrantID1 == -99)
                {
                    newSet.entrantID1 = PLAYER_BYE;
                }

                newSet.entrantID2 = GetIntParameter(set, GG_ENTRANT2);
                if (newSet.entrantID2 == -99)
                {
                    newSet.entrantID2 = PLAYER_BYE;
                }

                // Get match data
                newSet.entrant1wins = GetIntParameter(set, GG_ENTRANT1SCORE);
                newSet.entrant2wins = GetIntParameter(set, GG_ENTRANT2SCORE);
                newSet.winner = GetIntParameter(set, GG_WINNERID);
                newSet.state = GetIntParameter(set, GG_STATE);

                // Bracket rank
                newSet.wPlacement = GetIntParameter(set, GG_WPLACEMENT);
                newSet.lPlacement = GetIntParameter(set, GG_LPLACEMENT);

                // Round and match identifiers
                newSet.round = GetIntParameter(set, GG_ORIGINALROUND);
                int round = Math.Abs(newSet.round);

                if (newSet.round == -99)
                {
                    continue;
                }
                else if (newSet.round > 0)
                {
                    while (round > matchCountWinners.Count)
                    {
                        matchCountWinners.Add(0);
                    }

                    matchCountWinners[round - 1]++;
                    newSet.match = matchCountWinners[round - 1];
                }
                else if (newSet.round < 0)
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

        public string GetStringParameter(string input, string param)
        {
            if (input.IndexOf(param) != -1)
            {
                int start = input.IndexOf(param) + param.Length;

                // Error check for things that are not strings
                if (input.Substring(start, 1) != "\"")
                {
                    return string.Empty;
                }

                // Find the closing quotation mark
                start = start + 1;
                int end = input.IndexOf("\"", start);

                return input.Substring(start, end - start);
            }
            else
            {
                return string.Empty;
            }
        }
        #endregion

        #region Private Methods
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
