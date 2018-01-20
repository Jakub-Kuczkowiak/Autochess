namespace Autochess
{
    partial class dlgAskMove
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
            this.lstViewMovesList = new System.Windows.Forms.ListView();
            this.btnOK = new System.Windows.Forms.Button();
            this.chkNeverAskAgain = new System.Windows.Forms.CheckBox();
            this.btnRandomize = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lstViewMovesList
            // 
            this.lstViewMovesList.AutoArrange = false;
            this.lstViewMovesList.Dock = System.Windows.Forms.DockStyle.Top;
            this.lstViewMovesList.Font = new System.Drawing.Font("Segoe UI Light", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lstViewMovesList.Location = new System.Drawing.Point(0, 0);
            this.lstViewMovesList.MultiSelect = false;
            this.lstViewMovesList.Name = "lstViewMovesList";
            this.lstViewMovesList.Size = new System.Drawing.Size(448, 175);
            this.lstViewMovesList.TabIndex = 0;
            this.lstViewMovesList.UseCompatibleStateImageBehavior = false;
            this.lstViewMovesList.View = System.Windows.Forms.View.List;
            this.lstViewMovesList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lstViewMovesList_MouseDoubleClick);
            // 
            // btnOK
            // 
            this.btnOK.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnOK.Location = new System.Drawing.Point(0, 204);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(448, 44);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // chkNeverAskAgain
            // 
            this.chkNeverAskAgain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkNeverAskAgain.Location = new System.Drawing.Point(0, 175);
            this.chkNeverAskAgain.Name = "chkNeverAskAgain";
            this.chkNeverAskAgain.Size = new System.Drawing.Size(448, 29);
            this.chkNeverAskAgain.TabIndex = 3;
            this.chkNeverAskAgain.Text = "Never ask again about move";
            this.chkNeverAskAgain.UseVisualStyleBackColor = true;
            // 
            // btnRandomize
            // 
            this.btnRandomize.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnRandomize.Location = new System.Drawing.Point(216, 175);
            this.btnRandomize.Name = "btnRandomize";
            this.btnRandomize.Size = new System.Drawing.Size(232, 29);
            this.btnRandomize.TabIndex = 4;
            this.btnRandomize.Text = "Randomize";
            this.btnRandomize.UseVisualStyleBackColor = true;
            this.btnRandomize.Click += new System.EventHandler(this.btnRandomize_Click);
            // 
            // dlgAskMove
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(448, 248);
            this.Controls.Add(this.btnRandomize);
            this.Controls.Add(this.chkNeverAskAgain);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lstViewMovesList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "dlgAskMove";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Choose move";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lstViewMovesList;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.CheckBox chkNeverAskAgain;
        private System.Windows.Forms.Button btnRandomize;
    }
}