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
            this.richTextBoxLpOutput = new System.Windows.Forms.RichTextBox();
            this.buttonFill = new System.Windows.Forms.Button();
            this.buttonFillDoubles = new System.Windows.Forms.Button();
            this.richTextBoxLog = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBoxWinnersSingles = new System.Windows.Forms.CheckBox();
            this.checkBoxLosersSingles = new System.Windows.Forms.CheckBox();
            this.checkBoxFillUnfinishedSingles = new System.Windows.Forms.CheckBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageSingles = new System.Windows.Forms.TabPage();
            this.tabPageDoubles = new System.Windows.Forms.TabPage();
            this.checkBoxFillUnfinishedDoubles = new System.Windows.Forms.CheckBox();
            this.checkBoxLosersDoubles = new System.Windows.Forms.CheckBox();
            this.checkBoxWinnersDoubles = new System.Windows.Forms.CheckBox();
            this.buttonDoubles = new System.Windows.Forms.Button();
            this.textBoxURLDoubles = new System.Windows.Forms.TextBox();
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
            this.richTextBoxExLpWinnersBracket = new RichTextBoxEx();
            this.richTextBoxExLpLosersBracket = new RichTextBoxEx();
            this.checkBoxLockWinners = new System.Windows.Forms.CheckBox();
            this.checkBoxLockLosers = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownAdvance)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPageSingles.SuspendLayout();
            this.tabPageDoubles.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWinnersStart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWinnersEnd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWinnersOffset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLosersStart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLosersOffset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLosersEnd)).BeginInit();
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
            this.richTextBoxLog.Size = new System.Drawing.Size(225, 310);
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
            this.tabPageSingles.Controls.Add(this.label5);
            this.tabPageSingles.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPageSingles.Location = new System.Drawing.Point(4, 34);
            this.tabPageSingles.Name = "tabPageSingles";
            this.tabPageSingles.Size = new System.Drawing.Size(221, 311);
            this.tabPageSingles.TabIndex = 0;
            this.tabPageSingles.Text = "Singles";
            this.tabPageSingles.UseVisualStyleBackColor = true;
            // 
            // tabPageDoubles
            // 
            this.tabPageDoubles.Controls.Add(this.checkBoxFillUnfinishedDoubles);
            this.tabPageDoubles.Controls.Add(this.checkBoxLosersDoubles);
            this.tabPageDoubles.Controls.Add(this.checkBoxWinnersDoubles);
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
            // richTextBoxExLpWinnersBracket
            // 
            this.richTextBoxExLpWinnersBracket.Cue = null;
            this.richTextBoxExLpWinnersBracket.Location = new System.Drawing.Point(13, 367);
            this.richTextBoxExLpWinnersBracket.Name = "richTextBoxExLpWinnersBracket";
            this.richTextBoxExLpWinnersBracket.Size = new System.Drawing.Size(341, 150);
            this.richTextBoxExLpWinnersBracket.TabIndex = 13;
            this.richTextBoxExLpWinnersBracket.Text = "";
            // 
            // richTextBoxExLpLosersBracket
            // 
            this.richTextBoxExLpLosersBracket.Cue = null;
            this.richTextBoxExLpLosersBracket.Location = new System.Drawing.Point(360, 368);
            this.richTextBoxExLpLosersBracket.Name = "richTextBoxExLpLosersBracket";
            this.richTextBoxExLpLosersBracket.Size = new System.Drawing.Size(341, 150);
            this.richTextBoxExLpLosersBracket.TabIndex = 13;
            this.richTextBoxExLpLosersBracket.Text = "";
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
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(948, 690);
            this.Controls.Add(this.checkBoxLockLosers);
            this.Controls.Add(this.checkBoxLockWinners);
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
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownAdvance)).EndInit();
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
        private System.Windows.Forms.RichTextBox richTextBoxLpOutput;
        private System.Windows.Forms.Button buttonFill;
        private System.Windows.Forms.Button buttonFillDoubles;
        private System.Windows.Forms.RichTextBox richTextBoxLog;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBoxWinnersSingles;
        private System.Windows.Forms.CheckBox checkBoxLosersSingles;
        private System.Windows.Forms.CheckBox checkBoxFillUnfinishedSingles;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageSingles;
        private System.Windows.Forms.NumericUpDown numericUpDownWinnersStart;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numericUpDownWinnersEnd;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numericUpDownWinnersOffset;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TabPage tabPageDoubles;
        private System.Windows.Forms.CheckBox checkBoxFillUnfinishedDoubles;
        private System.Windows.Forms.CheckBox checkBoxLosersDoubles;
        private System.Windows.Forms.CheckBox checkBoxWinnersDoubles;
        private System.Windows.Forms.Button buttonDoubles;
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
    }
}

