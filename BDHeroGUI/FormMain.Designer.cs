using DotNetUtils.Controls;

namespace BDHeroGUI
{
    partial class FormMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxStatus = new System.Windows.Forms.TextBox();
            this.buttonScan = new System.Windows.Forms.Button();
            this.buttonCancelScan = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonCancelConvert = new System.Windows.Forms.Button();
            this.buttonConvert = new System.Windows.Forms.Button();
            this.panelRoot = new System.Windows.Forms.Panel();
            this.menuStripTop = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openBDROMFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openDiscToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.noBlurayDiscsFoundToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.playlistsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showAllPlaylistsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editFilterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tracksToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showAllTracksToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.filterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pluginsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.remuxerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.aboutBDHeroToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.submitABugReportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.suggestAFeatureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.documentationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.homepageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkForUpdatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.textBoxInput = new DotNetUtils.Controls.FileTextBox();
            this.splitContainerMain = new DotNetUtils.Controls.SplitContainerWithDivider();
            this.splitContainerTop = new DotNetUtils.Controls.SplitContainerWithDivider();
            this.textBoxOutput = new DotNetUtils.Controls.FileTextBox();
            this.progressBar = new DotNetUtils.Controls.ProgressBar2();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.searchForMetadataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.playlistListView = new BDHeroGUI.Components.PlaylistListView();
            this.mediaPanel = new BDHeroGUI.Components.MediaPanel();
            this.tracksPanel = new BDHeroGUI.Components.TracksPanel();
            this.panelRoot.SuspendLayout();
            this.menuStripTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerTop)).BeginInit();
            this.splitContainerTop.Panel1.SuspendLayout();
            this.splitContainerTop.Panel2.SuspendLayout();
            this.splitContainerTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Input BD-ROM:";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 477);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Status:";
            // 
            // textBoxStatus
            // 
            this.textBoxStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxStatus.Location = new System.Drawing.Point(3, 493);
            this.textBoxStatus.Multiline = true;
            this.textBoxStatus.Name = "textBoxStatus";
            this.textBoxStatus.ReadOnly = true;
            this.textBoxStatus.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxStatus.Size = new System.Drawing.Size(1054, 37);
            this.textBoxStatus.TabIndex = 7;
            // 
            // buttonScan
            // 
            this.buttonScan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonScan.Location = new System.Drawing.Point(901, 3);
            this.buttonScan.Name = "buttonScan";
            this.buttonScan.Size = new System.Drawing.Size(75, 23);
            this.buttonScan.TabIndex = 1;
            this.buttonScan.Text = "Scan";
            this.buttonScan.UseVisualStyleBackColor = true;
            this.buttonScan.Click += new System.EventHandler(this.buttonScan_Click);
            // 
            // buttonCancelScan
            // 
            this.buttonCancelScan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancelScan.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancelScan.Location = new System.Drawing.Point(982, 3);
            this.buttonCancelScan.Name = "buttonCancelScan";
            this.buttonCancelScan.Size = new System.Drawing.Size(75, 23);
            this.buttonCancelScan.TabIndex = 2;
            this.buttonCancelScan.Text = "Cancel";
            this.buttonCancelScan.UseVisualStyleBackColor = true;
            this.buttonCancelScan.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 455);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Output MKV file:";
            // 
            // buttonCancelConvert
            // 
            this.buttonCancelConvert.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancelConvert.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancelConvert.Location = new System.Drawing.Point(982, 450);
            this.buttonCancelConvert.Name = "buttonCancelConvert";
            this.buttonCancelConvert.Size = new System.Drawing.Size(75, 23);
            this.buttonCancelConvert.TabIndex = 6;
            this.buttonCancelConvert.Text = "Cancel";
            this.buttonCancelConvert.UseVisualStyleBackColor = true;
            this.buttonCancelConvert.Click += new System.EventHandler(this.buttonCancelConvert_Click);
            // 
            // buttonConvert
            // 
            this.buttonConvert.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonConvert.Location = new System.Drawing.Point(901, 450);
            this.buttonConvert.Name = "buttonConvert";
            this.buttonConvert.Size = new System.Drawing.Size(75, 23);
            this.buttonConvert.TabIndex = 5;
            this.buttonConvert.Text = "Convert";
            this.buttonConvert.UseVisualStyleBackColor = true;
            this.buttonConvert.Click += new System.EventHandler(this.buttonConvert_Click);
            // 
            // panelRoot
            // 
            this.panelRoot.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelRoot.Controls.Add(this.textBoxInput);
            this.panelRoot.Controls.Add(this.splitContainerMain);
            this.panelRoot.Controls.Add(this.label1);
            this.panelRoot.Controls.Add(this.buttonCancelConvert);
            this.panelRoot.Controls.Add(this.label3);
            this.panelRoot.Controls.Add(this.buttonConvert);
            this.panelRoot.Controls.Add(this.textBoxStatus);
            this.panelRoot.Controls.Add(this.textBoxOutput);
            this.panelRoot.Controls.Add(this.buttonScan);
            this.panelRoot.Controls.Add(this.label2);
            this.panelRoot.Controls.Add(this.progressBar);
            this.panelRoot.Controls.Add(this.buttonCancelScan);
            this.panelRoot.Location = new System.Drawing.Point(12, 27);
            this.panelRoot.Name = "panelRoot";
            this.panelRoot.Size = new System.Drawing.Size(1060, 562);
            this.panelRoot.TabIndex = 13;
            // 
            // menuStripTop
            // 
            this.menuStripTop.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.pluginsToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStripTop.Location = new System.Drawing.Point(0, 0);
            this.menuStripTop.Name = "menuStripTop";
            this.menuStripTop.Size = new System.Drawing.Size(1084, 24);
            this.menuStripTop.TabIndex = 14;
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openBDROMFolderToolStripMenuItem,
            this.openDiscToolStripMenuItem,
            this.toolStripMenuItem5,
            this.searchForMetadataToolStripMenuItem,
            this.toolStripMenuItem4,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.playlistsToolStripMenuItem,
            this.tracksToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.remuxerToolStripMenuItem,
            this.toolStripMenuItem1,
            this.optionsToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.homepageToolStripMenuItem,
            this.documentationToolStripMenuItem,
            this.submitABugReportToolStripMenuItem,
            this.suggestAFeatureToolStripMenuItem,
            this.toolStripMenuItem3,
            this.checkForUpdatesToolStripMenuItem,
            this.toolStripMenuItem2,
            this.aboutBDHeroToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // openBDROMFolderToolStripMenuItem
            // 
            this.openBDROMFolderToolStripMenuItem.Name = "openBDROMFolderToolStripMenuItem";
            this.openBDROMFolderToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openBDROMFolderToolStripMenuItem.Size = new System.Drawing.Size(241, 22);
            this.openBDROMFolderToolStripMenuItem.Text = "Open BD-ROM Folder...";
            this.openBDROMFolderToolStripMenuItem.Click += new System.EventHandler(this.openBDROMFolderToolStripMenuItem_Click);
            // 
            // openDiscToolStripMenuItem
            // 
            this.openDiscToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.noBlurayDiscsFoundToolStripMenuItem});
            this.openDiscToolStripMenuItem.Name = "openDiscToolStripMenuItem";
            this.openDiscToolStripMenuItem.Size = new System.Drawing.Size(241, 22);
            this.openDiscToolStripMenuItem.Text = "Open Disc";
            // 
            // noBlurayDiscsFoundToolStripMenuItem
            // 
            this.noBlurayDiscsFoundToolStripMenuItem.Enabled = false;
            this.noBlurayDiscsFoundToolStripMenuItem.Name = "noBlurayDiscsFoundToolStripMenuItem";
            this.noBlurayDiscsFoundToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.noBlurayDiscsFoundToolStripMenuItem.Text = "No Blu-ray Discs found";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(241, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // playlistsToolStripMenuItem
            // 
            this.playlistsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showAllPlaylistsToolStripMenuItem,
            this.editFilterToolStripMenuItem});
            this.playlistsToolStripMenuItem.Name = "playlistsToolStripMenuItem";
            this.playlistsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.playlistsToolStripMenuItem.Text = "&Playlists";
            // 
            // showAllPlaylistsToolStripMenuItem
            // 
            this.showAllPlaylistsToolStripMenuItem.Name = "showAllPlaylistsToolStripMenuItem";
            this.showAllPlaylistsToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.showAllPlaylistsToolStripMenuItem.Text = "Show &All Playlists";
            this.showAllPlaylistsToolStripMenuItem.Click += new System.EventHandler(this.showAllPlaylistsToolStripMenuItem_Click);
            // 
            // editFilterToolStripMenuItem
            // 
            this.editFilterToolStripMenuItem.Name = "editFilterToolStripMenuItem";
            this.editFilterToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.editFilterToolStripMenuItem.Text = "&Filter...";
            this.editFilterToolStripMenuItem.Click += new System.EventHandler(this.editFilterToolStripMenuItem_Click);
            // 
            // tracksToolStripMenuItem
            // 
            this.tracksToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showAllTracksToolStripMenuItem,
            this.filterToolStripMenuItem});
            this.tracksToolStripMenuItem.Name = "tracksToolStripMenuItem";
            this.tracksToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.tracksToolStripMenuItem.Text = "&Tracks";
            // 
            // showAllTracksToolStripMenuItem
            // 
            this.showAllTracksToolStripMenuItem.CheckOnClick = true;
            this.showAllTracksToolStripMenuItem.Enabled = false;
            this.showAllTracksToolStripMenuItem.Name = "showAllTracksToolStripMenuItem";
            this.showAllTracksToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.showAllTracksToolStripMenuItem.Text = "Show &All Tracks";
            // 
            // filterToolStripMenuItem
            // 
            this.filterToolStripMenuItem.Enabled = false;
            this.filterToolStripMenuItem.Name = "filterToolStripMenuItem";
            this.filterToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.filterToolStripMenuItem.Text = "&Filter...";
            // 
            // pluginsToolStripMenuItem
            // 
            this.pluginsToolStripMenuItem.Name = "pluginsToolStripMenuItem";
            this.pluginsToolStripMenuItem.Size = new System.Drawing.Size(58, 20);
            this.pluginsToolStripMenuItem.Text = "Plugins";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Enabled = false;
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.optionsToolStripMenuItem.Text = "&Options...";
            // 
            // remuxerToolStripMenuItem
            // 
            this.remuxerToolStripMenuItem.Enabled = false;
            this.remuxerToolStripMenuItem.Name = "remuxerToolStripMenuItem";
            this.remuxerToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.remuxerToolStripMenuItem.Text = "&Remuxer";
            this.remuxerToolStripMenuItem.ToolTipText = "Launches the Remuxer in a separate window";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(149, 6);
            // 
            // aboutBDHeroToolStripMenuItem
            // 
            this.aboutBDHeroToolStripMenuItem.Name = "aboutBDHeroToolStripMenuItem";
            this.aboutBDHeroToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1;
            this.aboutBDHeroToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.aboutBDHeroToolStripMenuItem.Text = "&About BDHero";
            this.aboutBDHeroToolStripMenuItem.Click += new System.EventHandler(this.aboutBDHeroToolStripMenuItem_Click);
            // 
            // submitABugReportToolStripMenuItem
            // 
            this.submitABugReportToolStripMenuItem.Name = "submitABugReportToolStripMenuItem";
            this.submitABugReportToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.submitABugReportToolStripMenuItem.Text = "Report a &Bug...";
            this.submitABugReportToolStripMenuItem.Click += new System.EventHandler(this.submitABugReportToolStripMenuItem_Click);
            // 
            // suggestAFeatureToolStripMenuItem
            // 
            this.suggestAFeatureToolStripMenuItem.Name = "suggestAFeatureToolStripMenuItem";
            this.suggestAFeatureToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.suggestAFeatureToolStripMenuItem.Text = "Suggest a &Feature...";
            this.suggestAFeatureToolStripMenuItem.Click += new System.EventHandler(this.suggestAFeatureToolStripMenuItem_Click);
            // 
            // documentationToolStripMenuItem
            // 
            this.documentationToolStripMenuItem.Name = "documentationToolStripMenuItem";
            this.documentationToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.documentationToolStripMenuItem.Text = "&Documentation";
            this.documentationToolStripMenuItem.Click += new System.EventHandler(this.documentationToolStripMenuItem_Click);
            // 
            // homepageToolStripMenuItem
            // 
            this.homepageToolStripMenuItem.Name = "homepageToolStripMenuItem";
            this.homepageToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.homepageToolStripMenuItem.Text = "&Homepage";
            this.homepageToolStripMenuItem.Click += new System.EventHandler(this.homepageToolStripMenuItem_Click);
            // 
            // checkForUpdatesToolStripMenuItem
            // 
            this.checkForUpdatesToolStripMenuItem.Enabled = false;
            this.checkForUpdatesToolStripMenuItem.Name = "checkForUpdatesToolStripMenuItem";
            this.checkForUpdatesToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.checkForUpdatesToolStripMenuItem.Text = "Check for Updates";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(173, 6);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(173, 6);
            // 
            // textBoxInput
            // 
            this.textBoxInput.AllowAnyExtension = false;
            this.textBoxInput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxInput.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.textBoxInput.DialogTitle = "Select a BD-ROM folder:";
            this.textBoxInput.DialogType = DotNetUtils.Controls.DialogType.OpenDirectory;
            this.textBoxInput.FileExtensions = null;
            this.textBoxInput.Location = new System.Drawing.Point(93, 3);
            this.textBoxInput.Name = "textBoxInput";
            this.textBoxInput.OverwritePrompt = false;
            this.textBoxInput.SelectedPath = "W:\\BD\\49123204_BLACK_HAWK_DOWN";
            this.textBoxInput.ShowNewFolderButton = false;
            this.textBoxInput.Size = new System.Drawing.Size(802, 24);
            this.textBoxInput.TabIndex = 0;
            this.textBoxInput.SelectedPathChanged += new System.EventHandler(this.textBoxInput_SelectedPathChanged);
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainerMain.Location = new System.Drawing.Point(3, 32);
            this.splitContainerMain.Name = "splitContainerMain";
            this.splitContainerMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.Controls.Add(this.splitContainerTop);
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.tracksPanel);
            this.splitContainerMain.Size = new System.Drawing.Size(1054, 412);
            this.splitContainerMain.SplitterDistance = 118;
            this.splitContainerMain.TabIndex = 3;
            // 
            // splitContainerTop
            // 
            this.splitContainerTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerTop.Location = new System.Drawing.Point(0, 0);
            this.splitContainerTop.Name = "splitContainerTop";
            // 
            // splitContainerTop.Panel1
            // 
            this.splitContainerTop.Panel1.Controls.Add(this.playlistListView);
            // 
            // splitContainerTop.Panel2
            // 
            this.splitContainerTop.Panel2.Controls.Add(this.mediaPanel);
            this.splitContainerTop.Size = new System.Drawing.Size(1054, 118);
            this.splitContainerTop.SplitterDistance = 682;
            this.splitContainerTop.TabIndex = 7;
            // 
            // textBoxOutput
            // 
            this.textBoxOutput.AllowAnyExtension = false;
            this.textBoxOutput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxOutput.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.textBoxOutput.DialogTitle = "Save MKV file:";
            this.textBoxOutput.DialogType = DotNetUtils.Controls.DialogType.SaveFile;
            this.textBoxOutput.FileExtensions = null;
            this.textBoxOutput.Location = new System.Drawing.Point(93, 450);
            this.textBoxOutput.Name = "textBoxOutput";
            this.textBoxOutput.SelectedPath = "W:\\BDHero\\test.mkv";
            this.textBoxOutput.Size = new System.Drawing.Size(802, 24);
            this.textBoxOutput.TabIndex = 4;
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(3, 536);
            this.progressBar.Maximum = 100000;
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(1054, 23);
            this.progressBar.Step = 1;
            this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar.TabIndex = 11;
            this.progressBar.TextOutline = true;
            this.progressBar.TextOutlineColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.progressBar.TextOutlineWidth = 2;
            this.progressBar.UseCustomColors = false;
            this.progressBar.ValuePercent = 0D;
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(238, 6);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(238, 6);
            // 
            // searchForMetadataToolStripMenuItem
            // 
            this.searchForMetadataToolStripMenuItem.Name = "searchForMetadataToolStripMenuItem";
            this.searchForMetadataToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.searchForMetadataToolStripMenuItem.Size = new System.Drawing.Size(241, 22);
            this.searchForMetadataToolStripMenuItem.Text = "Search for Metadata...";
            this.searchForMetadataToolStripMenuItem.Click += new System.EventHandler(this.searchForMetadataToolStripMenuItem_Click);
            // 
            // playlistListView
            // 
            this.playlistListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.playlistListView.Location = new System.Drawing.Point(0, 0);
            this.playlistListView.Name = "playlistListView";
            this.playlistListView.Playlists = null;
            this.playlistListView.SelectedPlaylist = null;
            this.playlistListView.ShowAll = false;
            this.playlistListView.Size = new System.Drawing.Size(682, 118);
            this.playlistListView.TabIndex = 1;
            // 
            // mediaPanel
            // 
            this.mediaPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mediaPanel.Location = new System.Drawing.Point(0, 0);
            this.mediaPanel.Name = "mediaPanel";
            this.mediaPanel.Size = new System.Drawing.Size(368, 118);
            this.mediaPanel.TabIndex = 0;
            // 
            // tracksPanel
            // 
            this.tracksPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tracksPanel.Location = new System.Drawing.Point(0, 0);
            this.tracksPanel.Name = "tracksPanel";
            this.tracksPanel.Playlist = null;
            this.tracksPanel.Size = new System.Drawing.Size(1054, 290);
            this.tracksPanel.TabIndex = 0;
            // 
            // FormMain
            // 
            this.AcceptButton = this.buttonScan;
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancelScan;
            this.ClientSize = new System.Drawing.Size(1084, 601);
            this.Controls.Add(this.panelRoot);
            this.Controls.Add(this.menuStripTop);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStripTop;
            this.Name = "FormMain";
            this.Text = "BDHero GUI";
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.FormMain_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.FormMain_DragEnter);
            this.DragLeave += new System.EventHandler(this.FormMain_DragLeave);
            this.panelRoot.ResumeLayout(false);
            this.panelRoot.PerformLayout();
            this.menuStripTop.ResumeLayout(false);
            this.menuStripTop.PerformLayout();
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
            this.splitContainerMain.ResumeLayout(false);
            this.splitContainerTop.Panel1.ResumeLayout(false);
            this.splitContainerTop.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerTop)).EndInit();
            this.splitContainerTop.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private DotNetUtils.Controls.FileTextBox textBoxInput;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxStatus;
        private System.Windows.Forms.Button buttonScan;
        private DotNetUtils.Controls.ProgressBar2 progressBar;
        private System.Windows.Forms.Button buttonCancelScan;
        private DotNetUtils.Controls.FileTextBox textBoxOutput;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonCancelConvert;
        private System.Windows.Forms.Button buttonConvert;
        private DotNetUtils.Controls.SplitContainerWithDivider splitContainerMain;
        private Components.TracksPanel tracksPanel;
        private SplitContainerWithDivider splitContainerTop;
        private Components.PlaylistListView playlistListView;
        private Components.MediaPanel mediaPanel;
        private System.Windows.Forms.Panel panelRoot;
        private System.Windows.Forms.MenuStrip menuStripTop;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openBDROMFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openDiscToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem noBlurayDiscsFoundToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem playlistsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showAllPlaylistsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editFilterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tracksToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showAllTracksToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem filterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pluginsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem remuxerToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem homepageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem documentationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem submitABugReportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem suggestAFeatureToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem checkForUpdatesToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem aboutBDHeroToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem searchForMetadataToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
    }
}

