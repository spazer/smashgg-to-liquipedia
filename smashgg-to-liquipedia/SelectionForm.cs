using smashgg_to_liquipedia.Smashgg;
using smashgg_to_liquipedia.Smashgg.Schema;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace smashgg_to_liquipedia
{
    public partial class SelectionForm : Form
    {
        TreeNode root;

        public SelectionForm(Tournament tournament, ref Event selectedEvent, ref object returnedData)
        {
            InitializeComponent();

            treeView1.BeginUpdate();

            root = treeView1.Nodes.Add(tournament.slug);

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

            root.Expand();
            treeView1.EndUpdate();
        }

        // Updates all child tree nodes recursively.
        private void CheckAllChildNodes(TreeNode treeNode, bool nodeChecked)
        {
            foreach (TreeNode node in treeNode.Nodes)
            {
                node.Checked = nodeChecked;
                if (node.Nodes.Count > 0)
                {
                    // If the current node has child nodes, call the CheckAllChildsNodes method recursively.
                    this.CheckAllChildNodes(node, nodeChecked);
                }
            }
        }

        // NOTE   This code can be added to the BeforeCheck event handler instead of the AfterCheck event.
        // After a tree node's Checked property is changed, all its child nodes are updated to the same value.
        private void node_AfterCheck(object sender, TreeViewEventArgs e)
        {
            // The code only executes if the user caused the checked state to change.
            if (e.Action != TreeViewAction.Unknown)
            {
                if (e.Node.Nodes.Count > 0)
                {
                    /* Calls the CheckAllChildNodes method, passing in the current 
                    Checked value of the TreeNode whose checked state changed. */
                    this.CheckAllChildNodes(e.Node, e.Node.Checked);
                }
            }
        }

        private void buttonPrizePool_Click(object sender, EventArgs e)
        {

        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {

        }

        private void buttonGetSets_Click(object sender, EventArgs e)
        {

        }
    }
}
