namespace TextFileEdit
{
    partial class TestStartUp
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TestStartUp));
            this.chkAllowCaretBeyondEOL = new System.Windows.Forms.CheckBox();
            this.chkConvertTabToSpaces = new System.Windows.Forms.CheckBox();
            this.lstBracketMatchingStyle = new System.Windows.Forms.ListBox();
            this.lblBracketMatchingStyle = new System.Windows.Forms.Label();
            this.chkEnableFolding = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chkHideMouseCursor = new System.Windows.Forms.CheckBox();
            this.chkIsIconBarVisible = new System.Windows.Forms.CheckBox();
            this.chkShowInvalidLines = new System.Windows.Forms.CheckBox();
            this.chkShowLineNumbers = new System.Windows.Forms.CheckBox();
            this.chkShowVRuler = new System.Windows.Forms.CheckBox();
            this.chkShowHRuler = new System.Windows.Forms.CheckBox();
            this.chkShowEOLMarkers = new System.Windows.Forms.CheckBox();
            this.chkShowTabs = new System.Windows.Forms.CheckBox();
            this.chkShowSpaces = new System.Windows.Forms.CheckBox();
            this.numTabIndent = new System.Windows.Forms.NumericUpDown();
            this.numVRow = new System.Windows.Forms.NumericUpDown();
            this.lblTabIndent = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lstIndentStyle = new System.Windows.Forms.ListBox();
            this.label5 = new System.Windows.Forms.Label();
            this.lstLineViewerStyle = new System.Windows.Forms.ListBox();
            this.mnuTFE = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lblTextRenderingHint = new System.Windows.Forms.Label();
            this.lstTextRenderingHint = new System.Windows.Forms.ListBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.txtTFE = new TextFileEdit.TextEditorControl();
            ((System.ComponentModel.ISupportInitialize)(this.numTabIndent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numVRow)).BeginInit();
            this.mnuTFE.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkAllowCaretBeyondEOL
            // 
            this.chkAllowCaretBeyondEOL.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkAllowCaretBeyondEOL.AutoSize = true;
            this.chkAllowCaretBeyondEOL.Location = new System.Drawing.Point(12, 276);
            this.chkAllowCaretBeyondEOL.Name = "chkAllowCaretBeyondEOL";
            this.chkAllowCaretBeyondEOL.Size = new System.Drawing.Size(142, 17);
            this.chkAllowCaretBeyondEOL.TabIndex = 1;
            this.chkAllowCaretBeyondEOL.Text = "Allow Caret Beyond EOL";
            this.chkAllowCaretBeyondEOL.UseVisualStyleBackColor = true;
            this.chkAllowCaretBeyondEOL.CheckedChanged += new System.EventHandler(this.chkAllowCaretBeyondEOL_CheckedChanged);
            // 
            // chkConvertTabToSpaces
            // 
            this.chkConvertTabToSpaces.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkConvertTabToSpaces.AutoSize = true;
            this.chkConvertTabToSpaces.Location = new System.Drawing.Point(12, 351);
            this.chkConvertTabToSpaces.Name = "chkConvertTabToSpaces";
            this.chkConvertTabToSpaces.Size = new System.Drawing.Size(141, 17);
            this.chkConvertTabToSpaces.TabIndex = 2;
            this.chkConvertTabToSpaces.Text = "Convert Tabs to Spaces";
            this.chkConvertTabToSpaces.UseVisualStyleBackColor = true;
            this.chkConvertTabToSpaces.CheckedChanged += new System.EventHandler(this.chkConvertTabToSpaces_CheckedChanged);
            // 
            // lstBracketMatchingStyle
            // 
            this.lstBracketMatchingStyle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lstBracketMatchingStyle.FormattingEnabled = true;
            this.lstBracketMatchingStyle.Location = new System.Drawing.Point(15, 315);
            this.lstBracketMatchingStyle.Name = "lstBracketMatchingStyle";
            this.lstBracketMatchingStyle.Size = new System.Drawing.Size(93, 30);
            this.lstBracketMatchingStyle.TabIndex = 3;
            this.lstBracketMatchingStyle.SelectedIndexChanged += new System.EventHandler(this.lstBracketMatchingStyle_SelectedIndexChanged);
            // 
            // lblBracketMatchingStyle
            // 
            this.lblBracketMatchingStyle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblBracketMatchingStyle.AutoSize = true;
            this.lblBracketMatchingStyle.Location = new System.Drawing.Point(12, 299);
            this.lblBracketMatchingStyle.Name = "lblBracketMatchingStyle";
            this.lblBracketMatchingStyle.Size = new System.Drawing.Size(117, 13);
            this.lblBracketMatchingStyle.TabIndex = 4;
            this.lblBracketMatchingStyle.Text = "Bracket Matching Style";
            // 
            // chkEnableFolding
            // 
            this.chkEnableFolding.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkEnableFolding.AutoSize = true;
            this.chkEnableFolding.Location = new System.Drawing.Point(12, 374);
            this.chkEnableFolding.Name = "chkEnableFolding";
            this.chkEnableFolding.Size = new System.Drawing.Size(96, 17);
            this.chkEnableFolding.TabIndex = 6;
            this.chkEnableFolding.Text = "Enable Folding";
            this.chkEnableFolding.UseVisualStyleBackColor = true;
            this.chkEnableFolding.Visible = false;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 394);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(133, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "(requires a folding strategy)";
            this.label1.Visible = false;
            // 
            // chkHideMouseCursor
            // 
            this.chkHideMouseCursor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkHideMouseCursor.AutoSize = true;
            this.chkHideMouseCursor.Location = new System.Drawing.Point(12, 413);
            this.chkHideMouseCursor.Name = "chkHideMouseCursor";
            this.chkHideMouseCursor.Size = new System.Drawing.Size(116, 17);
            this.chkHideMouseCursor.TabIndex = 8;
            this.chkHideMouseCursor.Text = "Hide Mouse Cursor";
            this.chkHideMouseCursor.UseVisualStyleBackColor = true;
            this.chkHideMouseCursor.CheckedChanged += new System.EventHandler(this.chkHideMouseCursor_CheckedChanged);
            // 
            // chkIsIconBarVisible
            // 
            this.chkIsIconBarVisible.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkIsIconBarVisible.AutoSize = true;
            this.chkIsIconBarVisible.Location = new System.Drawing.Point(179, 277);
            this.chkIsIconBarVisible.Name = "chkIsIconBarVisible";
            this.chkIsIconBarVisible.Size = new System.Drawing.Size(99, 17);
            this.chkIsIconBarVisible.TabIndex = 9;
            this.chkIsIconBarVisible.Text = "Icon Bar Visible";
            this.chkIsIconBarVisible.UseVisualStyleBackColor = true;
            this.chkIsIconBarVisible.CheckedChanged += new System.EventHandler(this.chkIsIconBarVisible_CheckedChanged);
            // 
            // chkShowInvalidLines
            // 
            this.chkShowInvalidLines.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkShowInvalidLines.AutoSize = true;
            this.chkShowInvalidLines.Location = new System.Drawing.Point(179, 300);
            this.chkShowInvalidLines.Name = "chkShowInvalidLines";
            this.chkShowInvalidLines.Size = new System.Drawing.Size(115, 17);
            this.chkShowInvalidLines.TabIndex = 10;
            this.chkShowInvalidLines.Text = "Show Invalid Lines";
            this.chkShowInvalidLines.UseVisualStyleBackColor = true;
            this.chkShowInvalidLines.CheckedChanged += new System.EventHandler(this.chkShowInvalidLines_CheckedChanged);
            // 
            // chkShowLineNumbers
            // 
            this.chkShowLineNumbers.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkShowLineNumbers.AutoSize = true;
            this.chkShowLineNumbers.Location = new System.Drawing.Point(179, 323);
            this.chkShowLineNumbers.Name = "chkShowLineNumbers";
            this.chkShowLineNumbers.Size = new System.Drawing.Size(121, 17);
            this.chkShowLineNumbers.TabIndex = 11;
            this.chkShowLineNumbers.Text = "Show Line Numbers";
            this.chkShowLineNumbers.UseVisualStyleBackColor = true;
            this.chkShowLineNumbers.CheckedChanged += new System.EventHandler(this.chkShowLineNumbers_CheckedChanged);
            // 
            // chkShowVRuler
            // 
            this.chkShowVRuler.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkShowVRuler.AutoSize = true;
            this.chkShowVRuler.Location = new System.Drawing.Point(179, 346);
            this.chkShowVRuler.Name = "chkShowVRuler";
            this.chkShowVRuler.Size = new System.Drawing.Size(119, 17);
            this.chkShowVRuler.TabIndex = 12;
            this.chkShowVRuler.Text = "Show Vertical Ruler";
            this.chkShowVRuler.UseVisualStyleBackColor = true;
            this.chkShowVRuler.CheckedChanged += new System.EventHandler(this.chkShowVRuler_CheckedChanged);
            // 
            // chkShowHRuler
            // 
            this.chkShowHRuler.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkShowHRuler.AutoSize = true;
            this.chkShowHRuler.Location = new System.Drawing.Point(179, 369);
            this.chkShowHRuler.Name = "chkShowHRuler";
            this.chkShowHRuler.Size = new System.Drawing.Size(131, 17);
            this.chkShowHRuler.TabIndex = 13;
            this.chkShowHRuler.Text = "Show Horizontal Ruler";
            this.chkShowHRuler.UseVisualStyleBackColor = true;
            this.chkShowHRuler.CheckedChanged += new System.EventHandler(this.chkShowHRuler_CheckedChanged);
            // 
            // chkShowEOLMarkers
            // 
            this.chkShowEOLMarkers.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkShowEOLMarkers.AutoSize = true;
            this.chkShowEOLMarkers.Location = new System.Drawing.Point(179, 391);
            this.chkShowEOLMarkers.Name = "chkShowEOLMarkers";
            this.chkShowEOLMarkers.Size = new System.Drawing.Size(113, 17);
            this.chkShowEOLMarkers.TabIndex = 14;
            this.chkShowEOLMarkers.Text = "Show EOL Marker";
            this.chkShowEOLMarkers.UseVisualStyleBackColor = true;
            this.chkShowEOLMarkers.CheckedChanged += new System.EventHandler(this.chkShowEOLMarkers_CheckedChanged);
            // 
            // chkShowTabs
            // 
            this.chkShowTabs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkShowTabs.AutoSize = true;
            this.chkShowTabs.Location = new System.Drawing.Point(179, 413);
            this.chkShowTabs.Name = "chkShowTabs";
            this.chkShowTabs.Size = new System.Drawing.Size(80, 17);
            this.chkShowTabs.TabIndex = 15;
            this.chkShowTabs.Text = "Show Tabs";
            this.chkShowTabs.UseVisualStyleBackColor = true;
            this.chkShowTabs.CheckedChanged += new System.EventHandler(this.chkShowTabs_CheckedChanged);
            // 
            // chkShowSpaces
            // 
            this.chkShowSpaces.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkShowSpaces.AutoSize = true;
            this.chkShowSpaces.Location = new System.Drawing.Point(332, 367);
            this.chkShowSpaces.Name = "chkShowSpaces";
            this.chkShowSpaces.Size = new System.Drawing.Size(92, 17);
            this.chkShowSpaces.TabIndex = 17;
            this.chkShowSpaces.Text = "Show Spaces";
            this.chkShowSpaces.UseVisualStyleBackColor = true;
            this.chkShowSpaces.CheckedChanged += new System.EventHandler(this.chkShowSpaces_CheckedChanged);
            // 
            // numTabIndent
            // 
            this.numTabIndent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.numTabIndent.Location = new System.Drawing.Point(333, 410);
            this.numTabIndent.Name = "numTabIndent";
            this.numTabIndent.Size = new System.Drawing.Size(138, 20);
            this.numTabIndent.TabIndex = 18;
            this.numTabIndent.ValueChanged += new System.EventHandler(this.numTabIndent_ValueChanged);
            // 
            // numVRow
            // 
            this.numVRow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.numVRow.Location = new System.Drawing.Point(487, 292);
            this.numVRow.Name = "numVRow";
            this.numVRow.Size = new System.Drawing.Size(120, 20);
            this.numVRow.TabIndex = 19;
            this.numVRow.ValueChanged += new System.EventHandler(this.numVRow_ValueChanged);
            // 
            // lblTabIndent
            // 
            this.lblTabIndent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblTabIndent.AutoSize = true;
            this.lblTabIndent.Location = new System.Drawing.Point(341, 395);
            this.lblTabIndent.Name = "lblTabIndent";
            this.lblTabIndent.Size = new System.Drawing.Size(59, 13);
            this.lblTabIndent.TabIndex = 20;
            this.lblTabIndent.Text = "Tab Indent";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(484, 276);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(95, 13);
            this.label3.TabIndex = 21;
            this.label3.Text = "Vertical Ruler Row";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(484, 322);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 13);
            this.label4.TabIndex = 23;
            this.label4.Text = "Indent Style";
            // 
            // lstIndentStyle
            // 
            this.lstIndentStyle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lstIndentStyle.FormattingEnabled = true;
            this.lstIndentStyle.Location = new System.Drawing.Point(487, 338);
            this.lstIndentStyle.Name = "lstIndentStyle";
            this.lstIndentStyle.Size = new System.Drawing.Size(93, 43);
            this.lstIndentStyle.TabIndex = 22;
            this.lstIndentStyle.SelectedIndexChanged += new System.EventHandler(this.lstIndentStyle_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(484, 390);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(79, 13);
            this.label5.TabIndex = 25;
            this.label5.Text = "Line View Style";
            // 
            // lstLineViewerStyle
            // 
            this.lstLineViewerStyle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lstLineViewerStyle.FormattingEnabled = true;
            this.lstLineViewerStyle.Location = new System.Drawing.Point(487, 406);
            this.lstLineViewerStyle.Name = "lstLineViewerStyle";
            this.lstLineViewerStyle.Size = new System.Drawing.Size(93, 30);
            this.lstLineViewerStyle.TabIndex = 24;
            this.lstLineViewerStyle.SelectedIndexChanged += new System.EventHandler(this.lstLineViewerStyle_SelectedIndexChanged);
            // 
            // mnuTFE
            // 
            this.mnuTFE.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.mnuTFE.Location = new System.Drawing.Point(0, 0);
            this.mnuTFE.Name = "mnuTFE";
            this.mnuTFE.Size = new System.Drawing.Size(722, 24);
            this.mnuTFE.TabIndex = 26;
            this.mnuTFE.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.newToolStripMenuItem.Text = "&New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.openToolStripMenuItem.Text = "&Open...";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Enabled = false;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.saveToolStripMenuItem.Text = "&Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.saveAsToolStripMenuItem.Text = "Save &As...";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(120, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // lblTextRenderingHint
            // 
            this.lblTextRenderingHint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblTextRenderingHint.AutoSize = true;
            this.lblTextRenderingHint.Location = new System.Drawing.Point(329, 279);
            this.lblTextRenderingHint.Name = "lblTextRenderingHint";
            this.lblTextRenderingHint.Size = new System.Drawing.Size(102, 13);
            this.lblTextRenderingHint.TabIndex = 27;
            this.lblTextRenderingHint.Text = "Text Rendering Hint";
            // 
            // lstTextRenderingHint
            // 
            this.lstTextRenderingHint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lstTextRenderingHint.FormattingEnabled = true;
            this.lstTextRenderingHint.Items.AddRange(new object[] {
            "SystemDefault",
            "SingleBitPerPixelGridFit",
            "SingleBitPerPixel",
            "AntiAliasGridFit",
            "AntiAlias",
            "ClearTypeGridFit"});
            this.lstTextRenderingHint.Location = new System.Drawing.Point(332, 299);
            this.lstTextRenderingHint.Name = "lstTextRenderingHint";
            this.lstTextRenderingHint.Size = new System.Drawing.Size(138, 56);
            this.lstTextRenderingHint.TabIndex = 28;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // txtTFE
            // 
            this.txtTFE.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTFE.BackColor = System.Drawing.SystemColors.Control;
            this.txtTFE.IsReadOnly = false;
            this.txtTFE.Location = new System.Drawing.Point(0, 24);
            this.txtTFE.Name = "txtTFE";
            this.txtTFE.ShowEOLMarkers = true;
            this.txtTFE.ShowLineNumbers = false;
            this.txtTFE.ShowSpaces = true;
            this.txtTFE.ShowTabs = true;
            this.txtTFE.Size = new System.Drawing.Size(722, 244);
            this.txtTFE.TabIndex = 0;
            this.txtTFE.Load += new System.EventHandler(this.txtTFE_Load);
            this.txtTFE.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtTFE_KeyPress);
            // 
            // TestStartUp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(722, 447);
            this.Controls.Add(this.lstTextRenderingHint);
            this.Controls.Add(this.lblTextRenderingHint);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lstLineViewerStyle);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lstIndentStyle);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblTabIndent);
            this.Controls.Add(this.numVRow);
            this.Controls.Add(this.numTabIndent);
            this.Controls.Add(this.chkShowSpaces);
            this.Controls.Add(this.chkShowTabs);
            this.Controls.Add(this.chkShowEOLMarkers);
            this.Controls.Add(this.chkShowHRuler);
            this.Controls.Add(this.chkShowVRuler);
            this.Controls.Add(this.chkShowLineNumbers);
            this.Controls.Add(this.chkShowInvalidLines);
            this.Controls.Add(this.chkIsIconBarVisible);
            this.Controls.Add(this.chkHideMouseCursor);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chkEnableFolding);
            this.Controls.Add(this.lblBracketMatchingStyle);
            this.Controls.Add(this.lstBracketMatchingStyle);
            this.Controls.Add(this.chkConvertTabToSpaces);
            this.Controls.Add(this.chkAllowCaretBeyondEOL);
            this.Controls.Add(this.txtTFE);
            this.Controls.Add(this.mnuTFE);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.mnuTFE;
            this.Name = "TestStartUp";
            this.Text = "Text File Edit";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TFETestStartUp_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.numTabIndent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numVRow)).EndInit();
            this.mnuTFE.ResumeLayout(false);
            this.mnuTFE.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private TextFileEdit.TextEditorControl txtTFE;
        private System.Windows.Forms.CheckBox chkAllowCaretBeyondEOL;
        private System.Windows.Forms.CheckBox chkConvertTabToSpaces;
        private System.Windows.Forms.ListBox lstBracketMatchingStyle;
        private System.Windows.Forms.Label lblBracketMatchingStyle;
        private System.Windows.Forms.CheckBox chkEnableFolding;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkHideMouseCursor;
        private System.Windows.Forms.CheckBox chkIsIconBarVisible;
        private System.Windows.Forms.CheckBox chkShowInvalidLines;
        private System.Windows.Forms.CheckBox chkShowLineNumbers;
        private System.Windows.Forms.CheckBox chkShowVRuler;
        private System.Windows.Forms.CheckBox chkShowHRuler;
        private System.Windows.Forms.CheckBox chkShowEOLMarkers;
        private System.Windows.Forms.CheckBox chkShowTabs;
        private System.Windows.Forms.CheckBox chkShowSpaces;
        private System.Windows.Forms.NumericUpDown numTabIndent;
        private System.Windows.Forms.NumericUpDown numVRow;
        private System.Windows.Forms.Label lblTabIndent;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ListBox lstIndentStyle;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ListBox lstLineViewerStyle;
        private System.Windows.Forms.MenuStrip mnuTFE;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Label lblTextRenderingHint;
        private System.Windows.Forms.ListBox lstTextRenderingHint;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;

    }
}

