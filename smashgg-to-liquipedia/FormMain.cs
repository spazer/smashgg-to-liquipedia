using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace smashgg_to_liquipedia
{
    public partial class FormMain : Form
    {
        static int PLAYER_BYE = -1;
        static string WINNERS_HEADER = "==Winners Bracket==";
        static string LOSERS_HEADER = "==Losers Bracket==";
        static string FINALS_SINGLES_HEADER = "==Final Singles Bracket==";
        static string FINALS_DOUBLES_HEADER = "==Final Doubles Bracket==";
        static string SMASH_DB_URI = "http://wiki.teamliquid.net/smash/api.php?action=parse&page=Liquipedia:Players_Regex&prop=revid|wikitext&format=json";
        static string FIGHTERS_DB_URI = "http://wiki.teamliquid.net/fighters/api.php?action=parse&page=Liquipedia:Players_Regex&prop=revid|wikitext&format=json";

        #region Bracket Template Contants
        static string deFinalBracketTemplateReset = "{{DEFinalBracket\r\n" +
                                                    "<!-- FROM WINNERS -->\r\n" +
                                                    "|r1m1p1= |r1m1p1flag= |r1m1p1score=\r\n" +
                                                    "|r1m1p2= |r1m1p2flag= |r1m1p2score=\r\n" +
                                                    "|r1m1win=\r\n\r\n" +
                                                    "<!-- FROM LOSERS -->\r\n" +
                                                    "|l1m1p1= |l1m1p1flag= |l1m1p1score=\r\n" +
                                                    "|l1m1p2= |l1m1p2flag= |l1m1p2score=\r\n" +
                                                    "|l1m1win=\r\n\r\n" +
                                                    "<!-- LOSERS FINALS -->\r\n" +
                                                    "|l2m1p1= |l2m1p1flag= |l2m1p1score=\r\n" +
                                                    "|l2m1p2= |l2m1p2flag= |l2m1p2score=\r\n" +
                                                    "|l2m1win=\r\n\r\n" +
                                                    "<!-- GRAND FINALS -->\r\n" +
                                                    "|r3m1p1= |r3m1p1flag= |r3m1p1score= |r3m2p1score=\r\n" +
                                                    "|r3m1p2= |r3m1p2flag= |r3m1p2score= |r3m2p2score=\r\n" +
                                                    "|r3m1win=\r\n" +
                                                    "}}";

        static string deFinalSmwBracketTemplateReset = "{{DEFinalSmwBracket\r\n" +
                                                    "|tourneylink=\r\n" +
                                                    "|tourneyname=\r\n" +
                                                    "|l1placement=4\r\n" +
                                                    "|r2placement=3\r\n" +
                                                    "|r3loserplacement=2\r\n" +
                                                    "|r3winnerplacement=1\r\n\r\n" +
                                                    "<!-- FROM WINNERS -->\r\n" +
                                                    "|r1m1p1= |r1m1p1flag= |r1m1p1score=\r\n" +
                                                    "|r1m1p2= |r1m1p2flag= |r1m1p2score=\r\n" +
                                                    "|r1m1win=\r\n\r\n" +
                                                    "<!-- FROM LOSERS -->\r\n" +
                                                    "|l1m1p1= |l1m1p1flag= |l1m1p1score=\r\n" +
                                                    "|l1m1p2= |l1m1p2flag= |l1m1p2score=\r\n" +
                                                    "|l1m1win=\r\n\r\n" +
                                                    "<!-- LOSERS FINALS -->\r\n" +
                                                    "|l2m1p1= |l2m1p1flag= |l2m1p1score=\r\n" +
                                                    "|l2m1p2= |l2m1p2flag= |l2m1p2score=\r\n" +
                                                    "|l2m1win=\r\n\r\n" +
                                                    "<!-- GRAND FINALS -->\r\n" +
                                                    "|r3m1p1= |r3m1p1flag= |r3m1p1score= |r3m2p1score=\r\n" +
                                                    "|r3m1p2= |r3m1p2flag= |r3m1p2score= |r3m2p2score=\r\n" +
                                                    "|r3m1win=\r\n" +
                                                    "}}";

        static string deFinalDoublesBracketTemplateReset = "{{DEFinalDoublesBracket\r\n" +
                                                        "<!-- FROM WINNERS -->\r\n" +
                                                        "|r1m1t1p1= |r1m1t1p1flag=\r\n" +
                                                        "|r1m1t1p2= |r1m1t1p2flag= |r1m1t1score=\r\n" +
                                                        "|r1m1t2p1= |r1m1t2p1flag=\r\n" +
                                                        "|r1m1t2p2= |r1m1t2p2flag= |r1m1t2score=\r\n" +
                                                        "|r1m1win=\r\n" +
                                                        "\r\n" +
                                                        "<!-- FROM LOSERS -->\r\n" +
                                                        "|l1m1t1p1= |l1m1t1p1flag=\r\n" +
                                                        "|l1m1t1p2= |l1m1t1p2flag= |l1m1t1score=\r\n" +
                                                        "|l1m1t2p1= |l1m1t2p1flag=\r\n" +
                                                        "|l1m1t2p2= |l1m1t2p2flag= |l1m1t2score=\r\n" +
                                                        "|l1m1win=\r\n" +
                                                        "\r\n" +
                                                        "<!-- LOSERS FINALS -->\r\n" +
                                                        "|l2m1t1p1= |l2m1t1p1flag=\r\n" +
                                                        "|l2m1t1p2= |l2m1t1p2flag= |l2m1t1score=\r\n" +
                                                        "|l2m1t2p1= |l2m1t2p1flag=\r\n" +
                                                        "|l2m1t2p2= |l2m1t2p2flag= |l2m1t2score=\r\n" +
                                                        "|l2m1win=\r\n" +
                                                        "\r\n" +
                                                        "<!-- GRAND FINALS -->\r\n" +
                                                        "|r3m1t1p1= |r3m1t1p1flag=\r\n" +
                                                        "|r3m1t1p2= |r3m1t1p2flag= |r3m1t1score= |r3m2t1score=\r\n" +
                                                        "|r3m1t2p1= |r3m1t2p1flag=\r\n" +
                                                        "|r3m1t2p2= |r3m1t2p2flag= |r3m1t2score= |r3m2t2score=\r\n" +
                                                        "|r3m1win=\r\n" +
                                                        "}}";

        static string deFinalDoublesSmwBracketTemplateReset = "{{DEFinalDoublesSmwBracket\r\n" +
                                                        "|tourneylink=\r\n" +
                                                        "|tourneyname=\r\n" +
                                                        "|l1placement=4\r\n" +
                                                        "|r2placement=3\r\n" +
                                                        "|r3loserplacement=2\r\n" +
                                                        "|r3winnerplacement=1\r\n\r\n" + 
                                                        "<!-- FROM WINNERS -->\r\n" +
                                                        "|r1m1t1p1= |r1m1t1p1flag=\r\n" +
                                                        "|r1m1t1p2= |r1m1t1p2flag= |r1m1t1score=\r\n" +
                                                        "|r1m1t2p1= |r1m1t2p1flag=\r\n" +
                                                        "|r1m1t2p2= |r1m1t2p2flag= |r1m1t2score=\r\n" +
                                                        "|r1m1win=\r\n" +
                                                        "\r\n" +
                                                        "<!-- FROM LOSERS -->\r\n" +
                                                        "|l1m1t1p1= |l1m1t1p1flag=\r\n" +
                                                        "|l1m1t1p2= |l1m1t1p2flag= |l1m1t1score=\r\n" +
                                                        "|l1m1t2p1= |l1m1t2p1flag=\r\n" +
                                                        "|l1m1t2p2= |l1m1t2p2flag= |l1m1t2score=\r\n" +
                                                        "|l1m1win=\r\n" +
                                                        "\r\n" +
                                                        "<!-- LOSERS FINALS -->\r\n" +
                                                        "|l2m1t1p1= |l2m1t1p1flag=\r\n" +
                                                        "|l2m1t1p2= |l2m1t1p2flag= |l2m1t1score=\r\n" +
                                                        "|l2m1t2p1= |l2m1t2p1flag=\r\n" +
                                                        "|l2m1t2p2= |l2m1t2p2flag= |l2m1t2score=\r\n" +
                                                        "|l2m1win=\r\n" +
                                                        "\r\n" +
                                                        "<!-- GRAND FINALS -->\r\n" +
                                                        "|r3m1t1p1= |r3m1t1p1flag=\r\n" +
                                                        "|r3m1t1p2= |r3m1t1p2flag= |r3m1t1score= |r3m2t1score=\r\n" +
                                                        "|r3m1t2p1= |r3m1t2p1flag=\r\n" +
                                                        "|r3m1t2p2= |r3m1t2p2flag= |r3m1t2score= |r3m2t2score=\r\n" +
                                                        "|r3m1win=\r\n" +
                                                        "}}";
        #endregion

        enum UrlNumberType { Phase, Phase_Group, None }
        enum EventType { Singles, Doubles, None }
        enum PoolType { Bracket, RoundRobin }
        public enum BracketSide { Winners, Losers }

        Dictionary<int, Entrant> entrantList = new Dictionary<int, Entrant>();
        List<Set> setList = new List<Set>();
        Dictionary<int, List<Set>> roundList = new Dictionary<int, List<Set>>();
        List<Phase> phaseList = new List<Phase>();
        Dictionary<int, int> matchOffsetPerRound = new Dictionary<int, int>();
        Dictionary<int, string> gameCharacterList = new Dictionary<int, string>();
        Dictionary<int, string> gameStageList = new Dictionary<int, string>();

        JObject tournamentStructure;
        string tournament = string.Empty;
        EventType retrievedDataType = EventType.None;

        bool loadedMatchDetailDictionary;

        LiquipediaBracket wBracket;
        LiquipediaBracket lBracket;
        LiquipediaBracket fBracket;

        PlayerDatabase playerdb;

        public FormMain()
        {
            InitializeComponent();

            SetCueText(textBoxURLSingles, FormStrings.CuetextURL);
            SetCueText(textBoxURLDoubles, FormStrings.CuetextURL);

            richTextBoxExLpWinnersBracket.Cue = FormStrings.CuetextLpWinners;
            richTextBoxExLpLosersBracket.Cue = FormStrings.CuetextLpLosers;

            richTextBoxExRegexFind.Cue = FormStrings.CuetextRegexFind;
            richTextBoxExRegexReplace.Cue = FormStrings.CuetextRegexReplace;

            wBracket = new LiquipediaBracket(WINNERS_HEADER, string.Empty);
            lBracket = new LiquipediaBracket(LOSERS_HEADER, string.Empty);
            fBracket = new LiquipediaBracket(FINALS_SINGLES_HEADER, deFinalBracketTemplateReset);

            if (radioButtonSmash.Checked == true)
            {
                playerdb = new PlayerDatabase(PlayerDatabase.DbSource.Smash);
            }
            else if (radioButtonFighters.Checked == true)
            {
                playerdb = new PlayerDatabase(PlayerDatabase.DbSource.Fighters);
            }
            UpdateRevID();
        }

        #region Buttons
        /// <summary>
        /// Indicates that user wishes to retrieve a bracket
        /// </summary>
        /// <param name="sender">N/A</param>
        /// <param name="e">N/A</param>
        private void buttonGetBracket_Click(object sender, EventArgs e)
        {
            LockControls();

            if(tabControl1.SelectedTab.Text == "Singles")
            {
                ProcessBracket(EventType.Singles);
                retrievedDataType = EventType.Singles;
            }
            else
            {
                ProcessBracket(EventType.Doubles);
                retrievedDataType = EventType.Doubles;
            }
            
            UnlockControls();
        }

        /// <summary>
        /// Indicates that user wishes to retrieve a phase
        /// </summary>
        /// <param name="sender">N/A</param>
        /// <param name="e">N/A</param>
        private void buttonGetPhase_Click(object sender, EventArgs e)
        {
            LockControls();

            if (tabControl1.SelectedTab.Text == "Singles")
            {
                ProcessPhase(EventType.Singles);
                retrievedDataType = EventType.Singles;
            }
            else
            {
                ProcessPhase(EventType.Doubles);
                retrievedDataType = EventType.Doubles;
            }

            UnlockControls();
        }

        /// <summary>
        /// Fill the liquipedia brackets with singles data
        /// </summary>
        /// <param name="sender">N/A</param>
        /// <param name="e">N/A</param>
        private void buttonFillSingles_Click(object sender, EventArgs e)
        {
            string output = string.Empty;
            string finalBracketOutput = string.Empty;
            loadedMatchDetailDictionary = false;

            if (retrievedDataType == EventType.Doubles)
            {
                richTextBoxLog.Text += "Retrieved data is doubles data\r\n";
                return;
            }

            if (checkBoxWinners.Checked == true)
            {
                if (richTextBoxExLpWinnersBracket.Text != FormStrings.CuetextLpWinners)
                {
                    if (wBracket.Header.Trim() != string.Empty)
                    {
                        output += wBracket.Header + "\r\n" + richTextBoxExLpWinnersBracket.Text + "\r\n";
                    }
                    else
                    {
                        output += richTextBoxExLpWinnersBracket.Text + "\r\n";
                    }
                }
            }

            if (checkBoxLosers.Checked == true)
            {
                if (richTextBoxExLpLosersBracket.Text != FormStrings.CuetextLpLosers)
                {
                    if (lBracket.Header.Trim() != string.Empty)
                    {
                        output += lBracket.Header +"\r\n" + richTextBoxExLpLosersBracket.Text + "\r\n";
                    }
                    else
                    {
                        output += richTextBoxExLpLosersBracket.Text + "\r\n";
                    }
                }
            }

            // If the corresponding checkbox is not checked, skip that side of the bracket
            if (checkBoxWinners.Checked)
            {
                fillBracketSingles((int)numericUpDownWinnersStart.Value, (int)numericUpDownWinnersEnd.Value, (int)numericUpDownWinnersOffset.Value, ref output);
            }
            if (checkBoxLosers.Checked)
            {
                fillBracketSingles(-(int)numericUpDownLosersStart.Value, -(int)numericUpDownLosersEnd.Value, (int)numericUpDownLosersOffset.Value, ref output);
            }
            if (checkBoxGuessFinal.Checked)
            {
                finalBracketOutput = fBracket.Bracket;

                foreach (KeyValuePair<int, List<Set>> gf in roundList)
                {
                    // Get grand finals
                    if (gf.Value[0].isGF == true)
                    {
                        // Fill in R3
                        fillBracketSingles(gf.Key, gf.Key, 3 - gf.Key, ref finalBracketOutput);

                        // Get losers finals
                        foreach (KeyValuePair<int, List<Set>> lf in roundList)
                        {
                            if (gf.Value[0].entrant2PrereqId == lf.Value[0].id)
                            {
                                // Fill in L2
                                fillBracketSingles(lf.Key, lf.Key, 2 - Math.Abs(lf.Key), ref finalBracketOutput);

                                // Get losers semis
                                foreach (KeyValuePair<int, List<Set>> ls in roundList)
                                {
                                    if (lf.Value[0].entrant2PrereqId == ls.Value[0].id)
                                    {
                                        // Fill in L1
                                        fillBracketSingles(ls.Key, ls.Key, 1 - Math.Abs(ls.Key), ref finalBracketOutput);
                                        break;
                                    }
                                }

                                break;
                            }
                        }

                        // Get winners finals
                        foreach (KeyValuePair<int, List<Set>> wf in roundList)
                        {
                            if (gf.Value[0].entrant1PrereqId == wf.Value[0].id)
                            {
                                // Fill in R1
                                fillBracketSingles(wf.Key, wf.Key, 1 - wf.Key, ref finalBracketOutput);
                                break;
                            }
                        }

                        break;
                    }
                }

                // Replace L2 with R2 because liquipedia markup is inconsistent
                finalBracketOutput = finalBracketOutput.Replace("l2m", "r2m");

                if (fBracket.Header.Trim() != string.Empty)
                {
                    output += fBracket.Header + "\r\n" + finalBracketOutput.Trim();
                }
                else
                {
                    output += finalBracketOutput.Trim();
                }
            }

            richTextBoxLpOutput.Text = output;

            buttonRegexReplace_Click(sender, e);
        }

        /// <summary>
        /// Fill the liquipedia brackets with doubles data
        /// </summary>
        /// <param name="sender">N/A</param>
        /// <param name="e">N/A</param>
        private void buttonFillDoubles_Click(object sender, EventArgs e)
        {
            if (retrievedDataType == EventType.Singles)
            {
                richTextBoxLog.Text += "Retrieved data is singles data\r\n";
                return;
            }

            string output = string.Empty;
            string finalBracketOutput = string.Empty;
            loadedMatchDetailDictionary = false;

            if (checkBoxWinners.Checked == true)
            {
                if (richTextBoxExLpWinnersBracket.Text != FormStrings.CuetextLpWinners)
                {
                    output += "==Winners Bracket==\r\n" + richTextBoxExLpWinnersBracket.Text + "\r\n";
                }
            }

            if (checkBoxLosers.Checked == true)
            {
                if (richTextBoxExLpLosersBracket.Text != FormStrings.CuetextLpLosers)
                {
                    output += "==Losers Bracket==\r\n" + richTextBoxExLpLosersBracket.Text + "\r\n";
                }
            }

            // If the corresponding checkbox is not checked, skip that side of the bracket
            if (checkBoxWinners.Checked)
            {
                fillBracketDoubles((int)numericUpDownWinnersStart.Value, (int)numericUpDownWinnersEnd.Value, (int)numericUpDownWinnersOffset.Value, ref output);
            }
            if (checkBoxLosers.Checked)
            {
                fillBracketDoubles(-(int)numericUpDownLosersStart.Value, -(int)numericUpDownLosersEnd.Value, (int)numericUpDownLosersOffset.Value, ref output);
            }
            if (checkBoxGuessFinal.Checked)
            {
                // Set the final bracket type depending on whether SMW is checked or not
                if (checkBoxSMW.Checked == true)
                {
                    finalBracketOutput = deFinalDoublesSmwBracketTemplateReset;
                }
                else
                {
                    finalBracketOutput = deFinalDoublesBracketTemplateReset;
                }

                foreach (KeyValuePair<int, List<Set>> gf in roundList)
                {
                    // Get grand finals
                    if (gf.Value[0].isGF == true)
                    {
                        // Fill in R3
                        fillBracketDoubles(gf.Key, gf.Key, 3 - gf.Key, ref finalBracketOutput);

                        // Get losers finals
                        foreach (KeyValuePair<int, List<Set>> lf in roundList)
                        {
                            if (gf.Value[0].entrant2PrereqId == lf.Value[0].id)
                            {
                                // Fill in L2
                                fillBracketDoubles(lf.Key, lf.Key, 2 - Math.Abs(lf.Key), ref finalBracketOutput);

                                // Get losers semis
                                foreach (KeyValuePair<int, List<Set>> ls in roundList)
                                {
                                    if (lf.Value[0].entrant2PrereqId == ls.Value[0].id)
                                    {
                                        // Fill in L1
                                        fillBracketDoubles(ls.Key, ls.Key, 1 - Math.Abs(ls.Key), ref finalBracketOutput);
                                        break;
                                    }
                                }

                                break;
                            }
                        }

                        // Get winners finals
                        foreach (KeyValuePair<int, List<Set>> wf in roundList)
                        {
                            if (gf.Value[0].entrant1PrereqId == wf.Value[0].id)
                            {
                                // Fill in R1
                                fillBracketDoubles(wf.Key, wf.Key, 1 - wf.Key, ref finalBracketOutput);
                                break;
                            }
                        }

                        break;
                    }
                }

                // Replace L2 with R2 because liquipedia markup is inconsistent
                finalBracketOutput = finalBracketOutput.Replace("l2m", "r2m");

                output += "==Final Doubles Bracket==\r\n" + finalBracketOutput;
            }

            richTextBoxLpOutput.Text = output;

            buttonRegexReplace_Click(sender, e);
        }

        /// <summary>
        /// Performs regex replacement on the liquipedia output textbox
        /// </summary>
        /// <param name="sender">N/A</param>
        /// <param name="e">N/A</param>
        private void buttonRegexReplace_Click(object sender, EventArgs e)
        {
            if (richTextBoxExRegexFind.Text != FormStrings.CuetextRegexFind)
            {
                string replace = string.Empty;
                if (richTextBoxExRegexReplace.Text != FormStrings.CuetextRegexReplace)
                {
                    replace = richTextBoxExRegexReplace.Text;
                }

                if (Regex.IsMatch(richTextBoxLpOutput.Text, richTextBoxExRegexFind.Text))
                {
                    richTextBoxLpOutput.Text = Regex.Replace(richTextBoxLpOutput.Text, richTextBoxExRegexFind.Text, replace);
                    richTextBoxLog.Text += "Match(es) found.\r\n";
                }
                else
                {
                    richTextBoxLog.Text += "No regex match found.\r\n";
                }
            }
        }

        /// <summary>
        /// Creates a prize pool template based on retrieved info
        /// </summary>
        /// <param name="sender">N/A</param>
        /// <param name="e">N/A</param>
        private void buttonPrizePool_Click(object sender, EventArgs e)
        {
            // Create a copy of roundList since we will be altering it
            List<SetsByRound> tempRoundList = new List<SetsByRound>();

            foreach (KeyValuePair<int, List<Set>> round in roundList)
            {
                bool roundInserted = false;
                
                // Winners bracket matches go at the start from smallest to largest
                if (round.Key > 0)
                {
                    // Iterate through the list and insert the current round before the next larger round
                    for (int i = 0; i < tempRoundList.Count; i++)
                    {
                        if (round.Key < tempRoundList[i].round)
                        {
                            tempRoundList.Insert(i, new SetsByRound(round.Key, round.Value));
                            roundInserted = true;
                        }
                    }

                    // If the round is not inserted, add it to the end of the list
                    if (!roundInserted)
                    {
                        tempRoundList.Add(new SetsByRound(round.Key, round.Value));
                    }
                }

                // Losers bracket matches go at the end from smallest magnitude to largest magnitude
                else if (round.Key < 0)
                {
                    // Iterate through the list and insert the current round before the next larger magnitude round
                    for (int i = 0; i < tempRoundList.Count; i++)
                    {
                        // Skip winners bracket rounds
                        if (tempRoundList[i].round > 0) continue;

                        if (Math.Abs(round.Key) < Math.Abs(tempRoundList[i].round))
                        {
                            tempRoundList.Insert(i, new SetsByRound(round.Key, round.Value));
                            roundInserted = true;
                        }
                    }

                    // If the round is not inserted, add it to the end of the list
                    if (!roundInserted)
                    {
                        tempRoundList.Add(new SetsByRound(round.Key, round.Value));
                    }
                }
            }

            // Move the grand finals set to the end of the list
            // Don't move anything if the grand finals are already at the end
            for (int i = 0; i < tempRoundList.Count - 1; i++)
            {
                // Only look at the first set in each round
                if (tempRoundList[i].sets[0].isGF)
                {
                    tempRoundList.Add(new SetsByRound(tempRoundList[i].round, tempRoundList[i].sets));
                    tempRoundList.RemoveAt(i);
                    break;
                }
            }

            //tempRoundList = tempRoundList.OrderBy(x => x.Key).ToDictionary(x => x.Key, x=> x.Value);

            // Set the number of entrants that the prize pool will have. Check to make sure there are enough entrants to fill the prize pool.
            int maxEntries = (int)numericUpDownPrizePool.Value;
            if (maxEntries > entrantList.Count)
            {
                richTextBoxLog.Text += "Cannot fill prize pool - Not enough entrants\r\n";
            }

            // Simple error checking
            if (!(retrievedDataType == EventType.Singles) || !(retrievedDataType == EventType.Doubles))
            {
                richTextBoxLog.Text += "Cannot fill prize pool - First retrieve data from smash.gg\r\n";
            }
            if (tempRoundList.Count <= 0)
            {
                richTextBoxLog.Text += "Cannot fill prize pool - No sets detected\r\n";
            };

            Dictionary<int, PoolRecord> record = new Dictionary<int, PoolRecord>();

            // Create a record for each player
            foreach (KeyValuePair<int, Entrant> entrant in entrantList)
            {
                record.Add(entrant.Key, new PoolRecord());
            }

            // Add data to each record based on set information
            richTextBoxLog.Text += "Figuring out ranks for each entrant\r\n";
            foreach (SetsByRound round in tempRoundList)
            {
                foreach (Set set in round.sets)
                {
                    record[set.entrantID1].isinGroup = true;
                    record[set.entrantID2].isinGroup = true;

                    // Find the winner of each match. Assume these are DE Brackets
                    if (set.winner == set.entrantID1)
                    {
                        record[set.entrantID1].rank = set.wPlacement;
                        record[set.entrantID2].rank = set.lPlacement;
                    }
                    else if (set.winner == set.entrantID2)
                    {
                        record[set.entrantID1].rank = set.lPlacement;
                        record[set.entrantID2].rank = set.wPlacement;
                    }
                }
            }

            // Remove entrants without listed sets (smash.gg seems to list extraneous entrants sometimes)
            // Also remove the bye entry
            for (int i = 0; i < record.Count; i++)
            {
                if (record.ElementAt(i).Value.isinGroup == false || record.ElementAt(i).Key == -1)
                {
                    record.Remove(record.ElementAt(i).Key);
                    i--;
                }
            }

            // Sort the entrants by their rank and W-L records
            record = record.OrderBy(x => x.Value.rank).ToDictionary(x => x.Key, x => x.Value);

            richTextBoxLog.Text += "Outputting results\r\n";

            // Ouptut the prize pool header
            if (retrievedDataType == EventType.Singles)
            {
                richTextBoxLpOutput.Text = "{{prize pool start}}\r\n";
            }
            else if (retrievedDataType == EventType.Doubles)
            {
                richTextBoxLpOutput.Text = "{{prize pool start doubles}}\r\n";
            }

            // Loop the bracket records to add entries to the prize pool
            int nextRow = 0;
            int startEntry = 0;
            while (startEntry < maxEntries)
            {
                bool rowSet = false;
                
                // Look ahead and see if there's entrants of equal rank
                for (int j = startEntry + 1; j < maxEntries; j++)
                {
                    // When a match is found, j is the next entrant with a new rank
                    if (record.ElementAt(startEntry).Value.rank != record.ElementAt(j).Value.rank)
                    {
                        nextRow = j;
                        rowSet = true;
                        break;
                    }

                    // If the end of the entrant list is reached, everything needs to be output, so set nextRow to maxEntries
                    else if (j == maxEntries - 1)
                    {
                        nextRow = maxEntries;
                        rowSet = true;
                        break;
                    }
                }

                if (!rowSet)
                {
                    nextRow = maxEntries;
                }

                // Ouptut all equal ranking entrants
                OutputPrizePoolTable(record, startEntry, nextRow - startEntry);

                // Skip all entrants that have been output
                startEntry = nextRow;
            }

            if (retrievedDataType == EventType.Singles)
            {
                richTextBoxLpOutput.Text += "{{prize pool end}}\r\n";
            }
            else if (retrievedDataType == EventType.Doubles)
            {
                richTextBoxLpOutput.Text += "{{prize pool end doubles}}\r\n";
            }

            richTextBoxLog.Text += "Done.\r\n";
        }

        /// <summary>
        /// Opens a window that allows the user to shift matches
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonWinnerShift_Click(object sender, EventArgs e)
        {
            if ((int)numericUpDownWinnersStart.Value > (int)numericUpDownWinnersEnd.Value) return;
            if ((int)numericUpDownWinnersStart.Value == 0 && (int)numericUpDownWinnersEnd.Value == 0) return;

            FormMatchShift shiftWindow = new FormMatchShift((int)numericUpDownWinnersStart.Value, (int)numericUpDownWinnersEnd.Value, BracketSide.Winners, ref matchOffsetPerRound);
            LockControls();
            shiftWindow.ShowDialog();
            UnlockControls();
        }

        /// <summary>
        /// Opens a window that allows the user to shift matches
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonloserShift_Click(object sender, EventArgs e)
        {
            if ((int)numericUpDownLosersStart.Value > (int)numericUpDownLosersEnd.Value) return;
            if ((int)numericUpDownLosersStart.Value == 0 && (int)numericUpDownLosersEnd.Value == 0) return;

            FormMatchShift shiftWindow = new FormMatchShift((int)numericUpDownLosersStart.Value, (int)numericUpDownLosersEnd.Value, BracketSide.Losers, ref matchOffsetPerRound);
            LockControls();
            shiftWindow.ShowDialog();
            UnlockControls();
        }

        /// <summary>
        /// Displays the headings that the program will use
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonHeadings_Click(object sender, EventArgs e)
        {
            LockControls();
            FormHeadings form = new FormHeadings(ref wBracket, ref lBracket, ref fBracket);
            form.ShowDialog();
            UnlockControls();
        }

        /// <summary>
        /// Retrieves a regex AKA database from Liquipedia, and parses it
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonAKA_Click(object sender, EventArgs e)
        {
            WebClient client = new WebClient();

            // Decide on the URL to use
            string json = string.Empty;
            if (radioButtonSmash.Checked)
            {
                json = client.DownloadString(SMASH_DB_URI);
            }
            else if (radioButtonFighters.Checked)
            {
                json = client.DownloadString(FIGHTERS_DB_URI);
            }
            else
            {
                return;
            }

            // Save the json to file, then read the file
            if (radioButtonSmash.Checked)
            {
                if (!playerdb.SaveDatabase(json, PlayerDatabase.DbSource.Smash))
                {
                    richTextBoxLog.Text = "Could not save Smash Database";
                }

                if (!playerdb.ReadDatabaseFromFile(PlayerDatabase.DbSource.Smash))
                {
                    richTextBoxLog.Text = "Could not retrieve Smash Database";
                }
            }
            else if (radioButtonFighters.Checked)
            {
                if (!playerdb.SaveDatabase(json, PlayerDatabase.DbSource.Fighters))
                {
                    richTextBoxLog.Text = "Could not save Fighters Database";
                }

                if (!playerdb.ReadDatabaseFromFile(PlayerDatabase.DbSource.Fighters))
                {
                    richTextBoxLog.Text = "Could not retrieve Fighters Database";
                }
            }
            else
            {
                return;
            }

            // Update the revision number
            UpdateRevID();
        }
        #endregion

        #region Data processing methods
        /// <summary>
        /// Retrieve and process smash.gg json for the singles or doubles bracket
        /// </summary>
        /// <param name="eventType">Bracket type</param>
        private void ProcessBracket(EventType eventType)
        {
            // Clear data
            ClearData();
            checkBoxWinners.Checked = false;
            checkBoxLosers.Checked = false;

            string json = string.Empty;

            // Retrieve tournament data
            UpdateTournamentStructure();

            // Get the phase group number and use it to request data
            int inputNumber;
            UrlNumberType parseResult;
            if (eventType == EventType.Singles)
            {
                parseResult = parseURL(textBoxURLSingles.Text, UrlNumberType.Phase_Group, out inputNumber);
            }
            else
            {
                parseResult = parseURL(textBoxURLDoubles.Text, UrlNumberType.Phase_Group, out inputNumber);
            }
            
            if (parseResult == UrlNumberType.Phase_Group)
            {
                if (!retrievePhaseGroup(inputNumber, out json)) return;
            }
            else if (parseResult == UrlNumberType.Phase)
            {
                foreach (Phase entry in phaseList)
                {
                    if (inputNumber == entry.phaseId)
                    {
                        // Cheat here and assume there's only one phase group
                        if (!retrievePhaseGroup(entry.id[0].id, out json))
                        {
                            richTextBoxLog.Text += "Error retrieving bracket.\r\n";
                            return;
                        }
                    }
                }
            }
            else if (parseResult == UrlNumberType.None) return;

            // Deserialize json
            JObject bracketJson = JsonConvert.DeserializeObject<JObject>(json);

            // Fill entrant and set lists
            smashgg parser = new smashgg();
            if (!parser.GetEntrants(bracketJson.SelectToken(SmashggStrings.Entities + "." + SmashggStrings.Entrants), ref entrantList, playerdb))
            {
                richTextBoxLog.Text += "No entrants detected.\r\n";
                return;
            }
            if (!parser.GetSets(bracketJson.SelectToken(SmashggStrings.Entities + "." + SmashggStrings.Sets), ref setList, checkBoxMatchDetails.Checked))
            {
                richTextBoxLog.Text += "No sets detected.\r\n";
                return;
            }

            // Fill round list based on set list
            foreach (Set currentSet in setList)
            {
                // Check if the round already exists in roundList
                if (roundList.ContainsKey(currentSet.displayRound))
                {
                    // Rounds with a larger originalRound take priority
                    if (Math.Abs(roundList[currentSet.displayRound][0].originalRound) < Math.Abs(currentSet.originalRound))
                    {
                        roundList[currentSet.displayRound].Clear();
                        roundList[currentSet.displayRound].Add(currentSet);
                    }
                    else if (Math.Abs(roundList[currentSet.displayRound][0].originalRound) > Math.Abs(currentSet.originalRound))
                    {
                        continue;
                    }
                    else
                    {
                        roundList[currentSet.displayRound].Add(currentSet);
                    }
                }
                else    // Make a new entry for the round if it doesn't exist
                {
                    // Add the current set to the newly created list
                    List<Set> newSetList = new List<Set>();
                    newSetList.Add(currentSet);

                    roundList.Add(currentSet.displayRound, newSetList);
                }
            }

            UpdateNumericUpDownControls(eventType);

            int entrantPadding = 0;
            int wPadding = 0;
            int lPadding = 0;

            // Add player name lengths together and take the biggest total. This will be the entrant padding value
            foreach (Entrant entrant in entrantList.Values)
            {
                int sum = 0;
                foreach (Player player in entrant.Players)
                {
                    sum += player.name.Length;
                }

                if (sum > entrantPadding)
                {
                    entrantPadding = sum;
                }
            }

            // Set padding for sets for textbox output
            for (int i = 0; i < roundList.Count; i++)
            {
                foreach (Set set in roundList.ElementAt(i).Value)
                {
                    if (set.originalRound > 0)
                    {
                        if (entrantList[set.entrantID1].Players[0].name.Length > wPadding)
                        {
                            wPadding = entrantList[set.entrantID1].Players[0].name.Length;
                        }
                    }
                    else
                    {
                        if (entrantList[set.entrantID1].Players[0].name.Length > lPadding)
                        {
                            lPadding = entrantList[set.entrantID1].Players[0].name.Length;
                        }
                    }
                }
            }

            // Output entrants to textbox
            outputEntrantsToTextBox(entrantPadding);

            // Sort rounds and sets
            roundList = roundList.OrderBy(x => Math.Abs(x.Key)).ToDictionary(x => x.Key, x => x.Value);
            for (int i = 0; i < roundList.Count; i++)
            {
                List<Set> tempList = roundList[roundList.ElementAt(i).Key];

                tempList = tempList.OrderBy(x => x.id).ToList();

                for (int j = 0; j < tempList.Count; j++)
                {
                    tempList[j].match = j + 1;
                }

                roundList[roundList.ElementAt(i).Key] = tempList;
            }
            
            // Output sets to textbox
            for (int i = 0; i < roundList.Count; i++)
            {
                foreach (Set set in roundList.ElementAt(i).Value)
                {
                    //if (checkBoxFillUnfinished.Checked == false && set.state == 1) continue;

                    if (set.displayRound > 0)
                    {
                        outputSetToTextBox(ref richTextBoxWinners, set, wPadding);
                    }
                    else
                    {
                        outputSetToTextBox(ref richTextBoxLosers, set, lPadding);
                    }
                }
            }

            // Generate default offsets of 0 for each round
            for (int i = 0; i < roundList.Count; i++)
            {
                matchOffsetPerRound.Add(roundList.ElementAt(i).Key, 0);
            }
        }

        /// <summary>
        /// Retrieve and process smash.gg json for the singles or doubles phase
        /// </summary>
        /// <param name="eventType">Bracket type</param>
        private void ProcessPhase(EventType eventType)
        {
            // Clear data
            ClearData();
            checkBoxWinners.Checked = false;
            checkBoxLosers.Checked = false;

            // Retrieve tournament data
            UpdateTournamentStructure();

            // Get the phase group number and use it to request data
            int phaseNumber;
            UrlNumberType parseResult;
            if (eventType == EventType.Singles)
            {
                parseResult = parseURL(textBoxURLSingles.Text, UrlNumberType.Phase, out phaseNumber);
            }
            else
            {
                parseResult = parseURL(textBoxURLDoubles.Text, UrlNumberType.Phase, out phaseNumber);
            }

            if (parseResult == UrlNumberType.Phase_Group || parseResult == UrlNumberType.None) return;

            // Find the matching phase in the tournament structure
            string json = string.Empty;
            smashgg parser = new smashgg();
            foreach (Phase phase in phaseList)
            {
                if (phase.phaseId == phaseNumber && phase.id.Count > 1)
                {
                    // Sort the list by wave and number
                    // Assume that if the first element has a wave and number, the rest do too
                    if (phase.id[0].waveNumberDetected)
                    {
                        phase.id = phase.id.OrderBy(c => c.Wave).ThenBy(c => c.Number).ToList();
                    }
                    else
                    {
                        phase.id = phase.id.OrderBy(c => c.DisplayIdentifier).ToList();
                    }

                    // Setup progress bar
                    progressBar.Minimum = 0;
                    progressBar.Maximum = phase.id.Count();
                    progressBar.Value = 1;
                    progressBar.Step = 1;

                    // Retrieve pages for each group
                    string lastWave = string.Empty;
                    List<string> waveHeaders = new List<string>();
                    for (int j = 0; j < phase.id.Count; j++)
                    {
                        // Increment the progress bar
                        progressBar.PerformStep();

                        Dictionary<int, PoolRecord> poolData = new Dictionary<int, PoolRecord>();

                        if (radioButtonBracket.Checked)
                        {
                            if (!GeneratePoolData(phase.id[j].id, parser, PoolType.Bracket, ref poolData)) continue;
                        }
                        else
                        {
                            if (!GeneratePoolData(phase.id[j].id, parser, PoolType.RoundRobin, ref poolData)) continue;
                        }

                        if (eventType == EventType.Singles)
                        {
                            OutputSinglesPhase(phase, poolData, ref lastWave, j);
                        }
                        else
                        {
                            OutputDoublesPhase(phase, poolData, ref lastWave, j);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Acquire, process, and output all data from the singles phase specified in the URL
        /// </summary>
        /// <param name="phase">Tournament phase to output</param>
        /// <param name="poolData">Pool records for each entrant</param>
        /// <param name="lastWave">Final wave in the pool</param>
        /// <param name="phaseElement">Pool in the phase to output</param>
        private void OutputSinglesPhase(Phase phase, Dictionary<int, PoolRecord> poolData, ref string lastWave, int phaseElement)
        {
            // Output to textbox
            // Wave headers
            if (phase.id[0].waveNumberDetected)
            {
                if (lastWave != phase.id[phaseElement].Wave)
                {
                    richTextBoxLpOutput.Text += "==Wave " + phase.id[phaseElement].Wave + "==\r\n";
                    richTextBoxLpOutput.Text += LpStrings.BoxStart + "\r\n";

                    lastWave = phase.id[phaseElement].Wave;
                }
            }
            else if (phaseElement == 0)    // Start a box at the first element
            {
                richTextBoxLpOutput.Text += LpStrings.BoxStart + "\r\n";
            }

            // Pool headers
            if (phase.id[0].waveNumberDetected)
            {
                richTextBoxLpOutput.Text += "===" + LpStrings.SortStart + phase.id[phaseElement].Wave + phase.id[phaseElement].Number.ToString() + LpStrings.SortEnd + "===\r\n";
                richTextBoxLpOutput.Text += LpStrings.GroupStart + "Bracket " + phase.id[phaseElement].Wave + phase.id[phaseElement].Number.ToString() + LpStrings.GroupStartWidth + "\r\n";
            }
            else
            {
                richTextBoxLpOutput.Text += "===" + LpStrings.SortStart + phase.id[phaseElement].Wave + phase.id[phaseElement].DisplayIdentifier.ToString() + LpStrings.SortEnd + "===\r\n";
                richTextBoxLpOutput.Text += LpStrings.GroupStart + "Bracket " + phase.id[phaseElement].DisplayIdentifier.ToString() + LpStrings.GroupStartWidth + "\r\n";
            }

            // Pool slots
            int lastRank = 0;
            int lastWin = 0;
            int lastLoss = 0;
            int advanceWinners = (int)numericUpDownAdvanceWinners.Value;
            int advanceLosers = (int)numericUpDownAdvanceLosers.Value;
            for (int i = 0; i < poolData.Count; i++)
            {
                // Skip bye
                if (poolData.ElementAt(i).Key == PLAYER_BYE)
                {
                    continue;
                }

                Player currentPlayer = entrantList[poolData.ElementAt(i).Key].Players[0];
                richTextBoxLpOutput.Text += LpStrings.SlotStart + currentPlayer.name +
                                                LpStrings.SlotFlag + currentPlayer.country +
                                                LpStrings.SlotMWin + poolData[poolData.ElementAt(i).Key].MatchesWin +
                                                LpStrings.SlotMLoss + poolData[poolData.ElementAt(i).Key].MatchesLoss;
                if (radioButtonRR.Checked == true)
                {
                    if (poolData[poolData.ElementAt(i).Key].MatchesWin == lastWin && poolData[poolData.ElementAt(i).Key].MatchesLoss == lastLoss)
                    {
                        richTextBoxLpOutput.Text += LpStrings.SlotPlace + lastRank;
                    }
                    else
                    {
                        richTextBoxLpOutput.Text += LpStrings.SlotPlace + (i + 1);
                        lastRank = i + 1;
                        lastWin = poolData[poolData.ElementAt(i).Key].MatchesWin;
                        lastLoss = poolData[poolData.ElementAt(i).Key].MatchesLoss;
                    }
                }
                else if (poolData[poolData.ElementAt(i).Key].rank != -99)
                {
                    richTextBoxLpOutput.Text += LpStrings.SlotPlace + poolData[poolData.ElementAt(i).Key].rank;
                }
                else
                {
                    richTextBoxLpOutput.Text += LpStrings.SlotPlace;
                }

                // Set background colors to represent the people who made it out of pools in winners/losers
                if (advanceWinners > 0)
                {
                    richTextBoxLpOutput.Text += LpStrings.SlotBg + "up";
                    advanceWinners--;
                }
                else if (advanceLosers > 0)
                {
                    richTextBoxLpOutput.Text += LpStrings.SlotBg + "stay";
                    advanceLosers--;
                }
                else
                {
                    richTextBoxLpOutput.Text += LpStrings.SlotBg + "down";
                }

                richTextBoxLpOutput.Text += LpStrings.SlotEnd + "\r\n";
            }

            // Pool footers
            richTextBoxLpOutput.Text += LpStrings.GroupEnd + "\r\n";
            if (phase.id[0].waveNumberDetected)     // Waves exist
            {
                if (phaseElement + 1 >= phase.id.Count)
                {
                    richTextBoxLpOutput.Text += LpStrings.BoxEnd + "\r\n\r\n";
                }
                else if (phase.id[phaseElement + 1].Wave != lastWave)
                {
                    richTextBoxLpOutput.Text += LpStrings.BoxEnd + "\r\n\r\n";
                }
                else
                {
                    richTextBoxLpOutput.Text += LpStrings.BoxBreak + "\r\n\r\n";
                }
            }
            else        // Waves don't exist
            {
                if (phaseElement == phase.id.Count - 1) // End box at the group end
                {
                    richTextBoxLpOutput.Text += LpStrings.BoxEnd + "\r\n\r\n";
                }
                else
                {
                    richTextBoxLpOutput.Text += LpStrings.BoxBreak + "\r\n\r\n";
                }
            }
        }

        /// <summary>
        /// Acquire, process, and output all data from the doubles phase specified in the URL
        /// </summary>
        /// <param name="phase">Phase to be processed</param>
        /// <param name="poolData">Records of each entrant in the pool</param>
        /// <param name="lastWave">Identifier of last wave</param>
        /// <param name="phaseElement">Element within the phase</param>
        private void OutputDoublesPhase(Phase phase, Dictionary<int, PoolRecord> poolData, ref string lastWave, int phaseElement)
        {
            // Output to textbox
            // Wave headers
            if (phase.id[0].waveNumberDetected)
            {
                if (lastWave != phase.id[phaseElement].Wave)
                {
                    richTextBoxLpOutput.Text += "==Wave " + phase.id[phaseElement].Wave + "==\r\n";
                    richTextBoxLpOutput.Text += LpStrings.BoxStart + "\r\n";

                    lastWave = phase.id[phaseElement].Wave;
                }
            }
            else if (phaseElement == 0)    // Start a box at the first element
            {
                richTextBoxLpOutput.Text += LpStrings.BoxStart + "\r\n";
            }

            // Pool headers
            if (phase.id[0].waveNumberDetected)
            {
                richTextBoxLpOutput.Text += LpStrings.SortStart + "===" + phase.id[phaseElement].Wave + phase.id[phaseElement].Number.ToString() + "===" + LpStrings.SortEnd + "\r\n";
                richTextBoxLpOutput.Text += LpStrings.GroupStart + "Bracket " + phase.id[phaseElement].Wave + phase.id[phaseElement].Number.ToString() + LpStrings.GroupStartWidth + "\r\n";
            }
            else
            {
                richTextBoxLpOutput.Text += LpStrings.SortStart + "===" + phase.id[phaseElement].Wave + phase.id[phaseElement].DisplayIdentifier.ToString() + "===" + LpStrings.SortEnd + "\r\n";
                richTextBoxLpOutput.Text += LpStrings.GroupStart + "Bracket " + phase.id[phaseElement].DisplayIdentifier.ToString() + LpStrings.GroupStartWidth + "\r\n";
            }

            // Pool slots
            int lastRank = 0;
            int lastMatchWin = 0;
            int lastMatchLoss = 0;
            double lastWinrate = 0.0;
            int advanceWinners = (int)numericUpDownAdvanceWinners.Value;
            int advanceLosers = (int)numericUpDownAdvanceLosers.Value;
            for (int i = 0; i < poolData.Count; i++)
            {
                // Skip bye
                if (poolData.ElementAt(i).Key == PLAYER_BYE)
                {
                    continue;
                }

                // Output players
                richTextBoxLpOutput.Text += LpStrings.DoublesSlotStart +
                                            LpStrings.DoublesSlotP1 + entrantList[poolData.ElementAt(i).Key].Players[0].name +
                                            LpStrings.DoublesSlotP1Flag + entrantList[poolData.ElementAt(i).Key].Players[0].country +
                                            LpStrings.DoublesSlotP2 + entrantList[poolData.ElementAt(i).Key].Players[1].name +
                                            LpStrings.DoublesSlotP2Flag + entrantList[poolData.ElementAt(i).Key].Players[1].country +
                                            LpStrings.SlotMWin + poolData[poolData.ElementAt(i).Key].MatchesWin +
                                            LpStrings.SlotMLoss + poolData[poolData.ElementAt(i).Key].MatchesLoss;

                if (radioButtonRR.Checked == true)
                {
                    if (poolData[poolData.ElementAt(i).Key].MatchesWin == lastMatchWin && poolData[poolData.ElementAt(i).Key].MatchesLoss == lastMatchLoss)
                    {
                        if (poolData[poolData.ElementAt(i).Key].GameWinrate == lastWinrate)
                        {
                            richTextBoxLpOutput.Text += LpStrings.SlotPlace + lastRank;
                        }
                        else
                        {
                            richTextBoxLpOutput.Text += LpStrings.SlotPlace + (i + 1);
                            lastRank = i + 1;
                            lastMatchWin = poolData[poolData.ElementAt(i).Key].MatchesWin;
                            lastMatchLoss = poolData[poolData.ElementAt(i).Key].MatchesLoss;
                            lastWinrate = poolData[poolData.ElementAt(i).Key].GameWinrate;
                        }
                    }
                    else
                    {
                        richTextBoxLpOutput.Text += LpStrings.SlotPlace + (i + 1);
                        lastRank = i + 1;
                        lastMatchWin = poolData[poolData.ElementAt(i).Key].MatchesWin;
                        lastMatchLoss = poolData[poolData.ElementAt(i).Key].MatchesLoss;
                        lastWinrate = poolData[poolData.ElementAt(i).Key].GameWinrate;
                    }
                }
                else if (poolData[poolData.ElementAt(i).Key].rank != -99)
                {
                    richTextBoxLpOutput.Text += LpStrings.SlotPlace + poolData[poolData.ElementAt(i).Key].rank;
                }
                else
                {
                    richTextBoxLpOutput.Text += LpStrings.SlotPlace;
                }

                if (advanceWinners > 0)
                {
                    richTextBoxLpOutput.Text += LpStrings.SlotBg + "up";
                    advanceWinners--;
                }
                else if (advanceLosers > 0)
                {
                    richTextBoxLpOutput.Text += LpStrings.SlotBg + "stay";
                    advanceLosers--;
                }
                else
                {
                    richTextBoxLpOutput.Text += LpStrings.SlotBg + "down";
                }

                richTextBoxLpOutput.Text += LpStrings.SlotEnd + "\r\n";
            }

            // Pool footers
            richTextBoxLpOutput.Text += LpStrings.GroupEnd + "\r\n";
            if (phase.id[0].waveNumberDetected)     // Waves exist
            {
                if (phaseElement + 1 >= phase.id.Count)
                {
                    richTextBoxLpOutput.Text += LpStrings.BoxEnd + "\r\n\r\n";
                }
                else if (phase.id[phaseElement + 1].Wave != lastWave)
                {
                    richTextBoxLpOutput.Text += LpStrings.BoxEnd + "\r\n\r\n";
                }
                else
                {
                    richTextBoxLpOutput.Text += LpStrings.BoxBreak + "\r\n\r\n";
                }
            }
            else        // Waves don't exist
            {
                if (phaseElement == phase.id.Count - 1) // End box at the group end
                {
                    richTextBoxLpOutput.Text += LpStrings.BoxEnd + "\r\n\r\n";
                }
                else
                {
                    richTextBoxLpOutput.Text += LpStrings.BoxBreak + "\r\n\r\n";
                }
            }
        }

        /// <summary>
        /// Aggregates set data into data per player in a pool
        /// </summary>
        /// <param name="phase_group">Phase group number</param>
        /// <param name="parser">Parser for retrieving/processing json</param>
        /// <param name="poolType">Specifies bracket or round robin</param>
        /// <param name="record">List of player records for the pool</param>
        /// <returns>True if it completed succesfully, false otherwise</returns>
        private bool GeneratePoolData(int phase_group, smashgg parser, PoolType poolType, ref Dictionary<int, PoolRecord> record)
        {
            // Clear data
            ClearData();

            // Get json for group
            string json;
            if (!retrievePhaseGroup(phase_group, out json))
            {
                richTextBoxLog.Text += "Error retrieving bracket " + phase_group + ".\r\n";
                return false;
            }

            JObject bracketJson = JsonConvert.DeserializeObject<JObject>(json);

            // Parse entrant and set data
            parser.GetEntrants(bracketJson.SelectToken("entities.entrants"), ref entrantList, playerdb);
            parser.GetSets(bracketJson.SelectToken("entities.sets"), ref setList, false);

            // Create a record for each player
            foreach (KeyValuePair<int, Entrant> entrant in entrantList)
            {
                record.Add(entrant.Key, new PoolRecord());
            }

            // Add data to each record based on set information
            foreach (Set set in setList)
            {
                record[set.entrantID1].isinGroup = true;
                record[set.entrantID2].isinGroup = true;

                // Record match data for each player's record
                if (set.winner == set.entrantID1)
                {
                    if (set.entrantID2 == PLAYER_BYE) continue;

                    record[set.entrantID1].AddMatchWins(1);
                    record[set.entrantID2].AddMatchLosses(1);

                    if (set.entrant2wins != -1) // Ignore W-L for DQs for now
                    {
                        record[set.entrantID1].AddGameWins(set.entrant1wins);
                        record[set.entrantID2].AddGameWins(set.entrant2wins);
                        record[set.entrantID1].AddGameLosses(set.entrant2wins);
                        record[set.entrantID2].AddGameLosses(set.entrant1wins);
                    }

                    // DE Brackets will set ranks for players
                    if (poolType == PoolType.Bracket)
                    {
                        if (record[set.entrantID1].rank == 0 || set.wPlacement < record[set.entrantID1].rank)
                        {
                            if (set.wPlacement != -99)
                            {
                                record[set.entrantID1].rank = set.wPlacement;
                            }
                        }

                        if (record[set.entrantID2].rank == 0 || set.lPlacement < record[set.entrantID2].rank)
                        {
                            if (set.lPlacement != -99)
                            {
                                record[set.entrantID2].rank = set.lPlacement;
                            }
                        }
                    }
                }
                else if (set.winner == set.entrantID2)
                {
                    if (set.entrantID1 == PLAYER_BYE) continue;

                    record[set.entrantID2].AddMatchWins(1);
                    record[set.entrantID1].AddMatchLosses(1);

                    if (set.entrant1wins != -1)
                    {
                        record[set.entrantID1].AddGameWins(set.entrant1wins);
                        record[set.entrantID2].AddGameWins(set.entrant2wins);
                        record[set.entrantID1].AddGameLosses(set.entrant2wins);
                        record[set.entrantID2].AddGameLosses(set.entrant1wins);
                    }

                    if (record[set.entrantID1].rank == 0 || set.lPlacement < record[set.entrantID1].rank)
                    {
                        if (set.lPlacement != -99)
                        {
                            record[set.entrantID1].rank = set.lPlacement;
                        }
                    }

                    if (record[set.entrantID2].rank == 0 || set.wPlacement < record[set.entrantID2].rank)
                    {
                        if (set.wPlacement != -99)
                        {
                            record[set.entrantID2].rank = set.wPlacement;
                        }
                    }
                }
            }

            // Remove entrants without listed sets (smash.gg seems to list extraneous entrants sometimes)
            for (int i = 0; i < record.Count; i++)
            {
                if (record.ElementAt(i).Value.isinGroup == false)
                {
                    record.Remove(record.ElementAt(i).Key);
                    i--;
                }
            }

            // Sort the entrants by their rank and W-L records
            record = record.OrderBy(x => x.Value.rank).ThenByDescending(x => x.Value.MatchWinrate).ThenBy(x => x.Value.MatchesLoss).ThenByDescending(x => x.Value.MatchesWin).ThenByDescending(x => x.Value.GameWinrate).ToDictionary(x => x.Key, x => x.Value);

            // Rank round robin entrants
            if (poolType == PoolType.RoundRobin)
            {
                int lastRank = 0;
                int lastMatchWin = 0;
                int lastMatchLoss = 0;
                double lastWinrate = 0.0;

                for (int i = 0; i < record.Count; i++)
                {
                    if (record.ElementAt(i).Value.MatchesWin == lastMatchWin && record.ElementAt(i).Value.MatchesLoss == lastMatchLoss)
                    {
                        if (record.ElementAt(i).Value.GameWinrate == lastWinrate)   // Equal winrates means you share the same rank
                        {
                            record.ElementAt(i).Value.rank = lastRank;
                        }
                        else
                        {
                            record.ElementAt(i).Value.rank = i + 1;
                            lastRank = i + 1;
                            lastMatchWin = record.ElementAt(i).Value.MatchesWin;
                            lastMatchLoss = record.ElementAt(i).Value.MatchesLoss;
                            lastWinrate = record.ElementAt(i).Value.GameWinrate;
                        }
                    }
                    else
                    {
                        record.ElementAt(i).Value.rank = i + 1;
                        lastRank = i + 1;
                        lastMatchWin = record.ElementAt(i).Value.MatchesWin;
                        lastMatchLoss = record.ElementAt(i).Value.MatchesLoss;
                        lastWinrate = record.ElementAt(i).Value.GameWinrate;
                    }
                } 
            }

            return true;
        }

        /// <summary>
        /// Update the tournament structure with info from the smash.gg tournament json
        /// </summary>
        private void UpdateTournamentStructure()
        {
            TextBox box;
            if (tabControl1.SelectedTab.Text == "Singles")
            {
                box = textBoxURLSingles;
            }
            else
            {
                box = textBoxURLDoubles;
            }

            try
            {
                bool validUrl = false;

                string[] splitURL = box.Text.Split(new string[1] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < splitURL.Length; i++)
                {
                    // A valid url will have smash.gg followed by tournament
                    if (splitURL[i].ToLower() == "smash.gg" && splitURL[i + 1].ToLower() == "tournament")
                    {
                        if (tournament != splitURL[i + 2])
                        {
                            tournament = splitURL[i + 2];

                            // Retrieve tournament page and get the json into a JObject
                            WebRequest r = WebRequest.Create(SmashggStrings.UrlPrefixTourney + tournament + SmashggStrings.UrlSuffixPhase);
                            WebResponse resp = r.GetResponse();
                            using (StreamReader sr = new StreamReader(resp.GetResponseStream()))
                            {
                                tournamentStructure = JsonConvert.DeserializeObject<JObject>(sr.ReadToEnd());
                                validUrl = true;
                            }

                            break;
                        }
                    }
                }

                // Build phaseList using data from tournamentStructure json
                if (validUrl)
                {
                    phaseList.Clear();
                    foreach (JToken token in tournamentStructure.SelectToken(SmashggStrings.Entities + "." + SmashggStrings.Groups))
                    {
                        // If the phase already exists, append the phase group id into the id list
                        bool phaseExists = false;
                        foreach (Phase phase in phaseList)
                        {
                            if (phase.phaseId == token[SmashggStrings.PhaseId].Value<int>())
                            {
                                PhaseGroup newPhaseGroup = new PhaseGroup();
                                newPhaseGroup.id = token[SmashggStrings.ID].Value<int>();
                                newPhaseGroup.DisplayIdentifier = token[SmashggStrings.DisplayIdent].Value<string>();

                                phase.id.Add(newPhaseGroup);
                                phaseExists = true;
                                break;
                            }
                        }

                        // If the phase does not exist, create it
                        if (!phaseExists)
                        {
                            PhaseGroup newPhaseGroup = new PhaseGroup();
                            newPhaseGroup.id = token[SmashggStrings.ID].Value<int>();
                            newPhaseGroup.DisplayIdentifier = token[SmashggStrings.DisplayIdent].Value<string>();

                            Phase newPhase = new Phase();
                            newPhase.id.Add(newPhaseGroup);
                            newPhase.phaseId = token[SmashggStrings.PhaseId].Value<int>();

                            if (!token[SmashggStrings.WaveId].IsNullOrEmpty())
                            {
                                newPhase.WaveId = token[SmashggStrings.WaveId].Value<int>();
                            }

                            phaseList.Add(newPhase);
                        }
                    }
                }
            }
            catch
            {
                richTextBoxLog.Text += "Couldn't update tournament structure.\r\n";
            }
        }
        #endregion

        #region URL Methods
        /// <summary>
        /// Returns a phase or phase_group number
        /// </summary>
        /// <param name="url">URL to parse</param>
        /// <param name="type">The requested type of number</param>
        /// <param name="output">The phase or phase_group number</param>
        /// <returns>The type of output</returns>
        private UrlNumberType parseURL(string url, UrlNumberType type, out int output)
        {
            string[] splitURL = url.Split(new string[1] { "/" }, StringSplitOptions.RemoveEmptyEntries);

            // Try getting the phase_group if it is requested
            if (type == UrlNumberType.Phase_Group)
            {
                // https://smash.gg/tournament/the-big-house-6/events/melee-singles/brackets/76014?per_page=20&filter=%7B%22phaseId%22%3A76014%2C%22id%22%3A241487%7D&sort_by=-startAt&order=-1&page=1
                // Look for filter, phaseId, id
                int index = url.IndexOf("filter=%7B");
                if (index != -1)
                {
                    if (url.IndexOf("phaseId%22") != -1)
                    {
                        int startPos = url.IndexOf("id%22%3A", index);
                        if (startPos != -1)
                        {
                            startPos += "id%22%3A".Length;
                            int endPos = url.IndexOf("%7D", startPos);

                            if (endPos != -1)
                            {
                                // Take the number
                                if (int.TryParse(url.Substring(startPos, endPos - startPos), out output))
                                {
                                    return UrlNumberType.Phase_Group;
                                }
                            }
                        }
                    }
                }


                for (int i = 0; i < splitURL.Count(); i++)
                {
                    // Phase_group number comes is the 2nd after "bracket"
                    if (splitURL[i] == "brackets" && i + 2 < splitURL.Count())
                    {
                        // Take the number
                        if (int.TryParse(splitURL[i + 2], out output))
                        {
                            return UrlNumberType.Phase_Group;
                        }
                    }
                }
            }

            // Get the phase as a fallback
            for (int i = 0; i < splitURL.Count(); i++) 
            {
                // Phase number comes after "bracket"
                if (splitURL[i] == "brackets" && i + 1 < splitURL.Count())
                {
                    // Take the number
                    if (int.TryParse(splitURL[i + 1], out output))
                    {
                        return UrlNumberType.Phase;
                    }
                }
            }

            // Both methods have failed
            output = -1;
            return UrlNumberType.None;
        }

        /// <summary>
        /// Retrieves phase_group json from smash.gg using their api
        /// </summary> 
        private bool retrievePhaseGroup(int phaseGroup, out string json)
        {
            richTextBoxLog.Text += "Retrieving phase group " + phaseGroup;

            json = string.Empty;
            try
            {
                WebRequest r = WebRequest.Create(SmashggStrings.UrlPrefixPhaseGroup + phaseGroup + SmashggStrings.UrlSuffixPhaseGroup);
                WebResponse resp = r.GetResponse();
                using (StreamReader sr = new StreamReader(resp.GetResponseStream()))
                {
                    json = sr.ReadToEnd();
                }

                richTextBoxLog.Text += " - Success\r\n";
                return true;
            }
            catch (Exception ex)
            {
                richTextBoxLog.Text += " - Fail\r\n";
                richTextBoxLog.Text += ex + "\r\n";
                return false;
            }
        }
        #endregion

        #region TextBox Output Methods
        /// <summary>
        /// Fill liquipedia bracket with singles data
        /// </summary>
        /// <param name="startRound">Start round</param>
        /// <param name="endRound">End round</param>
        /// <param name="offset">Shift the round by this integer. Left is negative. Right is positive.</param>
        /// <param name="side">Side of the bracket to fill in</param>
        /// <param name="bracketText">Liquipedia markup</param>
        private void fillBracketSingles(int startRound, int endRound, int offset, ref string bracketText)
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

                    // Try to load the match details dictionary if not done already
                    if (!loadedMatchDetailDictionary)
                    {
                        LoadDictionaryFromCSV(ref gameCharacterList, currentSet.gameId, "Character");
                        LoadDictionaryFromCSV(ref gameStageList, currentSet.gameId, "Stage");

                        loadedMatchDetailDictionary = true;
                    }

                    // Add offsets
                    outputRound = Math.Abs(i) + offset;
                    int outputSet = currentSet.match + matchOffsetPerRound[i];

                    // Check for player byes
                    if (currentSet.entrantID1 == PLAYER_BYE && currentSet.entrantID2 == PLAYER_BYE)
                    {
                        // If both players are byes, skip this entry
                        continue;
                    }
                    else if (currentSet.entrantID1 == PLAYER_BYE)
                    {
                        // Fill in player 1 as a bye if fill byes is checked
                        if (checkBoxFillByes.Checked == true) FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + outputSet + LpStrings.P1, "Bye");

                        // Give player 2 a checkmark
                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + outputSet + LpStrings.P2, entrantList[currentSet.entrantID2].Players[0].name);
                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + outputSet + LpStrings.P2 + LpStrings.Flag, entrantList[currentSet.entrantID2].Players[0].country);
                        if (checkBoxFillByeWins.Checked == true)
                        {
                            FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + outputSet + LpStrings.P2 + LpStrings.Score, LpStrings.Checkmark);
                            FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + outputSet + LpStrings.Win, "2");
                        }
                    }
                    else if (currentSet.entrantID2 == PLAYER_BYE)
                    {
                        // Fill in player 2 as a bye
                        if (checkBoxFillByes.Checked == true) FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + outputSet + LpStrings.P2, "Bye");

                        // Give player 1 a checkmark
                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + outputSet + LpStrings.P1, entrantList[currentSet.entrantID1].Players[0].name);
                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + outputSet + LpStrings.P1 + LpStrings.Flag, entrantList[currentSet.entrantID1].Players[0].country);

                        if (checkBoxFillByeWins.Checked == true)
                        {
                            FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + outputSet + LpStrings.P1 + LpStrings.Score, LpStrings.Checkmark);
                            FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + outputSet + LpStrings.Win, "1");
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
                                if (currentSet.entrant1wins != -99 && currentSet.entrant2wins != -99)
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
                            else
                            {
                                if (currentSet.entrant1wins != -99 && currentSet.entrant2wins != -99)
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
                        if (currentSet.isGF && currentSet.match == 2 && currentSet.games != null && checkBoxMatchDetails.Checked)
                        {
                            FillMatchDetailsSingles(bracketSide, outputRound, outputSet, currentSet, ref bracketText, true);
                        }
                        else if (currentSet.games != null && checkBoxMatchDetails.Checked)
                        {
                            FillMatchDetailsSingles(bracketSide, outputRound, outputSet, currentSet, ref bracketText, false);
                        }
                    }
                }
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

            if (rgxMatches[rgxMatches.Count - 1].Index != -1)
            {
                // Skip to the end of the line
                int insertionlocation = bracketText.IndexOf("\n", rgxMatches[rgxMatches.Count - 1].Index) + 1;

                string insertiontext = string.Empty;
                foreach (Game game in setData.games)
                {
                    if (!reverse)
                    {
                        // Insert character for player 1
                        string character;
                        if (gameCharacterList.ContainsKey(game.entrant1p1char))
                        {
                            character = gameCharacterList[game.entrant1p1char];
                        }
                        else if (game.entrant1p1char == -99 || game.entrant1p1char == 0)
                        {
                            character = string.Empty;
                        }
                        else
                        {
                            character = game.entrant1p1char.ToString();
                            richTextBoxLog.Text += "No character entry for number: " + character + "\r\n";
                        }

                        insertiontext += "|" + identifier + LpStrings.P1 + LpStrings.Character + game.gameOrder + "=" + character;

                        // Insert character for player 2
                        if (gameCharacterList.ContainsKey(game.entrant2p1char))
                        {
                            character = gameCharacterList[game.entrant2p1char];
                        }
                        else if (game.entrant2p1char == -99 || game.entrant2p1char == 0)
                        {
                            character = string.Empty;
                        }
                        else
                        {
                            character = game.entrant2p1char.ToString();
                            richTextBoxLog.Text += "No character entry for number: " + character + "\r\n";
                        }

                        insertiontext += " |" + identifier + LpStrings.P2 + LpStrings.Character + game.gameOrder + "=" + character;

                        // Insert stock counts
                        if (game.entrant1p1stocks == -99)
                        {
                            insertiontext += " |" + identifier + LpStrings.P1 + LpStrings.Stock + game.gameOrder + "=";
                        }
                        else
                        {
                            insertiontext += " |" + identifier + LpStrings.P1 + LpStrings.Stock + game.gameOrder + "=" + game.entrant1p1stocks;
                        }

                        if (game.entrant2p1stocks == -99)
                        {
                            insertiontext += " |" + identifier + LpStrings.P2 + LpStrings.Stock + game.gameOrder + "=";
                        }
                        else
                        {
                            insertiontext += " |" + identifier + LpStrings.P2 + LpStrings.Stock + game.gameOrder + "=" + game.entrant2p1stocks;
                        }

                        // Insert game winner
                        insertiontext += " |" + identifier + LpStrings.Win + game.gameOrder + "=";
                        if (game.winner == setData.entrantID1)
                        {
                            insertiontext += "1";
                        }
                        else if (game.winner == setData.entrantID2)
                        {
                            insertiontext += "2";
                        }

                        // Insert stage
                        string stage = string.Empty;
                        if (gameStageList.ContainsKey(game.stage))
                        {
                            stage = gameStageList[game.stage];
                        }
                        else if (game.stage == -99 || game.stage == 0)
                        {
                            stage = string.Empty;
                        }
                        else
                        {
                            stage = game.stage.ToString();
                            richTextBoxLog.Text += "No stage entry for number: " + stage + "\r\n";
                        }

                        insertiontext += " |" + identifier + LpStrings.Stage + game.gameOrder + "=" + stage + "\r\n";
                    }
                    else
                    {
                        // Insert character for player 1
                        string character;
                        if (gameCharacterList.ContainsKey(game.entrant2p1char))
                        {
                            character = gameCharacterList[game.entrant2p1char];
                        }
                        else if (game.entrant2p1char == -99)
                        {
                            character = string.Empty;
                        }
                        else
                        {
                            character = game.entrant2p1char.ToString();
                            richTextBoxLog.Text += "No character entry for number: " + character + "\r\n";
                        }

                        insertiontext += "|" + identifier + LpStrings.P1 + LpStrings.Character + game.gameOrder + "=" + character;

                        // Insert character for player 2
                        if (gameCharacterList.ContainsKey(game.entrant1p1char))
                        {
                            character = gameCharacterList[game.entrant1p1char];
                        }
                        else if (game.entrant1p1char == -99)
                        {
                            character = string.Empty;
                        }
                        else
                        {
                            character = game.entrant1p1char.ToString();
                            richTextBoxLog.Text += "No character entry for number: " + character + "\r\n";
                        }

                        insertiontext += " |" + identifier + LpStrings.P2 + LpStrings.Character + game.gameOrder + "=" + character;

                        // Insert stock counts
                        if (game.entrant2p1stocks == -99)
                        {
                            insertiontext += " |" + identifier + LpStrings.P1 + LpStrings.Stock + game.gameOrder + "=";
                        }
                        else
                        {
                            insertiontext += " |" + identifier + LpStrings.P1 + LpStrings.Stock + game.gameOrder + "=" + game.entrant2p1stocks;
                        }

                        if (game.entrant1p1stocks == -99)
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
                        else if (game.stage == -99)
                        {
                            stage = string.Empty;
                        }
                        else
                        {
                            stage = game.stage.ToString();
                            richTextBoxLog.Text += "No stage entry for number: " + stage + "\r\n";
                        }

                        insertiontext += " |" + identifier + LpStrings.Stage + game.gameOrder + "=" + stage + "\r\n";
                    }
                }

                bracketText = bracketText.Insert(insertionlocation, insertiontext + "\r\n");
            }
        }

        /// <summary>
        /// Fill liquipedia bracket with doubles data
        /// </summary>
        /// <param name="startRound">Start round</param>
        /// <param name="endRound">End round</param>
        /// <param name="offset">Offset round by this integer</param>
        /// <param name="bracketText">Liquipedia markup</param>
        private void fillBracketDoubles(int startRound, int endRound, int offset, ref string bracketText)
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
                    //if (checkBoxFillByes.Checked == false && currentSet.state == 1)
                    //{
                    //    continue;
                    //}

                    // Check for player byes
                    if (currentSet.entrantID1 == PLAYER_BYE && currentSet.entrantID2 == PLAYER_BYE)
                    {
                        // If both players are byes, skip this entry
                        continue;
                    }
                    else if (currentSet.entrantID1 == PLAYER_BYE)
                    {
                        // Fill in team 1 as a bye
                        if (checkBoxFillByes.Checked == true)
                        {
                            FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + outputSet + LpStrings.T1 + LpStrings.P1, "Bye");
                            FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + outputSet + LpStrings.T1 + LpStrings.P2, "Bye");
                        }

                        // Give team 2 a checkmark
                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + outputSet + LpStrings.T2 + LpStrings.P1, entrantList[currentSet.entrantID2].Players[0].name);
                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + outputSet + LpStrings.T2 + LpStrings.P1 + LpStrings.Flag, entrantList[currentSet.entrantID2].Players[0].country);
                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + outputSet + LpStrings.T2 + LpStrings.P2, entrantList[currentSet.entrantID2].Players[1].name);
                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + outputSet + LpStrings.T2 + LpStrings.P2 + LpStrings.Flag, entrantList[currentSet.entrantID2].Players[1].country);

                        if (checkBoxFillByeWins.Checked == true)
                        {
                            FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + outputSet + LpStrings.T2 + LpStrings.Score, LpStrings.Checkmark);
                            FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + outputSet + LpStrings.Win, "2");
                        }
                    }
                    else if (currentSet.entrantID2 == PLAYER_BYE)
                    {
                        // Fill in team 2 as a bye
                        if (checkBoxFillByes.Checked == true)
                        {
                            FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + outputSet + LpStrings.T2 + LpStrings.P1, "Bye");
                            FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + outputSet + LpStrings.T2 + LpStrings.P2, "Bye");
                        }

                        // Give team 1 a checkmark
                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + outputSet + LpStrings.T1 + LpStrings.P1, entrantList[currentSet.entrantID1].Players[0].name);
                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + outputSet + LpStrings.T1 + LpStrings.P1 + LpStrings.Flag, entrantList[currentSet.entrantID1].Players[0].country);
                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + outputSet + LpStrings.T1 + LpStrings.P2, entrantList[currentSet.entrantID1].Players[1].name);
                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + outputSet + LpStrings.T1 + LpStrings.P2 + LpStrings.Flag, entrantList[currentSet.entrantID1].Players[1].country);

                        if (checkBoxFillByeWins.Checked == true)
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
                                if (currentSet.entrant1wins != -99 && currentSet.entrant2wins != -99)
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
                                if (currentSet.entrant1wins != -99 && currentSet.entrant2wins != -99)
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
        /// Output entrants to textbox
        /// </summary>
        /// <param name="padding">Entrants padding value</param>
        private void outputEntrantsToTextBox(int padding)
        {
            // Output entrants to textbox
            foreach (KeyValuePair<int, Entrant> entrant in entrantList)
            {
                if (entrant.Key == -1) continue;

                if (entrant.Value.Players.Count == 1)
                {
                    richTextBoxEntrants.Text += entrant.Value.Players[0].playerID.ToString().PadRight(7);
                }
                else if (entrant.Value.Players.Count == 2)
                {
                    richTextBoxEntrants.Text += entrant.Value.Players[0].playerID.ToString().PadRight(7) + "/" + entrant.Value.Players[1].playerID.ToString().PadRight(7);
                }
                else return;

                int lastPlayerPadding = padding;

                // Output all players seperated by slashes
                for (int i = 0; i < entrant.Value.Players.Count; i++)
                {
                    if (i == entrant.Value.Players.Count - 1)
                    {
                        richTextBoxEntrants.Text += entrant.Value.Players[i].name.PadRight(lastPlayerPadding + (entrant.Value.Players.Count - 1) * 3);
                    }
                    else
                    {
                        richTextBoxEntrants.Text += entrant.Value.Players[i].name + " / ";
                        lastPlayerPadding -= entrant.Value.Players[i].name.Length;
                    }
                }

                richTextBoxEntrants.Text += "  ";

                // Output all countries seperated by slashes
                for (int i = 0; i < entrant.Value.Players.Count; i++)
                {
                    if (i == entrant.Value.Players.Count - 1)
                    {
                        richTextBoxEntrants.Text += entrant.Value.Players[i].country + "\r\n";
                    }
                    else
                    {
                        richTextBoxEntrants.Text += entrant.Value.Players[i].country + " / ";
                        lastPlayerPadding -= entrant.Value.Players[i].name.Length;
                    }
                }
            }
        }

        /// <summary>
        /// Output sets to textbox
        /// </summary>
        /// <param name="textbox">Target textbox</param>
        /// <param name="set">Set to output</param>
        /// <param name="p1padding">Padding value for the left side player</param>
        private void outputSetToTextBox(ref RichTextBox textbox, Set set, int p1padding)
        {
            // Output the round
            textbox.SelectionFont = new Font(textbox.Font, FontStyle.Regular);
            textbox.AppendText(set.displayRound.ToString().PadRight(4));

            // Highlight font if player 1 is the winner
            if (set.winner == set.entrantID1)
            {
                textbox.SelectionFont = new Font(textbox.Font, FontStyle.Italic);
            }
            else
            {
                textbox.SelectionFont = new Font(textbox.Font, FontStyle.Regular);
            }

            // Output player 1
            textbox.AppendText(entrantList[set.entrantID1].Players[0].name.PadRight(p1padding) + "  ");
            
            // Output player 1's score
            textbox.SelectionFont = new Font(textbox.Font, FontStyle.Regular);
            if (set.entrant1wins == -1)
            {
                textbox.AppendText("DQ");
            }
            else if (set.entrant1wins != -99)
            {
                textbox.AppendText(set.entrant1wins.ToString().PadLeft(2));
            }
            else
            {
                textbox.AppendText(" ?");
            }

            textbox.AppendText(" - ");

            // Output player 2's score
            if (set.entrant2wins == -1)
            {
                textbox.AppendText("DQ");
            }
            else if (set.entrant2wins != -99)
            {
                textbox.AppendText(set.entrant2wins.ToString().PadRight(2));
            }
            else
            {
                textbox.AppendText("? ");
            }

            // Highlight font if player 2 is the winner
            if (set.winner == set.entrantID2)
            {
                textbox.SelectionFont = new Font(textbox.Font, FontStyle.Italic);
            }
            else
            {
                textbox.SelectionFont = new Font(textbox.Font, FontStyle.Regular);
            }
            // Output player 2
            textbox.AppendText("  " + entrantList[set.entrantID2].Players[0].name + "\r\n");
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
            
            //int start = lpText.IndexOf("|" + param + "=");

            //if (start != -1)
            //{
            //    start += param.Length + 2;
            //    lpText = lpText.Insert(start, value);
            //}
        }

        /// <summary>
        /// Outputs a prize pool table based on available bracket info
        /// </summary>
        /// <param name="record">Records of the entrants in the bracket, sorted by rank</param>
        /// <param name="startEntrant">The first entrant to output in this row</param>
        /// <param name="rows">Number of rows to output</param>
        private void OutputPrizePoolTable(Dictionary<int, PoolRecord> record, int startEntrant, int rows)
        {
            // Output the start of the prize pool slot
            if (retrievedDataType == EventType.Singles)
            {
                richTextBoxLpOutput.Text += "{{prize pool slot|place=";
            }
            else if (retrievedDataType == EventType.Doubles)
            {
                richTextBoxLpOutput.Text += "{{prize pool slot doubles|place=";
            }

            // Output the place
            if (rows == 1)
            {
                richTextBoxLpOutput.Text += record.ElementAt(startEntrant).Value.rank;
            }
            else
            {
                richTextBoxLpOutput.Text += record.ElementAt(startEntrant).Value.rank + "-" + (record.ElementAt(startEntrant).Value.rank + rows - 1);
            }

            // Ouptut the parameter for prize money
            richTextBoxLpOutput.Text += "|usdprize=\r\n";


            if (retrievedDataType == EventType.Singles)
            {
                // Output the specified number of entrants
                for (int i = 0; i < rows; i++)
                {
                    // Assume there is only 1 player output their info
                    richTextBoxLpOutput.Text += "|" + entrantList[record.ElementAt(startEntrant + i).Key].Players[0].name +
                                                "|flag" + (i + 1) + "=" + entrantList[record.ElementAt(startEntrant + i).Key].Players[0].country +
                                                "|heads" + (i + 1) + "= |team" + (i + 1) + "=\r\n";
                }
            }
            else if (retrievedDataType == EventType.Doubles)
            {
                // Output the specified number of entrants
                for (int i = 0; i < rows; i++)
                {
                    // Assume there are 2 players, and output their info
                    richTextBoxLpOutput.Text += "|" + entrantList[record.ElementAt(startEntrant + i).Key].Players[0].name +
                                                "|flag" + (i + 1) + "p1=" + entrantList[record.ElementAt(startEntrant + i).Key].Players[0].country +
                                                "|heads" + (i + 1) + "p1=" +
                                                "|" + entrantList[record.ElementAt(startEntrant + i).Key].Players[1].name +
                                                "|flag" + (i + 1) + "p2=" + entrantList[record.ElementAt(startEntrant + i).Key].Players[1].country +
                                                "|heads" + (i + 1) + "p2=\r\n";
                }
            }

            richTextBoxLpOutput.Text += "}}\r\n";
        }
        #endregion

        #region Cue Banner
        // https://jasonkemp.ca/blog/the-missing-net-1-cue-banners-in-windows-forms-em_setcuebanner-text-prompt/
        private const int EM_SETCUEBANNER = 0x1501;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern Int32 SendMessage(IntPtr hWnd, int msg,
        int wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);

        public static void SetCueText(Control control, string text)
        {
            SendMessage(control.Handle, EM_SETCUEBANNER, 0, text);
        }
        #endregion

        #region Form methods
        /// <summary>
        /// Lock numericUpDown controls when the checkbox is checked
        /// </summary>
        /// <param name="sender">Checkbox</param>
        /// <param name="e"></param>
        private void checkBoxLock_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxLockWinners.Checked)
            {
                numericUpDownWinnersStart.Enabled = false;
                numericUpDownWinnersEnd.Enabled = false;
                numericUpDownWinnersOffset.Enabled = false;
            }
            else
            {
                numericUpDownWinnersStart.Enabled = true;
                numericUpDownWinnersEnd.Enabled = true;
                numericUpDownWinnersOffset.Enabled = true;
            }
            if (checkBoxLockLosers.Checked)
            {
                numericUpDownLosersStart.Enabled = false;
                numericUpDownLosersEnd.Enabled = false;
                numericUpDownLosersOffset.Enabled = false;
            }
            else
            {
                numericUpDownLosersStart.Enabled = true;
                numericUpDownLosersEnd.Enabled = true;
                numericUpDownLosersOffset.Enabled = true;
            }
        }

        /// <summary>
        /// Clear all textboxes, entrantList, setList, and roundList. tournamentStructure, tournament, and phaseList are untouched.
        /// </summary>
        private void ClearData()
        {
            richTextBoxLog.Clear();
            richTextBoxEntrants.Clear();
            richTextBoxWinners.Clear();
            richTextBoxLosers.Clear();
            entrantList.Clear();
            setList.Clear();
            roundList.Clear();
            matchOffsetPerRound.Clear();
        }

        /// <summary>
        /// Update the numericUpDown controls
        /// </summary>
        private void UpdateNumericUpDownControls(EventType eventType)
        {
            // Clear winners side numericUpDown controls unless it's locked
            if (!checkBoxLockWinners.Checked)
            {
                numericUpDownWinnersStart.Value = 0;
                numericUpDownWinnersEnd.Value = 0;
                numericUpDownWinnersOffset.Value = 0;
            }

            // Clear losers side numericUpDown controls unless it's locked
            if (!checkBoxLockLosers.Checked)
            {
                numericUpDownLosersStart.Value = 0;
                numericUpDownLosersEnd.Value = 0;
                numericUpDownLosersOffset.Value = 0;
            }

            // Set values in form controls
            foreach (int displayRound in roundList.Keys.ToList<int>())
            {
                int round = Math.Abs(displayRound);

                if (displayRound > 0)
                {
                    if (eventType == EventType.Singles)
                    {
                        checkBoxWinners.Checked = true;
                    }
                    else
                    {
                        checkBoxWinners.Checked = true;
                    }

                    if (checkBoxLockWinners.Checked) continue;

                    // Remember lowest round number
                    if (numericUpDownWinnersStart.Value == 0)
                    {
                        numericUpDownWinnersStart.Value = round;
                    }
                    else if (round < numericUpDownWinnersStart.Value)
                    {
                        numericUpDownWinnersStart.Value = round;
                    }

                    // Remember highest round number
                    if (numericUpDownWinnersEnd.Value == 0)
                    {
                        numericUpDownWinnersEnd.Value = round;
                    }
                    else if (round > numericUpDownWinnersEnd.Value)
                    {
                        numericUpDownWinnersEnd.Value = round;
                    }
                }
                else if (displayRound < 0)
                {
                    if (eventType == EventType.Singles)
                    {
                        checkBoxLosers.Checked = true;
                    }
                    else
                    {
                        checkBoxLosers.Checked = true;
                    }

                    if (checkBoxLockLosers.Checked) continue;

                    // Remember lowest round number
                    if (numericUpDownLosersStart.Value == 0)
                    {
                        numericUpDownLosersStart.Value = round;
                    }
                    else if (round < numericUpDownLosersStart.Value)
                    {
                        numericUpDownLosersStart.Value = round;
                    }

                    // Remember highest round number
                    if (numericUpDownLosersEnd.Value == 0)
                    {
                        numericUpDownLosersEnd.Value = round;
                    }
                    else if (round > numericUpDownLosersEnd.Value)
                    {
                        numericUpDownLosersEnd.Value = round;
                    }
                }
            }
        }

        /// <summary>
        /// Attempts to highlight all text in a textbox when given focus. Doesn't work consistently.
        /// </summary>
        /// <param name="sender">Textboxt to select all text in</param>
        /// <param name="e">N/A</param>
        private void textBoxURL_Enter(object sender, EventArgs e)
        {
            TextBox box = (TextBox)sender;

            BeginInvoke((Action)delegate
            {
                box.SelectAll();
            });
        }

        /// <summary>
        /// Locks form controls
        /// </summary>
        private void LockControls()
        {
            tabControl1.Enabled = false;
            textBoxURLDoubles.Enabled = false;
            textBoxURLSingles.Enabled = false;

            buttonFill.Enabled = false;
            buttonFillDoubles.Enabled = false;
            buttonGetPhase.Enabled = false;
            buttonRegexReplace.Enabled = false;
            buttonGetBracket.Enabled = false;
            buttonPrizePool.Enabled = false;
            buttonWinnerShift.Enabled = false;
            buttonLoserShift.Enabled = false;
            buttonHeadings.Enabled = false;

            checkBoxFillByes.Enabled = false;
            checkBoxGuessFinal.Enabled = false;
            checkBoxLockLosers.Enabled = false;
            checkBoxLockWinners.Enabled = false;
            checkBoxLosers.Enabled = false;
            checkBoxWinners.Enabled = false;
            checkBoxSMW.Enabled = false;
            checkBoxFillByeWins.Enabled = false;
            checkBoxMatchDetails.Enabled = false;

            numericUpDownAdvanceWinners.Enabled = false;
            numericUpDownAdvanceWinners.Enabled = false;
            numericUpDownAdvanceLosers.Enabled = false;
            numericUpDownAdvanceLosers.Enabled = false;
            numericUpDownLosersEnd.Enabled = false;
            numericUpDownLosersOffset.Enabled = false;
            numericUpDownLosersStart.Enabled = false;
            numericUpDownWinnersEnd.Enabled = false;
            numericUpDownWinnersOffset.Enabled = false;
            numericUpDownWinnersStart.Enabled = false;
            numericUpDownPrizePool.Enabled = false;

            richTextBoxEntrants.Enabled = false;
            richTextBoxExLpLosersBracket.Enabled = false;
            richTextBoxExLpWinnersBracket.Enabled = false;
            richTextBoxExRegexFind.Enabled = false;
            richTextBoxExRegexReplace.Enabled = false;
            richTextBoxLog.Enabled = false;
            richTextBoxWinners.Enabled = false;
            richTextBoxLosers.Enabled = false;
            richTextBoxLpOutput.Enabled = false;
        }

        /// <summary>
        /// Unlocks form controls
        /// </summary>
        private void UnlockControls()
        {
            tabControl1.Enabled = true;
            textBoxURLDoubles.Enabled = true;
            textBoxURLSingles.Enabled = true;

            buttonFill.Enabled = true;
            buttonFillDoubles.Enabled = true;
            buttonGetPhase.Enabled = true;
            buttonRegexReplace.Enabled = true;
            buttonGetBracket.Enabled = true;
            buttonPrizePool.Enabled = true;
            buttonWinnerShift.Enabled = true;
            buttonLoserShift.Enabled = true;
            buttonHeadings.Enabled = true;

            checkBoxFillByes.Enabled = true;
            checkBoxGuessFinal.Enabled = true;
            checkBoxLockLosers.Enabled = true;
            checkBoxLockWinners.Enabled = true;
            checkBoxLosers.Enabled = true;
            checkBoxWinners.Enabled = true;
            checkBoxSMW.Enabled = true;
            checkBoxFillByeWins.Enabled = true;
            checkBoxMatchDetails.Enabled = true;

            numericUpDownAdvanceWinners.Enabled = true;
            numericUpDownAdvanceWinners.Enabled = true;
            numericUpDownAdvanceLosers.Enabled = true;
            numericUpDownAdvanceLosers.Enabled = true;
            numericUpDownLosersEnd.Enabled = true;
            numericUpDownLosersOffset.Enabled = true;
            numericUpDownLosersStart.Enabled = true;
            numericUpDownWinnersEnd.Enabled = true;
            numericUpDownWinnersOffset.Enabled = true;
            numericUpDownWinnersStart.Enabled = true;
            numericUpDownPrizePool.Enabled = true;

            richTextBoxEntrants.Enabled = true;
            richTextBoxExLpLosersBracket.Enabled = true;
            richTextBoxExLpWinnersBracket.Enabled = true;
            richTextBoxExRegexFind.Enabled = true;
            richTextBoxExRegexReplace.Enabled = true;
            richTextBoxLog.Enabled = true;
            richTextBoxWinners.Enabled = true;
            richTextBoxLosers.Enabled = true;
            richTextBoxLpOutput.Enabled = true;

            // Re-lock numericUpDown controls if needed
            checkBoxLock_CheckedChanged(new object(), new EventArgs());
        }
        #endregion

        #region Header Selection Methods
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab.Text == "Singles")
            {
                fBracket.Header = FINALS_SINGLES_HEADER;

                if (checkBoxSMW.Checked)
                {
                    fBracket.Bracket = deFinalSmwBracketTemplateReset;
                }
                else
                {
                    fBracket.Bracket = deFinalBracketTemplateReset;
                }
            }
            else if (tabControl1.SelectedTab.Text == "Doubles")
            {
                fBracket.Header = FINALS_DOUBLES_HEADER;

                if (checkBoxSMW.Checked)
                {
                    fBracket.Bracket = deFinalDoublesSmwBracketTemplateReset;
                }
                else
                {
                    fBracket.Bracket = deFinalDoublesBracketTemplateReset;
                }
            }
        }

        private void checkBoxSMW_CheckedChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab.Text == "Singles")
            {
                if (checkBoxSMW.Checked)
                {
                    fBracket.Bracket = deFinalSmwBracketTemplateReset;
                }
                else
                {
                    fBracket.Bracket = deFinalBracketTemplateReset;
                }
            }
            else if (tabControl1.SelectedTab.Text == "Doubles")
            {
                if (checkBoxSMW.Checked)
                {
                    fBracket.Bracket = deFinalDoublesSmwBracketTemplateReset;
                }
                else
                {
                    fBracket.Bracket = deFinalDoublesBracketTemplateReset;
                }
            }
        }
        #endregion

        /// <summary>
        /// Updates the revision number of the AKA database
        /// </summary>
        private void UpdateRevID()
        {
            if (playerdb == null) return;

            int revID = playerdb.RevID;

            if (revID == 0)
            {
                labelAkaDatabaseRev.Text = "Rev: none";
            }
            else
            {
                labelAkaDatabaseRev.Text = "Rev: " + revID.ToString();
            }
        }

        /// <summary>
        /// Switch the AKA database when the radio buttons are changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButtonDatabase_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonSmash.Checked)
            {
                if (!playerdb.ReadDatabaseFromFile(PlayerDatabase.DbSource.Smash))
                {
                    richTextBoxLog.Text = "Could not retrieve Smash Database\r\n";
                }
            }
            else if (radioButtonFighters.Checked)
            {
                if (!playerdb.ReadDatabaseFromFile(PlayerDatabase.DbSource.Fighters))
                {
                    richTextBoxLog.Text = "Could not retrieve Fighters Database\r\n";
                }
            }
            else
            {
                return;
            }

            UpdateRevID();
        }

        /// <summary>
        /// Loads a dictionary with values from a CSV file
        /// </summary>
        /// <param name="targetDictionary">The dictionary to fill</param>
        /// <param name="gameId">ID of the game. Melee = 1, Wii U = 3, 64 = 4</param>
        /// <param name="datatype"></param>
        private void LoadDictionaryFromCSV(ref Dictionary<int,string> targetDictionary, int gameId, string datatype)
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
                                richTextBoxLog.Text += "Invalid "+ datatype +" number in: " + input + "\r\n";
                            }
                        }
                        else
                        {
                            richTextBoxLog.Text += "Couldn't parse " + datatype + " entry: " + input + "\r\n";
                        }
                    }
                }
            }
            catch
            {
                richTextBoxLog.Text += "Couldn't find CSV file: " + filename + "\r\n";
            }
        }
    }
}
