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
    public partial class Authentication : Form
    {
        public string token;

        public Authentication(ref string token)
        {
            InitializeComponent();

            this.token = token;
            textBox1.Text = token;
        }

        private void Authentication_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.token = textBox1.Text;
        }
    }
}
