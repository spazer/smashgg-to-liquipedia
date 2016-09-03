using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;

namespace smashgg_api
{
    public partial class Form1 : Form
    {
        // URLs
        static string GG_URL_PHASEGROUP = "https://api.smash.gg/phase_group/";
        static string GG_URL_PHASE = "https://api.smash.gg/phase/";
        static string GG_URL_BRACKET = "?expand[]=sets&expand[]=entrants";
        static string GG_URL_GROUPS = "?expand[]=groups";

        // Expands
        static string GG_ENTRANTS = "\"entrants\":";
        static string GG_SETS = "\"sets\":";
        static string GG_GROUPS = "\"groups\":";

        // General parameters
        static string GG_ID = "\"id\":";

        // Group parameters
        static string GG_DISPLAYIDENT = "\"displayIdentifier\":";

        // Player parameters
        static string GG_GAMERTAG = "\"gamerTag\":";
        static string GG_COUNTRY = "\"country\":";
        static string GG_PLAYERS = "\"players\":";

        // Set parameters
        static string GG_ENTRANT1 = "\"entrant1Id\":";
        static string GG_ENTRANT2 = "\"entrant2Id\":";
        static string GG_WPLACEMENT = "\"wPlacement\":";
        static string GG_LPLACEMENT = "\"lPlacement\":";
        static string GG_ROUND = "\"round\":";
        static string GG_ORIGINALROUND = "\"originalRound\":";
        static string GG_ENTRANT1SCORE = "\"entrant1Score\":";
        static string GG_ENTRANT2SCORE = "\"entrant2Score\":";
        static string GG_WINNERID = "\"winnerId\":";
        static string GG_ENTRANT1TYPE = "\"entrant1PrereqType\":";
        static string GG_ENTRANT2TYPE = "\"entrant2PrereqType\":";
        static string GG_STATE = "\"state\":";

        // Liquipedia parameters
        static string LP_P1 = "p1";
        static string LP_P2 = "p2";
        static string LP_WROUND = "r";
        static string LP_LROUND = "l";
        static string LP_MATCH = "m";
        static string LP_FLAG = "flag";
        static string LP_SCORE = "score";
        static string LP_WIN = "win";

        // Liquipedia pool tables
        static string LP_BOX_START = "{{Box|start|padding=4em}}";
        static string LP_BOX_BREAK = "{{Box|break|padding=4em}}";
        static string LP_BOX_END = "{{Box|end}}";
        static string LP_SORT_START = "{{HiddenSort|";
        static string LP_SORT_END = "}}";
        static string LP_GROUP_START = "{{GroupTableStart | ";
        static string LP_GROUP_STARTWIDTH = " |width=300px}}";
        static string LP_GROUP_END = "{{GroupTableEnd}}";
        static string LP_SLOT_START = "{{GroupTableSlot|";
        static string LP_SLOT_FLAG = " |flag=";
        static string LP_SLOT_PLACE = " |place=";
        static string LP_SLOT_MWIN = " |win_m=";
        static string LP_SLOT_MLOSS = " |lose_m=";
        static string LP_SLOT_BG = " |bg=";
        static string LP_SLOT_END = "}}";
        static string LP_CHECKMARK = "{{win}}";

        // Misc
        static int PLAYER_BYE = -1;

        Dictionary<int, Player> entrantList = new Dictionary<int, Player>();
        Dictionary<string, string> flagList = new Dictionary<string, string>();
        List<Set> setList = new List<Set>();

        public Form1()
        {
            InitializeComponent();

            // Populate flag abbreviation list
            using (StreamReader file = new StreamReader("Flag List.csv"))
            {
                while(!file.EndOfStream)
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

        private void buttonBracket_Click(object sender, EventArgs e)
        {
            // Clear data
            richTextBoxLog.Clear();
            richTextBoxEntrants.Clear();
            richTextBoxWinners.Clear();
            richTextBoxLosers.Clear();
            entrantList.Clear();
            setList.Clear();

            string webText = string.Empty;
            int endPos = 0;

            // Retrieve webpage via api
            try
            {
                WebRequest r = WebRequest.Create(GG_URL_PHASEGROUP + textBoxURL.Text + GG_URL_BRACKET);
                WebResponse resp = r.GetResponse();
                using (StreamReader sr = new StreamReader(resp.GetResponseStream()))
                {
                    webText = sr.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                richTextBoxLog.Text += ex + "\r\n";
            }

            string rawEntrants = ExpandOnly(webText, GG_ENTRANTS, 0, out endPos);
            GetEntrants(rawEntrants);

            string rawSets = ExpandOnly(webText, GG_SETS, 0, out endPos);
            GetSets(rawSets);
        }

        private void buttonFill_Click(object sender, EventArgs e)
        {
            string output = richTextBoxLiquipedia.Text;
            string bracketSide = string.Empty;

            foreach (Set set in setList)
            {
                // Detect what side of the bracket this set is in. If the corresponding checkbox is not checked, skip that side of the bracket
                if (set.round > 0 && checkBoxWinners.Checked == true)
                {
                    bracketSide = LP_WROUND;
                }
                else if (set.round < 0 && checkBoxLosers.Checked == true)
                {
                    bracketSide = LP_LROUND;
                }
                else
                {
                    continue;
                }

                // Skip unfinished sets unless otherwise specified
                if(checkBoxFillUnfinished.Checked == false && set.state == 1)
                {
                    continue;
                }

                // The set's round number must be within the bounds of the numericUpDown controls
                if (Math.Abs(set.round) >= numericUpDownStart.Value && Math.Abs(set.round) <= numericUpDownEnd.Value)
                {
                    // Offset the output round by the number specified in the numericUpDown control
                    int outputRound = Math.Abs(set.round) + (int)numericUpDownOffset.Value;

                    // Check for player byes
                    if (set.entrantID1 == PLAYER_BYE && set.entrantID2 == PLAYER_BYE)
                    {
                        // If both players are byes, skip this entry
                        continue;
                    }
                    else if (set.entrantID1 == PLAYER_BYE)
                    {
                        // Fill in player 1 as a bye
                        FillLPParameter(ref output, bracketSide + outputRound + LP_MATCH + set.match + LP_P1, "Bye");

                        // Give player 2 a checkmark
                        FillLPParameter(ref output, bracketSide + outputRound + LP_MATCH + set.match + LP_P2, entrantList[set.entrantID2].name);
                        FillLPParameter(ref output, bracketSide + outputRound + LP_MATCH + set.match + LP_P2 + LP_FLAG, entrantList[set.entrantID2].country);
                        FillLPParameter(ref output, bracketSide + outputRound + LP_MATCH + set.match + LP_P2 + LP_SCORE, LP_CHECKMARK);

                        FillLPParameter(ref output, bracketSide + outputRound + LP_MATCH + set.match + LP_WIN, "2");
                    }
                    else if (set.entrantID2 == PLAYER_BYE)
                    {
                        // Fill in player 2 as a bye
                        FillLPParameter(ref output, bracketSide + outputRound + LP_MATCH + set.match + LP_P2, "Bye");

                        // Give player 1 a checkmark
                        FillLPParameter(ref output, bracketSide + outputRound + LP_MATCH + set.match + LP_P1, entrantList[set.entrantID1].name);
                        FillLPParameter(ref output, bracketSide + outputRound + LP_MATCH + set.match + LP_P1 + LP_FLAG, entrantList[set.entrantID1].country);
                        FillLPParameter(ref output, bracketSide + outputRound + LP_MATCH + set.match + LP_P1 + LP_SCORE, LP_CHECKMARK);

                        FillLPParameter(ref output, bracketSide + outputRound + LP_MATCH + set.match + LP_WIN, "1");
                    }
                    else
                    {
                        // Fill in the set normally
                        FillLPParameter(ref output, bracketSide + outputRound + LP_MATCH + set.match + LP_P1, entrantList[set.entrantID1].name);
                        FillLPParameter(ref output, bracketSide + outputRound + LP_MATCH + set.match + LP_P1 + LP_FLAG, entrantList[set.entrantID1].country);
                        FillLPParameter(ref output, bracketSide + outputRound + LP_MATCH + set.match + LP_P2, entrantList[set.entrantID2].name);
                        FillLPParameter(ref output, bracketSide + outputRound + LP_MATCH + set.match + LP_P2 + LP_FLAG, entrantList[set.entrantID2].country);

                        // Check for DQs
                        if (set.entrant1wins == -1)
                        {
                            FillLPParameter(ref output, bracketSide + outputRound + LP_MATCH + set.match + LP_P1 + LP_SCORE, "DQ");
                            FillLPParameter(ref output, bracketSide + outputRound + LP_MATCH + set.match + LP_P2 + LP_SCORE, LP_CHECKMARK);
                        }
                        else if (set.entrant2wins == -1)
                        {
                            FillLPParameter(ref output, bracketSide + outputRound + LP_MATCH + set.match + LP_P2 + LP_SCORE, "DQ");
                            FillLPParameter(ref output, bracketSide + outputRound + LP_MATCH + set.match + LP_P1 + LP_SCORE, LP_CHECKMARK);
                        }
                        else
                        {
                            FillLPParameter(ref output, bracketSide + outputRound + LP_MATCH + set.match + LP_P1 + LP_SCORE, set.entrant1wins.ToString());
                            FillLPParameter(ref output, bracketSide + outputRound + LP_MATCH + set.match + LP_P2 + LP_SCORE, set.entrant2wins.ToString());
                        }

                        // Set the winner
                        if (set.winner == set.entrantID1)
                        {
                            FillLPParameter(ref output, bracketSide + outputRound + LP_MATCH + set.match + LP_WIN, "1");
                        }
                        else if (set.winner == set.entrantID2)
                        {
                            FillLPParameter(ref output, bracketSide + outputRound + LP_MATCH + set.match + LP_WIN, "2");
                        }
                    }
                }
            }

            richTextBoxLiquipedia.Text = output;
        }

        private void buttonPhase_Click(object sender, EventArgs e)
        {
            // Clear data
            richTextBoxLog.Clear();
            richTextBoxEntrants.Clear();
            richTextBoxWinners.Clear();
            richTextBoxLosers.Clear();
            entrantList.Clear();
            setList.Clear();

            string webText;
            int endPos = 0;

            // Retrieve webpage via api
            WebRequest r = WebRequest.Create(GG_URL_PHASE + textBoxURL.Text + GG_URL_GROUPS);
            WebResponse resp = r.GetResponse();
            using (StreamReader sr = new StreamReader(resp.GetResponseStream()))
            {
                webText = sr.ReadToEnd();
            }

            // Get a list of groups
            List<string> rawGroupList = new List<string>();
            string temp;
            string rawGroups = ExpandOnly(webText, GG_GROUPS, 0, out endPos);
            endPos = 0;
            do
            {
                temp = SplitExpand(rawGroups, endPos, out endPos);
                if (temp != string.Empty)
                {
                    rawGroupList.Add(temp);
                }
            } while (temp != string.Empty);

            List<Group> groupList = new List<Group>();
            foreach (string groupString in rawGroupList)
            {
                Group newGroup = new Group();
                newGroup.DisplayIdentifier = GetStringParameter(groupString, GG_DISPLAYIDENT);
                newGroup.id = GetIntParameter(groupString, GG_ID);

                groupList.Add(newGroup);
            }

            rawGroupList.Clear();

            // Sort the list by wave and number
            groupList = groupList.OrderBy(c => c.Wave).ThenBy(c => c.Number).ToList();

            // Retrieve pages for each group and process them
            string lastWave = string.Empty;
            for (int j = 0; j < groupList.Count; j++) 
            {
                r = WebRequest.Create(GG_URL_PHASEGROUP + groupList[j].id + GG_URL_BRACKET);
                resp = r.GetResponse();
                using (StreamReader sr = new StreamReader(resp.GetResponseStream()))
                {
                    webText = sr.ReadToEnd();
                }

                // Clear data
                entrantList.Clear();
                setList.Clear();
                richTextBoxEntrants.Clear();
                richTextBoxWinners.Clear();
                richTextBoxLosers.Clear();

                string rawEntrants = ExpandOnly(webText, GG_ENTRANTS, 0, out endPos);
                GetEntrants(rawEntrants);

                string rawSets = ExpandOnly(webText, GG_SETS, 0, out endPos);
                GetSets(rawSets);

                Dictionary<int, PoolRecord> poolData = new Dictionary<int, PoolRecord>();
                foreach (KeyValuePair<int,Player> entrant in entrantList)
                {
                    poolData.Add(entrant.Key, new PoolRecord());
                }

                foreach (Set set in setList)
                {
                    // Record match data for each player's record
                    if (set.winner == set.entrantID1)
                    {
                        poolData[set.entrantID1].matchesWin++;
                        poolData[set.entrantID2].matchesLoss++;

                        if (set.entrant2wins != -1)
                        {
                            poolData[set.entrantID1].gamesWin += set.entrant1wins;
                            poolData[set.entrantID2].gamesWin += set.entrant2wins;
                            poolData[set.entrantID1].gamesLoss += set.entrant2wins;
                            poolData[set.entrantID2].gamesLoss += set.entrant1wins;
                        }

                        if (poolData[set.entrantID1].rank == 0 || set.wPlacement < poolData[set.entrantID1].rank)
                        {
                            poolData[set.entrantID1].rank = set.wPlacement;
                        }

                        if (poolData[set.entrantID2].rank == 0 || set.lPlacement < poolData[set.entrantID2].rank)
                        {
                            poolData[set.entrantID2].rank = set.lPlacement;
                        }
                    }
                    else if (set.winner == set.entrantID2)
                    {
                        poolData[set.entrantID2].matchesWin++;
                        poolData[set.entrantID1].matchesLoss++;

                        if (set.entrant1wins != -1)
                        {
                            poolData[set.entrantID1].gamesWin += set.entrant1wins;
                            poolData[set.entrantID2].gamesWin += set.entrant2wins;
                            poolData[set.entrantID1].gamesLoss += set.entrant2wins;
                            poolData[set.entrantID2].gamesLoss += set.entrant1wins;
                        }

                        if (poolData[set.entrantID1].rank == 0 || set.lPlacement < poolData[set.entrantID1].rank)
                        {
                            poolData[set.entrantID1].rank = set.lPlacement;
                        }

                        if (poolData[set.entrantID2].rank == 0 || set.wPlacement < poolData[set.entrantID2].rank)
                        {
                            poolData[set.entrantID2].rank = set.wPlacement;
                        }
                    }
                }

                // Sort the entrants by their rank and W-L records
                poolData = poolData.OrderBy(x => x.Value.rank).ThenBy(x => x.Value.matchesLoss).ThenByDescending(x => x.Value.matchesWin).ToDictionary(x => x.Key, x => x.Value);

                // Output to textbox
                // Wave headers
                if (lastWave != groupList[j].Wave)
                {
                    richTextBoxLiquipedia.Text += "==Wave " + groupList[j].Wave + "==\r\n";
                    richTextBoxLiquipedia.Text += LP_BOX_START + "\r\n";
                    
                    lastWave = groupList[j].Wave;
                }

                // Pool headers
                richTextBoxLiquipedia.Text += LP_SORT_START + "===" + groupList[j].Wave + groupList[j].Number.ToString() + "===" + LP_SORT_END + "\r\n";
                richTextBoxLiquipedia.Text += LP_GROUP_START + "Bracket " + groupList[j].Wave + groupList[j].Number.ToString() + LP_GROUP_STARTWIDTH + "\r\n";

                // Pool slots
                int lastRank = 0;
                int lastWin = 0;
                int lastLoss = 0;
                int advance = (int)numericUpDownAdvance.Value;
                for (int i = 0; i < poolData.Count; i++)
                {
                    Player currentPlayer = entrantList[poolData.ElementAt(i).Key];
                    richTextBoxLiquipedia.Text += LP_SLOT_START + currentPlayer.name +
                                                  LP_SLOT_FLAG + currentPlayer.country +
                                                  LP_SLOT_MWIN + poolData[poolData.ElementAt(i).Key].matchesWin +
                                                  LP_SLOT_MLOSS + poolData[poolData.ElementAt(i).Key].matchesLoss;
                    if (radioButtonRoundRobin.Checked == true)
                    {
                        if (poolData[poolData.ElementAt(i).Key].matchesWin == lastWin && poolData[poolData.ElementAt(i).Key].matchesLoss == lastLoss)
                        {
                            richTextBoxLiquipedia.Text += LP_SLOT_PLACE + lastRank; 
                        }
                        else
                        {
                            richTextBoxLiquipedia.Text += LP_SLOT_PLACE + (i + 1);
                            lastRank = i + 1;
                            lastWin = poolData[poolData.ElementAt(i).Key].matchesWin;
                            lastLoss = poolData[poolData.ElementAt(i).Key].matchesLoss;
                        }
                    }
                    else if (poolData[poolData.ElementAt(i).Key].rank == -99)
                    {
                        richTextBoxLiquipedia.Text += LP_SLOT_PLACE + poolData[poolData.ElementAt(i).Key].rank;
                    }
                    else
                    {
                        richTextBoxLiquipedia.Text += LP_SLOT_PLACE;
                    }

                    if (i < advance)
                    {
                        richTextBoxLiquipedia.Text += LP_SLOT_BG + "up";
                    }
                    else
                    {
                        richTextBoxLiquipedia.Text += LP_SLOT_BG + "down";
                    }

                    richTextBoxLiquipedia.Text += LP_SLOT_END + "\r\n";
                }

                // Pool footers
                richTextBoxLiquipedia.Text += LP_GROUP_END + "\r\n";
                if (j + 1 >= groupList.Count)
                {
                    richTextBoxLiquipedia.Text += LP_BOX_END + "\r\n\r\n";
                }
                else if (groupList[j + 1].Wave != lastWave)
                {
                    richTextBoxLiquipedia.Text += LP_BOX_END + "\r\n\r\n";
                }
                else
                {
                    richTextBoxLiquipedia.Text += LP_BOX_BREAK + "\r\n\r\n";
                }
            }
        }

        private void FillLPParameter(ref string input, string param, string value)
        {
            int start = input.IndexOf("|" + param + "=");

            if (start != -1)
            {
                start += param.Length + 2;
                input = input.Insert(start, value);
            }
        }

        private string ExpandOnly(string input, string title, int startPos, out int endPos)
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

        private string SplitExpand(string input, int startPos, out int endPos)
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

        private int GetIntParameter(string input, string param)
        {
            if (input.IndexOf(param) != -1)
            {
                int start = input.IndexOf(param) + param.Length;
                int length = 0;     // Current length of int
                int temp;
                int output = -99;

                do 
                {
                    if (int.TryParse(input.Substring(start, length + 1), out temp))
                    {
                        output = temp;
                        length++;
                    }
                    else
                    {
                        // Keep going if a dash is detected since it may be a negative number.
                        if (input.Substring(start, length+1) == "-")
                        {
                            length++;
                        }
                        else if (length == 0)
                        {
                            return -99;
                        }
                        else
                        {
                            return output;
                        }
                    }
                } while (length < 15);  // Arbitrary limit

                return -99;
            }
            else
            {
                return -99;
            }
        }

        private string GetStringParameter(string input, string param)
        {
            if (input.IndexOf(param) != -1)
            {
                int start = input.IndexOf(param) + param.Length;

                // Error check for things that are not strings
                if(input.Substring(start,1) != "\"")
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

        private void GetEntrants(string input)
        {
            List<string> rawEntrantData = new List<string>();
            int endPos = 0;
            string temp;

            // Add bye info
            entrantList.Add(-1, new Player("Bye", string.Empty));

            // Divide input into manageable chunks
            do
            {
                temp = SplitExpand(input, endPos, out endPos);
                if (temp != string.Empty)
                {
                    rawEntrantData.Add(temp);
                }
            } while (temp != string.Empty);

            int padding = 0;
            foreach (string entrant in rawEntrantData)
            {
                Player newPlayer = new Player();

                // Get player ID
                int id = GetIntParameter(entrant, GG_ID);

                // Get player tag
                newPlayer.name = GetStringParameter(entrant, GG_GAMERTAG);
                newPlayer.name = System.Text.RegularExpressions.Regex.Unescape(newPlayer.name);
                if (newPlayer.name.Length > padding)
                {
                    padding = newPlayer.name.Length;
                }

                // Get player country
                temp = ExpandOnly(entrant, GG_PLAYERS, 0, out endPos);
                string country = GetStringParameter(temp, GG_COUNTRY);
                newPlayer.country = CountryAbbreviation(country);

                entrantList.Add(id, newPlayer);
            }

            // Output to textbox
            foreach (KeyValuePair<int,Player> entrant in entrantList)
            {
                if (entrant.Key == -1) continue;

                richTextBoxEntrants.Text += entrant.Key + "\t" + entrant.Value.name.PadRight(padding + 2) + "\t" + entrant.Value.country + "\r\n";
            }
        }

        private void GetSets(string input)
        {
            List<string> rawSetData = new List<string>();
            int endPos = 0;
            string temp;

            // Reset values
            checkBoxWinners.Checked = false;
            checkBoxLosers.Checked = false;
            numericUpDownStart.Value = 0;
            numericUpDownEnd.Value = 0;
            numericUpDownOffset.Value = 0;

            // Divide input into manageable chunks
            do
            {
                temp = SplitExpand(input, endPos, out endPos);
                if (temp != string.Empty)
                {
                    rawSetData.Add(temp);
                }
            } while (temp != string.Empty);

            // Get set data
            List<int> matchCountWinners = new List<int>();
            List<int> matchCountLosers = new List<int>();
            foreach (string set in rawSetData)
            {
                Set newSet = new Set();

                // Get the entrant IDs. Set the entrant as a bye if it is null.
                newSet.entrantID1 = GetIntParameter(set, GG_ENTRANT1);
                if (newSet.entrantID1 == -99)
                {
                    //if (GetStringParameter(set, GG_ENTRANT1TYPE) == "bye")
                    //{
                        newSet.entrantID1 = PLAYER_BYE;
                    //}
                }
                newSet.entrantID2 = GetIntParameter(set, GG_ENTRANT2);
                if (newSet.entrantID2 == -99)
                {
                    //if (GetStringParameter(set, GG_ENTRANT2TYPE) == "bye")
                    //{
                        newSet.entrantID2 = PLAYER_BYE;
                    //}
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
                else if(newSet.round > 0)
                {
                    while (round > matchCountWinners.Count)
                    {
                        matchCountWinners.Add(0);
                    }

                    matchCountWinners[round - 1]++;
                    newSet.match = matchCountWinners[round - 1];
                    checkBoxWinners.Checked = true;
                }
                else if (newSet.round < 0)
                {
                    while (round > matchCountLosers.Count)
                    {
                        matchCountLosers.Add(0);
                    }

                    matchCountLosers[round - 1]++;
                    newSet.match = matchCountLosers[round - 1];
                    checkBoxLosers.Checked = true;
                }

                // Remember lowest round number
                if (numericUpDownStart.Value == 0)
                {
                    numericUpDownStart.Value = round;
                }
                else if (round < numericUpDownStart.Value)
                {
                    numericUpDownStart.Value = round;
                }

                // Remember highest round number
                if (numericUpDownEnd.Value == 0)
                {
                    numericUpDownEnd.Value = round;
                }
                else if (round > numericUpDownEnd.Value)
                {
                    numericUpDownEnd.Value = round;
                }

                setList.Add(newSet);
            }

            // Set padding for textbox output
            int wpadding = 0;
            int lpadding = 0;
            foreach (Set set in setList)
            {
                if (set.round > 0)
                {
                    if (entrantList[set.entrantID1].name.Length > wpadding)
                    {
                        wpadding = entrantList[set.entrantID1].name.Length;
                    }
                }
                else
                {
                    if (entrantList[set.entrantID1].name.Length > lpadding)
                    {
                        lpadding = entrantList[set.entrantID1].name.Length;
                    }
                }
            }

            // Output to textbox
            foreach (Set set in setList)
            {
                if (checkBoxFillUnfinished.Checked == false && set.state == 1)
                {
                    continue;
                }

                if (set.round > 0)
                {
                    richTextBoxWinners.Text += set.round + "\t" + entrantList[set.entrantID1].name.PadRight(wpadding) + "\t" + 
                                               set.entrant1wins.ToString() + " - " + set.entrant2wins.ToString() + "\t" +
                                               entrantList[set.entrantID2].name + "\r\n";
                }
                else
                {
                    richTextBoxLosers.Text += set.round + "\t" + entrantList[set.entrantID1].name.PadRight(wpadding) + "\t" +
                                              set.entrant1wins.ToString() + " - " + set.entrant2wins.ToString() + "\t" +
                                              entrantList[set.entrantID2].name + "\r\n";
                }
            }
        }

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
                richTextBoxLog.Text += "Did not find: " + country + "\r\n";
                return string.Empty;
            }
        }

        class Player
        {
            public string name;
            public string country;
            
            // Blank constructor
            public Player() { }

            // Overloaded constructor
            public Player(string inputName, string inputCountry)
            {
                name = inputName;
                country = inputCountry;
            }
        }

        class Team
        {

        }

        class Set
        {
            public int entrantID1;
            public int entrantID2;
            public int entrant1wins;
            public int entrant2wins;
            public int winner;
            public int round;
            public int match;
            public int state;

            public int wPlacement;
            public int lPlacement;
        }

        class PoolRecord
        {
            public int matchesWin;
            public int matchesLoss;
            public int gamesWin;
            public int gamesLoss;
            public int rank;

            public PoolRecord()
            {
                matchesWin = 0;
                matchesLoss = 0;
                gamesWin = 0;
                gamesLoss = 0;
                rank = 0;
            }
        }

        class Group
        {
            private string displayIdentifier;
            private string wave;
            private int number;
            public int id;

            public Group()
            {
                wave = string.Empty;
                number = 0;
            }

            // When this is set, try to split the value into a letter (wave) and number (bracket number in wave)
            public string DisplayIdentifier
            {
                get
                {
                    return displayIdentifier;
                }
                set
                {
                    displayIdentifier = value;
                    
                    // Attempt to split the identifier into waves
                    if (displayIdentifier.Length > 1)
                    {
                        // The wave should be a letter
                        if (char.IsLetter(displayIdentifier,0))
                        {
                            // Ensure the rest of the identifier is a number
                            bool validNumber = true;
                            for (int i = 1; i < displayIdentifier.Length; i++)
                            {
                                if (!char.IsDigit(displayIdentifier, i))
                                {
                                    validNumber = false;
                                }
                            }

                            // If all conditions are met, fill in the wave and number variables
                            if (validNumber)
                            {
                                wave = displayIdentifier.Substring(0, 1);
                                number = int.Parse(displayIdentifier.Substring(1, displayIdentifier.Length - 1));
                            }
                        }
                    }
                }
            }

            public string Wave
            {
                get { return wave; }
            }

            public int Number
            {
                get { return number; }
            }
        }
    }
}
