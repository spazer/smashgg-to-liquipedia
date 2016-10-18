﻿using System;
using System.Collections.Generic;
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

namespace smashgg_api
{
    public partial class Form1 : Form
    {
        static int PLAYER_BYE = -1;
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

        static string deFinalBracketTemplate =  "{{DEFinalBracket\r\n" +
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
                                                "|r3m1p1= |r3m1p1flag= |r3m1p1score=\r\n" +
                                                "|r3m1p2= |r3m1p2flag= |r3m1p2score=\r\n" +
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

        static string deFinalDoublesBracketTemplate = "{{DEFinalDoublesBracket\r\n" +
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
                                                            "|r3m1t1p2= |r3m1t1p2flag= |r3m1t1score=\r\n" +
                                                            "|r3m1t2p1= |r3m1t2p1flag=\r\n" +
                                                            "|r3m1t2p2= |r3m1t2p2flag= |r3m1t2score=\r\n" +
                                                            "|r3m1win=\r\n" +
                                                            "}}";
        #endregion

        enum UrlNumberType { Phase, Phase_Group, None }
        enum EventType { Singles, Doubles }
        enum PoolType { Bracket, RoundRobin }
        enum BracketSide { Winners, Losers }

        Dictionary<int, Entrant> entrantList = new Dictionary<int, Entrant>();
        List<Set> setList = new List<Set>();
        Dictionary<int, List<Set>> roundList = new Dictionary<int, List<Set>>();
        List<Phase> phaseList = new List<Phase>();

        JObject tournamentStructure;
        string tournament = string.Empty;

        public Form1()
        {
            InitializeComponent();

            SetCueText(textBoxURLSingles, FormStrings.CuetextURL);
            SetCueText(textBoxURLDoubles, FormStrings.CuetextURL);

            richTextBoxExLpWinnersBracket.Cue = FormStrings.CuetextLpWinners;
            richTextBoxExLpLosersBracket.Cue = FormStrings.CuetextLpLosers;

            richTextBoxExRegexFind.Cue = FormStrings.CuetextRegexFind;
            richTextBoxExRegexReplace.Cue = FormStrings.CuetextRegexReplace;
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
            }
            else
            {
                ProcessBracket(EventType.Doubles);
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
            }
            else
            {
                ProcessPhase(EventType.Doubles);
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

            if (richTextBoxExLpWinnersBracket.Text != FormStrings.CuetextLpWinners)
            {
                output += "==Winners Bracket==\r\n" + richTextBoxExLpWinnersBracket.Text + "\r\n";
            }

            if (richTextBoxExLpLosersBracket.Text != FormStrings.CuetextLpLosers)
            {
                output += "==Losers Bracket==\r\n" + richTextBoxExLpLosersBracket.Text + "\r\n";
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
                finalBracketOutput = deFinalBracketTemplate;

                foreach (KeyValuePair<int, List<Set>> gf in roundList)
                {
                    // Get grand finals
                    if (gf.Value[0].isGF == true)
                    {
                        if (gf.Value.Count > 1)
                        {
                            finalBracketOutput = deFinalBracketTemplateReset;
                        }

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

                output += "==Final Singles Bracket==\r\n" + finalBracketOutput;
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
            string output = string.Empty;
            string finalBracketOutput = string.Empty;

            if (richTextBoxExLpWinnersBracket.Text != FormStrings.CuetextLpWinners)
            {
                output += "==Winners Bracket==\r\n" + richTextBoxExLpWinnersBracket.Text + "\r\n";
            }

            if (richTextBoxExLpLosersBracket.Text != FormStrings.CuetextLpLosers)
            {
                output += "==Losers Bracket==\r\n" + richTextBoxExLpLosersBracket.Text + "\r\n";
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
                finalBracketOutput = deFinalDoublesBracketTemplate;

                foreach (KeyValuePair<int, List<Set>> gf in roundList)
                {
                    // Get grand finals
                    if (gf.Value[0].isGF == true)
                    {
                        if (gf.Value.Count > 1)
                        {
                            if (gf.Value[1].state == 3)
                            {
                                finalBracketOutput = deFinalDoublesBracketTemplateReset;
                            }
                        }

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
            if (!parser.GetEntrants(bracketJson.SelectToken(SmashggStrings.Entities + "." + SmashggStrings.Entrants), ref entrantList))
            {
                richTextBoxLog.Text += "No entrants detected.\r\n";
                return;
            }
            if (!parser.GetSets(bracketJson.SelectToken(SmashggStrings.Entities + "." + SmashggStrings.Sets), ref setList))
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

            // Output sets to textbox
            for (int i = 0; i < roundList.Count; i++)
            {
                foreach (Set set in roundList.ElementAt(i).Value)
                {
                    if (checkBoxFillUnfinished.Checked == false && set.state == 1) continue;

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
                            OutputSinglesPhase(phase, poolData, lastWave, j);
                        }
                        else
                        {
                            OutputDoublesPhase(phase, poolData, lastWave, j);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Acquire, process, and output all data from the singles phase specified in the URL
        /// </summary>
        private void OutputSinglesPhase(Phase phase, Dictionary<int, PoolRecord> poolData, string lastWave, int phaseElement)
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
            int lastWin = 0;
            int lastLoss = 0;
            int advance = (int)numericUpDownAdvanceWinners.Value;
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
                                                LpStrings.SlotMWin + poolData[poolData.ElementAt(i).Key].matchesWin +
                                                LpStrings.SlotMLoss + poolData[poolData.ElementAt(i).Key].matchesLoss;
                if (radioButtonRR.Checked == true)
                {
                    if (poolData[poolData.ElementAt(i).Key].matchesWin == lastWin && poolData[poolData.ElementAt(i).Key].matchesLoss == lastLoss)
                    {
                        richTextBoxLpOutput.Text += LpStrings.SlotPlace + lastRank;
                    }
                    else
                    {
                        richTextBoxLpOutput.Text += LpStrings.SlotPlace + (i + 1);
                        lastRank = i + 1;
                        lastWin = poolData[poolData.ElementAt(i).Key].matchesWin;
                        lastLoss = poolData[poolData.ElementAt(i).Key].matchesLoss;
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

                if (advance > 0)
                {
                    richTextBoxLpOutput.Text += LpStrings.SlotBg + "up";
                    advance--;
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
        private void OutputDoublesPhase(Phase phase, Dictionary<int, PoolRecord> poolData, string lastWave, int phaseElement)
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
            int advance = (int)numericUpDownAdvanceWinners.Value;
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
                                            LpStrings.SlotMWin + poolData[poolData.ElementAt(i).Key].matchesWin +
                                            LpStrings.SlotMLoss + poolData[poolData.ElementAt(i).Key].matchesLoss;

                if (radioButtonRR.Checked == true)
                {
                    if (poolData[poolData.ElementAt(i).Key].matchesWin == lastMatchWin && poolData[poolData.ElementAt(i).Key].matchesLoss == lastMatchLoss)
                    {
                        if (poolData[poolData.ElementAt(i).Key].GameWinrate == lastWinrate)
                        {
                            richTextBoxLpOutput.Text += LpStrings.SlotPlace + lastRank;
                        }
                        else
                        {
                            richTextBoxLpOutput.Text += LpStrings.SlotPlace + (i + 1);
                            lastRank = i + 1;
                            lastMatchWin = poolData[poolData.ElementAt(i).Key].matchesWin;
                            lastMatchLoss = poolData[poolData.ElementAt(i).Key].matchesLoss;
                            lastWinrate = poolData[poolData.ElementAt(i).Key].GameWinrate;
                        }
                    }
                    else
                    {
                        richTextBoxLpOutput.Text += LpStrings.SlotPlace + (i + 1);
                        lastRank = i + 1;
                        lastMatchWin = poolData[poolData.ElementAt(i).Key].matchesWin;
                        lastMatchLoss = poolData[poolData.ElementAt(i).Key].matchesLoss;
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

                if (advance > 0)
                {
                    richTextBoxLpOutput.Text += LpStrings.SlotBg + "up";
                    advance--;
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
            parser.GetEntrants(bracketJson.SelectToken("entities.entrants"), ref entrantList);
            parser.GetSets(bracketJson.SelectToken("entities.sets"), ref setList);

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

                    record[set.entrantID1].matchesWin++;
                    record[set.entrantID2].matchesLoss++;

                    if (set.entrant2wins != -1) // Ignore W-L for DQs for now
                    {
                        record[set.entrantID1].AddWins(set.entrant1wins);
                        record[set.entrantID2].AddWins(set.entrant2wins);
                        record[set.entrantID1].AddLosses(set.entrant2wins);
                        record[set.entrantID2].AddLosses(set.entrant1wins);
                    }

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

                    record[set.entrantID2].matchesWin++;
                    record[set.entrantID1].matchesLoss++;

                    if (set.entrant1wins != -1)
                    {
                        record[set.entrantID1].AddWins(set.entrant1wins);
                        record[set.entrantID2].AddWins(set.entrant2wins);
                        record[set.entrantID1].AddLosses(set.entrant2wins);
                        record[set.entrantID2].AddLosses(set.entrant1wins);
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
            record = record.OrderBy(x => x.Value.rank).ThenBy(x => x.Value.matchesLoss).ThenByDescending(x => x.Value.matchesWin).ThenByDescending(x => x.Value.GameWinrate).ToDictionary(x => x.Key, x => x.Value);

            // Rank round robin entrants
            if (poolType == PoolType.RoundRobin)
            {
                int lastRank = 0;
                int lastMatchWin = 0;
                int lastMatchLoss = 0;
                double lastWinrate = 0.0;

                for (int i = 0; i < record.Count; i++)
                {
                    if (record.ElementAt(i).Value.matchesWin == lastMatchWin && record.ElementAt(i).Value.matchesLoss == lastMatchLoss)
                    {
                        if (record.ElementAt(i).Value.GameWinrate == lastWinrate)
                        {
                            record.ElementAt(i).Value.rank = lastRank;
                        }
                        else
                        {
                            record.ElementAt(i).Value.rank = i + 1;
                            lastRank = i + 1;
                            lastMatchWin = record.ElementAt(i).Value.matchesWin;
                            lastMatchLoss = record.ElementAt(i).Value.matchesLoss;
                            lastWinrate = record.ElementAt(i).Value.GameWinrate;
                        }
                    }
                    else
                    {
                        record.ElementAt(i).Value.rank = i + 1;
                        lastRank = i + 1;
                        lastMatchWin = record.ElementAt(i).Value.matchesWin;
                        lastMatchLoss = record.ElementAt(i).Value.matchesLoss;
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

                    outputRound = Math.Abs(i) + offset;

                    // Skip unfinished sets unless otherwise specified
                    if (checkBoxFillUnfinished.Checked == false && currentSet.state == 1)
                    {
                        continue;
                    }

                    // Check for player byes
                    if (currentSet.entrantID1 == PLAYER_BYE && currentSet.entrantID2 == PLAYER_BYE)
                    {
                        // If both players are byes, skip this entry
                        continue;
                    }
                    else if (currentSet.entrantID1 == PLAYER_BYE)
                    {
                        // Fill in player 1 as a bye
                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + currentSet.match + LpStrings.P1, "Bye");

                        // Give player 2 a checkmark
                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + currentSet.match + LpStrings.P2, entrantList[currentSet.entrantID2].Players[0].name);
                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + currentSet.match + LpStrings.P2 + LpStrings.Flag, entrantList[currentSet.entrantID2].Players[0].country);
                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + currentSet.match + LpStrings.P2 + LpStrings.Score, LpStrings.Checkmark);

                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + currentSet.match + LpStrings.Win, "2");
                    }
                    else if (currentSet.entrantID2 == PLAYER_BYE)
                    {
                        // Fill in player 2 as a bye
                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + currentSet.match + LpStrings.P2, "Bye");

                        // Give player 1 a checkmark
                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + currentSet.match + LpStrings.P1, entrantList[currentSet.entrantID1].Players[0].name);
                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + currentSet.match + LpStrings.P1 + LpStrings.Flag, entrantList[currentSet.entrantID1].Players[0].country);
                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + currentSet.match + LpStrings.P1 + LpStrings.Score, LpStrings.Checkmark);

                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + currentSet.match + LpStrings.Win, "1");
                    }
                    else
                    {
                        // Fill in the set normally
                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + currentSet.match + LpStrings.P1, entrantList[currentSet.entrantID1].Players[0].name);
                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + currentSet.match + LpStrings.P1 + LpStrings.Flag, entrantList[currentSet.entrantID1].Players[0].country);
                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + currentSet.match + LpStrings.P2, entrantList[currentSet.entrantID2].Players[0].name);
                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + currentSet.match + LpStrings.P2 + LpStrings.Flag, entrantList[currentSet.entrantID2].Players[0].country);

                        // Check for DQs
                        if (currentSet.entrant1wins == -1)
                        {
                            FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + currentSet.match + LpStrings.P1 + LpStrings.Score, "DQ");
                            FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + currentSet.match + LpStrings.P2 + LpStrings.Score, LpStrings.Checkmark);
                        }
                        else if (currentSet.entrant2wins == -1)
                        {
                            FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + currentSet.match + LpStrings.P2 + LpStrings.Score, "DQ");
                            FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + currentSet.match + LpStrings.P1 + LpStrings.Score, LpStrings.Checkmark);
                        }
                        else
                        {
                            // smash.gg switches P1 and P2 in the event of a bracket reset
                            if (currentSet.isGF && currentSet.match == 2)
                            {
                                if (currentSet.entrant1wins != -99 && currentSet.entrant2wins != -99)
                                {
                                    FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + currentSet.match + LpStrings.P2 + LpStrings.Score, currentSet.entrant1wins.ToString());
                                    FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + currentSet.match + LpStrings.P1 + LpStrings.Score, currentSet.entrant2wins.ToString());
                                }
                                else
                                {
                                    if (currentSet.winner == currentSet.entrantID1)
                                    {
                                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + currentSet.match + LpStrings.P2 + LpStrings.Score, "{{win}}");
                                    }
                                    else if (currentSet.winner == currentSet.entrantID2)
                                    {
                                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + currentSet.match + LpStrings.P1 + LpStrings.Score, "{{win}}");
                                    }
                                }
                            }
                            else
                            {
                                if (currentSet.entrant1wins != -99 && currentSet.entrant2wins != -99)
                                {
                                    FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + currentSet.match + LpStrings.P1 + LpStrings.Score, currentSet.entrant1wins.ToString());
                                    FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + currentSet.match + LpStrings.P2 + LpStrings.Score, currentSet.entrant2wins.ToString());
                                }
                                else
                                {
                                    if (currentSet.winner == currentSet.entrantID1)
                                    {
                                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + currentSet.match + LpStrings.P1 + LpStrings.Score, "{{win}}");
                                    }
                                    else if (currentSet.winner == currentSet.entrantID2)
                                    {
                                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + currentSet.match + LpStrings.P2 + LpStrings.Score, "{{win}}");
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
                                FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + currentSet.match + LpStrings.Win, "1");
                            }
                            else if (currentSet.winner == currentSet.entrantID2)
                            {
                                FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + currentSet.match + LpStrings.Win, "2");
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

                    // Skip unfinished sets unless otherwise specified
                    if (checkBoxFillUnfinished.Checked == false && currentSet.state == 1)
                    {
                        continue;
                    }

                    // Check for player byes
                    if (currentSet.entrantID1 == PLAYER_BYE && currentSet.entrantID2 == PLAYER_BYE)
                    {
                        // If both players are byes, skip this entry
                        continue;
                    }
                    else if (currentSet.entrantID1 == PLAYER_BYE)
                    {
                        // Fill in team 1 as a bye
                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + currentSet.match + LpStrings.T1 + LpStrings.P1, "Bye");
                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + currentSet.match + LpStrings.T1 + LpStrings.P2, "Bye");

                        // Give team 2 a checkmark
                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + currentSet.match + LpStrings.T2 + LpStrings.P1, entrantList[currentSet.entrantID2].Players[0].name);
                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + currentSet.match + LpStrings.T2 + LpStrings.P1 + LpStrings.Flag, entrantList[currentSet.entrantID2].Players[0].country);
                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + currentSet.match + LpStrings.T2 + LpStrings.P2, entrantList[currentSet.entrantID2].Players[1].name);
                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + currentSet.match + LpStrings.T2 + LpStrings.P2 + LpStrings.Flag, entrantList[currentSet.entrantID2].Players[1].country);
                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + currentSet.match + LpStrings.T2 + LpStrings.Score, LpStrings.Checkmark);

                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + currentSet.match + LpStrings.Win, "2");
                    }
                    else if (currentSet.entrantID2 == PLAYER_BYE)
                    {
                        // Fill in team 2 as a bye
                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + currentSet.match + LpStrings.T2 + LpStrings.P1, "Bye");
                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + currentSet.match + LpStrings.T2 + LpStrings.P2, "Bye");

                        // Give team 1 a checkmark
                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + currentSet.match + LpStrings.T1 + LpStrings.P1, entrantList[currentSet.entrantID1].Players[0].name);
                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + currentSet.match + LpStrings.T1 + LpStrings.P1 + LpStrings.Flag, entrantList[currentSet.entrantID1].Players[0].country);
                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + currentSet.match + LpStrings.T1 + LpStrings.P2, entrantList[currentSet.entrantID1].Players[1].name);
                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + currentSet.match + LpStrings.T1 + LpStrings.P2 + LpStrings.Flag, entrantList[currentSet.entrantID1].Players[1].country);
                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + currentSet.match + LpStrings.T1 + LpStrings.Score, LpStrings.Checkmark);

                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + currentSet.match + LpStrings.Win, "1");
                    }
                    else
                    {
                        // Fill in the currentSet normally
                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + currentSet.match + LpStrings.T1 + LpStrings.P1, entrantList[currentSet.entrantID1].Players[0].name);
                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + currentSet.match + LpStrings.T1 + LpStrings.P1 + LpStrings.Flag, entrantList[currentSet.entrantID1].Players[0].country);
                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + currentSet.match + LpStrings.T1 + LpStrings.P2, entrantList[currentSet.entrantID1].Players[1].name);
                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + currentSet.match + LpStrings.T1 + LpStrings.P2 + LpStrings.Flag, entrantList[currentSet.entrantID1].Players[1].country);
                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + currentSet.match + LpStrings.T2 + LpStrings.P1, entrantList[currentSet.entrantID2].Players[0].name);
                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + currentSet.match + LpStrings.T2 + LpStrings.P1 + LpStrings.Flag, entrantList[currentSet.entrantID2].Players[0].country);
                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + currentSet.match + LpStrings.T2 + LpStrings.P2, entrantList[currentSet.entrantID2].Players[1].name);
                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + currentSet.match + LpStrings.T2 + LpStrings.P2 + LpStrings.Flag, entrantList[currentSet.entrantID2].Players[1].country);

                        // Check for DQs
                        if (currentSet.entrant1wins == -1)
                        {
                            FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + currentSet.match + LpStrings.T1 + LpStrings.Score, "DQ");
                            FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + currentSet.match + LpStrings.T2 + LpStrings.Score, LpStrings.Checkmark);
                        }
                        else if (currentSet.entrant2wins == -1)
                        {
                            FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + currentSet.match + LpStrings.T2 + LpStrings.Score, "DQ");
                            FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + currentSet.match + LpStrings.T1 + LpStrings.Score, LpStrings.Checkmark);
                        }
                        else
                        {
                            if (currentSet.entrant1wins != -99 && currentSet.entrant2wins != -99)
                            {
                                FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + currentSet.match + LpStrings.T1 + LpStrings.Score, currentSet.entrant1wins.ToString());
                                FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + currentSet.match + LpStrings.T2 + LpStrings.Score, currentSet.entrant2wins.ToString());
                            }
                            else
                            {
                                if (currentSet.winner == currentSet.entrantID1)
                                {
                                    FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + currentSet.match + LpStrings.T1 + LpStrings.Score, "{{win}}");
                                }
                                else if (currentSet.winner == currentSet.entrantID2)
                                {
                                    FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + currentSet.match + LpStrings.T2 + LpStrings.Score, "{{win}}");
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
                                FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + currentSet.match + LpStrings.Win, "1");
                            }
                            else if (currentSet.winner == currentSet.entrantID2)
                            {
                                FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + currentSet.match + LpStrings.Win, "2");
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

                richTextBoxEntrants.Text += entrant.Key.ToString().PadRight(8);

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
            int start = lpText.IndexOf("|" + param + "=");

            if (start != -1)
            {
                start += param.Length + 2;
                lpText = lpText.Insert(start, value);
            }
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
            if (!checkBoxLockWinners.Checked)
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

            checkBoxFillUnfinished.Enabled = false;
            checkBoxGuessFinal.Enabled = false;
            checkBoxLockLosers.Enabled = false;
            checkBoxLockWinners.Enabled = false;
            checkBoxLosers.Enabled = false;
            checkBoxWinners.Enabled = false;

            numericUpDownAdvanceWinners.Enabled = false;
            numericUpDownAdvanceWinners.Enabled = false;
            numericUpDownLosersEnd.Enabled = false;
            numericUpDownLosersOffset.Enabled = false;
            numericUpDownLosersStart.Enabled = false;
            numericUpDownWinnersEnd.Enabled = false;
            numericUpDownWinnersOffset.Enabled = false;
            numericUpDownWinnersStart.Enabled = false;

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

            checkBoxFillUnfinished.Enabled = true;
            checkBoxGuessFinal.Enabled = true;
            checkBoxLockLosers.Enabled = true;
            checkBoxLockWinners.Enabled = true;
            checkBoxLosers.Enabled = true;
            checkBoxWinners.Enabled = true;

            numericUpDownAdvanceWinners.Enabled = true;
            numericUpDownAdvanceWinners.Enabled = true;
            numericUpDownLosersEnd.Enabled = true;
            numericUpDownLosersOffset.Enabled = true;
            numericUpDownLosersStart.Enabled = true;
            numericUpDownWinnersEnd.Enabled = true;
            numericUpDownWinnersOffset.Enabled = true;
            numericUpDownWinnersStart.Enabled = true;

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
    }
}
