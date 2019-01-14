namespace smashgg_to_liquipedia
{
    partial class SelectionForm
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
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonPrizePool = new System.Windows.Forms.Button();
            this.buttonGetSets = new System.Windows.Forms.Button();
            this.buttonRefresh = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.CheckBoxes = true;
            this.treeView1.Location = new System.Drawing.Point(12, 25);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(402, 320);
            this.treeView1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Select ONE item:";
            // 
            // buttonPrizePool
            // 
            this.buttonPrizePool.Location = new System.Drawing.Point(12, 351);
            this.buttonPrizePool.Name = "buttonPrizePool";
            this.buttonPrizePool.Size = new System.Drawing.Size(119, 23);
            this.buttonPrizePool.TabIndex = 2;
            this.buttonPrizePool.Text = "Prize Pool/Standings";
            this.buttonPrizePool.UseVisualStyleBackColor = true;
            this.buttonPrizePool.Click += new System.EventHandler(this.buttonPrizePool_Click);
            // 
            // buttonGetSets
            // 
            this.buttonGetSets.Location = new System.Drawing.Point(162, 351);
            this.buttonGetSets.Name = "buttonGetSets";
            this.buttonGetSets.Size = new System.Drawing.Size(75, 23);
            this.buttonGetSets.TabIndex = 2;
            this.buttonGetSets.Text = "Get Sets";
            this.buttonGetSets.UseVisualStyleBackColor = true;
            this.buttonGetSets.Click += new System.EventHandler(this.buttonGetSets_Click);
            // 
            // buttonRefresh
            // 
            this.buttonRefresh.Location = new System.Drawing.Point(339, 351);
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.Size = new System.Drawing.Size(75, 23);
            this.buttonRefresh.TabIndex = 2;
            this.buttonRefresh.Text = "Refresh";
            this.buttonRefresh.UseVisualStyleBackColor = true;
            this.buttonRefresh.Click += new System.EventHandler(this.buttonRefresh_Click);
            // 
            // SelectionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(426, 391);
            this.Controls.Add(this.buttonRefresh);
            this.Controls.Add(this.buttonGetSets);
            this.Controls.Add(this.buttonPrizePool);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.treeView1);
            this.Name = "SelectionForm";
            this.Text = "SelectionForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonPrizePool;
        private System.Windows.Forms.Button buttonGetSets;
        private System.Windows.Forms.Button buttonRefresh;
    }
}