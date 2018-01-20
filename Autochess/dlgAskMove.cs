// Copyright (C) Jakub Kuczkowiak 2017

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Autochess
{
    public partial class dlgAskMove : Form
    {
        private string MoveResult = null;

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams createParams = base.CreateParams;
                createParams.Style &= ~0x80000; // WS_SYSMENU

                return createParams;
            }
        }

        public dlgAskMove(List<String> SMovesList)
        {
            InitializeComponent();

            for (int i = 0; i < SMovesList.Count; i++)
                lstViewMovesList.Items.Add(SMovesList.ElementAt(i));
        }

        public String GetSelectedMove()
        {
            return MoveResult;
        }

        public bool IsNeverAskAgainSelected()
        {
            return chkNeverAskAgain.Checked;
        }

        private void lstViewMovesList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lstViewMovesList.SelectedItems.Count == 1)
            {
                MoveResult = lstViewMovesList.SelectedItems[0].Text;

                if (!chkNeverAskAgain.Checked)
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                else
                    this.DialogResult = System.Windows.Forms.DialogResult.Yes;
            }
        }
        
        private void btnOK_Click(object sender, EventArgs e)
        {
            // THe user has not selected any move.
            if (lstViewMovesList.SelectedItems.Count == 1)
            {
                MoveResult = lstViewMovesList.SelectedItems[0].Text;

                if (!chkNeverAskAgain.Checked)
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                else
                    this.DialogResult = System.Windows.Forms.DialogResult.Yes;
            }
            else
            {
                MessageBox.Show("Choose the move form the list.", "The move has not been chosen.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // TODO: Should we just randomize in that case? Discutate.
            }
        }

        private void btnRandomize_Click(object sender, EventArgs e)
        {
            if (!chkNeverAskAgain.Checked)
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            else
                this.DialogResult = System.Windows.Forms.DialogResult.Abort;
        }
    }
}
