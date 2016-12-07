namespace smashgg_to_liquipedia
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
            this.buttonGetBracket = new System.Windows.Forms.Button();
            this.buttonGetPhase = new System.Windows.Forms.Button();
            this.radioButtonBracket = new System.Windows.Forms.RadioButton();
            this.radioButtonRR = new System.Windows.Forms.RadioButton();
            this.label5 = new System.Windows.Forms.Label();
            this.numericUpDownAdvanceWinners = new System.Windows.Forms.NumericUpDown();
            this.richTextBoxWinners = new System.Windows.Forms.RichTextBox();
            this.richTextBoxLosers = new System.Windows.Forms.RichTextBox();
            this.richTextBoxLpOutput = new System.Windows.Forms.RichTextBox();
            this.buttonFill = new System.Windows.Forms.Button();
            this.buttonFillDoubles = new System.Windows.Forms.Button();
            this.richTextBoxLog = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBoxWinners = new System.Windows.Forms.CheckBox();
            this.checkBoxLosers = new System.Windows.Forms.CheckBox();
            this.checkBoxFillUnfinished = new System.Windows.Forms.CheckBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageSingles = new System.Windows.Forms.TabPage();
            this.tabPageDoubles = new System.Windows.Forms.TabPage();
            this.textBoxURLDoubles = new System.Windows.Forms.TextBox();
            this.buttonRegexReplace = new System.Windows.Forms.Button();
            this.numericUpDownWinnersStart = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.numericUpDownWinnersEnd = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.numericUpDownWinnersOffset = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.numericUpDownLosersStart = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.numericUpDownLosersOffset = new System.Windows.Forms.NumericUpDown();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.numericUpDownLosersEnd = new System.Windows.Forms.NumericUpDown();
            this.checkBoxLockWinners = new System.Windows.Forms.CheckBox();
            this.checkBoxLockLosers = new System.Windows.Forms.CheckBox();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.checkBoxGuessFinal = new System.Windows.Forms.CheckBox();
            this.richTextBoxExRegexReplace = new RichTextBoxEx();
            this.richTextBoxExRegexFind = new RichTextBoxEx();
            this.richTextBoxExLpWinnersBracket = new RichTextBoxEx();
            this.richTextBoxExLpLosersBracket = new RichTextBoxEx();
            this.numericUpDownAdvanceLosers = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownAdvanceWinners)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPageSingles.SuspendLayout();
            this.tabPageDoubles.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWinnersStart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWinnersEnd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWinnersOffset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLosersStart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLosersOffset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLosersEnd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownAdvanceLosers)).BeginInit();
            this.SuspendLayout();
            // 
            // richTextBoxEntrants
            // 
            this.richTextBoxEntrants.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBoxEntrants.Location = new System.Drawing.Point(12, 15);
            this.richTextBoxEntrants.Name = "richTextBoxEntrants";
            this.richTextBoxEntrants.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.richTextBoxEntrants.Size = new System.Drawing.Size(691, 130);
            this.richTextBoxEntrants.TabIndex = 0;
            this.richTextBoxEntrants.Text = "";
            // 
            // textBoxURLSingles
            // 
            this.textBoxURLSingles.Location = new System.Drawing.Point(3, 16);
            this.textBoxURLSingles.Name = "textBoxURLSingles";
            this.textBoxURLSingles.Size = new System.Drawing.Size(215, 20);
            this.textBoxURLSingles.TabIndex = 1;
            this.textBoxURLSingles.Enter += new System.EventHandler(this.textBoxURL_Enter);
            // 
            // buttonGetBracket
            // 
            this.buttonGetBracket.Location = new System.Drawing.Point(709, 107);
            this.buttonGetBracket.Name = "buttonGetBracket";
            this.buttonGetBracket.Size = new System.Drawing.Size(75, 23);
            this.buttonGetBracket.TabIndex = 2;
            this.buttonGetBracket.Text = "Get Bracket";
            this.buttonGetBracket.UseVisualStyleBackColor = true;
            this.buttonGetBracket.Click += new System.EventHandler(this.buttonGetBracket_Click);
            // 
            // buttonGetPhase
            // 
            this.buttonGetPhase.Location = new System.Drawing.Point(863, 107);
            this.buttonGetPhase.Name = "buttonGetPhase";
            this.buttonGetPhase.Size = new System.Drawing.Size(75, 23);
            this.buttonGetPhase.TabIndex = 2;
            this.buttonGetPhase.Text = "Get Phase";
            this.buttonGetPhase.UseVisualStyleBackColor = true;
            this.buttonGetPhase.Click += new System.EventHandler(this.buttonGetPhase_Click);
            // 
            // radioButtonBracket
            // 
            this.radioButtonBracket.AutoSize = true;
            this.radioButtonBracket.Checked = true;
            this.radioButtonBracket.Location = new System.Drawing.Point(847, 136);
            this.radioButtonBracket.Name = "radioButtonBracket";
            this.radioButtonBracket.Size = new System.Drawing.Size(91, 17);
            this.radioButtonBracket.TabIndex = 8;
            this.radioButtonBracket.TabStop = true;
            this.radioButtonBracket.Text = "Bracket Pools";
            this.radioButtonBracket.UseVisualStyleBackColor = true;
            // 
            // radioButtonRR
            // 
            this.radioButtonRR.AutoSize = true;
            this.radioButtonRR.Location = new System.Drawing.Point(847, 159);
            this.radioButtonRR.Name = "radioButtonRR";
            this.radioButtonRR.Size = new System.Drawing.Size(88, 17);
            this.radioButtonRR.TabIndex = 9;
            this.radioButtonRR.Text = "Round Robin";
            this.radioButtonRR.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(885, 177);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(50, 28);
            this.label5.TabIndex = 5;
            this.label5.Text = "Advance Winners";
            // 
            // numericUpDownAdvanceWinners
            // 
            this.numericUpDownAdvanceWinners.Location = new System.Drawing.Point(847, 182);
            this.numericUpDownAdvanceWinners.Name = "numericUpDownAdvanceWinners";
            this.numericUpDownAdvanceWinners.Size = new System.Drawing.Size(32, 20);
            this.numericUpDownAdvanceWinners.TabIndex = 10;
            this.numericUpDownAdvanceWinners.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // richTextBoxWinners
            // 
            this.richTextBoxWinners.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBoxWinners.Location = new System.Drawing.Point(12, 210);
            this.richTextBoxWinners.Name = "richTextBoxWinners";
            this.richTextBoxWinners.Size = new System.Drawing.Size(342, 151);
            this.richTextBoxWinners.TabIndex = 0;
            this.richTextBoxWinners.Text = "";
            // 
            // richTextBoxLosers
            // 
            this.richTextBoxLosers.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBoxLosers.Location = new System.Drawing.Point(361, 210);
            this.richTextBoxLosers.Name = "richTextBoxLosers";
            this.richTextBoxLosers.Size = new System.Drawing.Size(342, 151);
            this.richTextBoxLosers.TabIndex = 0;
            this.richTextBoxLosers.Text = "";
            // 
            // richTextBoxLpOutput
            // 
            this.richTextBoxLpOutput.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBoxLpOutput.Location = new System.Drawing.Point(12, 524);
            this.richTextBoxLpOutput.Name = "richTextBoxLpOutput";
            this.richTextBoxLpOutput.Size = new System.Drawing.Size(691, 154);
            this.richTextBoxLpOutput.TabIndex = 0;
            this.richTextBoxLpOutput.Text = "";
            // 
            // buttonFill
            // 
            this.buttonFill.Location = new System.Drawing.Point(716, 286);
            this.buttonFill.Name = "buttonFill";
            this.buttonFill.Size = new System.Drawing.Size(75, 23);
            this.buttonFill.TabIndex = 2;
            this.buttonFill.Text = "Fill Singles";
            this.buttonFill.UseVisualStyleBackColor = true;
            this.buttonFill.Click += new System.EventHandler(this.buttonFillSingles_Click);
            // 
            // buttonFillDoubles
            // 
            this.buttonFillDoubles.Location = new System.Drawing.Point(716, 315);
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
            this.richTextBoxLog.Size = new System.Drawing.Size(225, 281);
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
            // checkBoxWinners
            // 
            this.checkBoxWinners.AutoSize = true;
            this.checkBoxWinners.Location = new System.Drawing.Point(709, 185);
            this.checkBoxWinners.Name = "checkBoxWinners";
            this.checkBoxWinners.Size = new System.Drawing.Size(65, 17);
            this.checkBoxWinners.TabIndex = 7;
            this.checkBoxWinners.Text = "Winners";
            this.checkBoxWinners.UseVisualStyleBackColor = true;
            // 
            // checkBoxLosers
            // 
            this.checkBoxLosers.AutoSize = true;
            this.checkBoxLosers.Location = new System.Drawing.Point(709, 208);
            this.checkBoxLosers.Name = "checkBoxLosers";
            this.checkBoxLosers.Size = new System.Drawing.Size(57, 17);
            this.checkBoxLosers.TabIndex = 7;
            this.checkBoxLosers.Text = "Losers";
            this.checkBoxLosers.UseVisualStyleBackColor = true;
            // 
            // checkBoxFillUnfinished
            // 
            this.checkBoxFillUnfinished.AutoSize = true;
            this.checkBoxFillUnfinished.Location = new System.Drawing.Point(709, 231);
            this.checkBoxFillUnfinished.Name = "checkBoxFillUnfinished";
            this.checkBoxFillUnfinished.Size = new System.Drawing.Size(111, 17);
            this.checkBoxFillUnfinished.TabIndex = 7;
            this.checkBoxFillUnfinished.Text = "Fill unfinished sets";
            this.checkBoxFillUnfinished.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageSingles);
            this.tabControl1.Controls.Add(this.tabPageDoubles);
            this.tabControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.Location = new System.Drawing.Point(709, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(229, 89);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPageSingles
            // 
            this.tabPageSingles.Controls.Add(this.textBoxURLSingles);
            this.tabPageSingles.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPageSingles.Location = new System.Drawing.Point(4, 34);
            this.tabPageSingles.Name = "tabPageSingles";
            this.tabPageSingles.Size = new System.Drawing.Size(221, 51);
            this.tabPageSingles.TabIndex = 0;
            this.tabPageSingles.Text = "Singles";
            this.tabPageSingles.UseVisualStyleBackColor = true;
            // 
            // tabPageDoubles
            // 
            this.tabPageDoubles.Controls.Add(this.textBoxURLDoubles);
            this.tabPageDoubles.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPageDoubles.Location = new System.Drawing.Point(4, 34);
            this.tabPageDoubles.Name = "tabPageDoubles";
            this.tabPageDoubles.Size = new System.Drawing.Size(221, 51);
            this.tabPageDoubles.TabIndex = 0;
            this.tabPageDoubles.Text = "Doubles";
            this.tabPageDoubles.UseVisualStyleBackColor = true;
            // 
            // textBoxURLDoubles
            // 
            this.textBoxURLDoubles.Location = new System.Drawing.Point(3, 16);
            this.textBoxURLDoubles.Name = "textBoxURLDoubles";
            this.textBoxURLDoubles.Size = new System.Drawing.Size(215, 20);
            this.textBoxURLDoubles.TabIndex = 2;
            this.textBoxURLDoubles.Enter += new System.EventHandler(this.textBoxURL_Enter);
            // 
            // buttonRegexReplace
            // 
            this.buttonRegexReplace.Location = new System.Drawing.Point(828, 315);
            this.buttonRegexReplace.Name = "buttonRegexReplace";
            this.buttonRegexReplace.Size = new System.Drawing.Size(106, 23);
            this.buttonRegexReplace.TabIndex = 2;
            this.buttonRegexReplace.Text = "Regex Replace";
            this.buttonRegexReplace.UseVisualStyleBackColor = true;
            this.buttonRegexReplace.Click += new System.EventHandler(this.buttonRegexReplace_Click);
            // 
            // numericUpDownWinnersStart
            // 
            this.numericUpDownWinnersStart.Location = new System.Drawing.Point(64, 187);
            this.numericUpDownWinnersStart.Name = "numericUpDownWinnersStart";
            this.numericUpDownWinnersStart.Size = new System.Drawing.Size(38, 20);
            this.numericUpDownWinnersStart.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(149, 170);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Offset";
            // 
            // numericUpDownWinnersEnd
            // 
            this.numericUpDownWinnersEnd.Location = new System.Drawing.Point(108, 187);
            this.numericUpDownWinnersEnd.Name = "numericUpDownWinnersEnd";
            this.numericUpDownWinnersEnd.Size = new System.Drawing.Size(38, 20);
            this.numericUpDownWinnersEnd.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(105, 170);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(26, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "End";
            // 
            // numericUpDownWinnersOffset
            // 
            this.numericUpDownWinnersOffset.Location = new System.Drawing.Point(152, 187);
            this.numericUpDownWinnersOffset.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numericUpDownWinnersOffset.Minimum = new decimal(new int[] {
            20,
            0,
            0,
            -2147483648});
            this.numericUpDownWinnersOffset.Name = "numericUpDownWinnersOffset";
            this.numericUpDownWinnersOffset.Size = new System.Drawing.Size(38, 20);
            this.numericUpDownWinnersOffset.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(61, 170);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Start";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(12, 194);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(46, 13);
            this.label9.TabIndex = 6;
            this.label9.Text = "Winners";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(358, 194);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(38, 13);
            this.label10.TabIndex = 6;
            this.label10.Text = "Losers";
            // 
            // numericUpDownLosersStart
            // 
            this.numericUpDownLosersStart.Location = new System.Drawing.Point(402, 187);
            this.numericUpDownLosersStart.Name = "numericUpDownLosersStart";
            this.numericUpDownLosersStart.Size = new System.Drawing.Size(38, 20);
            this.numericUpDownLosersStart.TabIndex = 7;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(399, 170);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(29, 13);
            this.label11.TabIndex = 10;
            this.label11.Text = "Start";
            // 
            // numericUpDownLosersOffset
            // 
            this.numericUpDownLosersOffset.Location = new System.Drawing.Point(490, 187);
            this.numericUpDownLosersOffset.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numericUpDownLosersOffset.Minimum = new decimal(new int[] {
            20,
            0,
            0,
            -2147483648});
            this.numericUpDownLosersOffset.Name = "numericUpDownLosersOffset";
            this.numericUpDownLosersOffset.Size = new System.Drawing.Size(38, 20);
            this.numericUpDownLosersOffset.TabIndex = 8;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(443, 170);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(26, 13);
            this.label12.TabIndex = 11;
            this.label12.Text = "End";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(487, 170);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(35, 13);
            this.label13.TabIndex = 12;
            this.label13.Text = "Offset";
            // 
            // numericUpDownLosersEnd
            // 
            this.numericUpDownLosersEnd.Location = new System.Drawing.Point(446, 187);
            this.numericUpDownLosersEnd.Name = "numericUpDownLosersEnd";
            this.numericUpDownLosersEnd.Size = new System.Drawing.Size(38, 20);
            this.numericUpDownLosersEnd.TabIndex = 9;
            // 
            // checkBoxLockWinners
            // 
            this.checkBoxLockWinners.AutoSize = true;
            this.checkBoxLockWinners.Location = new System.Drawing.Point(196, 188);
            this.checkBoxLockWinners.Name = "checkBoxLockWinners";
            this.checkBoxLockWinners.Size = new System.Drawing.Size(50, 17);
            this.checkBoxLockWinners.TabIndex = 14;
            this.checkBoxLockWinners.Text = "Lock";
            this.checkBoxLockWinners.UseVisualStyleBackColor = true;
            this.checkBoxLockWinners.CheckedChanged += new System.EventHandler(this.checkBoxLock_CheckedChanged);
            // 
            // checkBoxLockLosers
            // 
            this.checkBoxLockLosers.AutoSize = true;
            this.checkBoxLockLosers.Location = new System.Drawing.Point(534, 188);
            this.checkBoxLockLosers.Name = "checkBoxLockLosers";
            this.checkBoxLockLosers.Size = new System.Drawing.Size(50, 17);
            this.checkBoxLockLosers.TabIndex = 14;
            this.checkBoxLockLosers.Text = "Lock";
            this.checkBoxLockLosers.UseVisualStyleBackColor = true;
            this.checkBoxLockLosers.CheckedChanged += new System.EventHandler(this.checkBoxLock_CheckedChanged);
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(709, 655);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(225, 23);
            this.progressBar.TabIndex = 15;
            // 
            // checkBoxGuessFinal
            // 
            this.checkBoxGuessFinal.AutoSize = true;
            this.checkBoxGuessFinal.Location = new System.Drawing.Point(709, 254);
            this.checkBoxGuessFinal.Name = "checkBoxGuessFinal";
            this.checkBoxGuessFinal.Size = new System.Drawing.Size(121, 17);
            this.checkBoxGuessFinal.TabIndex = 14;
            this.checkBoxGuessFinal.Text = "Guess Final Bracket";
            this.checkBoxGuessFinal.UseVisualStyleBackColor = true;
            this.checkBoxGuessFinal.CheckedChanged += new System.EventHandler(this.checkBoxLock_CheckedChanged);
            // 
            // richTextBoxExRegexReplace
            // 
            this.richTextBoxExRegexReplace.Cue = null;
            this.richTextBoxExRegexReplace.Location = new System.Drawing.Point(361, 472);
            this.richTextBoxExRegexReplace.Name = "richTextBoxExRegexReplace";
            this.richTextBoxExRegexReplace.Size = new System.Drawing.Size(342, 46);
            this.richTextBoxExRegexReplace.TabIndex = 16;
            this.richTextBoxExRegexReplace.Text = "";
            // 
            // richTextBoxExRegexFind
            // 
            this.richTextBoxExRegexFind.Cue = null;
            this.richTextBoxExRegexFind.Location = new System.Drawing.Point(12, 472);
            this.richTextBoxExRegexFind.Name = "richTextBoxExRegexFind";
            this.richTextBoxExRegexFind.Size = new System.Drawing.Size(342, 46);
            this.richTextBoxExRegexFind.TabIndex = 16;
            this.richTextBoxExRegexFind.Text = "";
            // 
            // richTextBoxExLpWinnersBracket
            // 
            this.richTextBoxExLpWinnersBracket.Cue = null;
            this.richTextBoxExLpWinnersBracket.Location = new System.Drawing.Point(13, 367);
            this.richTextBoxExLpWinnersBracket.Name = "richTextBoxExLpWinnersBracket";
            this.richTextBoxExLpWinnersBracket.Size = new System.Drawing.Size(341, 89);
            this.richTextBoxExLpWinnersBracket.TabIndex = 13;
            this.richTextBoxExLpWinnersBracket.Text = "";
            // 
            // richTextBoxExLpLosersBracket
            // 
            this.richTextBoxExLpLosersBracket.Cue = null;
            this.richTextBoxExLpLosersBracket.Location = new System.Drawing.Point(360, 368);
            this.richTextBoxExLpLosersBracket.Name = "richTextBoxExLpLosersBracket";
            this.richTextBoxExLpLosersBracket.Size = new System.Drawing.Size(341, 88);
            this.richTextBoxExLpLosersBracket.TabIndex = 13;
            this.richTextBoxExLpLosersBracket.Text = "";
            // 
            // numericUpDownAdvanceLosers
            // 
            this.numericUpDownAdvanceLosers.Location = new System.Drawing.Point(847, 215);
            this.numericUpDownAdvanceLosers.Name = "numericUpDownAdvanceLosers";
            this.numericUpDownAdvanceLosers.Size = new System.Drawing.Size(32, 20);
            this.numericUpDownAdvanceLosers.TabIndex = 10;
            this.numericUpDownAdvanceLosers.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(885, 210);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(50, 27);
            this.label6.TabIndex = 5;
            this.label6.Text = "Advance Losers";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(948, 690);
            this.Controls.Add(this.richTextBoxExRegexReplace);
            this.Controls.Add(this.numericUpDownAdvanceLosers);
            this.Controls.Add(this.numericUpDownAdvanceWinners);
            this.Controls.Add(this.richTextBoxExRegexFind);
            this.Controls.Add(this.radioButtonRR);
            this.Controls.Add(this.radioButtonBracket);
            this.Controls.Add(this.buttonGetBracket);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.buttonGetPhase);
            this.Controls.Add(this.checkBoxGuessFinal);
            this.Controls.Add(this.checkBoxLockLosers);
            this.Controls.Add(this.checkBoxFillUnfinished);
            this.Controls.Add(this.buttonRegexReplace);
            this.Controls.Add(this.checkBoxLosers);
            this.Controls.Add(this.checkBoxLockWinners);
            this.Controls.Add(this.buttonFill);
            this.Controls.Add(this.buttonFillDoubles);
            this.Controls.Add(this.checkBoxWinners);
            this.Controls.Add(this.richTextBoxExLpWinnersBracket);
            this.Controls.Add(this.richTextBoxExLpLosersBracket);
            this.Controls.Add(this.numericUpDownLosersStart);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.numericUpDownLosersOffset);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.numericUpDownLosersEnd);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.richTextBoxLog);
            this.Controls.Add(this.richTextBoxLpOutput);
            this.Controls.Add(this.richTextBoxLosers);
            this.Controls.Add(this.richTextBoxWinners);
            this.Controls.Add(this.richTextBoxEntrants);
            this.Controls.Add(this.numericUpDownWinnersStart);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.numericUpDownWinnersOffset);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.numericUpDownWinnersEnd);
            this.Name = "Form1";
            this.Text = "Smash.gg to Liquipedia";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownAdvanceWinners)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPageSingles.ResumeLayout(false);
            this.tabPageSingles.PerformLayout();
            this.tabPageDoubles.ResumeLayout(false);
            this.tabPageDoubles.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWinnersStart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWinnersEnd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWinnersOffset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLosersStart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLosersOffset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLosersEnd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownAdvanceLosers)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBoxEntrants;
        private System.Windows.Forms.TextBox textBoxURLSingles;
        private System.Windows.Forms.Button buttonGetBracket;
        private System.Windows.Forms.Button buttonGetPhase;
        private System.Windows.Forms.RadioButton radioButtonBracket;
        private System.Windows.Forms.RadioButton radioButtonRR;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown numericUpDownAdvanceWinners;
        private System.Windows.Forms.RichTextBox richTextBoxWinners;
        private System.Windows.Forms.RichTextBox richTextBoxLosers;
        private System.Windows.Forms.RichTextBox richTextBoxLpOutput;
        private System.Windows.Forms.Button buttonFill;
        private System.Windows.Forms.Button buttonFillDoubles;
        private System.Windows.Forms.RichTextBox richTextBoxLog;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBoxWinners;
        private System.Windows.Forms.CheckBox checkBoxLosers;
        private System.Windows.Forms.CheckBox checkBoxFillUnfinished;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageSingles;
        private System.Windows.Forms.NumericUpDown numericUpDownWinnersStart;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numericUpDownWinnersEnd;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numericUpDownWinnersOffset;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TabPage tabPageDoubles;
        private System.Windows.Forms.TextBox textBoxURLDoubles;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown numericUpDownLosersStart;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.NumericUpDown numericUpDownLosersOffset;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.NumericUpDown numericUpDownLosersEnd;
        private RichTextBoxEx richTextBoxExLpLosersBracket;
        private RichTextBoxEx richTextBoxExLpWinnersBracket;
        private System.Windows.Forms.CheckBox checkBoxLockWinners;
        private System.Windows.Forms.CheckBox checkBoxLockLosers;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.CheckBox checkBoxGuessFinal;
        private RichTextBoxEx richTextBoxExRegexFind;
        private RichTextBoxEx richTextBoxExRegexReplace;
        private System.Windows.Forms.Button buttonRegexReplace;
        private System.Windows.Forms.NumericUpDown numericUpDownAdvanceLosers;
        private System.Windows.Forms.Label label6;
    }
}

