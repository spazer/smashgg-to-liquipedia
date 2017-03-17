namespace smashgg_to_liquipedia
{
    partial class FormHeadings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.richTextBoxWinners = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.richTextBoxLosers = new System.Windows.Forms.RichTextBox();
            this.richTextBoxFinals = new System.Windows.Forms.RichTextBox();
            this.richTextBoxFinalsBracket = new System.Windows.Forms.RichTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // richTextBoxWinners
            // 
            this.richTextBoxWinners.Location = new System.Drawing.Point(12, 25);
            this.richTextBoxWinners.Name = "richTextBoxWinners";
            this.richTextBoxWinners.Size = new System.Drawing.Size(295, 105);
            this.richTextBoxWinners.TabIndex = 0;
            this.richTextBoxWinners.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(124, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Winners Bracket Header";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(339, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(116, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Losers Bracket Header";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 149);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(112, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Finals Bracket Header";
            // 
            // richTextBoxLosers
            // 
            this.richTextBoxLosers.Location = new System.Drawing.Point(342, 25);
            this.richTextBoxLosers.Name = "richTextBoxLosers";
            this.richTextBoxLosers.Size = new System.Drawing.Size(295, 105);
            this.richTextBoxLosers.TabIndex = 3;
            this.richTextBoxLosers.Text = "";
            // 
            // richTextBoxFinals
            // 
            this.richTextBoxFinals.Location = new System.Drawing.Point(15, 165);
            this.richTextBoxFinals.Name = "richTextBoxFinals";
            this.richTextBoxFinals.Size = new System.Drawing.Size(292, 186);
            this.richTextBoxFinals.TabIndex = 4;
            this.richTextBoxFinals.Text = "";
            // 
            // richTextBoxFinalsBracket
            // 
            this.richTextBoxFinalsBracket.Location = new System.Drawing.Point(342, 165);
            this.richTextBoxFinalsBracket.Name = "richTextBoxFinalsBracket";
            this.richTextBoxFinalsBracket.Size = new System.Drawing.Size(292, 186);
            this.richTextBoxFinalsBracket.TabIndex = 6;
            this.richTextBoxFinalsBracket.Text = "";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(339, 149);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(74, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Finals Bracket";
            // 
            // FormHeadings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(658, 363);
            this.Controls.Add(this.richTextBoxFinalsBracket);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.richTextBoxFinals);
            this.Controls.Add(this.richTextBoxLosers);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.richTextBoxWinners);
            this.Name = "FormHeadings";
            this.Text = "FormHeadings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBoxWinners;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RichTextBox richTextBoxLosers;
        private System.Windows.Forms.RichTextBox richTextBoxFinals;
        private System.Windows.Forms.RichTextBox richTextBoxFinalsBracket;
        private System.Windows.Forms.Label label4;
    }
}