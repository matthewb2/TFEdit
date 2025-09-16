using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace TextFileEdit
{
    public partial class TestStartUp : Form
    {
        bool bTextChanged;
        
        public TestStartUp()
        {
            InitializeComponent();

            lstBracketMatchingStyle.Items.Add(TextFileEdit.Document.BracketMatchingStyle.Before);
            lstBracketMatchingStyle.Items.Add(TextFileEdit.Document.BracketMatchingStyle.After);

            lstIndentStyle.Items.Add(TextFileEdit.Document.IndentStyle.None);
            lstIndentStyle.Items.Add(TextFileEdit.Document.IndentStyle.Auto);
            lstIndentStyle.Items.Add(TextFileEdit.Document.IndentStyle.Smart);

            lstLineViewerStyle.Items.Add(TextFileEdit.Document.LineViewerStyle.None);
            lstLineViewerStyle.Items.Add(TextFileEdit.Document.LineViewerStyle.FullRow);

            chkAllowCaretBeyondEOL.Checked = txtTFE.AllowCaretBeyondEOL;

            lstBracketMatchingStyle.SelectedIndex=lstBracketMatchingStyle.FindString(txtTFE.BracketMatchingStyle.ToString());

            chkConvertTabToSpaces.Checked = txtTFE.ConvertTabsToSpaces;
            chkHideMouseCursor.Checked = txtTFE.HideMouseCursor;
            chkIsIconBarVisible.Checked = txtTFE.IsIconBarVisible;
            chkShowInvalidLines.Checked = txtTFE.ShowInvalidLines;
            chkShowLineNumbers.Checked = txtTFE.ShowLineNumbers;
            chkShowVRuler.Checked = txtTFE.ShowVRuler;
            chkShowHRuler.Checked = txtTFE.ShowHRuler;
            chkShowEOLMarkers.Checked = txtTFE.ShowEOLMarkers;
            chkShowTabs.Checked = txtTFE.ShowTabs;
            lstTextRenderingHint.SelectedIndex = (int) txtTFE.TextRenderingHint;
            chkShowSpaces.Checked = txtTFE.ShowSpaces;
            numTabIndent.Value = txtTFE.TabIndent;
            numVRow.Value = txtTFE.VRulerRow;

            bTextChanged = false;
            this.txtTFE.Document.DocumentChanged += new TextFileEdit.Document.DocumentEventHandler(txtTFEDocument_DocumentTextChanged);
        }

        private void chkAllowCaretBeyondEOL_CheckedChanged(object sender, EventArgs e)
        {
            if(txtTFE.AllowCaretBeyondEOL != chkAllowCaretBeyondEOL.Checked)
                txtTFE.AllowCaretBeyondEOL = chkAllowCaretBeyondEOL.Checked;
        }

        private void lstBracketMatchingStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtTFE.BracketMatchingStyle = (TextFileEdit.Document.BracketMatchingStyle) lstBracketMatchingStyle.SelectedItem;
        }

        private void chkConvertTabToSpaces_CheckedChanged(object sender, EventArgs e)
        {
            if(txtTFE.ConvertTabsToSpaces != chkConvertTabToSpaces.Checked)
             txtTFE.ConvertTabsToSpaces = chkConvertTabToSpaces.Checked;
        }

        private void chkHideMouseCursor_CheckedChanged(object sender, EventArgs e)
        {
            if (txtTFE.HideMouseCursor != chkHideMouseCursor.Checked)
                txtTFE.HideMouseCursor = chkHideMouseCursor.Checked;
        }

        private void chkIsIconBarVisible_CheckedChanged(object sender, EventArgs e)
        {
            if (txtTFE.IsIconBarVisible != chkIsIconBarVisible.Checked)
                txtTFE.IsIconBarVisible = chkIsIconBarVisible.Checked;
        }

        private void chkShowInvalidLines_CheckedChanged(object sender, EventArgs e)
        {
            if (txtTFE.ShowInvalidLines != chkShowInvalidLines.Checked)
                txtTFE.ShowInvalidLines = chkShowInvalidLines.Checked;
        }

        private void chkShowLineNumbers_CheckedChanged(object sender, EventArgs e)
        {
            if (txtTFE.ShowLineNumbers != chkShowLineNumbers.Checked)
                txtTFE.ShowLineNumbers = chkShowLineNumbers.Checked;
        }

        private void chkShowVRuler_CheckedChanged(object sender, EventArgs e)
        {
            if (txtTFE.ShowVRuler != chkShowVRuler.Checked)
                txtTFE.ShowVRuler = chkShowVRuler.Checked;
        }

        private void chkShowHRuler_CheckedChanged(object sender, EventArgs e)
        {
            if (txtTFE.ShowHRuler != chkShowHRuler.Checked)
                txtTFE.ShowHRuler = chkShowHRuler.Checked;
        }

        private void chkShowEOLMarkers_CheckedChanged(object sender, EventArgs e)
        {
            if (txtTFE.ShowEOLMarkers != chkShowEOLMarkers.Checked)
                txtTFE.ShowEOLMarkers = chkShowEOLMarkers.Checked;
        }

        private void chkShowTabs_CheckedChanged(object sender, EventArgs e)
        {
            if (txtTFE.ShowTabs != chkShowTabs.Checked)
                txtTFE.ShowTabs = chkShowTabs.Checked;
        }

        private void chkShowSpaces_CheckedChanged(object sender, EventArgs e)
        {
            if (txtTFE.ShowSpaces != chkShowSpaces.Checked)
                txtTFE.ShowSpaces = chkShowSpaces.Checked;
        }

        private void numTabIndent_ValueChanged(object sender, EventArgs e)
        {
            if (txtTFE.TabIndent != numTabIndent.Value)
                txtTFE.TabIndent = (int) numTabIndent.Value;
        }

        private void numVRow_ValueChanged(object sender, EventArgs e)
        {
            if (txtTFE.VRulerRow != numVRow.Value)
                txtTFE.VRulerRow = (int) numVRow.Value;
        }

        private void lstIndentStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtTFE.IndentStyle = (TextFileEdit.Document.IndentStyle)lstIndentStyle.SelectedItem;

        }

        private void lstLineViewerStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtTFE.LineViewerStyle = (TextFileEdit.Document.LineViewerStyle)lstLineViewerStyle.SelectedItem;

        }

        private void txtTFE_Load(object sender, EventArgs e)
        {
            
        }

        void txtTFE_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            
        }

        private void txtTFEDocument_DocumentTextChanged(object sender, EventArgs e)
        {
            bTextChanged = true;
        }

        private void TFETestStartUp_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!CheckForExitAllowed())
                e.Cancel = true;
        }

        private bool CheckForExitAllowed()
        {
            bool ret=false;
            DialogResult res = DialogResult.Yes;

            if (bTextChanged)
                res = MessageBox.Show("Document changes not saved, OK to exit?", "Exit without saving?", MessageBoxButtons.YesNo);
            
            if (res == DialogResult.Yes)
                ret = true;

            return ret;
        }

        private void SetTitle()
        {
            this.Text = "Text File Edit";
            if (txtTFE.FileName != null && txtTFE.FileName.Length > 0)
                this.Text+=": "+txtTFE.FileName;
        }

        #region Menu Handling
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult res = DialogResult.Yes;

            if (bTextChanged)
                res = MessageBox.Show("Document changes not saved, OK to proceed?", "New file without saving?", MessageBoxButtons.YesNo);

            if (res == DialogResult.Yes)
            {
                //New document
                txtTFE.FileName = null;
                SetTitle();
                txtTFE.ResetText();
                bTextChanged = false;
                saveToolStripMenuItem.Enabled = false;
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult res=DialogResult.Yes;

            if (bTextChanged)
                res = MessageBox.Show("Document changes not saved, OK to proceed?", "Open another file without saving?", MessageBoxButtons.YesNo);

            if (res == DialogResult.Yes)
            {
                // Show the open dialog
                openFileDialog1.FileName = txtTFE.FileName;
                if (openFileDialog1.ShowDialog() == DialogResult.OK) // Test result.
                {
                    txtTFE.LoadFile(openFileDialog1.FileName);
                    SetTitle();
                    bTextChanged = false;
                    saveToolStripMenuItem.Enabled = true;
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CheckForExitAllowed())
            {
                //Same check done by Form closing so pretend everthing ready to close
                bTextChanged = false;
                Application.Exit();
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtTFE.SaveFile(txtTFE.FileName);
            bTextChanged = false;
        }
        
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Show the save dialog and get result.
            saveFileDialog1.FileName = txtTFE.FileName;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtTFE.SaveFile(saveFileDialog1.FileName);
                SetTitle();
                bTextChanged = false;
                saveToolStripMenuItem.Enabled = true;
            }
        }
        #endregion
    }
}