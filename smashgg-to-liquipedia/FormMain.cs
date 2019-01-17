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
    public partial class FormMain : Form
    {
        static string SMASH_DB_URI = "http://liquipedia.net/smash/api.php?action=parse&page=Liquipedia:Players_Regex&prop=revid|wikitext&format=json";
        static string FIGHTERS_DB_URI = "http://liquipedia.net/fighters/api.php?action=parse&page=Liquipedia:Players_Regex&prop=revid|wikitext&format=json";

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

        public enum BracketSide { Winners, Losers }

        Dictionary<int, Entrant> entrantList = new Dictionary<int, Entrant>();
        List<Set> setList = new List<Set>();
        Dictionary<int, List<Set>> roundList = new Dictionary<int, List<Set>>();
        List<Phase> phaseList = new List<Phase>();
        Dictionary<int, int> matchOffsetPerRound = new Dictionary<int, int>();

        Tournament tournament;
        Event selectedEvent;
        int selectedObjectId;
        Smashgg.TreeNodeData.NodeType selectedObjectType;
        
        PlayerDatabase playerdb;

        string authToken = string.Empty;

        public FormMain()
        {
            InitializeComponent();

            try
            {
                StreamReader fileinfo = new StreamReader(@"userinfo");
                authToken = fileinfo.ReadLine().Trim().Unprotect();

                fileinfo.Close();
            }
            catch
            {
                Console.WriteLine("No saved credentials.");
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
            string slug = textBoxTournamentUrl.Text.Substring(tournamentMarker+11, endtournamentMarker - (tournamentMarker + 11));

            bool success;
            string errors;
            ApiQueries apiQuery = new ApiQueries(authToken, out success);
            if (!success)
            {
                richTextBoxLog.Text += "Failed to get API endpoint\r\n";
            }
            else
            {
                tournament = apiQuery.GetEvents(slug, out errors);
                if (errors != string.Empty)
                {
                    richTextBoxLog.Text += errors;
                }
                else
                {
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
                                    waveNodeTag.nodetype = TreeNodeData.NodeType.Wave;
                                    waveNode.Tag = waveNodeTag;

                                    // Set wave text
                                    waveNode.Text = "Wave " + wave.Key + " (" + wave.Value.Count + " groups)";

                                    // Add phasegroups associated with each wave
                                    int phasegroupActive = 0;
                                    int phasegroupComplete = 0;
                                    foreach (PhaseGroup phasegroup in wave.Value)
                                    {
                                        TreeNode phasegroupNode = new TreeNode(phasegroup.displayIdentifier + " " + phasegroup.Wave);

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
                                    else if (phasegroupActive > 1)
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
                                else if (waveActive > 1)
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
                                if (phase.phasegroups.Count >= 2)
                                {
                                    foreach (PhaseGroup phasegroup in phase.phasegroups)
                                    {
                                        TreeNode phasegroupNode = new TreeNode(phasegroup.displayIdentifier + " " + phasegroup.Wave);

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
                                    foreach (PhaseGroup phasegroup in phase.phasegroups)
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
                                if (phasegroupComplete == phase.phasegroups.Count)
                                {
                                    phaseNode.BackColor = Color.LightGreen;
                                }
                                else if (phasegroupActive > 1)
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
                }
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
            Liquipedia.LpOutput lpout = new Liquipedia.LpOutput(selectedEvent.Type, ref entrantList, ref setList, ref roundList);

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
                    checkBoxFillByes.Checked, checkBoxFillByeWins.Checked, checkBoxR1Only.Checked, checkBoxUnfinished.Checked, checkBoxMatchDetails.Checked);
            }
            if (checkBoxLosers.Checked)
            {
                lpout.fillBracketSingles(-(int)numericUpDownLosersStart.Value, -(int)numericUpDownLosersEnd.Value, (int)numericUpDownLosersOffset.Value, ref output, matchOffsetPerRound,
                    checkBoxFillByes.Checked, checkBoxFillByeWins.Checked, checkBoxR1Only.Checked, checkBoxUnfinished.Checked, checkBoxMatchDetails.Checked);
            }
            if (checkBoxGuessFinal.Checked)
            {
                finalBracketOutput = richTextBoxExLpFinalBracket.Text;

                foreach (KeyValuePair<int, List<Set>> gf in roundList)
                {
                    // Get grand finals
                    if (gf.Value[0].isGF == true)
                    {
                        // Fill in R3
                        lpout.fillBracketSingles(gf.Key, gf.Key, 3 - gf.Key, ref finalBracketOutput, matchOffsetPerRound,
                            checkBoxFillByes.Checked, checkBoxFillByeWins.Checked, checkBoxR1Only.Checked, checkBoxUnfinished.Checked, checkBoxMatchDetails.Checked);

                        // Get losers finals
                        foreach (KeyValuePair<int, List<Set>> lf in roundList)
                        {
                            if (gf.Value[0].entrant2PrereqId == lf.Value[0].id)
                            {
                                // Fill in L2
                                lpout.fillBracketSingles(lf.Key, lf.Key, 2 - Math.Abs(lf.Key), ref finalBracketOutput, matchOffsetPerRound,
                                    checkBoxFillByes.Checked, checkBoxFillByeWins.Checked, checkBoxR1Only.Checked, checkBoxUnfinished.Checked, checkBoxMatchDetails.Checked);

                                // Get losers semis
                                foreach (KeyValuePair<int, List<Set>> ls in roundList)
                                {
                                    if (lf.Value[0].entrant2PrereqId == ls.Value[0].id)
                                    {
                                        // Fill in L1
                                        lpout.fillBracketSingles(ls.Key, ls.Key, 1 - Math.Abs(ls.Key), ref finalBracketOutput, matchOffsetPerRound,
                                            checkBoxFillByes.Checked, checkBoxFillByeWins.Checked, checkBoxR1Only.Checked, checkBoxUnfinished.Checked, checkBoxMatchDetails.Checked);
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
                                lpout.fillBracketSingles(wf.Key, wf.Key, 1 - wf.Key, ref finalBracketOutput, matchOffsetPerRound,
                                    checkBoxFillByes.Checked, checkBoxFillByeWins.Checked, checkBoxR1Only.Checked, checkBoxUnfinished.Checked, checkBoxMatchDetails.Checked);
                                break;
                            }
                        }

                        break;
                    }
                }

                // Replace L2 with R2 because liquipedia markup is inconsistent
                finalBracketOutput = finalBracketOutput.Replace("l2m", "r2m");

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
        private void buttonFillDoubles_Click(object sender, EventArgs e)
        {
            string output = string.Empty;
            string finalBracketOutput = string.Empty;
            Liquipedia.LpOutput lpout = new Liquipedia.LpOutput(selectedEvent.Type, ref entrantList, ref setList, ref roundList);

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
                lpout.fillBracketDoubles((int)numericUpDownWinnersStart.Value, (int)numericUpDownWinnersEnd.Value, (int)numericUpDownWinnersOffset.Value, ref output, matchOffsetPerRound,
                    checkBoxFillByes.Checked, checkBoxFillByeWins.Checked, checkBoxR1Only.Checked, checkBoxUnfinished.Checked);
            }
            if (checkBoxLosers.Checked)
            {
                lpout.fillBracketDoubles(-(int)numericUpDownLosersStart.Value, -(int)numericUpDownLosersEnd.Value, (int)numericUpDownLosersOffset.Value, ref output, matchOffsetPerRound,
                    checkBoxFillByes.Checked, checkBoxFillByeWins.Checked, checkBoxR1Only.Checked, checkBoxUnfinished.Checked);
            }
            if (checkBoxGuessFinal.Checked)
            {
                foreach (KeyValuePair<int, List<Set>> gf in roundList)
                {
                    // Get grand finals
                    if (gf.Value[0].isGF == true)
                    {
                        // Fill in R3
                        lpout.fillBracketDoubles(gf.Key, gf.Key, 3 - gf.Key, ref finalBracketOutput, matchOffsetPerRound,
                            checkBoxFillByes.Checked, checkBoxFillByeWins.Checked, checkBoxR1Only.Checked, checkBoxUnfinished.Checked);

                        // Get losers finals
                        foreach (KeyValuePair<int, List<Set>> lf in roundList)
                        {
                            if (gf.Value[0].entrant2PrereqId == lf.Value[0].id)
                            {
                                // Fill in L2
                                lpout.fillBracketDoubles(lf.Key, lf.Key, 2 - Math.Abs(lf.Key), ref finalBracketOutput, matchOffsetPerRound,
                                    checkBoxFillByes.Checked, checkBoxFillByeWins.Checked, checkBoxR1Only.Checked, checkBoxUnfinished.Checked);

                                // Get losers semis
                                foreach (KeyValuePair<int, List<Set>> ls in roundList)
                                {
                                    if (lf.Value[0].entrant2PrereqId == ls.Value[0].id)
                                    {
                                        // Fill in L1
                                        lpout.fillBracketDoubles(ls.Key, ls.Key, 1 - Math.Abs(ls.Key), ref finalBracketOutput, matchOffsetPerRound,
                                            checkBoxFillByes.Checked, checkBoxFillByeWins.Checked, checkBoxR1Only.Checked, checkBoxUnfinished.Checked);
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
                                lpout.fillBracketDoubles(wf.Key, wf.Key, 1 - wf.Key, ref finalBracketOutput, matchOffsetPerRound,
                                    checkBoxFillByes.Checked, checkBoxFillByeWins.Checked, checkBoxR1Only.Checked, checkBoxUnfinished.Checked);
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
            richTextBoxLog.Text += lpout.Log;
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
                    record[set.slots[0].entrant.id].isinGroup = true;
                    record[set.slots[1].entrant.id].isinGroup = true;

                    // Find the winner of each match. Assume these are DE Brackets
                    if (set.winnerId == set.slots[0].entrant.id)
                    {
                        record[set.slots[0].entrant.id].rank = set.wPlacement;
                        record[set.slots[1].entrant.id].rank = set.lPlacement;
                    }
                    else if (set.winnerId == set.slots[1].entrant.id)
                    {
                        record[set.slots[0].entrant.id].rank = set.lPlacement;
                        record[set.slots[1].entrant.id].rank = set.wPlacement;
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
            if (selectedEvent.Type == Event.EventType.Singles)
            {
                richTextBoxLpOutput.Text = "{{prize pool start}}\r\n";
            }
            else if (selectedEvent.Type == Event.EventType.Doubles)
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
            Liquipedia.LpOutput lpout = new Liquipedia.LpOutput(selectedEvent.Type, ref entrantList, ref setList, ref roundList);

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

            bool foundMatch = false;
            Tournament.ActivityState phaseGroupState = Tournament.ActivityState.Invalid;
            string title = string.Empty;
            //foreach (Phase phase in phaseList)
            //{
            //    for (int i = 0; i < phase.id.Count; i++)
            //    {
            //        if (phase.id[i].id == global_phase_group)
            //        {
            //            GeneratePoolData(global_phase_group, parser, poolType, ref poolData, ref phaseGroupState);
            //            title = phase.id[i].DisplayIdentifier;
            //            foundMatch = true;

            //            break;
            //        }
            //    }

            //    if (foundMatch) break;
            //}

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
            // Clear data
            ClearData();
            checkBoxWinners.Checked = false;
            checkBoxLosers.Checked = false;

            string json = string.Empty;

            // Deserialize json

            // Fill entrant and set lists


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
                    if (entrantList.ContainsKey(set.slots[0].entrant.id))
                    {
                        if (set.originalRound > 0)
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
                    if (!entrantList.ContainsKey(set.slots[0].entrant.id))
                    {
                        continue;
                    }
                    if (!entrantList.ContainsKey(set.slots[1].entrant.id))
                    {
                        continue;
                    }

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
        private void ProcessPhase(Event.EventType eventType)
        {
            // Clear data
            ClearData();
            checkBoxWinners.Checked = false;
            checkBoxLosers.Checked = false;

            // Find the matching phase in the tournament structure
            string json = string.Empty;

            //foreach (Phase phase in phaseList)
            //{
            //    if (phase.phaseId == phaseNumber && phase.id.Count > 1)
            //    {
            //        // Sort the list by wave and number
            //        // Assume that if the first element has a wave and number, the rest do too
            //        if (phase.id[0].identifierType == PhaseGroup.IdentiferType.WaveNumber)
            //        {
            //            phase.id = phase.id.OrderBy(c => c.Wave).ThenBy(c => c.Number).ToList();
            //        }
            //        else if (phase.id[0].identifierType == PhaseGroup.IdentiferType.NumberOnly)
            //        {
            //            phase.id = phase.id.OrderBy(c => Convert.ToInt32(c.DisplayIdentifier)).ToList();
            //        }
            //        else
            //        { 
            //            phase.id = phase.id.OrderBy(c => c.DisplayIdentifier).ToList();
            //        }

            //        // Setup progress bar
            //        progressBar.Minimum = 0;
            //        progressBar.Maximum = phase.id.Count();
            //        progressBar.Value = 1;
            //        progressBar.Step = 1;

            //        // Retrieve pages for each group
            //        string lastWave = string.Empty;
            //        List<string> waveHeaders = new List<string>();
            //        //for (int j = 0; j < phase.id.Count; j++)
            //        {
            //            // Increment the progress bar
            //            progressBar.PerformStep();

            //            Dictionary<int, PoolRecord> poolData = new Dictionary<int, PoolRecord>();
            //            Tournament.ActivityState groupState = Tournament.ActivityState.Unknown;

            //            // Set the pool type
            //            PoolRecord.PoolType poolType;
            //            if (radioButtonBracket.Checked)
            //            {
            //                poolType = PoolRecord.PoolType.Bracket;
            //            }
            //            else if (radioButtonRR.Checked)
            //            {
            //                poolType = PoolRecord.PoolType.RoundRobin;
            //            }
            //            else
            //            {
            //                richTextBoxLog.Text += "Specify a bracket type.\r\n";
            //                return;
            //            }

            //            //if (!GeneratePoolData(phase.id[j].id, parser, poolType, ref poolData, ref groupState)) continue;

            //            //if (eventType == EventType.Singles)
            //            //{
            //            //    OutputSinglesPhase(phase, poolData, ref lastWave, j, groupState, poolType);
            //            //}
            //            //else
            //            //{
            //            //    OutputDoublesPhase(phase, poolData, ref lastWave, j, groupState, poolType);
            //            //}
            //        }
            //    }
            //}
        }

        /// <summary>
        /// Acquire, process, and output all data from the singles phase specified in the URL
        /// </summary>
        /// <param name="phase">Tournament phase to output</param>
        /// <param name="poolData">Pool records for each entrant</param>
        /// <param name="lastWave">Final wave in the pool</param>
        /// <param name="phaseElement">Pool in the phase to output</param>
        private void OutputSinglesPhase(Phase phase, Dictionary<int, PoolRecord> poolData, ref string lastWave, int phaseElement, Tournament.ActivityState state, PoolRecord.PoolType poolType)
        {
            Liquipedia.LpOutput lpout = new Liquipedia.LpOutput(selectedEvent.Type, ref entrantList, ref setList, ref roundList);

            // Output to textbox
            // Wave headers
            //if (phase.id[0].identifierType == PhaseGroup.IdentiferType.WaveNumber)
            //{
            //    if (lastWave != phase.id[phaseElement].Wave)
            //    {
            //        richTextBoxLpOutput.Text += "==Wave " + phase.id[phaseElement].Wave + "==\r\n";
            //        richTextBoxLpOutput.Text += LpStrings.BoxStart + "\r\n";

            //        lastWave = phase.id[phaseElement].Wave;
            //    }
            //}
            //else if (phaseElement == 0)    // Start a box at the first element
            //{
            //    richTextBoxLpOutput.Text += LpStrings.BoxStart + "\r\n";
            //}

            // Pool headers
            string title = string.Empty;
            //if (phase.id[0].identifierType == PhaseGroup.IdentiferType.WaveNumber)
            //{
            //    richTextBoxLpOutput.Text += "===" + LpStrings.SortStart + phase.id[phaseElement].Wave + phase.id[phaseElement].Number.ToString() + LpStrings.SortEnd + "===\r\n";
            //    title = phase.id[phaseElement].Wave + phase.id[phaseElement].Number.ToString();
            //}
            //else
            //{
            //    richTextBoxLpOutput.Text += "===" + LpStrings.SortStart + phase.id[phaseElement].Wave + phase.id[phaseElement].DisplayIdentifier.ToString() + LpStrings.SortEnd + "===\r\n";
            //    title = phase.id[phaseElement].DisplayIdentifier.ToString();
            //}

            //// Output the group
            //richTextBoxLpOutput.Text += lpout.OutputSinglesGroup(title, poolData, state, (int)numericUpDownAdvanceWinners.Value, (int)numericUpDownAdvanceLosers.Value, checkBoxMatchDetails.Checked, poolType);
            //richTextBoxLog.Text += lpout.Log;

            //// Box handling
            //if (phase.id[0].identifierType == PhaseGroup.IdentiferType.WaveNumber)     // Waves exist
            //{
            //    if (phaseElement + 1 >= phase.id.Count)
            //    {
            //        richTextBoxLpOutput.Text += LpStrings.BoxEnd + "\r\n\r\n";
            //    }
            //    else if (phase.id[phaseElement + 1].Wave != lastWave)
            //    {
            //        richTextBoxLpOutput.Text += LpStrings.BoxEnd + "\r\n\r\n";
            //    }
            //    else
            //    {
            //        richTextBoxLpOutput.Text += LpStrings.BoxBreak + "\r\n\r\n";
            //    }
            //}
            //else        // Waves don't exist
            //{
            //    if (phaseElement == phase.id.Count - 1) // End box at the group end
            //    {
            //        richTextBoxLpOutput.Text += LpStrings.BoxEnd + "\r\n\r\n";
            //    }
            //    else
            //    {
            //        richTextBoxLpOutput.Text += LpStrings.BoxBreak + "\r\n\r\n";
            //    }
            //}
        }

        /// <summary>
        /// Acquire, process, and output all data from the doubles phase specified in the URL
        /// </summary>
        /// <param name="phase">Phase to be processed</param>
        /// <param name="poolData">Records of each entrant in the pool</param>
        /// <param name="lastWave">Identifier of last wave</param>
        /// <param name="phaseElement">Element within the phase</param>
        private void OutputDoublesPhase(Phase phase, Dictionary<int, PoolRecord> poolData, ref string lastWave, int phaseElement, Tournament.ActivityState state, PoolRecord.PoolType poolType)
        {
            Liquipedia.LpOutput lpout = new Liquipedia.LpOutput(selectedEvent.Type, ref entrantList, ref setList, ref roundList);
            
            // Output to textbox
            // Wave headers
            //if (phase.id[0].identifierType == PhaseGroup.IdentiferType.WaveNumber)
            //{
            //    if (lastWave != phase.id[phaseElement].Wave)
            //    {
            //        richTextBoxLpOutput.Text += "==Wave " + phase.id[phaseElement].Wave + "==\r\n";
            //        richTextBoxLpOutput.Text += LpStrings.BoxStart + "\r\n";

            //        lastWave = phase.id[phaseElement].Wave;
            //    }
            //}
            //else if (phaseElement == 0)    // Start a box at the first element
            //{
            //    richTextBoxLpOutput.Text += LpStrings.BoxStart + "\r\n";
            //}

            //// Pool headers
            //string title = string.Empty;
            //if (phase.id[0].identifierType == PhaseGroup.IdentiferType.WaveNumber)
            //{
            //    richTextBoxLpOutput.Text += "===" + LpStrings.SortStart + phase.id[phaseElement].Wave + phase.id[phaseElement].Number.ToString() + LpStrings.SortEnd + "===" + "\r\n";
            //    title = phase.id[phaseElement].Wave + phase.id[phaseElement].Number.ToString();
            //}
            //else
            //{
            //    richTextBoxLpOutput.Text += "===" + LpStrings.SortStart + phase.id[phaseElement].Wave + phase.id[phaseElement].DisplayIdentifier.ToString() + LpStrings.SortEnd + "===" + "\r\n";
            //    title = phase.id[phaseElement].DisplayIdentifier.ToString();
            //}

            //// Output pool
            //richTextBoxLpOutput.Text += lpout.OutputDoublesGroup(title, poolData, state, (int)numericUpDownAdvanceWinners.Value, (int)numericUpDownAdvanceLosers.Value, checkBoxMatchDetails.Checked, poolType);
            //richTextBoxLog.Text += lpout.Log;

            //// Pool footers
            //if (phase.id[0].identifierType == PhaseGroup.IdentiferType.WaveNumber)     // Waves exist
            //{
            //    if (phaseElement + 1 >= phase.id.Count)
            //    {
            //        richTextBoxLpOutput.Text += LpStrings.BoxEnd + "\r\n\r\n";
            //    }
            //    else if (phase.id[phaseElement + 1].Wave != lastWave)
            //    {
            //        richTextBoxLpOutput.Text += LpStrings.BoxEnd + "\r\n\r\n";
            //    }
            //    else
            //    {
            //        richTextBoxLpOutput.Text += LpStrings.BoxBreak + "\r\n\r\n";
            //    }
            //}
            //else        // Waves don't exist
            //{
            //    if (phaseElement == phase.id.Count - 1) // End box at the group end
            //    {
            //        richTextBoxLpOutput.Text += LpStrings.BoxEnd + "\r\n\r\n";
            //    }
            //    else
            //    {
            //        richTextBoxLpOutput.Text += LpStrings.BoxBreak + "\r\n\r\n";
            //    }
            //}
        }

        /// <summary>
        /// Aggregates set data into data per player in a pool
        /// </summary>
        /// <param name="phase_group">Phase group number</param>
        /// <param name="parser">Parser for retrieving/processing json</param>
        /// <param name="PoolRecord.PoolType">Specifies bracket or round robin</param>
        /// <param name="record">List of player records for the pool</param>
        /// <returns>True if it completed succesfully, false otherwise</returns>
        private bool GeneratePoolData(int phase_group, PoolRecord.PoolType poolType, ref Dictionary<int, PoolRecord> record, ref Tournament.ActivityState phaseState)
        {
            // Clear data
            ClearData();

            // Create a record for each player
            foreach (KeyValuePair<int, Entrant> entrant in entrantList)
            {
                record.Add(entrant.Key, new PoolRecord());
            }

            // Add data to each record based on set information
            foreach (Set set in setList)
            {
                if (entrantList.ContainsKey(set.slots[0].entrant.id))
                {
                    record[set.slots[0].entrant.id].isinGroup = true;
                }
                else
                {
                    continue;
                }

                if (entrantList.ContainsKey(set.slots[1].entrant.id))
                {
                    record[set.slots[1].entrant.id].isinGroup = true;
                }
                else
                {
                    continue;
                }

                // Record match data for each player's record
                if (set.winnerId == set.slots[0].entrant.id)
                {
                    if (set.slots[1].entrant.id == Consts.PLAYER_BYE) continue;

                    record[set.slots[0].entrant.id].AddMatchWins(1);
                    record[set.slots[1].entrant.id].AddMatchLosses(1);

                    if (set.entrant2wins != -1) // Ignore W-L for DQs for now
                    {
                        record[set.slots[0].entrant.id].AddGameWins(set.entrant1wins);
                        record[set.slots[1].entrant.id].AddGameWins(set.entrant2wins);
                        record[set.slots[0].entrant.id].AddGameLosses(set.entrant2wins);
                        record[set.slots[1].entrant.id].AddGameLosses(set.entrant1wins);

                        record[set.slots[0].entrant.id].AddMatchesActuallyPlayed(1);
                        record[set.slots[1].entrant.id].AddMatchesActuallyPlayed(1);
                    }

                    // DE Brackets will set ranks for players
                    if (poolType == PoolRecord.PoolType.Bracket)
                    {
                        // Player 1 rank
                        if (record[set.slots[0].entrant.id].rank == 0 || set.wPlacement < record[set.slots[0].entrant.id].rank)
                        {
                            if (set.wPlacement != Consts.UNKNOWN)
                            {
                                record[set.slots[0].entrant.id].rank = set.wPlacement;
                            }
                        }
                        // Player 2 rank
                        if (record[set.slots[1].entrant.id].rank == 0 || set.lPlacement < record[set.slots[1].entrant.id].rank)
                        {
                            if (set.lPlacement != Consts.UNKNOWN)
                            {
                                record[set.slots[1].entrant.id].rank = set.lPlacement;
                            }
                        }

                        // Check if player 1 is done playing matches
                        if (set.wProgressingPhaseGroupId != Consts.UNKNOWN)
                        {
                            record[set.slots[0].entrant.id].MatchesComplete = true;
                        }
                        // Check if player 2 is done playing matches
                        if (set.lProgressingPhaseGroupId != Consts.UNKNOWN || set.displayRound < 0)
                        {
                            record[set.slots[1].entrant.id].MatchesComplete = true;
                        }
                    }
                }
                else if (set.winnerId == set.slots[1].entrant.id)
                {
                    if (set.slots[0].entrant.id == Consts.PLAYER_BYE) continue;

                    record[set.slots[1].entrant.id].AddMatchWins(1);
                    record[set.slots[0].entrant.id].AddMatchLosses(1);

                    if (set.entrant1wins != -1)
                    {
                        record[set.slots[0].entrant.id].AddGameWins(set.entrant1wins);
                        record[set.slots[1].entrant.id].AddGameWins(set.entrant2wins);
                        record[set.slots[0].entrant.id].AddGameLosses(set.entrant2wins);
                        record[set.slots[1].entrant.id].AddGameLosses(set.entrant1wins);

                        record[set.slots[0].entrant.id].AddMatchesActuallyPlayed(1);
                        record[set.slots[1].entrant.id].AddMatchesActuallyPlayed(1);
                    }

                    // DE Brackets will set ranks for players
                    if (poolType == PoolRecord.PoolType.Bracket)
                    {
                        if (record[set.slots[0].entrant.id].rank == 0 || set.lPlacement < record[set.slots[0].entrant.id].rank)
                        {
                            if (set.lPlacement != Consts.UNKNOWN)
                            {
                                record[set.slots[0].entrant.id].rank = set.lPlacement;
                            }
                        }

                        if (record[set.slots[1].entrant.id].rank == 0 || set.wPlacement < record[set.slots[1].entrant.id].rank)
                        {
                            if (set.wPlacement != Consts.UNKNOWN)
                            {
                                record[set.slots[1].entrant.id].rank = set.wPlacement;
                            }
                        }

                        // Check if player 2 is done playing matches
                        if (set.wProgressingPhaseGroupId != Consts.UNKNOWN)
                        {
                            record[set.slots[1].entrant.id].MatchesComplete = true;
                        }
                        // Check if player 1 is done playing matches
                        if (set.lProgressingPhaseGroupId != Consts.UNKNOWN || set.displayRound < 0)
                        {
                            record[set.slots[0].entrant.id].MatchesComplete = true;
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
            if (poolType == PoolRecord.PoolType.Bracket)
            {
                record = record.OrderBy(x => x.Value.rank).ThenByDescending(x => x.Value.MatchWinrate).ThenBy(x => x.Value.MatchesLoss).ThenByDescending(x => x.Value.MatchesWin).ThenByDescending(x => x.Value.GameWinrate).ToDictionary(x => x.Key, x => x.Value);
            }
            else if (poolType == PoolRecord.PoolType.RoundRobin)
            {
                foreach (int entrant in record.Keys)
                {
                    //record[entrant].rank = entrantList[entrant].Placement;
                }

                record = record.OrderBy(x => x.Value.rank).ThenByDescending(x => x.Value.MatchWinrate).ThenBy(x => x.Value.MatchesLoss).ThenByDescending(x => x.Value.MatchesWin).ThenByDescending(x => x.Value.GameWinrate).ToDictionary(x => x.Key, x => x.Value);
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
                    richTextBoxEntrants.Text += entrant.Value.participants[0].playerId.ToString().PadRight(7);
                }
                else if (entrant.Value.participants.Count == 2)
                {
                    richTextBoxEntrants.Text += entrant.Value.participants[0].playerId.ToString().PadRight(7) + "/" + entrant.Value.participants[1].playerId.ToString().PadRight(7);
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
                        richTextBoxEntrants.Text += entrant.Value.participants[i].contactInfo.country + "\r\n";
                    }
                    else
                    {
                        richTextBoxEntrants.Text += entrant.Value.participants[i].contactInfo.country + " / ";
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
            textbox.AppendText(set.displayRound.ToString().PadRight(4));

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
        /// <param name="record">Records of the entrants in the bracket, sorted by rank</param>
        /// <param name="startEntrant">The first entrant to output in this row</param>
        /// <param name="rows">Number of rows to output</param>
        private void OutputPrizePoolTable(Dictionary<int, PoolRecord> record, int startEntrant, int rows)
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
                richTextBoxLpOutput.Text += record.ElementAt(startEntrant).Value.rank;
            }
            else
            {
                richTextBoxLpOutput.Text += record.ElementAt(startEntrant).Value.rank + "-" + (record.ElementAt(startEntrant).Value.rank + rows - 1);
            }

            // Ouptut the parameter for prize money
            richTextBoxLpOutput.Text += "|usdprize=\r\n";


            if (selectedEvent.Type == Event.EventType.Singles)
            {
                // Output the specified number of entrants
                for (int i = 0; i < rows; i++)
                {
                    // Assume there is only 1 player output their info
                    richTextBoxLpOutput.Text += "|" + entrantList[record.ElementAt(startEntrant + i).Key].participants[0].gamerTag +
                                                "|flag" + (i + 1) + "=" + entrantList[record.ElementAt(startEntrant + i).Key].participants[0].contactInfo.country +
                                                "|heads" + (i + 1) + "= |team" + (i + 1) + "=\r\n";
                }
            }
            else if (selectedEvent.Type == Event.EventType.Doubles)
            {
                // Output the specified number of entrants
                for (int i = 0; i < rows; i++)
                {
                    // Assume there are 2 players, and output their info
                    richTextBoxLpOutput.Text += "|" + entrantList[record.ElementAt(startEntrant + i).Key].participants[0].gamerTag +
                                                "|flag" + (i + 1) + "p1=" + entrantList[record.ElementAt(startEntrant + i).Key].participants[0].contactInfo.country +
                                                "|heads" + (i + 1) + "p1=" +
                                                "|" + entrantList[record.ElementAt(startEntrant + i).Key].participants[1].gamerTag +
                                                "|flag" + (i + 1) + "p2=" + entrantList[record.ElementAt(startEntrant + i).Key].participants[1].contactInfo.country +
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
            buttonFillDoubles.Enabled = false;
            buttonGetPhase.Enabled = false;
            buttonGetBracket.Enabled = false;
            buttonPrizePool.Enabled = false;
            buttonWinnerShift.Enabled = false;
            buttonLoserShift.Enabled = false;
            buttonGroupTable.Enabled = false;

            checkBoxFillByes.Enabled = false;
            checkBoxR1Only.Enabled = false;
            checkBoxGuessFinal.Enabled = false;
            checkBoxLockLosers.Enabled = false;
            checkBoxLockWinners.Enabled = false;
            checkBoxLosers.Enabled = false;
            checkBoxWinners.Enabled = false;
            checkBoxFillByeWins.Enabled = false;
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
            textBoxTournamentUrl.Enabled = true;

            buttonFill.Enabled = true;
            buttonFillDoubles.Enabled = true;
            buttonGetPhase.Enabled = true;
            buttonGetBracket.Enabled = true;
            buttonPrizePool.Enabled = true;
            buttonWinnerShift.Enabled = true;
            buttonLoserShift.Enabled = true;
            buttonGroupTable.Enabled = true;

            checkBoxFillByes.Enabled = true;
            checkBoxR1Only.Enabled = true;
            checkBoxGuessFinal.Enabled = true;
            checkBoxLockLosers.Enabled = true;
            checkBoxLockWinners.Enabled = true;
            checkBoxLosers.Enabled = true;
            checkBoxWinners.Enabled = true;
            checkBoxFillByeWins.Enabled = true;
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
            richTextBoxLog.Enabled = true;
            richTextBoxWinners.Enabled = true;
            richTextBoxLosers.Enabled = true;
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

                    // Set checked node properties
                    selectedEvent = FindParentEvent(e.Node);
                    selectedObjectId = tag.id;
                    selectedObjectType = tag.nodetype;
                }
                else if (checkedNodes == 1)
                {
                    TreeNodeData tag = (TreeNodeData)e.Node.Tag;

                    // Set checked node properties
                    selectedEvent = FindParentEvent(e.Node);
                    selectedObjectId = tag.id;
                    selectedObjectType = tag.nodetype;
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
            switch (selectedObjectType)
            {
                case TreeNodeData.NodeType.Phase:
                    break;
                case TreeNodeData.NodeType.PhaseGroup:
                    break;
                case TreeNodeData.NodeType.Wave:
                    break;
                default:
                    richTextBoxLog.Text += "Can't do anything with that object\r\n";
                    break;
            }
        }
    }
}
