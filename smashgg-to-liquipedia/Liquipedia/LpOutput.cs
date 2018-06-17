
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace smashgg_to_liquipedia.Liquipedia
{
    class LpOutput
    {
        private string log;
        bool loadedMatchDetailDictionary;

        Dictionary<int, Entrant> entrantList;
        List<Set> setList;
        Dictionary<int, List<Set>> roundList;
        Entrant.EntrantType entrantType;

        Dictionary<int, string> gameCharacterList = new Dictionary<int, string>();
        Dictionary<int, string> gameStageList = new Dictionary<int, string>();

        /// <summary>
        /// LpOutput constructor
        /// </summary>
        /// <param name="form">Reference to the main form</param>
        public LpOutput(ref Dictionary<int, Entrant> entrantList, ref List<Set> setList, ref Dictionary<int, List<Set>> roundList, ref Entrant.EntrantType entrantType)
        {
            log = string.Empty;

            this.entrantList = entrantList;
            this.setList = setList;
            this.roundList = roundList;
            this.entrantType = entrantType;

            loadedMatchDetailDictionary = false;
        }

        /// <summary>
        /// Creates match details for a set
        /// </summary>
        /// <param name="set">Set information</param>
        /// <param name="game">Game information</param>
        /// <param name="identifier">Liquipedia identifier</param>
        /// <returns></returns>
        public string GenerateMatchDetailsSingles(Set set, Game game, string identifier)
        {
            if (!loadedMatchDetailDictionary)
            {
                LoadDictionaryFromCSV(ref gameCharacterList, set.gameId, "Character");
                LoadDictionaryFromCSV(ref gameStageList, set.gameId, "Stage");

                loadedMatchDetailDictionary = true;
            }

            string insertiontext = string.Empty;

            // Insert character for player 1
            string character;
            if (gameCharacterList.ContainsKey(game.entrant1p1char))
            {
                character = gameCharacterList[game.entrant1p1char];
            }
            else if (game.entrant1p1char == Consts.UNKNOWN || game.entrant1p1char == 0)
            {
                character = string.Empty;
            }
            else
            {
                character = game.entrant1p1char.ToString();
                log += "No character entry for number: " + character + "\r\n";
            }

            insertiontext += "|" + identifier + LpStrings.P1 + LpStrings.Character + game.gameOrder + "=" + character;

            // Insert character for player 2
            if (gameCharacterList.ContainsKey(game.entrant2p1char))
            {
                character = gameCharacterList[game.entrant2p1char];
            }
            else if (game.entrant2p1char == Consts.UNKNOWN || game.entrant2p1char == 0)
            {
                character = string.Empty;
            }
            else
            {
                character = game.entrant2p1char.ToString();
                log += "No character entry for number: " + character + "\r\n";
            }

            insertiontext += " |" + identifier + LpStrings.P2 + LpStrings.Character + game.gameOrder + "=" + character;

            // Insert stock counts
            if (game.entrant1p1stocks == Consts.UNKNOWN)
            {
                insertiontext += " |" + identifier + LpStrings.P1 + LpStrings.Stock + game.gameOrder + "=";
            }
            else
            {
                insertiontext += " |" + identifier + LpStrings.P1 + LpStrings.Stock + game.gameOrder + "=" + game.entrant1p1stocks;
            }

            if (game.entrant2p1stocks == Consts.UNKNOWN)
            {
                insertiontext += " |" + identifier + LpStrings.P2 + LpStrings.Stock + game.gameOrder + "=";
            }
            else
            {
                insertiontext += " |" + identifier + LpStrings.P2 + LpStrings.Stock + game.gameOrder + "=" + game.entrant2p1stocks;
            }

            // Insert game winner
            insertiontext += " |" + identifier + LpStrings.Win + game.gameOrder + "=";
            if (game.winner == set.entrantID1)
            {
                insertiontext += "1";
            }
            else if (game.winner == set.entrantID2)
            {
                insertiontext += "2";
            }

            // Insert stage
            string stage = string.Empty;
            if (gameStageList.ContainsKey(game.stage))
            {
                stage = gameStageList[game.stage];
            }
            else if (game.stage == Consts.UNKNOWN || game.stage == 0)
            {
                stage = string.Empty;
            }
            else
            {
                stage = game.stage.ToString();
                log += "No stage entry for number: " + stage + "\r\n";
            }

            insertiontext += " |" + identifier + LpStrings.Stage + game.gameOrder + "=" + stage + "\r\n";

            return insertiontext;
        }


        /// <summary>
        /// Loads a dictionary with values from a CSV file
        /// </summary>
        /// <param name="targetDictionary">The dictionary to fill</param>
        /// <param name="gameId">ID of the game. Melee = 1, Wii U = 3, 64 = 4</param>
        /// <param name="datatype"></param>
        private void LoadDictionaryFromCSV(ref Dictionary<int, string> targetDictionary, int gameId, string datatype)
        {
            // Clear the list
            targetDictionary.Clear();

            string filename = datatype + " Lists\\" + datatype + " List " + gameId + ".csv";

            // Read csv if available
            try
            {
                using (StreamReader file = new StreamReader(filename, System.Text.Encoding.Default))
                {
                    while (!file.EndOfStream)
                    {
                        string input = file.ReadLine();
                        string[] splitinput = input.Split(',');

                        if (splitinput.Count() == 2)
                        {
                            int num;
                            if (int.TryParse(splitinput[0], out num))
                            {
                                targetDictionary.Add(num, splitinput[1]);
                            }
                            else
                            {
                                log += "Invalid " + datatype + " number in: " + input + "\r\n";
                            }
                        }
                        else
                        {
                            log += "Couldn't parse " + datatype + " entry: " + input + "\r\n";
                        }
                    }
                }
            }
            catch
            {
                log += "Couldn't find CSV file: " + filename + "\r\n";
            }
        }

        /// <summary>
        /// Fills match details
        /// </summary>
        /// <param name="bracketSide">Winners or Losers Bracket</param>
        /// <param name="round">Round number</param>
        /// <param name="match">Match number</param>
        /// <param name="setData">Set data</param>
        /// <param name="bracketText">Text of the bracket to fill</param>
        /// <param name="reverse">Reverses entrant 1 and 2. Used for grand final resets</param>
        private void FillMatchDetailsSingles(string bracketSide, int round, int match, Set setData, ref string bracketText, bool reverse)
        {
            string identifier = bracketSide + round + LpStrings.Match + match;

            // Find the last occurance of r1m1 or a similar match identifier
            Regex rgx = new Regex(identifier + @"[a-zA-Z]");
            MatchCollection rgxMatches = rgx.Matches(bracketText);

            if (rgxMatches.Count == 0)
            {
                return;
            }

            // If filled out details already exist for this match, exit
            Regex detailRegex = new Regex("\\|" + identifier + @"(p[12])?(char|stock|win|stage)([1-9]=)[a-zA-Z]");
            MatchCollection detailMatches = detailRegex.Matches(bracketText);
            if (detailMatches.Count > 0)
            {
                return;
            }

            if (rgxMatches[rgxMatches.Count - 1].Index != -1)
            {
                // Skip to the end of the line
                int insertionlocation = bracketText.IndexOf("\n", rgxMatches[rgxMatches.Count - 1].Index) + 1;

                string insertiontext = string.Empty;
                foreach (Game game in setData.games)
                {
                    if (!reverse)
                    {
                        insertiontext += GenerateMatchDetailsSingles(setData, game, identifier);
                    }
                    else
                    {
                        // Insert character for player 1
                        string character;
                        if (gameCharacterList.ContainsKey(game.entrant2p1char))
                        {
                            character = gameCharacterList[game.entrant2p1char];
                        }
                        else if (game.entrant2p1char == Consts.UNKNOWN)
                        {
                            character = string.Empty;
                        }
                        else
                        {
                            character = game.entrant2p1char.ToString();
                            log += "No character entry for number: " + character + "\r\n";
                        }

                        insertiontext += "|" + identifier + LpStrings.P1 + LpStrings.Character + game.gameOrder + "=" + character;

                        // Insert character for player 2
                        if (gameCharacterList.ContainsKey(game.entrant1p1char))
                        {
                            character = gameCharacterList[game.entrant1p1char];
                        }
                        else if (game.entrant1p1char == Consts.UNKNOWN)
                        {
                            character = string.Empty;
                        }
                        else
                        {
                            character = game.entrant1p1char.ToString();
                            log += "No character entry for number: " + character + "\r\n";
                        }

                        insertiontext += " |" + identifier + LpStrings.P2 + LpStrings.Character + game.gameOrder + "=" + character;

                        // Insert stock counts
                        if (game.entrant2p1stocks == Consts.UNKNOWN)
                        {
                            insertiontext += " |" + identifier + LpStrings.P1 + LpStrings.Stock + game.gameOrder + "=";
                        }
                        else
                        {
                            insertiontext += " |" + identifier + LpStrings.P1 + LpStrings.Stock + game.gameOrder + "=" + game.entrant2p1stocks;
                        }

                        if (game.entrant1p1stocks == Consts.UNKNOWN)
                        {
                            insertiontext += " |" + identifier + LpStrings.P2 + LpStrings.Stock + game.gameOrder + "=";
                        }
                        else
                        {
                            insertiontext += " |" + identifier + LpStrings.P2 + LpStrings.Stock + game.gameOrder + "=" + game.entrant1p1stocks;
                        }

                        // Insert game winner
                        insertiontext += " |" + identifier + LpStrings.Win + game.gameOrder + "=";
                        if (game.winner == setData.entrantID1)
                        {
                            insertiontext += "2";
                        }
                        else if (game.winner == setData.entrantID2)
                        {
                            insertiontext += "1";
                        }

                        // Insert stage
                        string stage = string.Empty;
                        if (gameStageList.ContainsKey(game.stage))
                        {
                            stage = gameStageList[game.stage];
                        }
                        else if (game.stage == Consts.UNKNOWN)
                        {
                            stage = string.Empty;
                        }
                        else
                        {
                            stage = game.stage.ToString();
                            log += "No stage entry for number: " + stage + "\r\n";
                        }

                        insertiontext += " |" + identifier + LpStrings.Stage + game.gameOrder + "=" + stage + "\r\n";
                    }
                }

                if (insertiontext.Trim() != string.Empty)
                {
                    bracketText = bracketText.Insert(insertionlocation, insertiontext + "\r\n");
                }
            }
        }

        /// <summary>
        /// Fill liquipedia bracket with singles data
        /// </summary>
        /// <param name="startRound">Start round</param>
        /// <param name="endRound">End round</param>
        /// <param name="offset">Shift the round by this integer. Left is negative. Right is positive.</param>
        /// <param name="side">Side of the bracket to fill in</param>
        /// <param name="bracketText">Liquipedia markup</param>
        public void fillBracketSingles(int startRound, int endRound, int offset, ref string bracketText, Dictionary<int, int> matchOffsetPerRound, 
                                       bool fillByes, bool fillByeWins, bool r1Only, bool includeUnfinished, bool matchDetails)
        {
            int increment;
            string bracketSide;

            if (startRound > 0)
            {
                increment = 1;
                bracketSide = LpStrings.WRound;
            }
            else
            {
                increment = -1;
                bracketSide = LpStrings.LRound;
            }

            for (int i = startRound; Math.Abs(i) <= Math.Abs(endRound); i += increment)
            {
                int outputRound;

                // Skip rounds that don't exist
                if (!roundList.ContainsKey(i)) continue;

                // Iterate through all sets in the round
                for (int j = 0; j < roundList[i].Count; j++)
                {
                    Set currentSet = roundList[i][j];

                    // Skip rounds with missing entrants
                    if (!entrantList.ContainsKey(currentSet.entrantID1)) continue;
                    if (!entrantList.ContainsKey(currentSet.entrantID2)) continue;

                    // Add offsets
                    outputRound = Math.Abs(i) + offset;
                    int outputSet = currentSet.match + matchOffsetPerRound[i];

                    // Check for player byes
                    if (currentSet.entrantID1 == Consts.PLAYER_BYE && currentSet.entrantID2 == Consts.PLAYER_BYE)
                    {
                        // If both players are byes, skip this entry
                        continue;
                    }
                    else if (currentSet.entrantID1 == Consts.PLAYER_BYE)
                    {
                        // Fill in player 1 as a bye if fill byes is checked
                        if (fillByes == true)
                        {
                            if (!r1Only || (r1Only && outputRound == 1))
                            {
                                FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + outputSet + LpStrings.P1, "Bye");
                            }
                        }

                        // Fill player 2 info
                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + outputSet + LpStrings.P2, entrantList[currentSet.entrantID2].Players[0].name);
                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + outputSet + LpStrings.P2 + LpStrings.Flag, entrantList[currentSet.entrantID2].Players[0].country);

                        // Give player 2 a checkmark
                        if (fillByeWins == true)
                        {
                            if (!r1Only || (r1Only && outputRound == 1))
                            {
                                FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + outputSet + LpStrings.P2 + LpStrings.Score, LpStrings.Checkmark);
                                FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + outputSet + LpStrings.Win, "2");
                            }
                        }
                    }
                    else if (currentSet.entrantID2 == Consts.PLAYER_BYE)
                    {
                        // Fill in player 2 as a bye
                        if (fillByes == true)
                        {
                            if (!r1Only || (r1Only && outputRound == 1))
                            {
                                FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + outputSet + LpStrings.P2, "Bye");
                            }
                        }

                        // Fill player 1 info
                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + outputSet + LpStrings.P1, entrantList[currentSet.entrantID1].Players[0].name);
                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + outputSet + LpStrings.P1 + LpStrings.Flag, entrantList[currentSet.entrantID1].Players[0].country);

                        // Give player 1 a checkmark
                        if (fillByeWins == true)
                        {
                            if (!r1Only || (r1Only && outputRound == 1))
                            {
                                FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + outputSet + LpStrings.P1 + LpStrings.Score, LpStrings.Checkmark);
                                FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + outputSet + LpStrings.Win, "1");
                            }
                        }
                    }
                    else
                    {
                        // Fill in the set normally
                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + outputSet + LpStrings.P1, entrantList[currentSet.entrantID1].Players[0].name);
                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + outputSet + LpStrings.P1 + LpStrings.Flag, entrantList[currentSet.entrantID1].Players[0].country);
                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + outputSet + LpStrings.P2, entrantList[currentSet.entrantID2].Players[0].name);
                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + outputSet + LpStrings.P2 + LpStrings.Flag, entrantList[currentSet.entrantID2].Players[0].country);

                        // Check for DQs
                        if (currentSet.entrant1wins == -1)
                        {
                            FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + outputSet + LpStrings.P1 + LpStrings.Score, "DQ");
                            FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + outputSet + LpStrings.P2 + LpStrings.Score, LpStrings.Checkmark);
                        }
                        else if (currentSet.entrant2wins == -1)
                        {
                            FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + outputSet + LpStrings.P2 + LpStrings.Score, "DQ");
                            FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + outputSet + LpStrings.P1 + LpStrings.Score, LpStrings.Checkmark);
                        }
                        else
                        {
                            // smash.gg switches P1 and P2 in the event of a bracket reset
                            if (currentSet.isGF && currentSet.match == 2)
                            {
                                if (includeUnfinished || currentSet.State == smashgg.State.Completed)
                                {
                                    if (currentSet.entrant1wins != Consts.UNKNOWN && currentSet.entrant2wins != Consts.UNKNOWN)
                                    {
                                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + outputSet + LpStrings.P2 + LpStrings.Score, currentSet.entrant1wins.ToString());
                                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + outputSet + LpStrings.P1 + LpStrings.Score, currentSet.entrant2wins.ToString());
                                    }
                                    else
                                    {
                                        if (currentSet.winner == currentSet.entrantID1)
                                        {
                                            FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + outputSet + LpStrings.P2 + LpStrings.Score, "{{win}}");
                                        }
                                        else if (currentSet.winner == currentSet.entrantID2)
                                        {
                                            FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + outputSet + LpStrings.P1 + LpStrings.Score, "{{win}}");
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (includeUnfinished || currentSet.State == smashgg.State.Completed)
                                {
                                    if (currentSet.entrant1wins != Consts.UNKNOWN && currentSet.entrant2wins != Consts.UNKNOWN)
                                    {
                                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + outputSet + LpStrings.P1 + LpStrings.Score, currentSet.entrant1wins.ToString());
                                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + outputSet + LpStrings.P2 + LpStrings.Score, currentSet.entrant2wins.ToString());
                                    }
                                    else
                                    {
                                        if (currentSet.winner == currentSet.entrantID1)
                                        {
                                            FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + outputSet + LpStrings.P1 + LpStrings.Score, "{{win}}");
                                        }
                                        else if (currentSet.winner == currentSet.entrantID2)
                                        {
                                            FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + outputSet + LpStrings.P2 + LpStrings.Score, "{{win}}");
                                        }
                                    }
                                }
                            }
                        }

                        // Set the winner
                        if (currentSet.isGF && currentSet.match == 2)
                        {
                            if (currentSet.winner == currentSet.entrantID1)
                            {
                                FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + 1 + LpStrings.Win, "2");
                            }
                            else if (currentSet.winner == currentSet.entrantID2)
                            {
                                FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + 1 + LpStrings.Win, "1");
                            }
                        }
                        else if (currentSet.isGF && currentSet.match == 1 && roundList[i].Count > 1)
                        {
                            if (currentSet.winner == currentSet.entrantID1)
                            {
                                FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + 1 + LpStrings.Win, "1");
                            }
                        }
                        else
                        {
                            if (currentSet.winner == currentSet.entrantID1)
                            {
                                FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + outputSet + LpStrings.Win, "1");
                            }
                            else if (currentSet.winner == currentSet.entrantID2)
                            {
                                FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + outputSet + LpStrings.Win, "2");
                            }
                        }

                        // Fill in match details if available
                        if (includeUnfinished || currentSet.State == smashgg.State.Completed)
                        {
                            if (currentSet.isGF && currentSet.match == 2 && currentSet.games != null && matchDetails)
                            {
                                FillMatchDetailsSingles(bracketSide, outputRound, outputSet, currentSet, ref bracketText, true);
                            }
                            else if (currentSet.games != null && matchDetails)
                            {
                                FillMatchDetailsSingles(bracketSide, outputRound, outputSet, currentSet, ref bracketText, false);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Fill liquipedia bracket with doubles data
        /// </summary>
        /// <param name="startRound">Start round</param>
        /// <param name="endRound">End round</param>
        /// <param name="offset">Offset round by this integer</param>
        /// <param name="bracketText">Liquipedia markup</param>
        public void fillBracketDoubles(int startRound, int endRound, int offset, ref string bracketText, Dictionary<int, int> matchOffsetPerRound, 
                                       bool fillByes, bool fillByeWins, bool r1Only, bool includeUnfinished)
        {
            int increment;
            string bracketSide;
            if (startRound > 0)
            {
                increment = 1;
                bracketSide = LpStrings.WRound;
            }
            else
            {
                increment = -1;
                bracketSide = LpStrings.LRound;
            }

            for (int i = startRound; Math.Abs(i) <= Math.Abs(endRound); i += increment)
            {
                int outputRound;

                // Skip rounds that don't exist
                if (!roundList.ContainsKey(i)) continue;

                // Iterate through all sets in the round
                for (int j = 0; j < roundList[i].Count; j++)
                {
                    Set currentSet = roundList[i][j];

                    // Offset the output round as specified
                    outputRound = Math.Abs(i) + offset;
                    int outputSet = currentSet.match + matchOffsetPerRound[i];

                    // Skip unfinished sets unless otherwise specified
                    //if (fillByes == false && currentSet.state == 1)
                    //{
                    //    continue;
                    //}

                    // Check for player byes
                    if (currentSet.entrantID1 == Consts.PLAYER_BYE && currentSet.entrantID2 == Consts.PLAYER_BYE)
                    {
                        // If both players are byes, skip this entry
                        continue;
                    }
                    else if (currentSet.entrantID1 == Consts.PLAYER_BYE)
                    {
                        // Fill in team 1 as a bye
                        if (fillByes == true)
                        {
                            FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + outputSet + LpStrings.T1 + LpStrings.P1, "Bye");
                            FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + outputSet + LpStrings.T1 + LpStrings.P2, "Bye");
                        }

                        // Give team 2 a checkmark
                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + outputSet + LpStrings.T2 + LpStrings.P1, entrantList[currentSet.entrantID2].Players[0].name);
                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + outputSet + LpStrings.T2 + LpStrings.P1 + LpStrings.Flag, entrantList[currentSet.entrantID2].Players[0].country);
                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + outputSet + LpStrings.T2 + LpStrings.P2, entrantList[currentSet.entrantID2].Players[1].name);
                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + outputSet + LpStrings.T2 + LpStrings.P2 + LpStrings.Flag, entrantList[currentSet.entrantID2].Players[1].country);

                        if (fillByeWins == true)
                        {
                            FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + outputSet + LpStrings.T2 + LpStrings.Score, LpStrings.Checkmark);
                            FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + outputSet + LpStrings.Win, "2");
                        }
                    }
                    else if (currentSet.entrantID2 == Consts.PLAYER_BYE)
                    {
                        // Fill in team 2 as a bye
                        if (fillByes == true)
                        {
                            FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + outputSet + LpStrings.T2 + LpStrings.P1, "Bye");
                            FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + outputSet + LpStrings.T2 + LpStrings.P2, "Bye");
                        }

                        // Give team 1 a checkmark
                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + outputSet + LpStrings.T1 + LpStrings.P1, entrantList[currentSet.entrantID1].Players[0].name);
                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + outputSet + LpStrings.T1 + LpStrings.P1 + LpStrings.Flag, entrantList[currentSet.entrantID1].Players[0].country);
                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + outputSet + LpStrings.T1 + LpStrings.P2, entrantList[currentSet.entrantID1].Players[1].name);
                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + outputSet + LpStrings.T1 + LpStrings.P2 + LpStrings.Flag, entrantList[currentSet.entrantID1].Players[1].country);

                        if (fillByeWins == true)
                        {
                            FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + outputSet + LpStrings.T1 + LpStrings.Score, LpStrings.Checkmark);
                            FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + outputSet + LpStrings.Win, "1");
                        }
                    }
                    else
                    {
                        // Fill in the currentSet normally
                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + outputSet + LpStrings.T1 + LpStrings.P1, entrantList[currentSet.entrantID1].Players[0].name);
                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + outputSet + LpStrings.T1 + LpStrings.P1 + LpStrings.Flag, entrantList[currentSet.entrantID1].Players[0].country);
                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + outputSet + LpStrings.T1 + LpStrings.P2, entrantList[currentSet.entrantID1].Players[1].name);
                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + outputSet + LpStrings.T1 + LpStrings.P2 + LpStrings.Flag, entrantList[currentSet.entrantID1].Players[1].country);
                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + outputSet + LpStrings.T2 + LpStrings.P1, entrantList[currentSet.entrantID2].Players[0].name);
                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + outputSet + LpStrings.T2 + LpStrings.P1 + LpStrings.Flag, entrantList[currentSet.entrantID2].Players[0].country);
                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + outputSet + LpStrings.T2 + LpStrings.P2, entrantList[currentSet.entrantID2].Players[1].name);
                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + outputSet + LpStrings.T2 + LpStrings.P2 + LpStrings.Flag, entrantList[currentSet.entrantID2].Players[1].country);

                        // Check for DQs
                        if (currentSet.entrant1wins == -1)
                        {
                            FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + outputSet + LpStrings.T1 + LpStrings.Score, "DQ");
                            FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + outputSet + LpStrings.T2 + LpStrings.Score, LpStrings.Checkmark);
                        }
                        else if (currentSet.entrant2wins == -1)
                        {
                            FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + outputSet + LpStrings.T2 + LpStrings.Score, "DQ");
                            FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + outputSet + LpStrings.T1 + LpStrings.Score, LpStrings.Checkmark);
                        }
                        else
                        {
                            // smash.gg switches P1 and P2 in the event of a bracket reset
                            if (currentSet.isGF && currentSet.match == 2)
                            {
                                if (currentSet.entrant1wins != Consts.UNKNOWN && currentSet.entrant2wins != Consts.UNKNOWN)
                                {
                                    FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + outputSet + LpStrings.T2 + LpStrings.Score, currentSet.entrant1wins.ToString());
                                    FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + outputSet + LpStrings.T1 + LpStrings.Score, currentSet.entrant2wins.ToString());
                                }
                                else
                                {
                                    if (currentSet.winner == currentSet.entrantID1)
                                    {
                                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + outputSet + LpStrings.T2 + LpStrings.Score, "{{win}}");
                                    }
                                    else if (currentSet.winner == currentSet.entrantID2)
                                    {
                                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + outputSet + LpStrings.T1 + LpStrings.Score, "{{win}}");
                                    }
                                }
                            }
                            else
                            {
                                if (currentSet.entrant1wins != Consts.UNKNOWN && currentSet.entrant2wins != Consts.UNKNOWN)
                                {
                                    FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + outputSet + LpStrings.T1 + LpStrings.Score, currentSet.entrant1wins.ToString());
                                    FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + outputSet + LpStrings.T2 + LpStrings.Score, currentSet.entrant2wins.ToString());
                                }
                                else
                                {
                                    if (currentSet.winner == currentSet.entrantID1)
                                    {
                                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + outputSet + LpStrings.T1 + LpStrings.Score, "{{win}}");
                                    }
                                    else if (currentSet.winner == currentSet.entrantID2)
                                    {
                                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + outputSet + LpStrings.T2 + LpStrings.Score, "{{win}}");
                                    }
                                }
                            }
                        }

                        // Set the winner
                        if (currentSet.isGF && currentSet.match == 2)
                        {
                            if (currentSet.winner == currentSet.entrantID1)
                            {
                                FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + 1 + LpStrings.Win, "2");
                            }
                            else if (currentSet.winner == currentSet.entrantID2)
                            {
                                FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + 1 + LpStrings.Win, "1");
                            }
                        }
                        else if (currentSet.isGF && currentSet.match == 1 && roundList[i].Count > 1)
                        {
                            if (currentSet.winner == currentSet.entrantID1)
                            {
                                FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + 1 + LpStrings.Win, "1");
                            }
                        }
                        else
                        {
                            if (currentSet.winner == currentSet.entrantID1)
                            {
                                FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + outputSet + LpStrings.Win, "1");
                            }
                            else if (currentSet.winner == currentSet.entrantID2)
                            {
                                FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + outputSet + LpStrings.Win, "2");
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Find the specified liquipedia parameter and insert the specified value
        /// </summary>
        /// <param name="lpText">Block of liquipedia markup</param>
        /// <param name="param">Parameter to fill in</param>
        /// <param name="value">Value of the parameter</param>
        private void FillLPParameter(ref string lpText, string param, string value)
        {
            if (value == string.Empty) return;

            Regex rgx = new Regex(@"(\|" + param + @"=)([ \r\n])");
            Match match = rgx.Match(lpText);

            if (match.Success)
            {
                lpText = lpText.Replace(match.Groups[1].Value + match.Groups[2].Value, match.Groups[1].Value + value + match.Groups[2].Value);
            }
        }

        /// <summary>
        /// Output all data from the singles phase group
        /// </summary>
        /// <param name="groupTitle">Title of the group</param>
        /// <param name="poolData">Pool records for each entrant</param>
        /// <returns>A group table</returns>
        public string OutputSinglesGroup(string groupTitle, Dictionary<int, PoolRecord> poolData, smashgg.State groupState, int advanceWinners, int advanceLosers, bool matchDetails, PoolRecord.PoolType poolType)
        {
            string output = string.Empty;
            int totalWinners = advanceWinners;

            // Group header
            output = LpStrings.GroupStart + "Bracket " + groupTitle + LpStrings.GroupStartWidth + "\r\n";

            // Group slots
            for (int i = 0; i < poolData.Count; i++)
            {
                // Skip bye
                if (poolData.ElementAt(i).Key == Consts.PLAYER_BYE)
                {
                    continue;
                }

                Player currentPlayer = entrantList[poolData.ElementAt(i).Key].Players[0];
                output += LpStrings.SlotStart + currentPlayer.name +
                          LpStrings.SlotFlag + currentPlayer.country;
                if (poolData[poolData.ElementAt(i).Key].matchesActuallyPlayed == 0)
                {
                    output += LpStrings.DQ + "true";
                }
                else
                {
                    output += LpStrings.SlotMWin + poolData[poolData.ElementAt(i).Key].MatchesWin +
                              LpStrings.SlotMLoss + poolData[poolData.ElementAt(i).Key].MatchesLoss;
                }
                          

                if (poolData[poolData.ElementAt(i).Key].rank != Consts.UNKNOWN)
                {
                    if (advanceWinners > 0 && poolData[poolData.ElementAt(i).Key].rank != 0 && poolType != PoolRecord.PoolType.RoundRobin)
                    {
                        output += LpStrings.SlotPlace + (poolData[poolData.ElementAt(i).Key].rank - totalWinners);
                    }
                    else if (advanceLosers > 0 && poolData[poolData.ElementAt(i).Key].rank != 0 && poolType != PoolRecord.PoolType.RoundRobin)
                    {
                        output += LpStrings.SlotPlace + (poolData[poolData.ElementAt(i).Key].rank - 1);
                    }
                    else
                    {
                        output += LpStrings.SlotPlace + poolData[poolData.ElementAt(i).Key].rank;
                    }
                }
                else
                {
                    output += LpStrings.SlotPlace;
                }

                // Set background colors (or placement colors) to differentiate people who progressed vs those who are eliminated
                if (groupState == smashgg.State.Completed || poolData[poolData.ElementAt(i).Key].MatchesComplete)
                {
                    output += LpStrings.SlotBg;
                }
                else
                {
                    output += LpStrings.SlotPbg;
                }

                // Set colors
                if (advanceWinners > 0)
                {
                    output += "up";
                    advanceWinners--;
                }
                else if (advanceLosers > 0)
                {
                    output += "stay";
                    advanceLosers--;
                }
                else
                {
                    output += "down";
                }

                output += LpStrings.SlotEnd + "\r\n";
            }

            // Group footer
            output += LpStrings.GroupEnd + "\r\n";


            // Match details
            if (matchDetails)
            {
                output += LpStrings.MatchListStart + LpStrings.GroupStartWidth + "\r\n";
                foreach (Set currentSet in setList)
                {
                    Player p1 = entrantList[currentSet.entrantID1].Players[0];
                    Player p2 = entrantList[currentSet.entrantID2].Players[0];

                    output += LpStrings.MatchStages;

                    output += "|" + LpStrings.P1 + "=" + p1.name + " |" + LpStrings.P1 + LpStrings.Flag + "=" + p1.country + " |" + LpStrings.P1 + LpStrings.Score + "=" + currentSet.entrant1wins + "\r\n" +
                              "|" + LpStrings.P2 + "=" + p2.name + " |" + LpStrings.P2 + LpStrings.Flag + "=" + p2.country + " |" + LpStrings.P2 + LpStrings.Score + "=" + currentSet.entrant2wins + "\r\n" +
                              "|" + LpStrings.Win + "=";

                    if (currentSet.winner == currentSet.entrantID1) { output += "1\r\n"; }
                    else if (currentSet.winner == currentSet.entrantID2) { output += "2\r\n"; }

                    foreach (Game game in currentSet.games)
                    {
                        output += GenerateMatchDetailsSingles(currentSet, game, string.Empty);
                    }

                    output += LpStrings.MatchStagesEnd + "\r\n";
                }
                output += LpStrings.MatchListEnd + "\r\n";
            }

            return output;
        }

        /// <summary>
        /// Output all data from the doubles phase group
        /// </summary>
        /// <param name="groupTitle">Title of the group</param>
        /// <param name="poolData">Pool records for each entrant</param>
        /// <returns>A group table</returns>
        public string OutputDoublesGroup(string groupTitle, Dictionary<int, PoolRecord> poolData, smashgg.State groupState, int advanceWinners, int advanceLosers, bool matchDetails, PoolRecord.PoolType poolType)
        {
            string output = string.Empty;
            int totalWinners = advanceWinners;

            // Group header
            output = LpStrings.GroupStart + "Bracket " + groupTitle + LpStrings.GroupStartWidth + "\r\n";

            // Group slots
            for (int i = 0; i < poolData.Count; i++)
            {
                // Skip bye
                if (poolData.ElementAt(i).Key == Consts.PLAYER_BYE)
                {
                    continue;
                }

                // Output players
                output += LpStrings.DoublesSlotStart +
                          LpStrings.DoublesSlotP1 + entrantList[poolData.ElementAt(i).Key].Players[0].name +
                          LpStrings.DoublesSlotP1Flag + entrantList[poolData.ElementAt(i).Key].Players[0].country +
                          LpStrings.DoublesSlotP2 + entrantList[poolData.ElementAt(i).Key].Players[1].name +
                          LpStrings.DoublesSlotP2Flag + entrantList[poolData.ElementAt(i).Key].Players[1].country +
                          LpStrings.SlotMWin + poolData[poolData.ElementAt(i).Key].MatchesWin +
                          LpStrings.SlotMLoss + poolData[poolData.ElementAt(i).Key].MatchesLoss;

                if (poolData[poolData.ElementAt(i).Key].rank != Consts.UNKNOWN)
                {
                    if (advanceWinners > 0 && poolData[poolData.ElementAt(i).Key].rank != 0 && poolType != PoolRecord.PoolType.RoundRobin)
                    {
                        output += LpStrings.SlotPlace + (poolData[poolData.ElementAt(i).Key].rank - totalWinners);
                    }
                    else if (advanceLosers > 0 && poolData[poolData.ElementAt(i).Key].rank != 0 && poolType != PoolRecord.PoolType.RoundRobin)
                    {
                        output += LpStrings.SlotPlace + (poolData[poolData.ElementAt(i).Key].rank - 1);
                    }
                    else
                    {
                        output += LpStrings.SlotPlace + poolData[poolData.ElementAt(i).Key].rank;
                    }
                }
                else
                {
                    output += LpStrings.SlotPlace;
                }

                // Set background colors (or placement colors) to differentiate people who progressed vs those who are eliminated
                if (groupState == smashgg.State.Completed || poolData[poolData.ElementAt(i).Key].MatchesComplete)
                {
                    output += LpStrings.SlotBg;
                }
                else
                {
                    output += LpStrings.SlotPbg;
                }

                // Set colors
                if (advanceWinners > 0)
                {
                    output += "up";
                    advanceWinners--;
                }
                else if (advanceLosers > 0)
                {
                    output += "stay";
                    advanceLosers--;
                }
                else
                {
                    output += "down";
                }

                output += LpStrings.SlotEnd + "\r\n";
            }


            // Group footer
            output += LpStrings.GroupEnd + "\r\n";


            // Match details
            if (matchDetails)
            {
                output += LpStrings.MatchListStart + LpStrings.GroupStartWidth + "\r\n";
                foreach (Set currentSet in setList)
                {
                    Player p1 = entrantList[currentSet.entrantID1].Players[0];
                    Player p2 = entrantList[currentSet.entrantID2].Players[0];

                    output += LpStrings.MatchStages;

                    output += "|" + LpStrings.P1 + "=" + p1.name + " |" + LpStrings.P1 + LpStrings.Flag + "=" + p1.country + " |" + LpStrings.P1 + LpStrings.Score + "=" + currentSet.entrant1wins + "\r\n" +
                              "|" + LpStrings.P2 + "=" + p2.name + " |" + LpStrings.P2 + LpStrings.Flag + "=" + p2.country + " |" + LpStrings.P2 + LpStrings.Score + "=" + currentSet.entrant2wins + "\r\n" +
                              "|" + LpStrings.Win + "=";

                    if (currentSet.winner == currentSet.entrantID1) { output += "1\r\n"; }
                    else if (currentSet.winner == currentSet.entrantID2) { output += "2\r\n"; }

                    foreach (Game game in currentSet.games)
                    {
                        output += GenerateMatchDetailsSingles(currentSet, game, string.Empty);
                    }

                    output += LpStrings.MatchStagesEnd + "\r\n";
                }
                output += LpStrings.MatchListEnd + "\r\n";
            }

            return output;
        }


        public string Log
        {
            get
            {
                string outputlog = log;
                log = string.Empty;
                return outputlog;
            }
        }
    }
}
