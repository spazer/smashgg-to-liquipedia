namespace smashgg_api
{
    partial class Form1
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
            this.richTextBoxEntrants = new System.Windows.Forms.RichTextBox();
            this.textBoxURLSingles = new System.Windows.Forms.TextBox();
            this.buttonBracket = new System.Windows.Forms.Button();
            this.buttonPhase = new System.Windows.Forms.Button();
            this.radioButtonBracket = new System.Windows.Forms.RadioButton();
            this.radioButtonRoundRobin = new System.Windows.Forms.RadioButton();
            this.label5 = new System.Windows.Forms.Label();
            this.numericUpDownAdvance = new System.Windows.Forms.NumericUpDown();
            this.richTextBoxWinners = new System.Windows.Forms.RichTextBox();
            this.richTextBoxLosers = new System.Windows.Forms.RichTextBox();
            this.richTextBoxLiquipedia = new System.Windows.Forms.RichTextBox();
            this.buttonFill = new System.Windows.Forms.Button();
            this.buttonFillDoubles = new System.Windows.Forms.Button();
            this.richTextBoxLog = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBoxWinnersSingles = new System.Windows.Forms.CheckBox();
            this.checkBoxLosersSingles = new System.Windows.Forms.CheckBox();
            this.checkBoxFillUnfinishedSingles = new System.Windows.Forms.CheckBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageSingles = new System.Windows.Forms.TabPage();
            this.numericUpDownStartSingles = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.numericUpDownEndSingles = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.numericUpDownOffsetSingles = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.tabPageDoubles = new System.Windows.Forms.TabPage();
            this.checkBoxFillUnfinishedDoubles = new System.Windows.Forms.CheckBox();
            this.checkBoxLosersDoubles = new System.Windows.Forms.CheckBox();
            this.checkBoxWinnersDoubles = new System.Windows.Forms.CheckBox();
            this.numericUpDownStartDoubles = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.numericUpDownEndDoubles = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.numericUpDownOffsetDoubles = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.buttonDoubles = new System.Windows.Forms.Button();
            this.textBoxURLDoubles = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownAdvance)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPageSingles.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownStartSingles)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownEndSingles)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownOffsetSingles)).BeginInit();
            this.tabPageDoubles.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownStartDoubles)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownEndDoubles)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownOffsetDoubles)).BeginInit();
            this.SuspendLayout();
            // 
            // richTextBoxEntrants
            // 
            this.richTextBoxEntrants.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBoxEntrants.Location = new System.Drawing.Point(12, 15);
            this.richTextBoxEntrants.Name = "richTextBoxEntrants";
            this.richTextBoxEntrants.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.richTextBoxEntrants.Size = new System.Drawing.Size(691, 170);
            this.richTextBoxEntrants.TabIndex = 0;
            this.richTextBoxEntrants.Text = "";
            // 
            // textBoxURLSingles
            // 
            this.textBoxURLSingles.Location = new System.Drawing.Point(3, 3);
            this.textBoxURLSingles.Name = "textBoxURLSingles";
            this.textBoxURLSingles.Size = new System.Drawing.Size(187, 20);
            this.textBoxURLSingles.TabIndex = 1;
            // 
            // buttonBracket
            // 
            this.buttonBracket.Location = new System.Drawing.Point(3, 29);
            this.buttonBracket.Name = "buttonBracket";
            this.buttonBracket.Size = new System.Drawing.Size(75, 23);
            this.buttonBracket.TabIndex = 2;
            this.buttonBracket.Text = "Get Bracket";
            this.buttonBracket.UseVisualStyleBackColor = true;
            this.buttonBracket.Click += new System.EventHandler(this.buttonBracket_Click);
            // 
            // buttonPhase
            // 
            this.buttonPhase.Location = new System.Drawing.Point(115, 29);
            this.buttonPhase.Name = "buttonPhase";
            this.buttonPhase.Size = new System.Drawing.Size(75, 23);
            this.buttonPhase.TabIndex = 2;
            this.buttonPhase.Text = "Get Phase";
            this.buttonPhase.UseVisualStyleBackColor = true;
            this.buttonPhase.Click += new System.EventHandler(this.buttonPhase_Click);
            // 
            // radioButtonBracket
            // 
            this.radioButtonBracket.AutoSize = true;
            this.radioButtonBracket.Checked = true;
            this.radioButtonBracket.Location = new System.Drawing.Point(115, 59);
            this.radioButtonBracket.Name = "radioButtonBracket";
            this.radioButtonBracket.Size = new System.Drawing.Size(91, 17);
            this.radioButtonBracket.TabIndex = 8;
            this.radioButtonBracket.TabStop = true;
            this.radioButtonBracket.Text = "Bracket Pools";
            this.radioButtonBracket.UseVisualStyleBackColor = true;
            // 
            // radioButtonRoundRobin
            // 
            this.radioButtonRoundRobin.AutoSize = true;
            this.radioButtonRoundRobin.Location = new System.Drawing.Point(115, 82);
            this.radioButtonRoundRobin.Name = "radioButtonRoundRobin";
            this.radioButtonRoundRobin.Size = new System.Drawing.Size(88, 17);
            this.radioButtonRoundRobin.TabIndex = 9;
            this.radioButtonRoundRobin.Text = "Round Robin";
            this.radioButtonRoundRobin.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(155, 107);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(50, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Advance";
            // 
            // numericUpDownAdvance
            // 
            this.numericUpDownAdvance.Location = new System.Drawing.Point(115, 105);
            this.numericUpDownAdvance.Name = "numericUpDownAdvance";
            this.numericUpDownAdvance.Size = new System.Drawing.Size(32, 20);
            this.numericUpDownAdvance.TabIndex = 10;
            this.numericUpDownAdvance.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // richTextBoxWinners
            // 
            this.richTextBoxWinners.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBoxWinners.Location = new System.Drawing.Point(12, 191);
            this.richTextBoxWinners.Name = "richTextBoxWinners";
            this.richTextBoxWinners.Size = new System.Drawing.Size(342, 170);
            this.richTextBoxWinners.TabIndex = 0;
            this.richTextBoxWinners.Text = "";
            // 
            // richTextBoxLosers
            // 
            this.richTextBoxLosers.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBoxLosers.Location = new System.Drawing.Point(361, 191);
            this.richTextBoxLosers.Name = "richTextBoxLosers";
            this.richTextBoxLosers.Size = new System.Drawing.Size(342, 170);
            this.richTextBoxLosers.TabIndex = 0;
            this.richTextBoxLosers.Text = "";
            // 
            // richTextBoxLiquipedia
            // 
            this.richTextBoxLiquipedia.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBoxLiquipedia.Location = new System.Drawing.Point(12, 367);
            this.richTextBoxLiquipedia.Name = "richTextBoxLiquipedia";
            this.richTextBoxLiquipedia.Size = new System.Drawing.Size(691, 170);
            this.richTextBoxLiquipedia.TabIndex = 0;
            this.richTextBoxLiquipedia.Text = "";
            // 
            // buttonFill
            // 
            this.buttonFill.Location = new System.Drawing.Point(3, 276);
            this.buttonFill.Name = "buttonFill";
            this.buttonFill.Size = new System.Drawing.Size(75, 23);
            this.buttonFill.TabIndex = 2;
            this.buttonFill.Text = "Fill Bracket";
            this.buttonFill.UseVisualStyleBackColor = true;
            this.buttonFill.Click += new System.EventHandler(this.buttonFillSingles_Click);
            // 
            // buttonFillDoubles
            // 
            this.buttonFillDoubles.Location = new System.Drawing.Point(3, 276);
            this.buttonFillDoubles.Name = "buttonFillDoubles";
            this.buttonFillDoubles.Size = new System.Drawing.Size(75, 23);
            this.buttonFillDoubles.TabIndex = 2;
            this.buttonFillDoubles.Text = "Fill Doubles";
            this.buttonFillDoubles.UseVisualStyleBackColor = true;
            this.buttonFillDoubles.Click += new System.EventHandler(this.buttonFillDoubles_Click);
            // 
            // richTextBoxLog
            // 
            this.richTextBoxLog.Location = new System.Drawing.Point(709, 368);
            this.richTextBoxLog.Name = "richTextBoxLog";
            this.richTextBoxLog.Size = new System.Drawing.Size(225, 169);
            this.richTextBoxLog.TabIndex = 4;
            this.richTextBoxLog.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(706, 348);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(28, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Log:";
            // 
            // checkBoxWinnersSingles
            // 
            this.checkBoxWinnersSingles.AutoSize = true;
            this.checkBoxWinnersSingles.Location = new System.Drawing.Point(3, 207);
            this.checkBoxWinnersSingles.Name = "checkBoxWinnersSingles";
            this.checkBoxWinnersSingles.Size = new System.Drawing.Size(65, 17);
            this.checkBoxWinnersSingles.TabIndex = 7;
            this.checkBoxWinnersSingles.Text = "Winners";
            this.checkBoxWinnersSingles.UseVisualStyleBackColor = true;
            // 
            // checkBoxLosersSingles
            // 
            this.checkBoxLosersSingles.AutoSize = true;
            this.checkBoxLosersSingles.Location = new System.Drawing.Point(3, 230);
            this.checkBoxLosersSingles.Name = "checkBoxLosersSingles";
            this.checkBoxLosersSingles.Size = new System.Drawing.Size(57, 17);
            this.checkBoxLosersSingles.TabIndex = 7;
            this.checkBoxLosersSingles.Text = "Losers";
            this.checkBoxLosersSingles.UseVisualStyleBackColor = true;
            // 
            // checkBoxFillUnfinishedSingles
            // 
            this.checkBoxFillUnfinishedSingles.AutoSize = true;
            this.checkBoxFillUnfinishedSingles.Location = new System.Drawing.Point(3, 253);
            this.checkBoxFillUnfinishedSingles.Name = "checkBoxFillUnfinishedSingles";
            this.checkBoxFillUnfinishedSingles.Size = new System.Drawing.Size(111, 17);
            this.checkBoxFillUnfinishedSingles.TabIndex = 7;
            this.checkBoxFillUnfinishedSingles.Text = "Fill unfinished sets";
            this.checkBoxFillUnfinishedSingles.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageSingles);
            this.tabControl1.Controls.Add(this.tabPageDoubles);
            this.tabControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.Location = new System.Drawing.Point(709, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(229, 349);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPageSingles
            // 
            this.tabPageSingles.Controls.Add(this.textBoxURLSingles);
            this.tabPageSingles.Controls.Add(this.numericUpDownAdvance);
            this.tabPageSingles.Controls.Add(this.buttonBracket);
            this.tabPageSingles.Controls.Add(this.radioButtonRoundRobin);
            this.tabPageSingles.Controls.Add(this.radioButtonBracket);
            this.tabPageSingles.Controls.Add(this.buttonFill);
            this.tabPageSingles.Controls.Add(this.checkBoxFillUnfinishedSingles);
            this.tabPageSingles.Controls.Add(this.checkBoxLosersSingles);
            this.tabPageSingles.Controls.Add(this.buttonPhase);
            this.tabPageSingles.Controls.Add(this.checkBoxWinnersSingles);
            this.tabPageSingles.Controls.Add(this.numericUpDownStartSingles);
            this.tabPageSingles.Controls.Add(this.label4);
            this.tabPageSingles.Controls.Add(this.numericUpDownEndSingles);
            this.tabPageSingles.Controls.Add(this.label3);
            this.tabPageSingles.Controls.Add(this.numericUpDownOffsetSingles);
            this.tabPageSingles.Controls.Add(this.label2);
            this.tabPageSingles.Controls.Add(this.label5);
            this.tabPageSingles.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPageSingles.Location = new System.Drawing.Point(4, 34);
            this.tabPageSingles.Name = "tabPageSingles";
            this.tabPageSingles.Size = new System.Drawing.Size(221, 311);
            this.tabPageSingles.TabIndex = 0;
            this.tabPageSingles.Text = "Singles";
            this.tabPageSingles.UseVisualStyleBackColor = true;
            // 
            // numericUpDownStartSingles
            // 
            this.numericUpDownStartSingles.Location = new System.Drawing.Point(3, 181);
            this.numericUpDownStartSingles.Name = "numericUpDownStartSingles";
            this.numericUpDownStartSingles.Size = new System.Drawing.Size(38, 20);
            this.numericUpDownStartSingles.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(88, 164);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Offset";
            // 
            // numericUpDownEndSingles
            // 
            this.numericUpDownEndSingles.Location = new System.Drawing.Point(47, 180);
            this.numericUpDownEndSingles.Name = "numericUpDownEndSingles";
            this.numericUpDownEndSingles.Size = new System.Drawing.Size(38, 20);
            this.numericUpDownEndSingles.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(44, 164);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(26, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "End";
            // 
            // numericUpDownOffsetSingles
            // 
            this.numericUpDownOffsetSingles.Location = new System.Drawing.Point(91, 180);
            this.numericUpDownOffsetSingles.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numericUpDownOffsetSingles.Minimum = new decimal(new int[] {
            20,
            0,
            0,
            -2147483648});
            this.numericUpDownOffsetSingles.Name = "numericUpDownOffsetSingles";
            this.numericUpDownOffsetSingles.Size = new System.Drawing.Size(38, 20);
            this.numericUpDownOffsetSingles.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(0, 164);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Start";
            // 
            // tabPageDoubles
            // 
            this.tabPageDoubles.Controls.Add(this.checkBoxFillUnfinishedDoubles);
            this.tabPageDoubles.Controls.Add(this.checkBoxLosersDoubles);
            this.tabPageDoubles.Controls.Add(this.checkBoxWinnersDoubles);
            this.tabPageDoubles.Controls.Add(this.numericUpDownStartDoubles);
            this.tabPageDoubles.Controls.Add(this.label6);
            this.tabPageDoubles.Controls.Add(this.numericUpDownEndDoubles);
            this.tabPageDoubles.Controls.Add(this.label7);
            this.tabPageDoubles.Controls.Add(this.numericUpDownOffsetDoubles);
            this.tabPageDoubles.Controls.Add(this.label8);
            this.tabPageDoubles.Controls.Add(this.buttonDoubles);
            this.tabPageDoubles.Controls.Add(this.textBoxURLDoubles);
            this.tabPageDoubles.Controls.Add(this.buttonFillDoubles);
            this.tabPageDoubles.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPageDoubles.Location = new System.Drawing.Point(4, 34);
            this.tabPageDoubles.Name = "tabPageDoubles";
            this.tabPageDoubles.Size = new System.Drawing.Size(221, 311);
            this.tabPageDoubles.TabIndex = 0;
            this.tabPageDoubles.Text = "Doubles";
            this.tabPageDoubles.UseVisualStyleBackColor = true;
            // 
            // checkBoxFillUnfinishedDoubles
            // 
            this.checkBoxFillUnfinishedDoubles.AutoSize = true;
            this.checkBoxFillUnfinishedDoubles.Location = new System.Drawing.Point(3, 253);
            this.checkBoxFillUnfinishedDoubles.Name = "checkBoxFillUnfinishedDoubles";
            this.checkBoxFillUnfinishedDoubles.Size = new System.Drawing.Size(111, 17);
            this.checkBoxFillUnfinishedDoubles.TabIndex = 14;
            this.checkBoxFillUnfinishedDoubles.Text = "Fill unfinished sets";
            this.checkBoxFillUnfinishedDoubles.UseVisualStyleBackColor = true;
            // 
            // checkBoxLosersDoubles
            // 
            this.checkBoxLosersDoubles.AutoSize = true;
            this.checkBoxLosersDoubles.Location = new System.Drawing.Point(3, 230);
            this.checkBoxLosersDoubles.Name = "checkBoxLosersDoubles";
            this.checkBoxLosersDoubles.Size = new System.Drawing.Size(57, 17);
            this.checkBoxLosersDoubles.TabIndex = 15;
            this.checkBoxLosersDoubles.Text = "Losers";
            this.checkBoxLosersDoubles.UseVisualStyleBackColor = true;
            // 
            // checkBoxWinnersDoubles
            // 
            this.checkBoxWinnersDoubles.AutoSize = true;
            this.checkBoxWinnersDoubles.Location = new System.Drawing.Point(3, 207);
            this.checkBoxWinnersDoubles.Name = "checkBoxWinnersDoubles";
            this.checkBoxWinnersDoubles.Size = new System.Drawing.Size(65, 17);
            this.checkBoxWinnersDoubles.TabIndex = 16;
            this.checkBoxWinnersDoubles.Text = "Winners";
            this.checkBoxWinnersDoubles.UseVisualStyleBackColor = true;
            // 
            // numericUpDownStartDoubles
            // 
            this.numericUpDownStartDoubles.Location = new System.Drawing.Point(3, 181);
            this.numericUpDownStartDoubles.Name = "numericUpDownStartDoubles";
            this.numericUpDownStartDoubles.Size = new System.Drawing.Size(38, 20);
            this.numericUpDownStartDoubles.TabIndex = 8;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(88, 164);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Offset";
            // 
            // numericUpDownEndDoubles
            // 
            this.numericUpDownEndDoubles.Location = new System.Drawing.Point(47, 180);
            this.numericUpDownEndDoubles.Name = "numericUpDownEndDoubles";
            this.numericUpDownEndDoubles.Size = new System.Drawing.Size(38, 20);
            this.numericUpDownEndDoubles.TabIndex = 9;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(44, 164);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(26, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "End";
            // 
            // numericUpDownOffsetDoubles
            // 
            this.numericUpDownOffsetDoubles.Location = new System.Drawing.Point(91, 180);
            this.numericUpDownOffsetDoubles.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numericUpDownOffsetDoubles.Minimum = new decimal(new int[] {
            20,
            0,
            0,
            -2147483648});
            this.numericUpDownOffsetDoubles.Name = "numericUpDownOffsetDoubles";
            this.numericUpDownOffsetDoubles.Size = new System.Drawing.Size(38, 20);
            this.numericUpDownOffsetDoubles.TabIndex = 10;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(0, 164);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(29, 13);
            this.label8.TabIndex = 13;
            this.label8.Text = "Start";
            // 
            // buttonDoubles
            // 
            this.buttonDoubles.Location = new System.Drawing.Point(3, 29);
            this.buttonDoubles.Name = "buttonDoubles";
            this.buttonDoubles.Size = new System.Drawing.Size(75, 23);
            this.buttonDoubles.TabIndex = 3;
            this.buttonDoubles.Text = "Get Doubles";
            this.buttonDoubles.UseVisualStyleBackColor = true;
            this.buttonDoubles.Click += new System.EventHandler(this.buttonDoubles_Click);
            // 
            // textBoxURLDoubles
            // 
            this.textBoxURLDoubles.Location = new System.Drawing.Point(3, 3);
            this.textBoxURLDoubles.Name = "textBoxURLDoubles";
            this.textBoxURLDoubles.Size = new System.Drawing.Size(187, 20);
            this.textBoxURLDoubles.TabIndex = 2;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(948, 551);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.richTextBoxLog);
            this.Controls.Add(this.richTextBoxLiquipedia);
            this.Controls.Add(this.richTextBoxLosers);
            this.Controls.Add(this.richTextBoxWinners);
            this.Controls.Add(this.richTextBoxEntrants);
            this.Name = "Form1";
            this.Text = "Smash.gg to Liquipedia";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownAdvance)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPageSingles.ResumeLayout(false);
            this.tabPageSingles.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownStartSingles)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownEndSingles)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownOffsetSingles)).EndInit();
            this.tabPageDoubles.ResumeLayout(false);
            this.tabPageDoubles.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownStartDoubles)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownEndDoubles)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownOffsetDoubles)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBoxEntrants;
        private System.Windows.Forms.TextBox textBoxURLSingles;
        private System.Windows.Forms.Button buttonBracket;
        private System.Windows.Forms.Button buttonPhase;
        private System.Windows.Forms.RadioButton radioButtonBracket;
        private System.Windows.Forms.RadioButton radioButtonRoundRobin;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown numericUpDownAdvance;
        private System.Windows.Forms.RichTextBox richTextBoxWinners;
        private System.Windows.Forms.RichTextBox richTextBoxLosers;
        private System.Windows.Forms.RichTextBox richTextBoxLiquipedia;
        private System.Windows.Forms.Button buttonFill;
        private System.Windows.Forms.Button buttonFillDoubles;
        private System.Windows.Forms.RichTextBox richTextBoxLog;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBoxWinnersSingles;
        private System.Windows.Forms.CheckBox checkBoxLosersSingles;
        private System.Windows.Forms.CheckBox checkBoxFillUnfinishedSingles;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageSingles;
        private System.Windows.Forms.NumericUpDown numericUpDownStartSingles;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numericUpDownEndSingles;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numericUpDownOffsetSingles;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TabPage tabPageDoubles;
        private System.Windows.Forms.CheckBox checkBoxFillUnfinishedDoubles;
        private System.Windows.Forms.CheckBox checkBoxLosersDoubles;
        private System.Windows.Forms.CheckBox checkBoxWinnersDoubles;
        private System.Windows.Forms.NumericUpDown numericUpDownStartDoubles;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown numericUpDownEndDoubles;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown numericUpDownOffsetDoubles;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button buttonDoubles;
        private System.Windows.Forms.TextBox textBoxURLDoubles;
    }
}

