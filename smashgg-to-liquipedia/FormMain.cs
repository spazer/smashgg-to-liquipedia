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
using System.IO.Compression;
using smashgg_to_liquipedia.Smashgg.Schema;
using smashgg_to_liquipedia.Smashgg;

namespace smashgg_to_liquipedia
{
    public partial class FormMain : Form, IFormMain
    {
        static string SMASH_DB_URI = "http://liquipedia.net/smash/api.php?action=parse&page=Liquipedia:Players_Regex&prop=revid|wikitext&format=json";
        static string FIGHTERS_DB_URI = "http://liquipedia.net/fighters/api.php?action=parse&page=Liquipedia:Players_Regex&prop=revid|wikitext&format=json";

        public enum BracketSide { Winners, Losers }

        List<Seed> seedList = new List<Seed>();
        Dictionary<int, Entrant> entrantList = new Dictionary<int, Entrant>();
        List<Set> setList = new List<Set>();
        //List<Standing> groupStandings = new List<Standing>();
        Dictionary<int, List<Set>> roundList = new Dictionary<int, List<Set>>();
        Dictionary<int, int> matchOffsetPerRound = new Dictionary<int, int>();

        Tournament tournament;
        Event selectedEvent;
        int selectedObjectId;
        Smashgg.TreeNodeData.NodeType selectedObjectType;

        PlayerDatabase playerdb;
        ApiQueries apiQuery;

        string authToken = string.Empty;

        public string Log
        {
            get { return richTextBoxLog.Text; }
            set { richTextBoxLog.Text = value; }
        }

        public FormMain()
        {
            InitializeComponent();

            // Display version
            Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            this.Text = "Bracket Match Filler v" + $"{version}";

            try
            {
                StreamReader fileinfo = new StreamReader(@"userinfo");
                authToken = fileinfo.ReadLine().Trim().Unprotect();
                fileinfo.Close();

                if (authToken == "")
                {
                    buttonAuthentication.BackColor = Color.PaleVioletRed;
                }
            }
            catch
            {
                richTextBoxLog.Text += "No saved credentials. Set your smash.gg token first.\r\n";
                buttonAuthentication.BackColor = Color.PaleVioletRed;
            }

            SetCueText(textBoxTournamentUrl, FormStrings.CuetextURL);

            richTextBoxExLpWinnersBracket.Cue = FormStrings.CuetextLpWinners;
            richTextBoxExLpLosersBracket.Cue = FormStrings.CuetextLpLosers;

            if (radioButtonSmash.Checked == true)
            {
                playerdb = new PlayerDatabase(PlayerDatabase.DbSource.Smash);
            }
            else if (radioButtonFighters.Checked == true)
            {
                playerdb = new PlayerDatabase(PlayerDatabase.DbSource.Fighters);
            }
            UpdateRevID();

            bool success;
            apiQuery = new ApiQueries(authToken, playerdb, this, out success);
            if (!success)
            {
                richTextBoxLog.Text += "Failed to get API endpoint\r\n";
            }
            else
            {
                richTextBoxLog.Text += "Set up API query class successfully\r\n";
            }
        }

        #region Buttons
        /// <summary>
        /// Indicates that user wishes to retrieve a bracket
        /// </summary>
        /// <param name="sender">N/A</param>
        /// <param name="e">N/A</param>
        private void buttonGetTournament_Click(object sender, EventArgs e)
        {
            LockControls();

            tabControl.SelectedTab = tabPageTournamentExplorer;

            // Find tournament slug
            textBoxTournamentUrl.Text = textBoxTournamentUrl.Text.Trim();
            textBoxTournamentUrl.Text = textBoxTournamentUrl.Text.Replace("/events/", "/event/");
            int tournamentMarker = textBoxTournamentUrl.Text.IndexOf("tournament/");
            if (tournamentMarker == -1)
            {
                richTextBoxLog.Text += "Invalid URL\r\n";
                UnlockControls();
                return;
            }
            int endtournamentMarker = textBoxTournamentUrl.Text.IndexOf("/", tournamentMarker + 11);
            if (endtournamentMarker == -1)
            {
                richTextBoxLog.Text += "Invalid URL\r\n";
                UnlockControls();
                return;
            }
            string slug = textBoxTournamentUrl.Text.Substring(tournamentMarker + 11, endtournamentMarker - (tournamentMarker + 11));

            // Clear the treeview
            treeView1.Nodes.Clear();

            tournament = apiQuery.GetEvents(slug);
            if (tournament.events == null)
            {
                richTextBoxLog.Text += "Failed to get tournament\r\n";
                UnlockControls();
                return;
            }

            // Begin updating the treeview
            treeView1.BeginUpdate();
            TreeNode root = new TreeNode(tournament.slug);
            treeView1.Nodes.Add(root);

            // Go through all events in the tournament
            foreach (Event tournamentEvent in tournament.events)
            {
                TreeNode eventNode = new TreeNode(tournamentEvent.name);

                // Save relevant data to event tag
                TreeNodeData eventNodeTag = new TreeNodeData();
                eventNodeTag.id = tournamentEvent.id;
                eventNodeTag.name = tournamentEvent.name;
                eventNodeTag.nodetype = TreeNodeData.NodeType.Event;
                if (tournamentEvent.teamRosterSize != null)
                {
                    eventNodeTag.playersPerEntrant = tournamentEvent.teamRosterSize.maxPlayers;
                }
                eventNode.Tag = eventNodeTag;


                // Display number of entrants
                switch (tournamentEvent.Type)
                {
                    case Event.EventType.Singles:
                        eventNode.Text = tournamentEvent.name + " (" + tournamentEvent.numEntrants.ToString() + " players)";
                        break;
                    case Event.EventType.Doubles:
                        eventNode.Text = tournamentEvent.name + " (" + tournamentEvent.numEntrants.ToString() + " entrants, " + (2 * tournamentEvent.numEntrants).ToString() + " players)";
                        break;
                    default:
                        eventNode.Text = tournamentEvent.name + " (" + tournamentEvent.numEntrants.ToString() + " entrants)";
                        break;
                }

                // Set the event bg color
                switch (tournamentEvent.state)
                {
                    case Tournament.ActivityState.Completed:
                        eventNode.BackColor = Color.LightGreen;
                        break;
                    case Tournament.ActivityState.Active:
                        eventNode.BackColor = Color.LightYellow;
                        break;
                    default:
                        eventNode.BackColor = Color.LightPink;
                        break;
                }

                root.Nodes.Add(eventNode);

                foreach (Phase phase in tournamentEvent.phases)
                {
                    TreeNode phaseNode = new TreeNode(phase.name);

                    // Save relevant data to phase tag
                    TreeNodeData phaseNodeTag = new TreeNodeData();
                    phaseNodeTag.id = phase.id;
                    phaseNodeTag.name = phase.name;
                    phaseNodeTag.nodetype = TreeNodeData.NodeType.Phase;
                    phaseNode.Tag = phaseNodeTag;

                    if (phase.waves.Count > 0)
                    {
                        // Add waves for each phase
                        int waveActive = 0;
                        int waveComplete = 0;
                        foreach (KeyValuePair<string, List<PhaseGroup>> wave in phase.waves)
                        {
                            TreeNode waveNode = new TreeNode(wave.Key);

                            // Save relevant data to wave tag
                            TreeNodeData waveNodeTag = new TreeNodeData();
                            if (wave.Value[0].wave == null)
                            {
                                waveNodeTag.id = 0;
                            }
                            else
                            {
                                waveNodeTag.id = wave.Value[0].Number;
                            }
  
                            waveNodeTag.name = wave.Key;
                            waveNodeTag.nodetype = TreeNodeData.NodeType.Wave;
                            waveNode.Tag = waveNodeTag;

                            // Set wave text
                            waveNode.Text = "Wave " + wave.Key + " (" + wave.Value.Count + " groups)";

                            // Add phasegroups associated with each wave
                            int phasegroupActive = 0;
                            int phasegroupComplete = 0;
                            foreach (PhaseGroup phasegroup in wave.Value)
                            {
                                TreeNode phasegroupNode = new TreeNode(phasegroup.displayIdentifier);

                                // Save relevant data to phasegroup tag
                                TreeNodeData phasegroupNodeTag = new TreeNodeData();
                                phasegroupNodeTag.id = phasegroup.id;
                                phasegroupNodeTag.name = phasegroup.displayIdentifier;
                                phasegroupNodeTag.nodetype = TreeNodeData.NodeType.PhaseGroup;
                                phasegroupNode.Tag = phasegroupNodeTag;

                                // Set the phasegroup bg color
                                switch (phasegroup.State)
                                {
                                    case Tournament.ActivityState.Completed:
                                        phasegroupNode.BackColor = Color.LightGreen;
                                        phasegroupComplete++;
                                        break;
                                    case Tournament.ActivityState.Active:
                                        phasegroupNode.BackColor = Color.LightYellow;
                                        phasegroupActive++;
                                        break;
                                    default:
                                        phasegroupNode.BackColor = Color.LightPink;
                                        break;
                                }

                                waveNode.Nodes.Add(phasegroupNode);
                            }

                            if (phasegroupComplete == wave.Value.Count)
                            {
                                waveNode.BackColor = Color.LightGreen;
                                waveComplete++;
                            }
                            else if (phasegroupActive >= 1 || phasegroupComplete >= 1)
                            {
                                waveNode.BackColor = Color.LightYellow;
                                waveActive++;
                            }
                            else
                            {
                                waveNode.BackColor = Color.LightPink;
                            }

                            phaseNode.Nodes.Add(waveNode);
                        }

                        if (waveComplete == phase.waves.Count)
                        {
                            phaseNode.BackColor = Color.LightGreen;

                        }
                        else if (waveActive >= 1 || waveComplete >= 1)
                        {
                            phaseNode.BackColor = Color.LightYellow;
                        }
                        else
                        {
                            phaseNode.BackColor = Color.LightPink;
                        }
                    }
                    else
                    {
                        // Add phasegroups associated with each phase
                        int phasegroupActive = 0;
                        int phasegroupComplete = 0;
                        if (phase.phasegroups.nodes.Count >= 2)
                        {
                            foreach (PhaseGroup phasegroup in phase.phasegroups.nodes)
                            {
                                TreeNode phasegroupNode = new TreeNode(phasegroup.displayIdentifier + " " + phasegroup.WaveLetter);

                                // Save relevant data to phasegroup tag
                                TreeNodeData phasegroupNodeTag = new TreeNodeData();
                                phasegroupNodeTag.id = phasegroup.id;
                                phasegroupNodeTag.name = phasegroup.displayIdentifier;
                                phasegroupNodeTag.nodetype = TreeNodeData.NodeType.PhaseGroup;
                                phasegroupNode.Tag = phasegroupNodeTag;

                                // Set the phasegroup bg color
                                switch (phasegroup.State)
                                {
                                    case Tournament.ActivityState.Completed:
                                        phasegroupNode.BackColor = Color.LightGreen;
                                        phasegroupComplete++;
                                        break;
                                    case Tournament.ActivityState.Active:
                                        phasegroupNode.BackColor = Color.LightYellow;
                                        phasegroupActive++;
                                        break;
                                    default:
                                        phasegroupNode.BackColor = Color.LightPink;
                                        break;
                                }

                                phaseNode.Nodes.Add(phasegroupNode);
                            }
                        }
                        else
                        {
                            foreach (PhaseGroup phasegroup in phase.phasegroups.nodes)
                            {
                                switch (phasegroup.State)
                                {
                                    case Tournament.ActivityState.Completed:
                                        phasegroupComplete++;
                                        break;
                                    case Tournament.ActivityState.Active:
                                        phasegroupActive++;
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }

                        // Set bgcolor for phase
                        if (phasegroupComplete == phase.phasegroups.nodes.Count)
                        {
                            phaseNode.BackColor = Color.LightGreen;
                        }
                        else if (phasegroupActive >= 1 || phasegroupComplete >= 1)
                        {
                            phaseNode.BackColor = Color.LightYellow;
                        }
                        else
                        {
                            phaseNode.BackColor = Color.LightPink;
                        }
                    }

                    // Add phase to event
                    eventNode.Nodes.Add(phaseNode);
                }
            }

            // Expand the tournament node
            root.Expand();
            treeView1.EndUpdate();

            UnlockControls();

            richTextBoxLog.Text += "Get tournament complete.\r\n";
        }

        private void buttonFill_Click(object sender, EventArgs e)
        {
            if (selectedEvent.Type == Event.EventType.Doubles)
            {
                FillDoubles();
            }
            else
            {
                FillSingles();
            }
        }

        /// <summary>
        /// Fill the liquipedia brackets with singles data
        /// </summary>
        /// <param name="sender">N/A</param>
        /// <param name="e">N/A</param>
        private void FillSingles()
        {
            string output = string.Empty;
            string finalBracketOutput = string.Empty;
            Liquipedia.LpOutput lpout = new Liquipedia.LpOutput(selectedEvent, ref entrantList, ref setList, ref roundList);

            if (checkBoxWinners.Checked == true)
            {
                if (richTextBoxExLpWinnersBracket.Text != FormStrings.CuetextLpWinners)
                {
                    if (textBoxHeaderWinners.Text.Trim() != string.Empty)
                    {
                        output += textBoxHeaderWinners.Text + "\r\n" + richTextBoxExLpWinnersBracket.Text + "\r\n";
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
                    if (textBoxHeaderLosers.Text.Trim() != string.Empty)
                    {
                        output += textBoxHeaderLosers.Text + "\r\n" + richTextBoxExLpLosersBracket.Text + "\r\n";
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
                lpout.fillBracketSingles((int)numericUpDownWinnersStart.Value, (int)numericUpDownWinnersEnd.Value, (int)numericUpDownWinnersOffset.Value, ref output, matchOffsetPerRound,
                    checkBoxFillByes.Checked, checkBoxUnfinished.Checked, checkBoxMatchDetails.Checked, false);
            }
            if (checkBoxLosers.Checked)
            {
                lpout.fillBracketSingles(-(int)numericUpDownLosersStart.Value, -(int)numericUpDownLosersEnd.Value, (int)numericUpDownLosersOffset.Value, ref output, matchOffsetPerRound,
                    checkBoxFillByes.Checked, checkBoxUnfinished.Checked, checkBoxMatchDetails.Checked, false);
            }
            if (checkBoxGuessFinal.Checked)
            {
                finalBracketOutput = richTextBoxExLpFinalBracket.Text;

                // Assume the highest round is grand finals, the lowest round is losers finals, etc.
                int gfround = roundList.Keys.Max();
                int wfround = roundList.Keys.Max() - 1;
                int lfround = roundList.Keys.Min();
                int lsround = roundList.Keys.Min() + 1;

                // Fill in R3
                lpout.fillBracketSingles(gfround, gfround, 3 - gfround, ref finalBracketOutput, matchOffsetPerRound,
                    checkBoxFillByes.Checked, checkBoxUnfinished.Checked, checkBoxMatchDetails.Checked, false);

                // Fill in L2
                lpout.fillBracketSingles(lfround, lfround, 2 - Math.Abs(lfround), ref finalBracketOutput, matchOffsetPerRound,
                    checkBoxFillByes.Checked, checkBoxUnfinished.Checked, checkBoxMatchDetails.Checked, true);

                // Fill in L1
                lpout.fillBracketSingles(lsround, lsround, 1 - Math.Abs(lsround), ref finalBracketOutput, matchOffsetPerRound,
                    checkBoxFillByes.Checked, checkBoxUnfinished.Checked, checkBoxMatchDetails.Checked, false);

                // Fill in R1
                lpout.fillBracketSingles(wfround, wfround, 1 - wfround, ref finalBracketOutput, matchOffsetPerRound,
                    checkBoxFillByes.Checked, checkBoxUnfinished.Checked, checkBoxMatchDetails.Checked, false);

                if (textBoxHeaderFinals.Text.Trim() != string.Empty)
                {
                    output += textBoxHeaderFinals.Text + "\r\n" + finalBracketOutput.Trim();
                }
                else
                {
                    output += finalBracketOutput.Trim();
                }
            }

            richTextBoxLog.Text += lpout.Log;
            richTextBoxLpOutput.Text = output.Trim();
        }

        /// <summary>
        /// Fill the liquipedia brackets with doubles data
        /// </summary>
        /// <param name="sender">N/A</param>
        /// <param name="e">N/A</param>
        private void FillDoubles()
        {
            string output = string.Empty;
            string finalBracketOutput = string.Empty;
            Liquipedia.LpOutput lpout = new Liquipedia.LpOutput(selectedEvent, ref entrantList, ref setList, ref roundList);

            if (checkBoxWinners.Checked == true)
            {
                if (richTextBoxExLpWinnersBracket.Text != FormStrings.CuetextLpWinners)
                {
                    output += textBoxHeaderWinners.Text + "\r\n" + richTextBoxExLpWinnersBracket.Text + "\r\n";
                }
            }

            if (checkBoxLosers.Checked == true)
            {
                if (richTextBoxExLpLosersBracket.Text != FormStrings.CuetextLpLosers)
                {
                    output += textBoxHeaderLosers.Text + "\r\n" + richTextBoxExLpLosersBracket.Text + "\r\n";
                }
            }

            // If the corresponding checkbox is not checked, skip that side of the bracket
            if (checkBoxWinners.Checked)
            {
                lpout.fillBracketDoubles((int)numericUpDownWinnersStart.Value, (int)numericUpDownWinnersEnd.Value, (int)numericUpDownWinnersOffset.Value, ref output, matchOffsetPerRound,
                    checkBoxFillByes.Checked, checkBoxUnfinished.Checked, false);
            }
            if (checkBoxLosers.Checked)
            {
                lpout.fillBracketDoubles(-(int)numericUpDownLosersStart.Value, -(int)numericUpDownLosersEnd.Value, (int)numericUpDownLosersOffset.Value, ref output, matchOffsetPerRound,
                    checkBoxFillByes.Checked, checkBoxUnfinished.Checked, false);
            }
            if (checkBoxGuessFinal.Checked)
            {
                finalBracketOutput = richTextBoxExLpFinalBracket.Text;

                // Assume the highest round is grand finals, the lowest round is losers finals, etc.
                int gfround = roundList.Keys.Max();
                int wfround = roundList.Keys.Max() + 1;
                int lfround = roundList.Keys.Min();
                int lsround = roundList.Keys.Min() + 1;

                // Fill in R3
                lpout.fillBracketDoubles(gfround, gfround, 3 - gfround, ref finalBracketOutput, matchOffsetPerRound,
                    checkBoxFillByes.Checked, checkBoxUnfinished.Checked, false);

                // Fill in L2
                lpout.fillBracketDoubles(lfround, lfround, 2 - Math.Abs(lfround), ref finalBracketOutput, matchOffsetPerRound,
                    checkBoxFillByes.Checked, checkBoxUnfinished.Checked, true);

                // Fill in L1
                lpout.fillBracketDoubles(lsround, lsround, 1 - Math.Abs(lsround), ref finalBracketOutput, matchOffsetPerRound,
                    checkBoxFillByes.Checked, checkBoxUnfinished.Checked, false);

                // Fill in R1
                lpout.fillBracketDoubles(wfround, wfround, 1 - wfround, ref finalBracketOutput, matchOffsetPerRound,
                    checkBoxFillByes.Checked, checkBoxUnfinished.Checked, false);


                // Replace L2 with R2 because liquipedia markup is inconsistent
                finalBracketOutput = finalBracketOutput.Replace("l2m", "r2m");

                output += "==Final Doubles Bracket==\r\n" + finalBracketOutput;
            }

            richTextBoxLpOutput.Text = output;
            richTextBoxLog.Text += lpout.Log;
        }

        /// <summary>
        /// Creates a prize pool template based on retrieved info
        /// </summary>
        /// <param name="sender">N/A</param>
        /// <param name="e">N/A</param>
        private void buttonPrizePool_Click(object sender, EventArgs e)
        {
            if (selectedEvent == null)
            {
                richTextBoxLog.Text += "No event selected\r\n";
                return;
            }

            List<Standing> standingList = apiQuery.GetStandings(selectedEvent.id, (int)numericUpDownPrizePool.Value);


            richTextBoxLog.Text += "Outputting results\r\n";

            // Ouptut the prize pool header
            if (selectedEvent.Type == Event.EventType.Singles)
            {
                richTextBoxLpOutput.Text = "{{prize pool start}}\r\n";
            }
            else if (selectedEvent.Type == Event.EventType.Doubles)
            {
                richTextBoxLpOutput.Text = "{{prize pool start doubles}}\r\n";
            }

            // Loop through the records to remove placements of 0
            for (int i = 0; i < standingList.Count; i++)
            {
                if (standingList[i].placement == 0)
                {
                    standingList.RemoveAt(i);
                    i--;
                }
            }

            // Loop through the records to add entries to the prize pool
            int nextRow = 0;
            int startEntry = 0;
            while (startEntry < standingList.Count)
            {
                bool rowSet = false;

                // Look ahead and see if there's entrants of equal rank
                for (int j = startEntry + 1; j < standingList.Count; j++)
                {
                    // When a match is found, j is the next entrant with a new rank
                    if (standingList[startEntry].placement != standingList[j].placement)
                    {
                        nextRow = j;
                        rowSet = true;
                        break;
                    }

                    // If the end of the entrant list is reached, everything needs to be output, so set nextRow to maxEntries
                    else if (j == standingList.Count - 1)
                    {
                        nextRow = standingList.Count;
                        rowSet = true;
                        break;
                    }
                }

                if (!rowSet)
                {
                    nextRow = standingList.Count;
                }

                // Ouptut all equal ranking entrants
                OutputPrizePoolTable(standingList, startEntry, nextRow - startEntry);

                // Skip all entrants that have been output
                startEntry = nextRow;
            }

            if (selectedEvent.Type == Event.EventType.Singles)
            {
                richTextBoxLpOutput.Text += "{{prize pool end}}\r\n";
            }
            else if (selectedEvent.Type == Event.EventType.Doubles)
            {
                richTextBoxLpOutput.Text += "{{prize pool end doubles}}\r\n";
            }

            richTextBoxLpOutput.Text = richTextBoxLpOutput.Text.Trim();
            richTextBoxLog.Text += "Done.\r\n";
        }

        private void buttonGroupTable_Click(object sender, EventArgs e)
        {
            string output = string.Empty;
            Liquipedia.LpOutput lpout = new Liquipedia.LpOutput(selectedEvent, ref entrantList, ref setList, ref roundList);

            Dictionary<int, PoolRecord> poolData = new Dictionary<int, PoolRecord>();

            // Set the pool type
            PoolRecord.PoolType poolType;
            if (radioButtonBracket.Checked)
            {
                poolType = PoolRecord.PoolType.Bracket;
            }
            else if (radioButtonRR.Checked)
            {
                poolType = PoolRecord.PoolType.RoundRobin;
            }
            else
            {
                richTextBoxLog.Text += "Specify a bracket type.\r\n";
                return;
            }

            if (selectedEvent == null)
            {
                richTextBoxLog.Text += "Select a phasegroup and get tournament data first.\r\n";
                return;
            }

            Tournament.ActivityState phaseGroupState = Tournament.ActivityState.Invalid;
            string title = string.Empty;

            if (!GeneratePoolData(poolType, ref poolData)) richTextBoxLog.Text += "Error generating group table\r\n";

            if (selectedEvent.Type == Event.EventType.Singles)
            {
                output = lpout.OutputSinglesGroup(title, poolData, phaseGroupState, (int)numericUpDownAdvanceWinners.Value, (int)numericUpDownAdvanceLosers.Value, checkBoxMatchDetails.Checked, poolType);
            }
            else if (selectedEvent.Type == Event.EventType.Doubles)
            {
                output = lpout.OutputDoublesGroup(title, poolData, phaseGroupState, (int)numericUpDownAdvanceWinners.Value, (int)numericUpDownAdvanceLosers.Value, checkBoxMatchDetails.Checked, poolType);
            }

            richTextBoxLpOutput.Text = output.Trim();
            richTextBoxLog.Text += lpout.Log;
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
        /// Retrieves a regex AKA database from Liquipedia, and parses it
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonAKA_Click(object sender, EventArgs e)
        {
            WebClient client = new WebClient();
            client.Headers[HttpRequestHeader.AcceptEncoding] = "gzip";
            client.Headers[HttpRequestHeader.UserAgent] = "smashgg-to-liquipedia (https://github.com/spazer/smashgg-to-liquipedia)";

            // Decide on the URL to use
            string json = string.Empty;
            if (radioButtonSmash.Checked)
            {
                var responseStream = new GZipStream(client.OpenRead(SMASH_DB_URI), CompressionMode.Decompress);
                var reader = new StreamReader(responseStream);
                json = reader.ReadToEnd();
            }
            else if (radioButtonFighters.Checked)
            {
                var responseStream = new GZipStream(client.OpenRead(FIGHTERS_DB_URI), CompressionMode.Decompress);
                var reader = new StreamReader(responseStream);
                json = reader.ReadToEnd();
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
        private void ProcessBracket(Event.EventType eventType)
        {
            string json = string.Empty;

            // Fill round list based on set list
            foreach (Set currentSet in setList)
            {
                // Check if the round already exists in roundList
                if (roundList.ContainsKey(currentSet.round))
                {
                    roundList[currentSet.round].Add(currentSet);
                }
                else    // Make a new entry for the round if it doesn't exist
                {
                    // Add the current set to the newly created list
                    List<Set> newSetList = new List<Set>();
                    newSetList.Add(currentSet);

                    roundList.Add(currentSet.round, newSetList);
                }
            }

            // Clear out bye rounds in winners
            if (roundList.Keys.Max() > 0)
            {
                // Lower all round numbers so they start at 1
                if (!roundList.ContainsKey(1))
                {
                    int lastRound = 1;
                    for (int i = 1; i <= roundList.Keys.Max(); i++)
                    {
                        if (roundList.ContainsKey(i))
                        {
                            roundList[lastRound] = roundList[i];
                            roundList.Remove(i);
                            lastRound++;
                        }
                    }
                }

                for (int i = 1; i <= roundList.Keys.Max(); i++)
                {
                    bool actualMatch = false;
                    for (int j = 0; j < roundList[i].Count; j++)
                    {
                        if (roundList[i][j].DisplayScore == "Bye" || roundList[i][j].DisplayScore == "bye")
                        {
                            continue;
                        }
                        else
                        {
                            actualMatch = true;
                            break;
                        }
                    }

                    // If false, all matches are byes in this round
                    if (!actualMatch)
                    {
                        // Shift all remaining rounds back one
                        for (int j = i + 1; j <= roundList.Keys.Max(); j++)
                        {
                            roundList[j - 1] = roundList[j];
                        }

                        // Remove the last round and decrement the counter
                        roundList.Remove(roundList.Keys.Max());
                        i--;
                    }
                }
            }
            // Clear out bye rounds in losers
            if (roundList.Keys.Min() < 0)
            {
                // Lower all round numbers so they start at -1
                if (!roundList.ContainsKey(-1))
                {
                    int lastRound = -1;
                    for (int i = -1; i >= roundList.Keys.Min(); i--)
                    {
                        if (roundList.ContainsKey(i))
                        {
                            roundList[lastRound] = roundList[i];
                            roundList.Remove(i);
                            lastRound--;
                        }
                    }
                }

                for (int i = -1; i >= roundList.Keys.Min(); i--)
                {
                    bool actualMatch = false;
                    for (int j = 0; j < roundList[i].Count; j++)
                    {
                        if (roundList[i][j].DisplayScore == "Bye" || roundList[i][j].DisplayScore == "bye") 
                        {
                            continue;
                        }
                        else if (roundList[i][j].slots == null || (roundList[i][j].slots[0].entrant == null || roundList[i][j].slots[1].entrant == null))
                        {
                            continue;
                        }
                        else
                        {
                            actualMatch = true;
                            break;
                        }
                    }

                    // If false, all matches are byes in this round
                    if (!actualMatch)
                    {
                        // Shift all remaining rounds back one
                        for (int j = i; j >= roundList.Keys.Min() + 1; j--)
                        {
                            roundList[j] = roundList[j - 1];
                        }

                        // Remove the last round and decrement the counter
                        roundList.Remove(roundList.Keys.Min());
                        i++;
                    }
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
                foreach (Participant participant in entrant.participants)
                {
                    sum += participant.gamerTag.Length;
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
                    if (set.slots[0].entrant != null && entrantList.ContainsKey(set.slots[0].entrant.id))
                    {
                        if (set.round > 0)
                        {
                            if (entrantList[set.slots[0].entrant.id].participants[0].gamerTag.Length > wPadding)
                            {
                                wPadding = entrantList[set.slots[0].entrant.id].participants[0].gamerTag.Length;
                            }
                        }
                        else
                        {
                            if (entrantList[set.slots[0].entrant.id].participants[0].gamerTag.Length > lPadding)
                            {
                                lPadding = entrantList[set.slots[0].entrant.id].participants[0].gamerTag.Length;
                            }
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

                // Test to see if the id is valid. If not, this is a preview id
                bool isNumber = false;
                int number = 0;
                isNumber = int.TryParse(tempList[0].id, out number);

                // Only order the sets if this is not a preview
                if (isNumber)
                {
                    tempList = tempList.OrderBy(x => x.id).ToList();
                }

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
                    if (set.slots[0].entrant == null || !entrantList.ContainsKey(set.slots[0].entrant.id))
                    {
                        continue;
                    }
                    if (set.slots[1].entrant == null || !entrantList.ContainsKey(set.slots[1].entrant.id))
                    {
                        continue;
                    }

                    if (set.round > 0)
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
        /// Acquire, process, and output all data from the singles phase specified in the URL
        /// </summary>
        /// <param name="phasegroups">Tournament phase to output</param>
        /// <param name="poolData">Pool records for each entrant</param>
        /// <param name="lastWave">Final wave in the pool</param>
        /// <param name="currentGroup">Pool in the phase to output</param>
        private void OutputSinglesPhase(Phase phase, Dictionary<int, PoolRecord> poolData, ref string lastWave, PhaseGroup currentGroup, Tournament.ActivityState state, PoolRecord.PoolType poolType)
        {
            Liquipedia.LpOutput lpout = new Liquipedia.LpOutput(selectedEvent, ref entrantList, ref setList, ref roundList);

            // Output to textbox
            // Wave headers
            if (phase.phasegroups.nodes[0].identifierType == PhaseGroup.IdentiferType.WaveNumber)
            {
                if (lastWave != currentGroup.WaveLetter)
                {
                    richTextBoxLpOutput.Text += "==Wave " + currentGroup.WaveLetter + "==\r\n";
                    richTextBoxLpOutput.Text += LpStrings.BoxStart + "\r\n";

                    lastWave = currentGroup.WaveLetter;
                }
            }
            else if (phase.phasegroups.nodes.First() == currentGroup)    // Start a box at the first element
            {
                richTextBoxLpOutput.Text += LpStrings.BoxStart + "\r\n";
            }

            // Pool headers
            string title = string.Empty;
            if (phase.phasegroups.nodes[0].identifierType == PhaseGroup.IdentiferType.WaveNumber)
            {
                richTextBoxLpOutput.Text += "===" + LpStrings.SortStart + currentGroup.WaveLetter + currentGroup.Number.ToString() + LpStrings.SortEnd + "===\r\n";
                title = currentGroup.WaveLetter + currentGroup.Number.ToString();
            }
            else
            {
                richTextBoxLpOutput.Text += "===" + LpStrings.SortStart + currentGroup.WaveLetter + currentGroup.DisplayIdentifier.ToString() + LpStrings.SortEnd + "===\r\n";
                title = currentGroup.DisplayIdentifier.ToString();
            }

            // Output the group
            richTextBoxLpOutput.Text += lpout.OutputSinglesGroup(title, poolData, state, (int)numericUpDownAdvanceWinners.Value, (int)numericUpDownAdvanceLosers.Value, checkBoxMatchDetails.Checked, poolType);
            richTextBoxLog.Text += lpout.Log;

            // Box handling
            if (phase.phasegroups.nodes[0].identifierType == PhaseGroup.IdentiferType.WaveNumber)     // Waves exist
            {
                if (currentGroup == phase.waves.Where(q => q.Key == currentGroup.WaveLetter).First().Value.Last())
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
                if (currentGroup == phase.phasegroups.nodes.Last()) // End box at the group end
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
        /// <param name="currentGroup">Element within the phase</param>
        private void OutputDoublesPhase(Phase phase, Dictionary<int, PoolRecord> poolData, ref string lastWave, PhaseGroup currentGroup, Tournament.ActivityState state, PoolRecord.PoolType poolType)
        {
            Liquipedia.LpOutput lpout = new Liquipedia.LpOutput(selectedEvent, ref entrantList, ref setList, ref roundList);

            // Output to textbox
            // Wave headers
            if (phase.phasegroups.nodes[0].identifierType == PhaseGroup.IdentiferType.WaveNumber)
            {
                if (lastWave != currentGroup.WaveLetter)
                {
                    richTextBoxLpOutput.Text += "==Wave " + currentGroup.WaveLetter + "==\r\n";
                    richTextBoxLpOutput.Text += LpStrings.BoxStart + "\r\n";

                    lastWave = currentGroup.WaveLetter;
                }
            }
            else if (phase.phasegroups.nodes.First() == currentGroup)    // Start a box at the first element
            {
                richTextBoxLpOutput.Text += LpStrings.BoxStart + "\r\n";
            }

            // Pool headers
            string title = string.Empty;
            if (phase.phasegroups.nodes[0].identifierType == PhaseGroup.IdentiferType.WaveNumber)
            {
                richTextBoxLpOutput.Text += "===" + LpStrings.SortStart + currentGroup.WaveLetter + currentGroup.Number.ToString() + LpStrings.SortEnd + "===" + "\r\n";
                title = currentGroup.WaveLetter + currentGroup.Number.ToString();
            }
            else
            {
                richTextBoxLpOutput.Text += "===" + LpStrings.SortStart + currentGroup.WaveLetter + currentGroup.DisplayIdentifier.ToString() + LpStrings.SortEnd + "===" + "\r\n";
                title = currentGroup.DisplayIdentifier.ToString();
            }

            // Output pool
            richTextBoxLpOutput.Text += lpout.OutputDoublesGroup(title, poolData, state, (int)numericUpDownAdvanceWinners.Value, (int)numericUpDownAdvanceLosers.Value, checkBoxMatchDetails.Checked, poolType);
            richTextBoxLog.Text += lpout.Log;

            // Pool footers
            if (phase.phasegroups.nodes[0].identifierType == PhaseGroup.IdentiferType.WaveNumber)     // Waves exist
            {
                if (currentGroup == phase.waves.Where(q => q.Key == currentGroup.WaveLetter).First().Value.Last())
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
                if (currentGroup == phase.phasegroups.nodes.Last()) // End box at the group end
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
        /// <param name="poolType">Specifies bracket or round robin</param>
        /// <param name="record">List of player records for the pool</param>
        /// <param name="standings">A standings object with ranks for each entrant</param>
        /// <returns>True if it completed succesfully, false otherwise</returns>
        private bool GeneratePoolData(PoolRecord.PoolType poolType, List<Standing> standings, ref Dictionary<int, PoolRecord> record)
        {
            // Clear data
            ClearData();

            // Create a record for each entrant
            foreach (Seed seed in seedList)
            {
                record.Add(seed.entrant.id, new PoolRecord());
                List<Set> relevantSets = setList.Where(q => q.slots.Any(r => r.entrant.id == seed.entrant.id)).ToList<Set>();
                foreach (Set set in relevantSets)
                {
                    // Ignore byes
                    if (set.slots[0].prereqType == "bye" || set.slots[1].prereqType == "bye")
                    {
                        continue;
                    }

                    // Mark both players as present
                    record[set.slots[0].entrant.id].isinGroup = true;
                    record[set.slots[1].entrant.id].isinGroup = true;

                    // Tally the result
                    if (set.winnerId == seed.entrant.id)    // For a win
                    {
                        record[seed.entrant.id].AddMatchWins(1);

                        if (set.DisplayScore != "DQ" && set.DisplayScore != null) // Ignore W-L for DQs for now
                        {
                            if (seed.entrant.id.ToString() == set.slots[0].id)
                            {
                                record[seed.entrant.id].AddGameWins(set.entrant1wins);
                                record[seed.entrant.id].AddGameLosses(set.entrant2wins);
                            }
                            else if (seed.entrant.id.ToString() == set.slots[1].id)
                            {
                                record[seed.entrant.id].AddGameWins(set.entrant2wins);
                                record[seed.entrant.id].AddGameLosses(set.entrant1wins);
                            }

                            record[set.slots[0].entrant.id].AddMatchesActuallyPlayed(1);
                            record[set.slots[1].entrant.id].AddMatchesActuallyPlayed(1);
                        }
                    }
                    else if (set.winnerId != null)      // For a loss
                    {
                        record[set.slots[0].entrant.id].AddMatchLosses(1);

                        if (set.DisplayScore != "DQ" && set.DisplayScore != null) // Ignore W-L for DQs for now
                        {
                            if (seed.entrant.id.ToString() == set.slots[0].id)
                            {
                                record[seed.entrant.id].AddGameWins(set.entrant1wins);
                                record[seed.entrant.id].AddGameLosses(set.entrant2wins);
                            }
                            else if (seed.entrant.id.ToString() == set.slots[1].id)
                            {
                                record[seed.entrant.id].AddGameWins(set.entrant2wins);
                                record[seed.entrant.id].AddGameLosses(set.entrant1wins);
                            }

                            record[set.slots[0].entrant.id].AddMatchesActuallyPlayed(1);
                            record[set.slots[1].entrant.id].AddMatchesActuallyPlayed(1);
                        }
                    }
                }

                record[seed.entrant.id].rank = standings.Where(q => q.entrant.id.Equals(seed.entrant.id)).First().placement;
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
            if (poolType == PoolRecord.PoolType.Bracket)
            {
                record = record.OrderBy(x => x.Value.rank != 0).ThenBy(x => x.Value.rank).ThenByDescending(x => x.Value.MatchWinrate).ThenBy(x => x.Value.MatchesLoss).ThenByDescending(x => x.Value.MatchesWin).ThenByDescending(x => x.Value.GameWinrate).ToDictionary(x => x.Key, x => x.Value);
            }
            else if (poolType == PoolRecord.PoolType.RoundRobin)
            {
                //foreach (int entrant in record.Keys)
                //{
                //    record[entrant].rank = entrantList[entrant].Placement;
                //}

                record = record.OrderBy(x => x.Value.rank != 0).ThenBy(x => x.Value.rank).ThenByDescending(x => x.Value.MatchWinrate).ThenBy(x => x.Value.MatchesLoss).ThenByDescending(x => x.Value.MatchesWin).ThenByDescending(x => x.Value.GameWinrate).ToDictionary(x => x.Key, x => x.Value);
            }

            return true;
        }

        /// <summary>
        /// Aggregates set data into data per player in a pool. Determines rankings purely from set data
        /// </summary>
        /// <param name="poolType">Specifies bracket or round robin</param>
        /// <param name="record">List of player records for the pool</param>
        /// <returns>True if it completed succesfully, false otherwise</returns>
        private bool GeneratePoolData(PoolRecord.PoolType poolType, ref Dictionary<int, PoolRecord> record)
        {
            if (seedList == null)
            {
                richTextBoxLog.Text += "Null seed list\r\n";
                return false;
            }

            // Create a record for each entrant
            foreach (Seed seed in seedList)
            {
                if (seed.entrant == null)
                {
                    continue;
                }

                record.Add(seed.entrant.id, new PoolRecord());
                List<Set> relevantSets = setList.Where(q => q.slots.Any(r => r.entrant != null && r.entrant.id.Equals(seed.entrant.id))).ToList<Set>();
                foreach (Set set in relevantSets)
                {
                    // Ignore byes
                    if (set.slots[0].prereqType == "bye" || set.slots[1].prereqType == "bye")
                    {
                        continue;
                    }

                    if (set.slots[0].entrant != null && set.slots[1].entrant != null)
                    {
                        // Mark both players as present
                        record[seed.entrant.id].isinGroup = true;


                        // Tally the result
                        if (set.winnerId == seed.entrant.id)    // For a win
                        {
                            record[seed.entrant.id].AddMatchWins(1);

                            if (set.DisplayScore != "DQ" && set.DisplayScore != null) // Ignore W-L for DQs for now
                            {
                                if (seed.entrant.id.ToString() == set.slots[0].id)
                                {
                                    record[seed.entrant.id].AddGameWins(set.entrant1wins);
                                    record[seed.entrant.id].AddGameLosses(set.entrant2wins);
                                }
                                else if (seed.entrant.id.ToString() == set.slots[1].id)
                                {
                                    record[seed.entrant.id].AddGameWins(set.entrant2wins);
                                    record[seed.entrant.id].AddGameLosses(set.entrant1wins);
                                }

                                record[seed.entrant.id].AddMatchesActuallyPlayed(1);
                            }
                        }
                        else if (set.winnerId != null)      // For a loss
                        {
                            record[seed.entrant.id].AddMatchLosses(1);

                            // If in losers bracket
                            if (set.round < 0)
                            {
                                record[seed.entrant.id].MatchesComplete = true;
                            }

                            if (set.DisplayScore != "DQ" && set.DisplayScore != null) // Ignore W-L for DQs for now
                            {
                                if (seed.entrant.id.ToString() == set.slots[0].id)
                                {
                                    record[seed.entrant.id].AddGameWins(set.entrant1wins);
                                    record[seed.entrant.id].AddGameLosses(set.entrant2wins);
                                }
                                else if (seed.entrant.id.ToString() == set.slots[1].id)
                                {
                                    record[seed.entrant.id].AddGameWins(set.entrant2wins);
                                    record[seed.entrant.id].AddGameLosses(set.entrant1wins);
                                }

                                record[seed.entrant.id].AddMatchesActuallyPlayed(1);
                            }
                        }
                    }
                }

                // Figure out what the rankings are
                record[seed.entrant.id].rank = (int)seed.Placement;
            }


            if (poolType == PoolRecord.PoolType.Bracket)
            {
                //// Winners
                //for (int i = 1; i <= roundList.Keys.Max(); i++)
                //{
                //    foreach (Set set in roundList[i])
                //    {
                //        set.wPlacement = roundList[i].Count + 1;
                //        set.lPlacement = roundList[i].Count * 2 + 1;
                //    }
                //}

                //// Losers
                //int lastLoser = 2;
                //int newLoser = 3;
                //for (int i = roundList.Keys.Min(); i <= -1; i++)
                //{
                //    foreach (Set set in roundList[i])
                //    {
                //        set.wPlacement = lastLoser;
                //        set.lPlacement = newLoser;
                //    }
                //    lastLoser = newLoser;
                //    newLoser += roundList[i].Count;
                //}

                //foreach (List<Set> entry in roundList.Values)
                //{
                //    foreach (Set set in entry)
                //    {
                //        if (set.DisplayScore != "bye" && set.winnerId != null)
                //        {
                //            if (set.slots[0].entrant != null && set.slots[0].entrant.id == set.winnerId)
                //            {
                //                if (set.slots[0].entrant != null && set.wPlacement < record[set.slots[0].entrant.id].rank)
                //                {
                //                    record[set.slots[0].entrant.id].rank = set.wPlacement;
                //                }
                //                if (set.slots[1].entrant != null && set.lPlacement < record[set.slots[1].entrant.id].rank)
                //                {
                //                    record[set.slots[1].entrant.id].rank = set.lPlacement;
                //                }
                //            }
                //            else if (set.slots[1].entrant != null && set.slots[1].entrant.id == set.winnerId)
                //            {
                //                if (set.slots[0].entrant != null && set.lPlacement < record[set.slots[0].entrant.id].rank)
                //                {
                //                    record[set.slots[0].entrant.id].rank = set.lPlacement;
                //                }
                //                if (set.slots[1].entrant != null && set.wPlacement < record[set.slots[1].entrant.id].rank)
                //                {
                //                    record[set.slots[1].entrant.id].rank = set.wPlacement;
                //                }
                //            }
                //        }
                //    }
                //}
            }

            // Remove entrants without listed sets (smash.gg seems to list extraneous entrants sometimes)
            //for (int i = 0; i < record.Count; i++)
            //{
            //    if (record.ElementAt(i).Value.isinGroup == false)
            //    {
            //        record.Remove(record.ElementAt(i).Key);
            //        i--;
            //    }
            //}

            // Sort the entrants by their rank and W-L records
            if (poolType == PoolRecord.PoolType.Bracket)
            {
                record = record.OrderBy(x => x.Value.rank != 0).ThenBy(x => x.Value.rank).ThenByDescending(x => x.Value.MatchWinrate).ThenBy(x => x.Value.MatchesLoss).ThenByDescending(x => x.Value.MatchesWin).ThenByDescending(x => x.Value.GameWinrate).ToDictionary(x => x.Key, x => x.Value);
            }
            else if (poolType == PoolRecord.PoolType.RoundRobin)
            {
                //foreach (int entrant in record.Keys)
                //{
                //    record[entrant].rank = entrantList[entrant].Placement;
                //}

                record = record.OrderBy(x => x.Value.rank != 0).ThenBy(x => x.Value.rank).ThenByDescending(x => x.Value.MatchWinrate).ThenBy(x => x.Value.MatchesLoss).ThenByDescending(x => x.Value.MatchesWin).ThenByDescending(x => x.Value.GameWinrate).ToDictionary(x => x.Key, x => x.Value);
            }

            return true;
        }
        #endregion

        #region TextBox Output Methods
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

                if (entrant.Value.participants.Count == 1)
                {
                    richTextBoxEntrants.Text += entrant.Value.participants[0].player.id.ToString().PadRight(9);
                }
                else if (entrant.Value.participants.Count == 2)
                {
                    richTextBoxEntrants.Text += entrant.Value.participants[0].player.id.ToString().PadRight(9) + "/" + entrant.Value.participants[1].player.id.ToString().PadRight(9);
                }
                else return;

                int lastPlayerPadding = padding;

                // Output all players seperated by slashes
                for (int i = 0; i < entrant.Value.participants.Count; i++)
                {
                    if (i == entrant.Value.participants.Count - 1)
                    {
                        richTextBoxEntrants.Text += entrant.Value.participants[i].gamerTag.PadRight(lastPlayerPadding + (entrant.Value.participants.Count - 1) * 3);
                    }
                    else
                    {
                        richTextBoxEntrants.Text += entrant.Value.participants[i].gamerTag + " / ";
                        lastPlayerPadding -= entrant.Value.participants[i].gamerTag.Length;
                    }
                }

                richTextBoxEntrants.Text += "  ";

                // Output all countries seperated by slashes
                for (int i = 0; i < entrant.Value.participants.Count; i++)
                {
                    if (i == entrant.Value.participants.Count - 1)
                    {
                        richTextBoxEntrants.Text += entrant.Value.participants[i].user.location.country + "\r\n";
                    }
                    else
                    {
                        richTextBoxEntrants.Text += entrant.Value.participants[i].user.location.country + " / ";
                        lastPlayerPadding -= entrant.Value.participants[i].gamerTag.Length;
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
            textbox.AppendText(set.round.ToString().PadRight(4));

            // Highlight font if player 1 is the winner
            if (set.winnerId == set.slots[0].entrant.id)
            {
                textbox.SelectionFont = new Font(textbox.Font, FontStyle.Italic);
            }
            else
            {
                textbox.SelectionFont = new Font(textbox.Font, FontStyle.Regular);
            }

            // Output player 1
            textbox.AppendText(entrantList[set.slots[0].entrant.id].participants[0].gamerTag.PadRight(p1padding) + "  ");

            // Output player 1's score
            textbox.SelectionFont = new Font(textbox.Font, FontStyle.Regular);
            if (set.entrant1wins == -1)
            {
                textbox.AppendText("DQ");
            }
            else if (set.entrant1wins != Consts.UNKNOWN)
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
            else if (set.entrant2wins != Consts.UNKNOWN)
            {
                textbox.AppendText(set.entrant2wins.ToString().PadRight(2));
            }
            else
            {
                textbox.AppendText("? ");
            }

            // Highlight font if player 2 is the winner
            if (set.winnerId == set.slots[1].entrant.id)
            {
                textbox.SelectionFont = new Font(textbox.Font, FontStyle.Italic);
            }
            else
            {
                textbox.SelectionFont = new Font(textbox.Font, FontStyle.Regular);
            }
            // Output player 2
            textbox.AppendText("  " + entrantList[set.slots[1].entrant.id].participants[0].gamerTag + "\r\n");
        }

        /// <summary>
        /// Outputs a prize pool table based on available bracket info
        /// </summary>
        /// <param name="standings">Standings from smash.gg, sorted by rank</param>
        /// <param name="startEntrant">The first entrant to output in this row</param>
        /// <param name="rows">Number of rows to output</param>
        private void OutputPrizePoolTable(List<Standing> standings, int startEntrant, int rows)
        {
            // Output the start of the prize pool slot
            if (selectedEvent.Type == Event.EventType.Singles)
            {
                richTextBoxLpOutput.Text += "{{prize pool slot|place=";
            }
            else if (selectedEvent.Type == Event.EventType.Doubles)
            {
                richTextBoxLpOutput.Text += "{{prize pool slot doubles|place=";
            }

            // Output the place
            if (rows == 1)
            {
                richTextBoxLpOutput.Text += standings[startEntrant].placement;
            }
            else
            {
                richTextBoxLpOutput.Text += standings[startEntrant].placement + "-" + (standings[startEntrant].placement + rows - 1);
            }

            // Ouptut the parameter for prize money
            richTextBoxLpOutput.Text += "|usdprize=\r\n";


            if (selectedEvent.Type == Event.EventType.Singles)
            {
                // Output the specified number of entrants
                for (int i = 0; i < rows; i++)
                {
                    // Assume there is only 1 player output their info
                    richTextBoxLpOutput.Text += "|" + standings[startEntrant + i].entrant.participants[0].gamerTag +
                                                "|flag" + (i + 1) + "=" + standings[startEntrant + i].entrant.participants[0].user.location.country +
                                                "|heads" + (i + 1) + "= |team" + (i + 1) + "=\r\n";
                }
            }
            else if (selectedEvent.Type == Event.EventType.Doubles)
            {
                // Output the specified number of entrants
                for (int i = 0; i < rows; i++)
                {
                    // Assume there are 2 players, and output their info
                    richTextBoxLpOutput.Text += "|" + standings[startEntrant + i].entrant.participants[0].gamerTag +
                                                "|flag" + (i + 1) + "p1=" + standings[startEntrant + i].entrant.participants[0].user.location.country +
                                                "|heads" + (i + 1) + "p1=" +
                                                "|" + standings[startEntrant + i].entrant.participants[1].gamerTag +
                                                "|flag" + (i + 1) + "p2=" + standings[startEntrant + i].entrant.participants[1].user.location.country +
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
            richTextBoxEntrants.Clear();
            richTextBoxWinners.Clear();
            richTextBoxLosers.Clear();
            //entrantList.Clear();
            setList.Clear();
            seedList.Clear();
            roundList.Clear();
            matchOffsetPerRound.Clear();
        }

        private void ClearLog()
        {
            richTextBoxLog.Clear();
        }

        /// <summary>
        /// Update the numericUpDown controls
        /// </summary>
        private void UpdateNumericUpDownControls(Event.EventType eventType)
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
                    if (eventType == Event.EventType.Singles)
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
                    if (eventType == Event.EventType.Singles)
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
            textBoxTournamentUrl.Enabled = false;

            buttonFill.Enabled = false;
            buttonGetTournament.Enabled = false;
            buttonPrizePool.Enabled = false;
            buttonWinnerShift.Enabled = false;
            buttonLoserShift.Enabled = false;
            buttonGroupTable.Enabled = false;

            checkBoxFillByes.Enabled = false;
            checkBoxGuessFinal.Enabled = false;
            checkBoxLockLosers.Enabled = false;
            checkBoxLockWinners.Enabled = false;
            checkBoxLosers.Enabled = false;
            checkBoxWinners.Enabled = false;
            checkBoxMatchDetails.Enabled = false;
            checkBoxUnfinished.Enabled = false;

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
            richTextBoxExLpFinalBracket.Enabled = false;
            richTextBoxLog.Enabled = false;
            richTextBoxWinners.Enabled = false;
            richTextBoxLosers.Enabled = false;
            textBoxHeaderWinners.Enabled = false;
            textBoxHeaderLosers.Enabled = false;
            textBoxHeaderFinals.Enabled = false;
            richTextBoxLpOutput.Enabled = false;
        }

        /// <summary>
        /// Unlocks form controls
        /// </summary>
        private void UnlockControls()
        {
            textBoxTournamentUrl.Enabled = true;

            buttonFill.Enabled = true;
            buttonGetTournament.Enabled = true;
            buttonPrizePool.Enabled = true;
            buttonWinnerShift.Enabled = true;
            buttonLoserShift.Enabled = true;
            buttonGroupTable.Enabled = true;

            checkBoxFillByes.Enabled = true;
            checkBoxGuessFinal.Enabled = true;
            checkBoxLockLosers.Enabled = true;
            checkBoxLockWinners.Enabled = true;
            checkBoxLosers.Enabled = true;
            checkBoxWinners.Enabled = true;
            checkBoxMatchDetails.Enabled = true;
            checkBoxUnfinished.Enabled = true;

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
            richTextBoxExLpFinalBracket.Enabled = true;
            richTextBoxLog.Enabled = true;
            richTextBoxWinners.Enabled = true;
            richTextBoxLosers.Enabled = true;
            textBoxHeaderWinners.Enabled = true;
            textBoxHeaderLosers.Enabled = true;
            textBoxHeaderFinals.Enabled = true;
            richTextBoxLpOutput.Enabled = true;

            // Re-lock numericUpDown controls if needed
            checkBoxLock_CheckedChanged(new object(), new EventArgs());
        }
        #endregion

        #region AKA Database
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
        #endregion

        #region Authentication
        private void buttonAuth_Click(object sender, EventArgs e)
        {
            Authentication tokenWindow = new Authentication(ref authToken);
            tokenWindow.ShowDialog();

            authToken = tokenWindow.token;
            if (authToken == "")
            {
                buttonAuthentication.BackColor = Color.PaleVioletRed;
            }
            else
            {
                buttonAuthentication.BackColor = SystemColors.Control;
            }

            apiQuery.SetToken(authToken);
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (authToken != string.Empty)
            {
                try
                {
                    //Save credentials
                    StreamWriter userinfo = new StreamWriter(@"userinfo");
                    userinfo.WriteLine(authToken.Protect());

                    userinfo.Close();
                }
                catch
                {
                    Console.WriteLine("Couldn't save credentials.");
                }
            }
        }
        #endregion

        #region TreeView Methods
        private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            // The code only executes if the user caused the checked state to change.
            if (e.Action != TreeViewAction.Unknown)
            {
                int checkedNodes = CountCheckedTreeNodes(treeView1.Nodes[0]);

                if (checkedNodes >= 2)
                {
                    TreeNodeData tag = (TreeNodeData)e.Node.Tag;

                    // Deselect other nodes
                    DeselectOtherTreeNodes(treeView1.Nodes[0], e.Node);

                    //// Set checked node properties
                    //selectedEvent = FindParentEvent(e.Node);
                    //selectedObjectId = tag.id;
                    //selectedObjectType = tag.nodetype;
                }

                checkedNodes = CountCheckedTreeNodes(treeView1.Nodes[0]);
                if (checkedNodes == 1)
                {
                    TreeNodeData tag = (TreeNodeData)e.Node.Tag;

                    // Topmost node will cause an exception
                    if (e.Node == treeView1.Nodes[0]) return;

                    // Find the event node
                    selectedEvent = FindParentEvent(e.Node);

                    // Get the wave id
                    if (tag.nodetype == TreeNodeData.NodeType.Wave)
                    {
                        TreeNodeData samplePhaseGroupTag = (TreeNodeData)e.Node.Nodes[0].Tag;
                        TreeNode currentPhase = e.Node.Parent;

                        // Get phase
                        var testPhases = selectedEvent.phases.Where(p => p.name == currentPhase.Text).First();

                        // phaseGroups is typically null on the event object
                        selectedObjectId = testPhases.waves.Where(w => w.Value.First().id == samplePhaseGroupTag.id).First().Value.ElementAt(0).wave.id;
                        selectedObjectType = TreeNodeData.NodeType.Wave;
                    }

                    // Determine if there is only one phasegroup and select it if there is
                    else if (e.Node.Nodes.Count <= 1)
                    {
                        if (tag.nodetype == TreeNodeData.NodeType.Phase)
                        {
                            Phase selectedPhase = tournament.events.Where(q => q == selectedEvent).First<Event>().phases
                                                                   .Where(q => q.id == tag.id).First<Phase>();
                            if (selectedPhase.phasegroups.nodes.Count == 1)
                            {
                                selectedObjectId = selectedPhase.phasegroups.nodes[0].id;
                                selectedObjectType = TreeNodeData.NodeType.PhaseGroup;
                            }
                            else
                            {
                                selectedObjectId = tag.id;
                                selectedObjectType = tag.nodetype;
                            }
                        }
                        else
                        {
                            selectedObjectId = tag.id;
                            selectedObjectType = tag.nodetype;
                        }
                    }
                    else
                    {
                        selectedObjectId = tag.id;
                        selectedObjectType = tag.nodetype;
                    }
                }
                else
                {
                    // Clear checked node properties
                    selectedEvent = null;
                    selectedObjectId = 0;
                    selectedObjectType = TreeNodeData.NodeType.Unknown;
                }
            }
        }

        private int CountCheckedTreeNodes(TreeNode node)
        {
            int count = 0;
            if (node.Checked)
            {
                count++;
            }

            foreach (TreeNode child in node.Nodes)
            {
                count += CountCheckedTreeNodes(child);
            }

            return count;
        }

        private void DeselectOtherTreeNodes(TreeNode root, TreeNode node)
        {
            foreach (TreeNode child in root.Nodes)
            {
                if (child.Checked && child != node)
                {
                    child.Checked = false;
                }

                DeselectOtherTreeNodes(child, node);
            }
        }

        /// <summary>
        /// Recursively navigate up the tournament tree until you find the event node hosting the object
        /// </summary>
        /// <param name="node">The node in question</param>
        /// <returns>The id of the parent node</returns>
        private Event FindParentEvent(TreeNode node)
        {
            TreeNodeData tag = (TreeNodeData)node.Tag;
            if (tag.nodetype == TreeNodeData.NodeType.Event)
            {
                return tournament.events.Where(q => q.id == tag.id).First<Event>();
            }
            else
            {
                return FindParentEvent(node.Parent);
            }
        }
        #endregion

        private void buttonGetData_Click(object sender, EventArgs e)
        {
            // Clear data
            ClearData();
            checkBoxWinners.Checked = false;
            checkBoxLosers.Checked = false;
            richTextBoxLog.Clear();

            if (selectedEvent == null)
            {
                richTextBoxLog.Text += "Get a tournament and select an object first";
                return;
            }

            // Clear data
            ClearData();
            checkBoxWinners.Checked = false;
            checkBoxLosers.Checked = false;

            try
            {
                // Get entrants in the event
                if (selectedEvent.teamRosterSize == null)
                {
                    apiQuery.GetEventEntrants(selectedEvent.id, 1, out entrantList);
                }
                else
                {
                    apiQuery.GetEventEntrants(selectedEvent.id, selectedEvent.teamRosterSize.maxPlayers, out entrantList);
                }
            }
            catch (Exception error)
            {
                richTextBoxLpOutput.Text = "Failed to get event entrants\r\n";
                richTextBoxLpOutput.Text += error.Message;
            }

            try
            {
                switch (selectedObjectType)
                {
                    // Get all phasegroups in a phase
                    case TreeNodeData.NodeType.Phase:
                        Phase selectedPhase = selectedEvent.phases.Where(q => q.id == selectedObjectId).First<Phase>();

                        // For a single phasegroup phase, assume we just want the phasegroup.
                        if (selectedPhase.phasegroups.nodes.Count == 1)
                        {
                            richTextBoxLog.Text += string.Format("Single phasegroup phase detected\r\n");
                            if (!(apiQuery.GetSets(selectedObjectId, out setList, checkBoxMatchDetails.Checked, checkBoxSorting.Checked)))
                            {
                                richTextBoxLog.Text += string.Format("Get failed\r\n");
                                return;
                            }
                            if (setList == null)
                            {
                                richTextBoxLog.Text += string.Format("Set list not retrieved\r\n");
                                return;
                            }

                            // Get standings
                            seedList = apiQuery.GetSeedStandings(selectedObjectId);

                            // Generate the round list
                            ProcessBracket(selectedEvent.Type);
                            break;
                        }

                        // Set the pool type
                        PoolRecord.PoolType poolType;
                        if (radioButtonBracket.Checked)
                        {
                            poolType = PoolRecord.PoolType.Bracket;
                        }
                        else if (radioButtonRR.Checked)
                        {
                            poolType = PoolRecord.PoolType.RoundRobin;
                        }
                        else
                        {
                            richTextBoxLog.Text += "Specify a bracket type.\r\n";
                            return;
                        }



                        // Setup progress bar
                        progressBar.Minimum = 0;
                        progressBar.Maximum = selectedPhase.phasegroups.nodes.Count;
                        progressBar.Value = 1;
                        progressBar.Step = 1;

                        // Retrieve pages for each group
                        string lastWave = string.Empty;
                        foreach (PhaseGroup group in selectedPhase.phasegroups.nodes)
                        {
                            if (setList == null) setList = new List<Set>();

                            // Clear data
                            ClearData();
                            setList.Clear();

                            Dictionary<int, PoolRecord> poolData = new Dictionary<int, PoolRecord>();

                            // Get all sets in the phasegroup
                            apiQuery.GetSets(group.id, out setList, checkBoxMatchDetails.Checked, checkBoxSorting.Checked);
                            if (setList == null)
                            {
                                richTextBoxLog.Text += string.Format("Set list not retrieved for {0}\r\n", group.id);
                                continue;
                            }

                            // Get standings
                            seedList = apiQuery.GetSeedStandings(group.id);

                            // Generate the round list
                            ProcessBracket(selectedEvent.Type);

                            if (!GeneratePoolData(poolType, ref poolData)) continue;

                            if (selectedEvent.Type == Event.EventType.Singles)
                            {
                                OutputSinglesPhase(selectedPhase, poolData, ref lastWave, group, group.State, poolType);
                            }
                            else if (selectedEvent.Type == Event.EventType.Doubles)
                            {
                                OutputDoublesPhase(selectedPhase, poolData, ref lastWave, group, group.State, poolType);
                            }

                            // Increment the progress bar
                            progressBar.PerformStep();
                        }
                        break;

                    // Get a single phasegroup
                    case TreeNodeData.NodeType.PhaseGroup:
                        if (!(apiQuery.GetSets(selectedObjectId, out setList, checkBoxMatchDetails.Checked, checkBoxSorting.Checked)))
                        {
                            richTextBoxLog.Text += string.Format("Get failed\r\n");
                            return;
                        }
                        if (setList == null)
                        {
                            richTextBoxLog.Text += string.Format("Set list not retrieved\r\n");
                            return;
                        }

                        // Get standings
                        seedList = apiQuery.GetSeedStandings(selectedObjectId);

                        // Generate the round list
                        ProcessBracket(selectedEvent.Type);
                        break;

                    // Get a single wave in a phase
                    case TreeNodeData.NodeType.Wave:
                        // Set the pool type
                        if (radioButtonBracket.Checked)
                        {
                            poolType = PoolRecord.PoolType.Bracket;
                        }
                        else if (radioButtonRR.Checked)
                        {
                            poolType = PoolRecord.PoolType.RoundRobin;
                        }
                        else
                        {
                            richTextBoxLog.Text += "Specify a bracket type.\r\n";
                            return;
                        }

                        selectedPhase = selectedEvent.phases.Where(q => q.phasegroups.nodes.Any(r => r.wave.id == selectedObjectId)).First();
                        KeyValuePair<string, List<PhaseGroup>> wave = selectedPhase.waves.Where(q => (int)q.Value[0].wave.id == selectedObjectId).First();
                        lastWave = wave.Key;

                        // Setup progress bar
                        progressBar.Minimum = 0;
                        progressBar.Maximum = wave.Value.Count;
                        progressBar.Value = 1;
                        progressBar.Step = 1;

                        // Retrieve pages for each group
                        foreach (PhaseGroup group in wave.Value)
                        {
                            if (setList == null) setList = new List<Set>();
                            if (seedList == null) seedList = new List<Seed>();

                            // Clear data
                            ClearData();
                            setList.Clear();
                            seedList.Clear();

                            Dictionary<int, PoolRecord> poolData = new Dictionary<int, PoolRecord>();

                            // Get all sets in the phasegroup
                            apiQuery.GetSets(group.id, out setList, checkBoxMatchDetails.Checked, checkBoxSorting.Checked);
                            if (setList == null || setList.Count == 0)
                            {
                                richTextBoxLog.Text += string.Format("Set list not retrieved for {0}\r\n", group.id);
                                continue;
                            }
                            
                            // Get standings
                            seedList = apiQuery.GetSeedStandings(group.id);
                            if (seedList == null || seedList.Count == 0)
                            {
                                richTextBoxLog.Text += string.Format("Seed list not retrieved for {0}\r\n", group.id);
                                continue;
                            }
         
                            // Generate the round list
                            ProcessBracket(selectedEvent.Type);

                            if (!GeneratePoolData(poolType, ref poolData)) continue;

                            if (selectedEvent.Type == Event.EventType.Singles)
                            {
                                OutputSinglesPhase(selectedPhase, poolData, ref lastWave, group, group.State, poolType);
                            }
                            else if (selectedEvent.Type == Event.EventType.Doubles)
                            {
                                OutputDoublesPhase(selectedPhase, poolData, ref lastWave, group, group.State, poolType);
                            }

                            // Increment the progress bar
                            progressBar.PerformStep();
                        }

                        break;

                    // Invalid object selected - do nothing
                    default:
                        richTextBoxLog.Text += "Can't do anything with that object\r\n";
                        break;
                }
            }
            catch (Exception error)
            {
                richTextBoxLpOutput.Text = "Failed to get data\r\n";
                richTextBoxLpOutput.Text += error.Message;
                richTextBoxLpOutput.Text += error.StackTrace;
            }

    richTextBoxLog.Text += string.Format("Get data complete.\r\n");
        }

        /// <summary>
        /// Retrieves a single event for the tournament tree
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonGetEvent_Click(object sender, EventArgs e)
        {
            LockControls();

            tabControl.SelectedTab = tabPageTournamentExplorer;

            // Find tournament slug
            textBoxTournamentUrl.Text = textBoxTournamentUrl.Text.Trim();
            textBoxTournamentUrl.Text = textBoxTournamentUrl.Text.Replace("/events/", "/event/");
            int tournamentMarker = textBoxTournamentUrl.Text.IndexOf("tournament/");
            if (tournamentMarker == -1)
            {
                richTextBoxLog.Text += "Invalid URL\r\n";
                UnlockControls();
                return;
            }
            int endtournamentMarker = textBoxTournamentUrl.Text.IndexOf("/", tournamentMarker + 11);
            if (endtournamentMarker == -1)
            {
                endtournamentMarker = textBoxTournamentUrl.Text.Length;
            }
            int tournamentSlugLength = endtournamentMarker - (tournamentMarker + 11);
            string tournamentSlug = textBoxTournamentUrl.Text.Substring(tournamentMarker + 11, tournamentSlugLength);

            // Find event slug
            int eventMarker = textBoxTournamentUrl.Text.IndexOf("/event/");
            if (eventMarker == -1)
            {
                richTextBoxLog.Text += "Invalid URL - no event found\r\n";
                UnlockControls();
                return;
            }
            int endEventMarker = textBoxTournamentUrl.Text.IndexOf("/", eventMarker + 7);
            if (endEventMarker == -1)
            {
                endEventMarker = textBoxTournamentUrl.Text.Length;
            }
            string eventSlug = textBoxTournamentUrl.Text.Substring(tournamentMarker + 11, tournamentSlugLength + 7 + endEventMarker - (eventMarker + 7));

            // Clear the treeview
            treeView1.Nodes.Clear();
            tournament = apiQuery.GetSingleEvent(tournamentSlug, "tournament/" + eventSlug);
            if (tournament.events == null)
            {
                richTextBoxLog.Text += "Failed to get tournament\r\n";
                UnlockControls();
                return;
            }

            // Begin updating the treeview
            treeView1.BeginUpdate();
            TreeNode root = new TreeNode(tournament.slug);
            treeView1.Nodes.Add(root);

            // Go through all events in the tournament
            foreach (Event tournamentEvent in tournament.events)
            {
                TreeNode eventNode = new TreeNode(tournamentEvent.name);

                // Save relevant data to event tag
                TreeNodeData eventNodeTag = new TreeNodeData();
                eventNodeTag.id = tournamentEvent.id;
                eventNodeTag.name = tournamentEvent.name;
                eventNodeTag.nodetype = TreeNodeData.NodeType.Event;
                if (tournamentEvent.teamRosterSize != null)
                {
                    eventNodeTag.playersPerEntrant = tournamentEvent.teamRosterSize.maxPlayers;
                }
                eventNode.Tag = eventNodeTag;


                // Display number of entrants
                switch (tournamentEvent.Type)
                {
                    case Event.EventType.Singles:
                        eventNode.Text = tournamentEvent.name + " (" + tournamentEvent.numEntrants.ToString() + " players)";
                        break;
                    case Event.EventType.Doubles:
                        eventNode.Text = tournamentEvent.name + " (" + tournamentEvent.numEntrants.ToString() + " entrants, " + (2 * tournamentEvent.numEntrants).ToString() + " players)";
                        break;
                    default:
                        eventNode.Text = tournamentEvent.name + " (" + tournamentEvent.numEntrants.ToString() + " entrants)";
                        break;
                }

                // Set the event bg color
                switch (tournamentEvent.state)
                {
                    case Tournament.ActivityState.Completed:
                        eventNode.BackColor = Color.LightGreen;
                        break;
                    case Tournament.ActivityState.Active:
                        eventNode.BackColor = Color.LightYellow;
                        break;
                    default:
                        eventNode.BackColor = Color.LightPink;
                        break;
                }

                root.Nodes.Add(eventNode);

                foreach (Phase phase in tournamentEvent.phases)
                {
                    TreeNode phaseNode = new TreeNode(phase.name);

                    // Save relevant data to phase tag
                    TreeNodeData phaseNodeTag = new TreeNodeData();
                    phaseNodeTag.id = phase.id;
                    phaseNodeTag.name = phase.name;
                    phaseNodeTag.nodetype = TreeNodeData.NodeType.Phase;
                    phaseNode.Tag = phaseNodeTag;

                    if (phase.waves.Count > 0)
                    {
                        // Add waves for each phase
                        int waveActive = 0;
                        int waveComplete = 0;
                        foreach (KeyValuePair<string, List<PhaseGroup>> wave in phase.waves)
                        {
                            TreeNode waveNode = new TreeNode(wave.Key);

                            // Save relevant data to wave tag
                            TreeNodeData waveNodeTag = new TreeNodeData();
                            if (wave.Value[0].wave == null)
                            {
                                waveNodeTag.id = 0;
                            }
                            else
                            {
                                waveNodeTag.id = wave.Value[0].Number;
                            }

                            waveNodeTag.name = wave.Key;
                            waveNodeTag.nodetype = TreeNodeData.NodeType.Wave;
                            waveNode.Tag = waveNodeTag;

                            // Set wave text
                            waveNode.Text = "Wave " + wave.Key + " (" + wave.Value.Count + " groups)";

                            // Add phasegroups associated with each wave
                            int phasegroupActive = 0;
                            int phasegroupComplete = 0;
                            foreach (PhaseGroup phasegroup in wave.Value)
                            {
                                TreeNode phasegroupNode = new TreeNode(phasegroup.displayIdentifier);

                                // Save relevant data to phasegroup tag
                                TreeNodeData phasegroupNodeTag = new TreeNodeData();
                                phasegroupNodeTag.id = phasegroup.id;
                                phasegroupNodeTag.name = phasegroup.displayIdentifier;
                                phasegroupNodeTag.nodetype = TreeNodeData.NodeType.PhaseGroup;
                                phasegroupNode.Tag = phasegroupNodeTag;

                                // Set the phasegroup bg color
                                switch (phasegroup.State)
                                {
                                    case Tournament.ActivityState.Completed:
                                        phasegroupNode.BackColor = Color.LightGreen;
                                        phasegroupComplete++;
                                        break;
                                    case Tournament.ActivityState.Active:
                                        phasegroupNode.BackColor = Color.LightYellow;
                                        phasegroupActive++;
                                        break;
                                    default:
                                        phasegroupNode.BackColor = Color.LightPink;
                                        break;
                                }

                                waveNode.Nodes.Add(phasegroupNode);
                            }

                            if (phasegroupComplete == wave.Value.Count)
                            {
                                waveNode.BackColor = Color.LightGreen;
                                waveComplete++;
                            }
                            else if (phasegroupActive >= 1 || phasegroupComplete >= 1)
                            {
                                waveNode.BackColor = Color.LightYellow;
                                waveActive++;
                            }
                            else
                            {
                                waveNode.BackColor = Color.LightPink;
                            }

                            phaseNode.Nodes.Add(waveNode);
                        }

                        if (waveComplete == phase.waves.Count)
                        {
                            phaseNode.BackColor = Color.LightGreen;

                        }
                        else if (waveActive >= 1 || waveComplete >= 1)
                        {
                            phaseNode.BackColor = Color.LightYellow;
                        }
                        else
                        {
                            phaseNode.BackColor = Color.LightPink;
                        }
                    }
                    else
                    {
                        // Add phasegroups associated with each phase
                        int phasegroupActive = 0;
                        int phasegroupComplete = 0;
                        if (phase.phasegroups.nodes.Count >= 2)
                        {
                            foreach (PhaseGroup phasegroup in phase.phasegroups.nodes)
                            {
                                TreeNode phasegroupNode = new TreeNode(phasegroup.displayIdentifier + " " + phasegroup.WaveLetter);

                                // Save relevant data to phasegroup tag
                                TreeNodeData phasegroupNodeTag = new TreeNodeData();
                                phasegroupNodeTag.id = phasegroup.id;
                                phasegroupNodeTag.name = phasegroup.displayIdentifier;
                                phasegroupNodeTag.nodetype = TreeNodeData.NodeType.PhaseGroup;
                                phasegroupNode.Tag = phasegroupNodeTag;

                                // Set the phasegroup bg color
                                switch (phasegroup.State)
                                {
                                    case Tournament.ActivityState.Completed:
                                        phasegroupNode.BackColor = Color.LightGreen;
                                        phasegroupComplete++;
                                        break;
                                    case Tournament.ActivityState.Active:
                                        phasegroupNode.BackColor = Color.LightYellow;
                                        phasegroupActive++;
                                        break;
                                    default:
                                        phasegroupNode.BackColor = Color.LightPink;
                                        break;
                                }

                                phaseNode.Nodes.Add(phasegroupNode);
                            }
                        }
                        else
                        {
                            foreach (PhaseGroup phasegroup in phase.phasegroups.nodes)
                            {
                                switch (phasegroup.State)
                                {
                                    case Tournament.ActivityState.Completed:
                                        phasegroupComplete++;
                                        break;
                                    case Tournament.ActivityState.Active:
                                        phasegroupActive++;
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }

                        // Set bgcolor for phase
                        if (phasegroupComplete == phase.phasegroups.nodes.Count)
                        {
                            phaseNode.BackColor = Color.LightGreen;
                        }
                        else if (phasegroupActive >= 1 || phasegroupComplete >= 1)
                        {
                            phaseNode.BackColor = Color.LightYellow;
                        }
                        else
                        {
                            phaseNode.BackColor = Color.LightPink;
                        }
                    }

                    // Add phase to event
                    eventNode.Nodes.Add(phaseNode);
                }
            }

            // Expand the tournament node
            root.Expand();
            treeView1.EndUpdate();

            UnlockControls();

            richTextBoxLog.Text += "Get event complete.\r\n";
        }

        private void buttonAKAList_Click(object sender, EventArgs e)
        {
            // Check to see if there are entrants to output
            if (entrantList.Count == 0) {
                richTextBoxLog.Text += "No entrants to output.\r\n";
                return;
            }

            // Output entrants using smash.gg ids
            richTextBoxLpOutput.Clear();
            foreach (KeyValuePair<int, Entrant> entrant in entrantList)
            {
                var uniqueId = entrant.Value.participants[0].player.id;
                var country = entrant.Value.participants[0].user.location.country;
                var tag = entrant.Value.participants[0].gamerTag;

                richTextBoxLpOutput.Text += "{{AltSlot |player= |flag="  + country +
                    " |alts=" + tag +
                    " |smashgg=" + uniqueId +
                    "}}\r\n";
            }
        }

        private void buttonSWT_Click(object sender, EventArgs e)
        {
            if (selectedEvent == null)
            {
                richTextBoxLog.Text += "No event selected\r\n";
                return;
            }

            List<Standing> standingList = apiQuery.GetStandings(selectedEvent.id, (int)numericUpDownPrizePool.Value);


            richTextBoxLog.Text += "Outputting results\r\n";

            // Loop through the records to remove placements of 0
            for (int i = 0; i < standingList.Count; i++)
            {
                if (standingList[i].placement == 0)
                {
                    standingList.RemoveAt(i);
                    i--;
                }
            }

            // Loop through the records to add entries to the prize pool
            for (int j = 0; j < standingList.Count; j++)
            {
                if (selectedEvent.Type == Event.EventType.Singles)
                {
                    // Assume there is only 1 player output their info
                    richTextBoxLpOutput.Text += "|p" + (j+1) + "=" + standingList[j].entrant.participants[0].gamerTag +
                                                "|flag" + (j+1) + "=" + standingList[j].entrant.participants[0].user.location.country +
                                                "\r\n";
                }
            }

            richTextBoxLpOutput.Text = richTextBoxLpOutput.Text.Trim();
            richTextBoxLog.Text += "Done.\r\n";
        }
    }
}
