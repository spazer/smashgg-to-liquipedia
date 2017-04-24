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
    public partial class FormMatchShift : Form
    {
        static int ELEMENT_WIDTH = 60;
        static int ELEMENT_SPACING = 5;

        Dictionary<int, int> roundOffsets;
        FormMain.BracketSide side;
        List<NumericUpDown> updownList = new List<NumericUpDown>();

        public FormMatchShift(int start, int end, FormMain.BracketSide side, ref Dictionary<int,int> storedOffsets)
        {
            InitializeComponent();

            // Save values/references that we need locally
            roundOffsets = storedOffsets;
            this.side = side;

            // Set prefix text depending on which side this bracket is on
            string sideText;
            if (side == FormMain.BracketSide.Winners)
            {
                sideText = "R";
            }
            else
            {
                sideText = "L";
            }

            // Set the window size
            this.Size = new Size(10 + (ELEMENT_WIDTH + ELEMENT_SPACING) * (end - start + 2), 100);

            // Create an updown control for each round with a corresponding label
            for (int i = start; i <= end; i++)
            {
                Label label = new Label();
                label.Location = new Point(5 + (i - start) * (ELEMENT_WIDTH + ELEMENT_SPACING), 5);
                label.Name = "label" + i.ToString();
                label.Text = sideText + i.ToString();
                label.Size = new Size(40, 13);

                this.Controls.Add(label);

                NumericUpDown updown = new NumericUpDown();
                updown.Location = new Point(5 + (i - start) * (ELEMENT_WIDTH + ELEMENT_SPACING), 20);
                updown.Name = "numericUpDown" + i.ToString();
                updown.Size = new Size(40, 20);
                updown.Minimum = -1000;

                if (side == FormMain.BracketSide.Winners)
                {
                    updown.Value = (decimal)roundOffsets[i];
                }
                else
                {
                    updown.Value = (decimal)roundOffsets[-i];
                }

                updownList.Add(updown);
                this.Controls.Add(updown);
            }
        }

        private void FormMatchShift_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Transfer updown values into the offset dictionary
            foreach(NumericUpDown updown in updownList)
            {
                // Get the last letter of the name since this is the round number
                int num = int.Parse(updown.Name.Substring(updown.Name.Length - 1));

                if (side == FormMain.BracketSide.Winners)
                {
                    roundOffsets[num] = (int)updown.Value;
                }
                else
                {
                    roundOffsets[-num] = (int)updown.Value;
                }
            }
        }
    }
}
