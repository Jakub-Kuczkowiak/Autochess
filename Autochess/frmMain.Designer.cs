namespace Autochess
{
    partial class frmMain
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
            this.components = new System.ComponentModel.Container();
            this.btnStartEngine = new System.Windows.Forms.Button();
            this.txtMoves = new System.Windows.Forms.TextBox();
            this.txtTime = new System.Windows.Forms.TextBox();
            this.tmrHotkeys = new System.Windows.Forms.Timer(this.components);
            this.chkPondering = new System.Windows.Forms.CheckBox();
            this.tmrPause = new System.Windows.Forms.Timer(this.components);
            this.gbContempt = new System.Windows.Forms.GroupBox();
            this.rdoContempt_2 = new System.Windows.Forms.RadioButton();
            this.rdoContempt_1 = new System.Windows.Forms.RadioButton();
            this.rdoContempt_0 = new System.Windows.Forms.RadioButton();
            this.gbOpeningsBook = new System.Windows.Forms.GroupBox();
            this.rdoOpeningsBook_AskAboutMoves = new System.Windows.Forms.RadioButton();
            this.rdoOpeningsBook_RandomizeMoves = new System.Windows.Forms.RadioButton();
            this.chkOpeningsBook_UseOpeningsBook = new System.Windows.Forms.CheckBox();
            this.gbChessEngine = new System.Windows.Forms.GroupBox();
            this.rdoKomodo = new System.Windows.Forms.RadioButton();
            this.trbDelayMove = new System.Windows.Forms.TrackBar();
            this.lblMoveDelay = new System.Windows.Forms.Label();
            this.lblCopyright = new System.Windows.Forms.Label();
            this.lblTime = new System.Windows.Forms.Label();
            this.gbContempt.SuspendLayout();
            this.gbOpeningsBook.SuspendLayout();
            this.gbChessEngine.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trbDelayMove)).BeginInit();
            this.SuspendLayout();
            // 
            // btnStartEngine
            // 
            this.btnStartEngine.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStartEngine.Location = new System.Drawing.Point(12, 12);
            this.btnStartEngine.Name = "btnStartEngine";
            this.btnStartEngine.Size = new System.Drawing.Size(372, 61);
            this.btnStartEngine.TabIndex = 0;
            this.btnStartEngine.Text = "Start Engine";
            this.btnStartEngine.UseVisualStyleBackColor = true;
            this.btnStartEngine.Click += new System.EventHandler(this.btnStartEngine_Click);
            // 
            // txtMoves
            // 
            this.txtMoves.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMoves.Location = new System.Drawing.Point(12, 79);
            this.txtMoves.Name = "txtMoves";
            this.txtMoves.Size = new System.Drawing.Size(790, 20);
            this.txtMoves.TabIndex = 1;
            // 
            // txtTime
            // 
            this.txtTime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTime.Location = new System.Drawing.Point(469, 34);
            this.txtTime.Name = "txtTime";
            this.txtTime.Size = new System.Drawing.Size(333, 20);
            this.txtTime.TabIndex = 2;
            this.txtTime.Text = "80000";
            // 
            // tmrHotkeys
            // 
            this.tmrHotkeys.Enabled = true;
            this.tmrHotkeys.Interval = 1;
            this.tmrHotkeys.Tick += new System.EventHandler(this.tmrHotkeys_Tick);
            // 
            // chkPondering
            // 
            this.chkPondering.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chkPondering.AutoSize = true;
            this.chkPondering.Checked = true;
            this.chkPondering.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPondering.Location = new System.Drawing.Point(368, 128);
            this.chkPondering.Name = "chkPondering";
            this.chkPondering.Size = new System.Drawing.Size(74, 17);
            this.chkPondering.TabIndex = 5;
            this.chkPondering.Text = "Pondering";
            this.chkPondering.UseVisualStyleBackColor = true;
            // 
            // tmrPause
            // 
            this.tmrPause.Enabled = true;
            this.tmrPause.Interval = 1000;
            this.tmrPause.Tick += new System.EventHandler(this.tmrPause_Tick);
            // 
            // gbContempt
            // 
            this.gbContempt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbContempt.Controls.Add(this.rdoContempt_2);
            this.gbContempt.Controls.Add(this.rdoContempt_1);
            this.gbContempt.Controls.Add(this.rdoContempt_0);
            this.gbContempt.Location = new System.Drawing.Point(12, 163);
            this.gbContempt.Name = "gbContempt";
            this.gbContempt.Size = new System.Drawing.Size(790, 48);
            this.gbContempt.TabIndex = 6;
            this.gbContempt.TabStop = false;
            this.gbContempt.Text = "Contempt";
            // 
            // rdoContempt_2
            // 
            this.rdoContempt_2.AutoSize = true;
            this.rdoContempt_2.Dock = System.Windows.Forms.DockStyle.Right;
            this.rdoContempt_2.Location = new System.Drawing.Point(594, 16);
            this.rdoContempt_2.Name = "rdoContempt_2";
            this.rdoContempt_2.Size = new System.Drawing.Size(193, 29);
            this.rdoContempt_2.TabIndex = 9;
            this.rdoContempt_2.Text = "2 (Aggressive - Favors Attack Style)";
            this.rdoContempt_2.UseVisualStyleBackColor = true;
            // 
            // rdoContempt_1
            // 
            this.rdoContempt_1.AutoSize = true;
            this.rdoContempt_1.Checked = true;
            this.rdoContempt_1.Location = new System.Drawing.Point(315, 19);
            this.rdoContempt_1.Name = "rdoContempt_1";
            this.rdoContempt_1.Size = new System.Drawing.Size(146, 17);
            this.rdoContempt_1.TabIndex = 8;
            this.rdoContempt_1.TabStop = true;
            this.rdoContempt_1.Text = "1 (Default - Compromised)";
            this.rdoContempt_1.UseVisualStyleBackColor = true;
            // 
            // rdoContempt_0
            // 
            this.rdoContempt_0.AutoSize = true;
            this.rdoContempt_0.Dock = System.Windows.Forms.DockStyle.Left;
            this.rdoContempt_0.Location = new System.Drawing.Point(3, 16);
            this.rdoContempt_0.Name = "rdoContempt_0";
            this.rdoContempt_0.Size = new System.Drawing.Size(105, 29);
            this.rdoContempt_0.TabIndex = 7;
            this.rdoContempt_0.Text = "0 (Favors Draws)";
            this.rdoContempt_0.UseVisualStyleBackColor = true;
            // 
            // gbOpeningsBook
            // 
            this.gbOpeningsBook.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.gbOpeningsBook.Controls.Add(this.rdoOpeningsBook_AskAboutMoves);
            this.gbOpeningsBook.Controls.Add(this.rdoOpeningsBook_RandomizeMoves);
            this.gbOpeningsBook.Controls.Add(this.chkOpeningsBook_UseOpeningsBook);
            this.gbOpeningsBook.Location = new System.Drawing.Point(556, 105);
            this.gbOpeningsBook.Name = "gbOpeningsBook";
            this.gbOpeningsBook.Size = new System.Drawing.Size(240, 58);
            this.gbOpeningsBook.TabIndex = 7;
            this.gbOpeningsBook.TabStop = false;
            this.gbOpeningsBook.Text = "Openings Book [PROTOTYPE]";
            // 
            // rdoOpeningsBook_AskAboutMoves
            // 
            this.rdoOpeningsBook_AskAboutMoves.AutoSize = true;
            this.rdoOpeningsBook_AskAboutMoves.Checked = true;
            this.rdoOpeningsBook_AskAboutMoves.Location = new System.Drawing.Point(125, 35);
            this.rdoOpeningsBook_AskAboutMoves.Name = "rdoOpeningsBook_AskAboutMoves";
            this.rdoOpeningsBook_AskAboutMoves.Size = new System.Drawing.Size(107, 17);
            this.rdoOpeningsBook_AskAboutMoves.TabIndex = 10;
            this.rdoOpeningsBook_AskAboutMoves.TabStop = true;
            this.rdoOpeningsBook_AskAboutMoves.Text = "Ask about moves";
            this.rdoOpeningsBook_AskAboutMoves.UseVisualStyleBackColor = true;
            // 
            // rdoOpeningsBook_RandomizeMoves
            // 
            this.rdoOpeningsBook_RandomizeMoves.AutoSize = true;
            this.rdoOpeningsBook_RandomizeMoves.Location = new System.Drawing.Point(6, 35);
            this.rdoOpeningsBook_RandomizeMoves.Name = "rdoOpeningsBook_RandomizeMoves";
            this.rdoOpeningsBook_RandomizeMoves.Size = new System.Drawing.Size(113, 17);
            this.rdoOpeningsBook_RandomizeMoves.TabIndex = 9;
            this.rdoOpeningsBook_RandomizeMoves.Text = "Randomize Moves";
            this.rdoOpeningsBook_RandomizeMoves.UseVisualStyleBackColor = true;
            // 
            // chkOpeningsBook_UseOpeningsBook
            // 
            this.chkOpeningsBook_UseOpeningsBook.AutoSize = true;
            this.chkOpeningsBook_UseOpeningsBook.Checked = true;
            this.chkOpeningsBook_UseOpeningsBook.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkOpeningsBook_UseOpeningsBook.Location = new System.Drawing.Point(55, 17);
            this.chkOpeningsBook_UseOpeningsBook.Name = "chkOpeningsBook_UseOpeningsBook";
            this.chkOpeningsBook_UseOpeningsBook.Size = new System.Drawing.Size(121, 17);
            this.chkOpeningsBook_UseOpeningsBook.TabIndex = 8;
            this.chkOpeningsBook_UseOpeningsBook.Text = "Use Openings Book";
            this.chkOpeningsBook_UseOpeningsBook.UseVisualStyleBackColor = true;
            this.chkOpeningsBook_UseOpeningsBook.CheckedChanged += new System.EventHandler(this.chkOpeningsBook_UseOpeningsBook_CheckedChanged);
            // 
            // gbChessEngine
            // 
            this.gbChessEngine.Controls.Add(this.rdoKomodo);
            this.gbChessEngine.Location = new System.Drawing.Point(18, 105);
            this.gbChessEngine.Name = "gbChessEngine";
            this.gbChessEngine.Size = new System.Drawing.Size(293, 58);
            this.gbChessEngine.TabIndex = 8;
            this.gbChessEngine.TabStop = false;
            this.gbChessEngine.Text = "Chess Power Source [DETERMINER]";
            // 
            // rdoKomodo
            // 
            this.rdoKomodo.AutoSize = true;
            this.rdoKomodo.Checked = true;
            this.rdoKomodo.Location = new System.Drawing.Point(96, 23);
            this.rdoKomodo.Name = "rdoKomodo";
            this.rdoKomodo.Size = new System.Drawing.Size(73, 17);
            this.rdoKomodo.TabIndex = 9;
            this.rdoKomodo.TabStop = true;
            this.rdoKomodo.Text = "Komodo 8";
            this.rdoKomodo.UseVisualStyleBackColor = true;
            // 
            // trbDelayMove
            // 
            this.trbDelayMove.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trbDelayMove.Enabled = false;
            this.trbDelayMove.Location = new System.Drawing.Point(270, 217);
            this.trbDelayMove.Maximum = 20;
            this.trbDelayMove.Name = "trbDelayMove";
            this.trbDelayMove.Size = new System.Drawing.Size(532, 45);
            this.trbDelayMove.TabIndex = 9;
            // 
            // lblMoveDelay
            // 
            this.lblMoveDelay.AutoSize = true;
            this.lblMoveDelay.Enabled = false;
            this.lblMoveDelay.Location = new System.Drawing.Point(125, 227);
            this.lblMoveDelay.Name = "lblMoveDelay";
            this.lblMoveDelay.Size = new System.Drawing.Size(137, 13);
            this.lblMoveDelay.TabIndex = 10;
            this.lblMoveDelay.Text = "Minimum time to be delayed";
            // 
            // lblCopyright
            // 
            this.lblCopyright.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblCopyright.Location = new System.Drawing.Point(0, 259);
            this.lblCopyright.Name = "lblCopyright";
            this.lblCopyright.Size = new System.Drawing.Size(814, 22);
            this.lblCopyright.TabIndex = 11;
            this.lblCopyright.Text = "Copyright (C) Jakub Kuczkowiak 2017";
            this.lblCopyright.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.Location = new System.Drawing.Point(408, 37);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(55, 13);
            this.lblTime.TabIndex = 12;
            this.lblTime.Text = "Time (ms):";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(814, 281);
            this.Controls.Add(this.lblTime);
            this.Controls.Add(this.lblCopyright);
            this.Controls.Add(this.lblMoveDelay);
            this.Controls.Add(this.trbDelayMove);
            this.Controls.Add(this.gbChessEngine);
            this.Controls.Add(this.gbOpeningsBook);
            this.Controls.Add(this.gbContempt);
            this.Controls.Add(this.chkPondering);
            this.Controls.Add(this.txtTime);
            this.Controls.Add(this.txtMoves);
            this.Controls.Add(this.btnStartEngine);
            this.Name = "frmMain";
            this.Text = "Autochess v.2";
            this.TopMost = true;
            this.gbContempt.ResumeLayout(false);
            this.gbContempt.PerformLayout();
            this.gbOpeningsBook.ResumeLayout(false);
            this.gbOpeningsBook.PerformLayout();
            this.gbChessEngine.ResumeLayout(false);
            this.gbChessEngine.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trbDelayMove)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStartEngine;
        private System.Windows.Forms.TextBox txtMoves;
        private System.Windows.Forms.TextBox txtTime;
        private System.Windows.Forms.Timer tmrHotkeys;
        private System.Windows.Forms.CheckBox chkPondering;
        private System.Windows.Forms.Timer tmrPause;
        private System.Windows.Forms.GroupBox gbContempt;
        private System.Windows.Forms.RadioButton rdoContempt_2;
        private System.Windows.Forms.RadioButton rdoContempt_1;
        private System.Windows.Forms.RadioButton rdoContempt_0;
        private System.Windows.Forms.GroupBox gbOpeningsBook;
        private System.Windows.Forms.RadioButton rdoOpeningsBook_AskAboutMoves;
        private System.Windows.Forms.RadioButton rdoOpeningsBook_RandomizeMoves;
        private System.Windows.Forms.CheckBox chkOpeningsBook_UseOpeningsBook;
        private System.Windows.Forms.GroupBox gbChessEngine;
        private System.Windows.Forms.RadioButton rdoKomodo;
        private System.Windows.Forms.TrackBar trbDelayMove;
        private System.Windows.Forms.Label lblMoveDelay;
        private System.Windows.Forms.Label lblCopyright;
        private System.Windows.Forms.Label lblTime;
    }
}

