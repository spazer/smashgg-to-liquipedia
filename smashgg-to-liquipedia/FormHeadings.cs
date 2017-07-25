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
    public partial class FormHeadings : Form
    {
        public string wHeader;
        public string lHeader;
        public string finalHeader;
        public string finalBracket;

        public FormHeadings(ref Liquipedia.LpBracket winners, ref Liquipedia.LpBracket losers, ref Liquipedia.LpBracket finals)
        {
            InitializeComponent();

            richTextBoxWinners.DataBindings.Add("Text", winners, "Header");
            richTextBoxLosers.DataBindings.Add("Text", losers, "Header");
            richTextBoxFinals.DataBindings.Add("Text", finals, "Header");
            richTextBoxFinalsBracket.DataBindings.Add("Text", finals, "Bracket");
        }
    }
}
