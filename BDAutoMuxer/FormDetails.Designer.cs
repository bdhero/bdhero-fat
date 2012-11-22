namespace BDAutoMuxer
{
    partial class FormDetails
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
            this.continueButton = new System.Windows.Forms.Button();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageDisc = new System.Windows.Forms.TabPage();
            this.hiddenTrackLabel = new System.Windows.Forms.Label();
            this.tabPageOutput = new System.Windows.Forms.TabPage();
            this.groupBoxOutput = new System.Windows.Forms.GroupBox();
            this.textBoxOutputFileNamePreview = new System.Windows.Forms.TextBox();
            this.textBoxOutputDirPreview = new System.Windows.Forms.TextBox();
            this.labelOutputPlaceholders = new System.Windows.Forms.Label();
            this.textBoxOutputFileNameHint = new System.Windows.Forms.TextBox();
            this.textBoxReplaceSpaces = new System.Windows.Forms.TextBox();
            this.checkBoxReplaceSpaces = new System.Windows.Forms.CheckBox();
            this.labelOutputFileExtension = new System.Windows.Forms.Label();
            this.buttonOutputDir = new System.Windows.Forms.Button();
            this.textBoxOutputDir = new System.Windows.Forms.TextBox();
            this.textBoxOutputFileName = new System.Windows.Forms.TextBox();
            this.labelOutputFileName = new System.Windows.Forms.Label();
            this.labelOutputDirectory = new System.Windows.Forms.Label();
            this.groupBoxTracks = new System.Windows.Forms.GroupBox();
            this.groupBoxFilter = new System.Windows.Forms.GroupBox();
            this.listBoxSubtitleLanguages = new System.Windows.Forms.ListBox();
            this.listBoxAudioLanguages = new System.Windows.Forms.ListBox();
            this.comboBoxCommentary = new System.Windows.Forms.ComboBox();
            this.comboBoxCut = new System.Windows.Forms.ComboBox();
            this.comboBoxVideoLanguage = new System.Windows.Forms.ComboBox();
            this.labelSubtitleLanguages = new System.Windows.Forms.Label();
            this.labelAudioLanguages = new System.Windows.Forms.Label();
            this.labelCommentary = new System.Windows.Forms.Label();
            this.labelCut = new System.Windows.Forms.Label();
            this.labelVideoLanguage = new System.Windows.Forms.Label();
            this.groupBoxMasterOverride = new System.Windows.Forms.GroupBox();
            this.comboBoxAudienceLanguage = new System.Windows.Forms.ComboBox();
            this.labelAudienceLanguage = new System.Windows.Forms.Label();
            this.tabPageProgress = new System.Windows.Forms.TabPage();
            this.groupBoxTsMuxerProgress = new System.Windows.Forms.GroupBox();
            this.labelTsMuxerTimeRemainingElapsed = new System.Windows.Forms.Label();
            this.labelTsMuxerTimeSeparator = new System.Windows.Forms.Label();
            this.labelTsMuxerTimeRemaining = new System.Windows.Forms.Label();
            this.labelTsMuxerTimeElapsed = new System.Windows.Forms.Label();
            this.textBoxTsMuxerCommandLine = new System.Windows.Forms.TextBox();
            this.labelTsMuxerCommandLine = new System.Windows.Forms.Label();
            this.progressBarTsMuxer = new System.Windows.Forms.ProgressBar();
            this.labelTsMuxerProgress = new System.Windows.Forms.Label();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.progressLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openDiscToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rescanToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.submitToDbToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.remuxerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkForUpdatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonRescan = new System.Windows.Forms.Button();
            this.buttonBrowse = new System.Windows.Forms.Button();
            this.textBoxSource = new System.Windows.Forms.TextBox();
            this.labelSource = new System.Windows.Forms.Label();
            this.cancelButton = new System.Windows.Forms.Button();
            this.splitContainerDiscOuter = new BDAutoMuxer.views.SplitContainerWithDivider();
            this.panelMoviePoster = new System.Windows.Forms.Panel();
            this.pictureBoxMoviePoster = new System.Windows.Forms.PictureBox();
            this.panelMovieDetails = new System.Windows.Forms.Panel();
            this.searchResultListView = new System.Windows.Forms.ListView();
            this.NameColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.YearColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.PopularityColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.maskedTextBoxYear = new System.Windows.Forms.MaskedTextBox();
            this.searchButton = new System.Windows.Forms.Button();
            this.discLanguageComboBox = new System.Windows.Forms.ComboBox();
            this.movieNameTextBox = new System.Windows.Forms.TextBox();
            this.discLanguageLabel = new System.Windows.Forms.Label();
            this.movieNameLabel = new System.Windows.Forms.Label();
            this.playlistsSplitContainerOuter = new BDAutoMuxer.views.SplitContainerWithDivider();
            this.playlistsGroupBox = new System.Windows.Forms.GroupBox();
            this.buttonUnselectAll = new System.Windows.Forms.Button();
            this.buttonSubmitToDB = new System.Windows.Forms.Button();
            this.buttonSelectAll = new System.Windows.Forms.Button();
            this.showAllPlaylistsCheckbox = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.playlistDataGridView = new System.Windows.Forms.DataGridView();
            this.playlistsSplitContainerInner = new BDAutoMuxer.views.SplitContainerWithDivider();
            this.streamsGroupBox = new System.Windows.Forms.GroupBox();
            this.listViewStreamFiles = new System.Windows.Forms.ListView();
            this.columnHeaderFileName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderIndex = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderFileLength = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderFileEstimatedBytes = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderFileMeasuredBytes = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tracksGroupBox = new System.Windows.Forms.GroupBox();
            this.listViewStreams = new System.Windows.Forms.ListView();
            this.columnHeaderStreamCodec = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderStreamLanguage = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderBitrate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderDescription = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.splitContainerTracksOuter = new BDAutoMuxer.views.SplitContainerWithDivider();
            this.labelPlaylist = new System.Windows.Forms.Label();
            this.comboBoxPlaylist = new System.Windows.Forms.ComboBox();
            this.listViewVideoTracks = new System.Windows.Forms.ListView();
            this.columnHeaderVideoCodec = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderVideoResolution = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderVideoFrameRate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderVideoAspectRatio = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.labelVideoTracks = new System.Windows.Forms.Label();
            this.splitContainerTracksInner = new BDAutoMuxer.views.SplitContainerWithDivider();
            this.listViewAudioTracks = new System.Windows.Forms.ListView();
            this.columnHeaderAudioCodec = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderAudioLanguage = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderAudioChannels = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.labelAudioTracks = new System.Windows.Forms.Label();
            this.listViewSubtitleTracks = new System.Windows.Forms.ListView();
            this.columnHeaderSubtitleCodec = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderSubtitleLanguage = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.labelSubtitleTracks = new System.Windows.Forms.Label();
            this.tabControl.SuspendLayout();
            this.tabPageDisc.SuspendLayout();
            this.tabPageOutput.SuspendLayout();
            this.groupBoxOutput.SuspendLayout();
            this.groupBoxTracks.SuspendLayout();
            this.groupBoxFilter.SuspendLayout();
            this.groupBoxMasterOverride.SuspendLayout();
            this.tabPageProgress.SuspendLayout();
            this.groupBoxTsMuxerProgress.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerDiscOuter)).BeginInit();
            this.splitContainerDiscOuter.Panel1.SuspendLayout();
            this.splitContainerDiscOuter.Panel2.SuspendLayout();
            this.splitContainerDiscOuter.SuspendLayout();
            this.panelMoviePoster.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMoviePoster)).BeginInit();
            this.panelMovieDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.playlistsSplitContainerOuter)).BeginInit();
            this.playlistsSplitContainerOuter.Panel1.SuspendLayout();
            this.playlistsSplitContainerOuter.Panel2.SuspendLayout();
            this.playlistsSplitContainerOuter.SuspendLayout();
            this.playlistsGroupBox.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.playlistDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.playlistsSplitContainerInner)).BeginInit();
            this.playlistsSplitContainerInner.Panel1.SuspendLayout();
            this.playlistsSplitContainerInner.Panel2.SuspendLayout();
            this.playlistsSplitContainerInner.SuspendLayout();
            this.streamsGroupBox.SuspendLayout();
            this.tracksGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerTracksOuter)).BeginInit();
            this.splitContainerTracksOuter.Panel1.SuspendLayout();
            this.splitContainerTracksOuter.Panel2.SuspendLayout();
            this.splitContainerTracksOuter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerTracksInner)).BeginInit();
            this.splitContainerTracksInner.Panel1.SuspendLayout();
            this.splitContainerTracksInner.Panel2.SuspendLayout();
            this.splitContainerTracksInner.SuspendLayout();
            this.SuspendLayout();
            // 
            // continueButton
            // 
            this.continueButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.continueButton.Location = new System.Drawing.Point(613, 701);
            this.continueButton.Name = "continueButton";
            this.continueButton.Size = new System.Drawing.Size(75, 23);
            this.continueButton.TabIndex = 98;
            this.continueButton.Text = "Mux it!";
            this.continueButton.UseVisualStyleBackColor = true;
            this.continueButton.Click += new System.EventHandler(this.continueButton_Click);
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Controls.Add(this.tabPageDisc);
            this.tabControl.Controls.Add(this.tabPageOutput);
            this.tabControl.Controls.Add(this.tabPageProgress);
            this.tabControl.Location = new System.Drawing.Point(12, 76);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(757, 619);
            this.tabControl.TabIndex = 0;
            this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_TabIndexChanged);
            this.tabControl.TabIndexChanged += new System.EventHandler(this.tabControl_TabIndexChanged);
            // 
            // tabPageDisc
            // 
            this.tabPageDisc.Controls.Add(this.hiddenTrackLabel);
            this.tabPageDisc.Controls.Add(this.splitContainerDiscOuter);
            this.tabPageDisc.Location = new System.Drawing.Point(4, 22);
            this.tabPageDisc.Name = "tabPageDisc";
            this.tabPageDisc.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDisc.Size = new System.Drawing.Size(749, 593);
            this.tabPageDisc.TabIndex = 1;
            this.tabPageDisc.Text = "Disc";
            this.tabPageDisc.UseVisualStyleBackColor = true;
            this.tabPageDisc.SizeChanged += new System.EventHandler(this.ResizeDiscTab);
            // 
            // hiddenTrackLabel
            // 
            this.hiddenTrackLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.hiddenTrackLabel.AutoSize = true;
            this.hiddenTrackLabel.Location = new System.Drawing.Point(10, 571);
            this.hiddenTrackLabel.Name = "hiddenTrackLabel";
            this.hiddenTrackLabel.Size = new System.Drawing.Size(165, 13);
            this.hiddenTrackLabel.TabIndex = 101;
            this.hiddenTrackLabel.Tag = "The Track is physically present in the Stream file but absent from the Playlist f" +
                "ile.  ";
            this.hiddenTrackLabel.Text = "* Hidden track - will not be muxed";
            // 
            // tabPageOutput
            // 
            this.tabPageOutput.Controls.Add(this.groupBoxOutput);
            this.tabPageOutput.Controls.Add(this.groupBoxTracks);
            this.tabPageOutput.Controls.Add(this.groupBoxFilter);
            this.tabPageOutput.Controls.Add(this.groupBoxMasterOverride);
            this.tabPageOutput.Location = new System.Drawing.Point(4, 22);
            this.tabPageOutput.Name = "tabPageOutput";
            this.tabPageOutput.Size = new System.Drawing.Size(749, 593);
            this.tabPageOutput.TabIndex = 2;
            this.tabPageOutput.Text = "Output";
            this.tabPageOutput.UseVisualStyleBackColor = true;
            // 
            // groupBoxOutput
            // 
            this.groupBoxOutput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxOutput.Controls.Add(this.textBoxOutputFileNamePreview);
            this.groupBoxOutput.Controls.Add(this.textBoxOutputDirPreview);
            this.groupBoxOutput.Controls.Add(this.labelOutputPlaceholders);
            this.groupBoxOutput.Controls.Add(this.textBoxOutputFileNameHint);
            this.groupBoxOutput.Controls.Add(this.textBoxReplaceSpaces);
            this.groupBoxOutput.Controls.Add(this.checkBoxReplaceSpaces);
            this.groupBoxOutput.Controls.Add(this.labelOutputFileExtension);
            this.groupBoxOutput.Controls.Add(this.buttonOutputDir);
            this.groupBoxOutput.Controls.Add(this.textBoxOutputDir);
            this.groupBoxOutput.Controls.Add(this.textBoxOutputFileName);
            this.groupBoxOutput.Controls.Add(this.labelOutputFileName);
            this.groupBoxOutput.Controls.Add(this.labelOutputDirectory);
            this.groupBoxOutput.Location = new System.Drawing.Point(4, 449);
            this.groupBoxOutput.Name = "groupBoxOutput";
            this.groupBoxOutput.Size = new System.Drawing.Size(742, 141);
            this.groupBoxOutput.TabIndex = 3;
            this.groupBoxOutput.TabStop = false;
            this.groupBoxOutput.Text = "Output";
            // 
            // textBoxOutputFileNamePreview
            // 
            this.textBoxOutputFileNamePreview.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxOutputFileNamePreview.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxOutputFileNamePreview.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxOutputFileNamePreview.Location = new System.Drawing.Point(84, 119);
            this.textBoxOutputFileNamePreview.Name = "textBoxOutputFileNamePreview";
            this.textBoxOutputFileNamePreview.ReadOnly = true;
            this.textBoxOutputFileNamePreview.Size = new System.Drawing.Size(652, 13);
            this.textBoxOutputFileNamePreview.TabIndex = 14;
            this.textBoxOutputFileNamePreview.TabStop = false;
            this.textBoxOutputFileNamePreview.Text = "The Incredibles (2006) [1080p].m2ts";
            // 
            // textBoxOutputDirPreview
            // 
            this.textBoxOutputDirPreview.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxOutputDirPreview.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxOutputDirPreview.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxOutputDirPreview.Location = new System.Drawing.Point(84, 72);
            this.textBoxOutputDirPreview.Name = "textBoxOutputDirPreview";
            this.textBoxOutputDirPreview.ReadOnly = true;
            this.textBoxOutputDirPreview.Size = new System.Drawing.Size(652, 13);
            this.textBoxOutputDirPreview.TabIndex = 13;
            this.textBoxOutputDirPreview.TabStop = false;
            this.textBoxOutputDirPreview.Text = "C:\\Users\\Administrator\\AppData\\Local\\Temp\\INCREDIBLES";
            // 
            // labelOutputPlaceholders
            // 
            this.labelOutputPlaceholders.AutoSize = true;
            this.labelOutputPlaceholders.Location = new System.Drawing.Point(7, 20);
            this.labelOutputPlaceholders.Name = "labelOutputPlaceholders";
            this.labelOutputPlaceholders.Size = new System.Drawing.Size(71, 13);
            this.labelOutputPlaceholders.TabIndex = 12;
            this.labelOutputPlaceholders.Text = "Placeholders:";
            // 
            // textBoxOutputFileNameHint
            // 
            this.textBoxOutputFileNameHint.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxOutputFileNameHint.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxOutputFileNameHint.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxOutputFileNameHint.Location = new System.Drawing.Point(84, 20);
            this.textBoxOutputFileNameHint.Name = "textBoxOutputFileNameHint";
            this.textBoxOutputFileNameHint.ReadOnly = true;
            this.textBoxOutputFileNameHint.Size = new System.Drawing.Size(476, 13);
            this.textBoxOutputFileNameHint.TabIndex = 11;
            this.textBoxOutputFileNameHint.TabStop = false;
            this.textBoxOutputFileNameHint.Text = "%volume% %title% %year% %res% %vcodec% %acodec% %channels%";
            // 
            // textBoxReplaceSpaces
            // 
            this.textBoxReplaceSpaces.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxReplaceSpaces.Enabled = false;
            this.textBoxReplaceSpaces.Location = new System.Drawing.Point(703, 17);
            this.textBoxReplaceSpaces.Name = "textBoxReplaceSpaces";
            this.textBoxReplaceSpaces.Size = new System.Drawing.Size(29, 20);
            this.textBoxReplaceSpaces.TabIndex = 1;
            this.textBoxReplaceSpaces.Text = ".";
            this.textBoxReplaceSpaces.TextChanged += new System.EventHandler(this.textBoxReplaceSpaces_TextChanged);
            this.textBoxReplaceSpaces.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxReplaceSpaces_KeyPress);
            // 
            // checkBoxReplaceSpaces
            // 
            this.checkBoxReplaceSpaces.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxReplaceSpaces.AutoSize = true;
            this.checkBoxReplaceSpaces.Location = new System.Drawing.Point(566, 19);
            this.checkBoxReplaceSpaces.Name = "checkBoxReplaceSpaces";
            this.checkBoxReplaceSpaces.Size = new System.Drawing.Size(128, 17);
            this.checkBoxReplaceSpaces.TabIndex = 0;
            this.checkBoxReplaceSpaces.Text = "Replace spaces with:";
            this.checkBoxReplaceSpaces.UseVisualStyleBackColor = true;
            this.checkBoxReplaceSpaces.CheckedChanged += new System.EventHandler(this.checkBoxReplaceSpaces_CheckedChanged);
            // 
            // labelOutputFileExtension
            // 
            this.labelOutputFileExtension.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelOutputFileExtension.AutoSize = true;
            this.labelOutputFileExtension.Location = new System.Drawing.Point(700, 96);
            this.labelOutputFileExtension.Name = "labelOutputFileExtension";
            this.labelOutputFileExtension.Size = new System.Drawing.Size(32, 13);
            this.labelOutputFileExtension.TabIndex = 3;
            this.labelOutputFileExtension.Text = ".m2ts";
            // 
            // buttonOutputDir
            // 
            this.buttonOutputDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOutputDir.Location = new System.Drawing.Point(661, 43);
            this.buttonOutputDir.Name = "buttonOutputDir";
            this.buttonOutputDir.Size = new System.Drawing.Size(75, 23);
            this.buttonOutputDir.TabIndex = 3;
            this.buttonOutputDir.Text = "Browse...";
            this.buttonOutputDir.UseVisualStyleBackColor = true;
            this.buttonOutputDir.Click += new System.EventHandler(this.buttonOutputDir_Click);
            // 
            // textBoxOutputDir
            // 
            this.textBoxOutputDir.AllowDrop = true;
            this.textBoxOutputDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxOutputDir.Location = new System.Drawing.Point(84, 45);
            this.textBoxOutputDir.Name = "textBoxOutputDir";
            this.textBoxOutputDir.Size = new System.Drawing.Size(571, 20);
            this.textBoxOutputDir.TabIndex = 2;
            this.textBoxOutputDir.TextChanged += new System.EventHandler(this.textBoxOutputDir_TextChanged);
            this.textBoxOutputDir.DragDrop += new System.Windows.Forms.DragEventHandler(this.textBoxOutputDir_DragDrop);
            this.textBoxOutputDir.DragEnter += new System.Windows.Forms.DragEventHandler(this.textBoxOutputDir_DragEnter);
            // 
            // textBoxOutputFileName
            // 
            this.textBoxOutputFileName.AllowDrop = true;
            this.textBoxOutputFileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxOutputFileName.Location = new System.Drawing.Point(84, 93);
            this.textBoxOutputFileName.Name = "textBoxOutputFileName";
            this.textBoxOutputFileName.Size = new System.Drawing.Size(610, 20);
            this.textBoxOutputFileName.TabIndex = 4;
            this.textBoxOutputFileName.TextChanged += new System.EventHandler(this.textBoxOutputFileName_TextChanged);
            this.textBoxOutputFileName.DragDrop += new System.Windows.Forms.DragEventHandler(this.textBoxOutputFileName_DragDrop);
            this.textBoxOutputFileName.DragEnter += new System.Windows.Forms.DragEventHandler(this.textBoxOutputFileName_DragEnter);
            // 
            // labelOutputFileName
            // 
            this.labelOutputFileName.AutoSize = true;
            this.labelOutputFileName.Location = new System.Drawing.Point(6, 96);
            this.labelOutputFileName.Name = "labelOutputFileName";
            this.labelOutputFileName.Size = new System.Drawing.Size(55, 13);
            this.labelOutputFileName.TabIndex = 1;
            this.labelOutputFileName.Text = "File name:";
            // 
            // labelOutputDirectory
            // 
            this.labelOutputDirectory.AutoSize = true;
            this.labelOutputDirectory.Location = new System.Drawing.Point(6, 48);
            this.labelOutputDirectory.Name = "labelOutputDirectory";
            this.labelOutputDirectory.Size = new System.Drawing.Size(52, 13);
            this.labelOutputDirectory.TabIndex = 0;
            this.labelOutputDirectory.Text = "Directory:";
            // 
            // groupBoxTracks
            // 
            this.groupBoxTracks.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxTracks.Controls.Add(this.splitContainerTracksOuter);
            this.groupBoxTracks.Location = new System.Drawing.Point(256, 61);
            this.groupBoxTracks.Name = "groupBoxTracks";
            this.groupBoxTracks.Size = new System.Drawing.Size(486, 382);
            this.groupBoxTracks.TabIndex = 2;
            this.groupBoxTracks.TabStop = false;
            this.groupBoxTracks.Text = "Select Playlist / Tracks";
            this.groupBoxTracks.SizeChanged += new System.EventHandler(this.ResizeOutputTab);
            // 
            // groupBoxFilter
            // 
            this.groupBoxFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBoxFilter.Controls.Add(this.listBoxSubtitleLanguages);
            this.groupBoxFilter.Controls.Add(this.listBoxAudioLanguages);
            this.groupBoxFilter.Controls.Add(this.comboBoxCommentary);
            this.groupBoxFilter.Controls.Add(this.comboBoxCut);
            this.groupBoxFilter.Controls.Add(this.comboBoxVideoLanguage);
            this.groupBoxFilter.Controls.Add(this.labelSubtitleLanguages);
            this.groupBoxFilter.Controls.Add(this.labelAudioLanguages);
            this.groupBoxFilter.Controls.Add(this.labelCommentary);
            this.groupBoxFilter.Controls.Add(this.labelCut);
            this.groupBoxFilter.Controls.Add(this.labelVideoLanguage);
            this.groupBoxFilter.Location = new System.Drawing.Point(4, 61);
            this.groupBoxFilter.Name = "groupBoxFilter";
            this.groupBoxFilter.Size = new System.Drawing.Size(246, 382);
            this.groupBoxFilter.TabIndex = 1;
            this.groupBoxFilter.TabStop = false;
            this.groupBoxFilter.Text = "Filter";
            // 
            // listBoxSubtitleLanguages
            // 
            this.listBoxSubtitleLanguages.FormattingEnabled = true;
            this.listBoxSubtitleLanguages.Location = new System.Drawing.Point(115, 164);
            this.listBoxSubtitleLanguages.Name = "listBoxSubtitleLanguages";
            this.listBoxSubtitleLanguages.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBoxSubtitleLanguages.Size = new System.Drawing.Size(120, 56);
            this.listBoxSubtitleLanguages.TabIndex = 4;
            this.listBoxSubtitleLanguages.SelectedValueChanged += new System.EventHandler(this.FilterControlChanged);
            // 
            // listBoxAudioLanguages
            // 
            this.listBoxAudioLanguages.FormattingEnabled = true;
            this.listBoxAudioLanguages.Location = new System.Drawing.Point(115, 101);
            this.listBoxAudioLanguages.Name = "listBoxAudioLanguages";
            this.listBoxAudioLanguages.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBoxAudioLanguages.Size = new System.Drawing.Size(120, 56);
            this.listBoxAudioLanguages.TabIndex = 3;
            this.listBoxAudioLanguages.SelectedValueChanged += new System.EventHandler(this.FilterControlChanged);
            // 
            // comboBoxCommentary
            // 
            this.comboBoxCommentary.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCommentary.FormattingEnabled = true;
            this.comboBoxCommentary.Location = new System.Drawing.Point(115, 73);
            this.comboBoxCommentary.Name = "comboBoxCommentary";
            this.comboBoxCommentary.Size = new System.Drawing.Size(121, 21);
            this.comboBoxCommentary.TabIndex = 2;
            this.comboBoxCommentary.SelectedValueChanged += new System.EventHandler(this.FilterControlChanged);
            // 
            // comboBoxCut
            // 
            this.comboBoxCut.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCut.FormattingEnabled = true;
            this.comboBoxCut.Location = new System.Drawing.Point(115, 45);
            this.comboBoxCut.Name = "comboBoxCut";
            this.comboBoxCut.Size = new System.Drawing.Size(121, 21);
            this.comboBoxCut.TabIndex = 1;
            this.comboBoxCut.SelectedValueChanged += new System.EventHandler(this.FilterControlChanged);
            // 
            // comboBoxVideoLanguage
            // 
            this.comboBoxVideoLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxVideoLanguage.FormattingEnabled = true;
            this.comboBoxVideoLanguage.Location = new System.Drawing.Point(115, 17);
            this.comboBoxVideoLanguage.Name = "comboBoxVideoLanguage";
            this.comboBoxVideoLanguage.Size = new System.Drawing.Size(121, 21);
            this.comboBoxVideoLanguage.TabIndex = 0;
            this.comboBoxVideoLanguage.Tag = "Primary video language (many animated Disney movies contain several video renders" +
                " - e.g., French, Spanish, and English)";
            this.comboBoxVideoLanguage.SelectedValueChanged += new System.EventHandler(this.FilterControlChanged);
            // 
            // labelSubtitleLanguages
            // 
            this.labelSubtitleLanguages.AutoSize = true;
            this.labelSubtitleLanguages.Location = new System.Drawing.Point(6, 167);
            this.labelSubtitleLanguages.Name = "labelSubtitleLanguages";
            this.labelSubtitleLanguages.Size = new System.Drawing.Size(103, 13);
            this.labelSubtitleLanguages.TabIndex = 4;
            this.labelSubtitleLanguages.Text = "Subtitle language(s):";
            // 
            // labelAudioLanguages
            // 
            this.labelAudioLanguages.AutoSize = true;
            this.labelAudioLanguages.Location = new System.Drawing.Point(6, 104);
            this.labelAudioLanguages.Name = "labelAudioLanguages";
            this.labelAudioLanguages.Size = new System.Drawing.Size(95, 13);
            this.labelAudioLanguages.TabIndex = 3;
            this.labelAudioLanguages.Text = "Audio language(s):";
            // 
            // labelCommentary
            // 
            this.labelCommentary.AutoSize = true;
            this.labelCommentary.Location = new System.Drawing.Point(6, 76);
            this.labelCommentary.Name = "labelCommentary";
            this.labelCommentary.Size = new System.Drawing.Size(68, 13);
            this.labelCommentary.TabIndex = 2;
            this.labelCommentary.Text = "Commentary:";
            // 
            // labelCut
            // 
            this.labelCut.AutoSize = true;
            this.labelCut.Location = new System.Drawing.Point(6, 48);
            this.labelCut.Name = "labelCut";
            this.labelCut.Size = new System.Drawing.Size(26, 13);
            this.labelCut.TabIndex = 1;
            this.labelCut.Text = "Cut:";
            // 
            // labelVideoLanguage
            // 
            this.labelVideoLanguage.AutoSize = true;
            this.labelVideoLanguage.Location = new System.Drawing.Point(6, 20);
            this.labelVideoLanguage.Name = "labelVideoLanguage";
            this.labelVideoLanguage.Size = new System.Drawing.Size(84, 13);
            this.labelVideoLanguage.TabIndex = 0;
            this.labelVideoLanguage.Text = "Video language:";
            // 
            // groupBoxMasterOverride
            // 
            this.groupBoxMasterOverride.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxMasterOverride.Controls.Add(this.comboBoxAudienceLanguage);
            this.groupBoxMasterOverride.Controls.Add(this.labelAudienceLanguage);
            this.groupBoxMasterOverride.Location = new System.Drawing.Point(4, 4);
            this.groupBoxMasterOverride.Name = "groupBoxMasterOverride";
            this.groupBoxMasterOverride.Size = new System.Drawing.Size(738, 50);
            this.groupBoxMasterOverride.TabIndex = 0;
            this.groupBoxMasterOverride.TabStop = false;
            this.groupBoxMasterOverride.Text = "Master Override";
            // 
            // comboBoxAudienceLanguage
            // 
            this.comboBoxAudienceLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxAudienceLanguage.FormattingEnabled = true;
            this.comboBoxAudienceLanguage.Location = new System.Drawing.Point(115, 17);
            this.comboBoxAudienceLanguage.Name = "comboBoxAudienceLanguage";
            this.comboBoxAudienceLanguage.Size = new System.Drawing.Size(121, 21);
            this.comboBoxAudienceLanguage.TabIndex = 0;
            this.comboBoxAudienceLanguage.Tag = "Set all language filters (video, audio, and subtitle) to a single language";
            // 
            // labelAudienceLanguage
            // 
            this.labelAudienceLanguage.AutoSize = true;
            this.labelAudienceLanguage.Location = new System.Drawing.Point(6, 20);
            this.labelAudienceLanguage.Name = "labelAudienceLanguage";
            this.labelAudienceLanguage.Size = new System.Drawing.Size(102, 13);
            this.labelAudienceLanguage.TabIndex = 0;
            this.labelAudienceLanguage.Text = "Audience language:";
            // 
            // tabPageProgress
            // 
            this.tabPageProgress.Controls.Add(this.groupBoxTsMuxerProgress);
            this.tabPageProgress.Location = new System.Drawing.Point(4, 22);
            this.tabPageProgress.Name = "tabPageProgress";
            this.tabPageProgress.Size = new System.Drawing.Size(749, 593);
            this.tabPageProgress.TabIndex = 3;
            this.tabPageProgress.Text = "Progress";
            this.tabPageProgress.UseVisualStyleBackColor = true;
            // 
            // groupBoxTsMuxerProgress
            // 
            this.groupBoxTsMuxerProgress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxTsMuxerProgress.Controls.Add(this.labelTsMuxerTimeRemainingElapsed);
            this.groupBoxTsMuxerProgress.Controls.Add(this.labelTsMuxerTimeSeparator);
            this.groupBoxTsMuxerProgress.Controls.Add(this.labelTsMuxerTimeRemaining);
            this.groupBoxTsMuxerProgress.Controls.Add(this.labelTsMuxerTimeElapsed);
            this.groupBoxTsMuxerProgress.Controls.Add(this.textBoxTsMuxerCommandLine);
            this.groupBoxTsMuxerProgress.Controls.Add(this.labelTsMuxerCommandLine);
            this.groupBoxTsMuxerProgress.Controls.Add(this.progressBarTsMuxer);
            this.groupBoxTsMuxerProgress.Controls.Add(this.labelTsMuxerProgress);
            this.groupBoxTsMuxerProgress.Location = new System.Drawing.Point(4, 4);
            this.groupBoxTsMuxerProgress.Name = "groupBoxTsMuxerProgress";
            this.groupBoxTsMuxerProgress.Size = new System.Drawing.Size(738, 150);
            this.groupBoxTsMuxerProgress.TabIndex = 0;
            this.groupBoxTsMuxerProgress.TabStop = false;
            this.groupBoxTsMuxerProgress.Text = "tsMuxeR Progress";
            // 
            // labelTsMuxerTimeRemainingElapsed
            // 
            this.labelTsMuxerTimeRemainingElapsed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelTsMuxerTimeRemainingElapsed.AutoSize = true;
            this.labelTsMuxerTimeRemainingElapsed.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTsMuxerTimeRemainingElapsed.Location = new System.Drawing.Point(471, 20);
            this.labelTsMuxerTimeRemainingElapsed.Name = "labelTsMuxerTimeRemainingElapsed";
            this.labelTsMuxerTimeRemainingElapsed.Size = new System.Drawing.Size(135, 13);
            this.labelTsMuxerTimeRemainingElapsed.TabIndex = 36;
            this.labelTsMuxerTimeRemainingElapsed.Text = "Time Remaining / Elapsed:";
            // 
            // labelTsMuxerTimeSeparator
            // 
            this.labelTsMuxerTimeSeparator.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelTsMuxerTimeSeparator.AutoSize = true;
            this.labelTsMuxerTimeSeparator.Location = new System.Drawing.Point(667, 20);
            this.labelTsMuxerTimeSeparator.Name = "labelTsMuxerTimeSeparator";
            this.labelTsMuxerTimeSeparator.Size = new System.Drawing.Size(12, 13);
            this.labelTsMuxerTimeSeparator.TabIndex = 6;
            this.labelTsMuxerTimeSeparator.Text = "/";
            // 
            // labelTsMuxerTimeRemaining
            // 
            this.labelTsMuxerTimeRemaining.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelTsMuxerTimeRemaining.AutoSize = true;
            this.labelTsMuxerTimeRemaining.Location = new System.Drawing.Point(612, 20);
            this.labelTsMuxerTimeRemaining.Name = "labelTsMuxerTimeRemaining";
            this.labelTsMuxerTimeRemaining.Size = new System.Drawing.Size(49, 13);
            this.labelTsMuxerTimeRemaining.TabIndex = 5;
            this.labelTsMuxerTimeRemaining.Text = "00:00:00";
            // 
            // labelTsMuxerTimeElapsed
            // 
            this.labelTsMuxerTimeElapsed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelTsMuxerTimeElapsed.AutoSize = true;
            this.labelTsMuxerTimeElapsed.Location = new System.Drawing.Point(683, 20);
            this.labelTsMuxerTimeElapsed.Name = "labelTsMuxerTimeElapsed";
            this.labelTsMuxerTimeElapsed.Size = new System.Drawing.Size(49, 13);
            this.labelTsMuxerTimeElapsed.TabIndex = 4;
            this.labelTsMuxerTimeElapsed.Text = "00:00:00";
            // 
            // textBoxTsMuxerCommandLine
            // 
            this.textBoxTsMuxerCommandLine.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTsMuxerCommandLine.Location = new System.Drawing.Point(7, 84);
            this.textBoxTsMuxerCommandLine.Multiline = true;
            this.textBoxTsMuxerCommandLine.Name = "textBoxTsMuxerCommandLine";
            this.textBoxTsMuxerCommandLine.ReadOnly = true;
            this.textBoxTsMuxerCommandLine.Size = new System.Drawing.Size(725, 60);
            this.textBoxTsMuxerCommandLine.TabIndex = 3;
            // 
            // labelTsMuxerCommandLine
            // 
            this.labelTsMuxerCommandLine.AutoSize = true;
            this.labelTsMuxerCommandLine.Location = new System.Drawing.Point(7, 67);
            this.labelTsMuxerCommandLine.Name = "labelTsMuxerCommandLine";
            this.labelTsMuxerCommandLine.Size = new System.Drawing.Size(76, 13);
            this.labelTsMuxerCommandLine.TabIndex = 2;
            this.labelTsMuxerCommandLine.Text = "Command line:";
            // 
            // progressBarTsMuxer
            // 
            this.progressBarTsMuxer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBarTsMuxer.Location = new System.Drawing.Point(7, 37);
            this.progressBarTsMuxer.Maximum = 1000;
            this.progressBarTsMuxer.Name = "progressBarTsMuxer";
            this.progressBarTsMuxer.Size = new System.Drawing.Size(725, 23);
            this.progressBarTsMuxer.Step = 1;
            this.progressBarTsMuxer.TabIndex = 1;
            // 
            // labelTsMuxerProgress
            // 
            this.labelTsMuxerProgress.AutoSize = true;
            this.labelTsMuxerProgress.Location = new System.Drawing.Point(7, 20);
            this.labelTsMuxerProgress.Name = "labelTsMuxerProgress";
            this.labelTsMuxerProgress.Size = new System.Drawing.Size(30, 13);
            this.labelTsMuxerProgress.TabIndex = 0;
            this.labelTsMuxerProgress.Text = "0.0%";
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel,
            this.progressLabel,
            this.toolStripProgressBar});
            this.statusStrip.Location = new System.Drawing.Point(0, 727);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(781, 22);
            this.statusStrip.TabIndex = 6;
            this.statusStrip.Text = "Status Strip";
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(530, 17);
            this.statusLabel.Spring = true;
            this.statusLabel.Text = "Status";
            this.statusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // progressLabel
            // 
            this.progressLabel.Name = "progressLabel";
            this.progressLabel.Size = new System.Drawing.Size(34, 17);
            this.progressLabel.Text = "0.0%";
            this.progressLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // toolStripProgressBar
            // 
            this.toolStripProgressBar.Maximum = 1000;
            this.toolStripProgressBar.Name = "toolStripProgressBar";
            this.toolStripProgressBar.Size = new System.Drawing.Size(200, 16);
            this.toolStripProgressBar.Step = 1;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(781, 24);
            this.menuStrip1.TabIndex = 100;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openFolderToolStripMenuItem,
            this.openDiscToolStripMenuItem,
            this.rescanToolStripMenuItem,
            this.toolStripMenuItem1,
            this.submitToDbToolStripMenuItem,
            this.toolStripMenuItem2,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openFolderToolStripMenuItem
            // 
            this.openFolderToolStripMenuItem.Name = "openFolderToolStripMenuItem";
            this.openFolderToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openFolderToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.openFolderToolStripMenuItem.Text = "Open folder...";
            this.openFolderToolStripMenuItem.Click += new System.EventHandler(this.buttonBrowse_Click);
            // 
            // openDiscToolStripMenuItem
            // 
            this.openDiscToolStripMenuItem.Enabled = false;
            this.openDiscToolStripMenuItem.Name = "openDiscToolStripMenuItem";
            this.openDiscToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.openDiscToolStripMenuItem.Text = "Open disc";
            // 
            // rescanToolStripMenuItem
            // 
            this.rescanToolStripMenuItem.Name = "rescanToolStripMenuItem";
            this.rescanToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.rescanToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.rescanToolStripMenuItem.Text = "Rescan";
            this.rescanToolStripMenuItem.Click += new System.EventHandler(this.buttonRescan_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(191, 6);
            // 
            // submitToDbToolStripMenuItem
            // 
            this.submitToDbToolStripMenuItem.Name = "submitToDbToolStripMenuItem";
            this.submitToDbToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.submitToDbToolStripMenuItem.Text = "Submit to database";
            this.submitToDbToolStripMenuItem.Click += new System.EventHandler(this.SubmitJsonDisc);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(191, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.remuxerToolStripMenuItem,
            this.settingsToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // remuxerToolStripMenuItem
            // 
            this.remuxerToolStripMenuItem.Name = "remuxerToolStripMenuItem";
            this.remuxerToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.remuxerToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.remuxerToolStripMenuItem.Text = "Remuxer";
            this.remuxerToolStripMenuItem.Click += new System.EventHandler(this.remuxerToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.settingsToolStripMenuItem.Text = "Settings...";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem,
            this.checkForUpdatesToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Enabled = false;
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1;
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.aboutToolStripMenuItem.Text = "About";
            // 
            // checkForUpdatesToolStripMenuItem
            // 
            this.checkForUpdatesToolStripMenuItem.Name = "checkForUpdatesToolStripMenuItem";
            this.checkForUpdatesToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.checkForUpdatesToolStripMenuItem.Text = "Check for updates";
            this.checkForUpdatesToolStripMenuItem.Click += new System.EventHandler(this.checkForUpdatesToolStripMenuItem_Click);
            // 
            // buttonRescan
            // 
            this.buttonRescan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonRescan.Enabled = false;
            this.buttonRescan.Location = new System.Drawing.Point(694, 47);
            this.buttonRescan.Name = "buttonRescan";
            this.buttonRescan.Size = new System.Drawing.Size(75, 23);
            this.buttonRescan.TabIndex = 103;
            this.buttonRescan.Text = "Rescan";
            this.buttonRescan.UseVisualStyleBackColor = true;
            this.buttonRescan.Click += new System.EventHandler(this.buttonRescan_Click);
            // 
            // buttonBrowse
            // 
            this.buttonBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonBrowse.Location = new System.Drawing.Point(613, 47);
            this.buttonBrowse.Name = "buttonBrowse";
            this.buttonBrowse.Size = new System.Drawing.Size(75, 23);
            this.buttonBrowse.TabIndex = 102;
            this.buttonBrowse.Text = "Browse...";
            this.buttonBrowse.UseVisualStyleBackColor = true;
            this.buttonBrowse.Click += new System.EventHandler(this.buttonBrowse_Click);
            // 
            // textBoxSource
            // 
            this.textBoxSource.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxSource.Location = new System.Drawing.Point(15, 49);
            this.textBoxSource.Name = "textBoxSource";
            this.textBoxSource.Size = new System.Drawing.Size(592, 20);
            this.textBoxSource.TabIndex = 101;
            this.textBoxSource.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxSource_KeyPress);
            // 
            // labelSource
            // 
            this.labelSource.AutoSize = true;
            this.labelSource.Location = new System.Drawing.Point(12, 33);
            this.labelSource.Name = "labelSource";
            this.labelSource.Size = new System.Drawing.Size(141, 13);
            this.labelSource.TabIndex = 104;
            this.labelSource.Text = "Select the Source BD-ROM:";
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(694, 701);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 99;
            this.cancelButton.Text = "Close";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // splitContainerDiscOuter
            // 
            this.splitContainerDiscOuter.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainerDiscOuter.Location = new System.Drawing.Point(6, 7);
            this.splitContainerDiscOuter.Name = "splitContainerDiscOuter";
            this.splitContainerDiscOuter.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerDiscOuter.Panel1
            // 
            this.splitContainerDiscOuter.Panel1.Controls.Add(this.panelMoviePoster);
            this.splitContainerDiscOuter.Panel1.Controls.Add(this.panelMovieDetails);
            // 
            // splitContainerDiscOuter.Panel2
            // 
            this.splitContainerDiscOuter.Panel2.Controls.Add(this.playlistsSplitContainerOuter);
            this.splitContainerDiscOuter.Size = new System.Drawing.Size(737, 559);
            this.splitContainerDiscOuter.SplitterDistance = 137;
            this.splitContainerDiscOuter.TabIndex = 6;
            // 
            // panelMoviePoster
            // 
            this.panelMoviePoster.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelMoviePoster.Controls.Add(this.pictureBoxMoviePoster);
            this.panelMoviePoster.Location = new System.Drawing.Point(633, 0);
            this.panelMoviePoster.Name = "panelMoviePoster";
            this.panelMoviePoster.Size = new System.Drawing.Size(104, 135);
            this.panelMoviePoster.TabIndex = 20;
            // 
            // pictureBoxMoviePoster
            // 
            this.pictureBoxMoviePoster.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxMoviePoster.BackColor = System.Drawing.Color.Transparent;
            this.pictureBoxMoviePoster.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBoxMoviePoster.ErrorImage = null;
            this.pictureBoxMoviePoster.InitialImage = null;
            this.pictureBoxMoviePoster.Location = new System.Drawing.Point(5, 5);
            this.pictureBoxMoviePoster.Name = "pictureBoxMoviePoster";
            this.pictureBoxMoviePoster.Size = new System.Drawing.Size(93, 139);
            this.pictureBoxMoviePoster.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxMoviePoster.TabIndex = 0;
            this.pictureBoxMoviePoster.TabStop = false;
            this.pictureBoxMoviePoster.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureBoxMoviePoster_MouseClick);
            // 
            // panelMovieDetails
            // 
            this.panelMovieDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelMovieDetails.Controls.Add(this.searchResultListView);
            this.panelMovieDetails.Controls.Add(this.maskedTextBoxYear);
            this.panelMovieDetails.Controls.Add(this.searchButton);
            this.panelMovieDetails.Controls.Add(this.discLanguageComboBox);
            this.panelMovieDetails.Controls.Add(this.movieNameTextBox);
            this.panelMovieDetails.Controls.Add(this.discLanguageLabel);
            this.panelMovieDetails.Controls.Add(this.movieNameLabel);
            this.panelMovieDetails.Location = new System.Drawing.Point(0, 0);
            this.panelMovieDetails.Name = "panelMovieDetails";
            this.panelMovieDetails.Size = new System.Drawing.Size(627, 138);
            this.panelMovieDetails.TabIndex = 19;
            // 
            // searchResultListView
            // 
            this.searchResultListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.searchResultListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.NameColumn,
            this.YearColumn,
            this.PopularityColumn});
            this.searchResultListView.FullRowSelect = true;
            this.searchResultListView.HideSelection = false;
            this.searchResultListView.Location = new System.Drawing.Point(3, 57);
            this.searchResultListView.MultiSelect = false;
            this.searchResultListView.Name = "searchResultListView";
            this.searchResultListView.Size = new System.Drawing.Size(621, 77);
            this.searchResultListView.TabIndex = 4;
            this.searchResultListView.UseCompatibleStateImageBehavior = false;
            this.searchResultListView.View = System.Windows.Forms.View.Details;
            this.searchResultListView.SelectedIndexChanged += new System.EventHandler(this.searchResultListView_SelectedIndexChanged);
            // 
            // NameColumn
            // 
            this.NameColumn.Text = "Name";
            this.NameColumn.Width = 250;
            // 
            // YearColumn
            // 
            this.YearColumn.Text = "Year";
            this.YearColumn.Width = 50;
            // 
            // PopularityColumn
            // 
            this.PopularityColumn.Text = "Popularity";
            this.PopularityColumn.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.PopularityColumn.Width = 80;
            // 
            // maskedTextBoxYear
            // 
            this.maskedTextBoxYear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.maskedTextBoxYear.Location = new System.Drawing.Point(481, 30);
            this.maskedTextBoxYear.Mask = "0000";
            this.maskedTextBoxYear.Name = "maskedTextBoxYear";
            this.maskedTextBoxYear.Size = new System.Drawing.Size(42, 20);
            this.maskedTextBoxYear.TabIndex = 2;
            this.maskedTextBoxYear.Tag = "Movie year";
            this.maskedTextBoxYear.TextChanged += new System.EventHandler(this.maskedTextBoxYear_TextChanged);
            this.maskedTextBoxYear.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.movieNameTextBox_KeyPress);
            // 
            // searchButton
            // 
            this.searchButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.searchButton.Location = new System.Drawing.Point(529, 28);
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(98, 23);
            this.searchButton.TabIndex = 3;
            this.searchButton.Text = "Search TMDb";
            this.searchButton.UseVisualStyleBackColor = true;
            this.searchButton.Click += new System.EventHandler(this.searchButton_Click);
            // 
            // discLanguageComboBox
            // 
            this.discLanguageComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.discLanguageComboBox.FormattingEnabled = true;
            this.discLanguageComboBox.Location = new System.Drawing.Point(89, 3);
            this.discLanguageComboBox.Name = "discLanguageComboBox";
            this.discLanguageComboBox.Size = new System.Drawing.Size(121, 21);
            this.discLanguageComboBox.TabIndex = 0;
            this.discLanguageComboBox.Tag = "Primary audience language of the disc";
            this.discLanguageComboBox.SelectedIndexChanged += new System.EventHandler(this.discLanguageComboBox_SelectedIndexChanged);
            // 
            // movieNameTextBox
            // 
            this.movieNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.movieNameTextBox.Location = new System.Drawing.Point(89, 30);
            this.movieNameTextBox.Name = "movieNameTextBox";
            this.movieNameTextBox.Size = new System.Drawing.Size(386, 20);
            this.movieNameTextBox.TabIndex = 1;
            this.movieNameTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.movieNameTextBox_KeyPress);
            // 
            // discLanguageLabel
            // 
            this.discLanguageLabel.AutoSize = true;
            this.discLanguageLabel.Location = new System.Drawing.Point(3, 6);
            this.discLanguageLabel.Name = "discLanguageLabel";
            this.discLanguageLabel.Size = new System.Drawing.Size(80, 13);
            this.discLanguageLabel.TabIndex = 18;
            this.discLanguageLabel.Text = "Main language:";
            // 
            // movieNameLabel
            // 
            this.movieNameLabel.AutoSize = true;
            this.movieNameLabel.Location = new System.Drawing.Point(3, 33);
            this.movieNameLabel.Name = "movieNameLabel";
            this.movieNameLabel.Size = new System.Drawing.Size(68, 13);
            this.movieNameLabel.TabIndex = 14;
            this.movieNameLabel.Text = "Movie name:";
            // 
            // playlistsSplitContainerOuter
            // 
            this.playlistsSplitContainerOuter.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.playlistsSplitContainerOuter.BackColor = System.Drawing.Color.Transparent;
            this.playlistsSplitContainerOuter.Location = new System.Drawing.Point(0, 2);
            this.playlistsSplitContainerOuter.Name = "playlistsSplitContainerOuter";
            this.playlistsSplitContainerOuter.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // playlistsSplitContainerOuter.Panel1
            // 
            this.playlistsSplitContainerOuter.Panel1.Controls.Add(this.playlistsGroupBox);
            // 
            // playlistsSplitContainerOuter.Panel2
            // 
            this.playlistsSplitContainerOuter.Panel2.Controls.Add(this.playlistsSplitContainerInner);
            this.playlistsSplitContainerOuter.Size = new System.Drawing.Size(737, 415);
            this.playlistsSplitContainerOuter.SplitterDistance = 171;
            this.playlistsSplitContainerOuter.TabIndex = 6;
            // 
            // playlistsGroupBox
            // 
            this.playlistsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.playlistsGroupBox.Controls.Add(this.buttonUnselectAll);
            this.playlistsGroupBox.Controls.Add(this.buttonSubmitToDB);
            this.playlistsGroupBox.Controls.Add(this.buttonSelectAll);
            this.playlistsGroupBox.Controls.Add(this.showAllPlaylistsCheckbox);
            this.playlistsGroupBox.Controls.Add(this.panel1);
            this.playlistsGroupBox.Location = new System.Drawing.Point(4, 0);
            this.playlistsGroupBox.Name = "playlistsGroupBox";
            this.playlistsGroupBox.Size = new System.Drawing.Size(730, 168);
            this.playlistsGroupBox.TabIndex = 0;
            this.playlistsGroupBox.TabStop = false;
            this.playlistsGroupBox.Text = "Playlists";
            // 
            // buttonUnselectAll
            // 
            this.buttonUnselectAll.Location = new System.Drawing.Point(89, 20);
            this.buttonUnselectAll.Name = "buttonUnselectAll";
            this.buttonUnselectAll.Size = new System.Drawing.Size(75, 23);
            this.buttonUnselectAll.TabIndex = 1;
            this.buttonUnselectAll.Tag = "Clear the \"Main Movie\" checkbox on all playlists";
            this.buttonUnselectAll.Text = "Unselect All";
            this.buttonUnselectAll.UseVisualStyleBackColor = true;
            this.buttonUnselectAll.Click += new System.EventHandler(this.buttonUnselectAll_Click);
            // 
            // buttonSubmitToDB
            // 
            this.buttonSubmitToDB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSubmitToDB.Enabled = false;
            this.buttonSubmitToDB.Location = new System.Drawing.Point(640, 139);
            this.buttonSubmitToDB.Name = "buttonSubmitToDB";
            this.buttonSubmitToDB.Size = new System.Drawing.Size(84, 23);
            this.buttonSubmitToDB.TabIndex = 100;
            this.buttonSubmitToDB.Tag = "Submit the current Disc and Playlist configuration to the main movie database";
            this.buttonSubmitToDB.Text = "Submit to DB";
            this.buttonSubmitToDB.UseVisualStyleBackColor = true;
            this.buttonSubmitToDB.Click += new System.EventHandler(this.buttonSubmitToDB_Click);
            // 
            // buttonSelectAll
            // 
            this.buttonSelectAll.Location = new System.Drawing.Point(7, 20);
            this.buttonSelectAll.Name = "buttonSelectAll";
            this.buttonSelectAll.Size = new System.Drawing.Size(75, 23);
            this.buttonSelectAll.TabIndex = 0;
            this.buttonSelectAll.Tag = "Mark all enabled playlists as \"Main Movie\"";
            this.buttonSelectAll.Text = "Select All";
            this.buttonSelectAll.UseVisualStyleBackColor = true;
            this.buttonSelectAll.Click += new System.EventHandler(this.buttonSelectAll_Click);
            // 
            // showAllPlaylistsCheckbox
            // 
            this.showAllPlaylistsCheckbox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.showAllPlaylistsCheckbox.AutoSize = true;
            this.showAllPlaylistsCheckbox.Location = new System.Drawing.Point(9, 143);
            this.showAllPlaylistsCheckbox.Name = "showAllPlaylistsCheckbox";
            this.showAllPlaylistsCheckbox.Size = new System.Drawing.Size(105, 17);
            this.showAllPlaylistsCheckbox.TabIndex = 5;
            this.showAllPlaylistsCheckbox.Tag = "Show short playlists that are unlikely to be the main movie";
            this.showAllPlaylistsCheckbox.Text = "Show all playlists";
            this.showAllPlaylistsCheckbox.UseVisualStyleBackColor = true;
            this.showAllPlaylistsCheckbox.Click += new System.EventHandler(this.showAllPlaylistsCheckbox_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.playlistDataGridView);
            this.panel1.Location = new System.Drawing.Point(6, 49);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(718, 84);
            this.panel1.TabIndex = 4;
            // 
            // playlistDataGridView
            // 
            this.playlistDataGridView.AllowUserToAddRows = false;
            this.playlistDataGridView.AllowUserToDeleteRows = false;
            this.playlistDataGridView.AllowUserToResizeRows = false;
            this.playlistDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.playlistDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.playlistDataGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.playlistDataGridView.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.playlistDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.playlistDataGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
            this.playlistDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.playlistDataGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.playlistDataGridView.Location = new System.Drawing.Point(3, 3);
            this.playlistDataGridView.MultiSelect = false;
            this.playlistDataGridView.Name = "playlistDataGridView";
            this.playlistDataGridView.Size = new System.Drawing.Size(712, 78);
            this.playlistDataGridView.TabIndex = 0;
            this.playlistDataGridView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.playlistDataGridView_MouseClick);
            // 
            // playlistsSplitContainerInner
            // 
            this.playlistsSplitContainerInner.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.playlistsSplitContainerInner.Location = new System.Drawing.Point(0, 0);
            this.playlistsSplitContainerInner.Name = "playlistsSplitContainerInner";
            this.playlistsSplitContainerInner.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // playlistsSplitContainerInner.Panel1
            // 
            this.playlistsSplitContainerInner.Panel1.Controls.Add(this.streamsGroupBox);
            // 
            // playlistsSplitContainerInner.Panel2
            // 
            this.playlistsSplitContainerInner.Panel2.Controls.Add(this.tracksGroupBox);
            this.playlistsSplitContainerInner.Size = new System.Drawing.Size(737, 241);
            this.playlistsSplitContainerInner.SplitterDistance = 139;
            this.playlistsSplitContainerInner.TabIndex = 0;
            // 
            // streamsGroupBox
            // 
            this.streamsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.streamsGroupBox.Controls.Add(this.listViewStreamFiles);
            this.streamsGroupBox.Location = new System.Drawing.Point(4, 0);
            this.streamsGroupBox.Name = "streamsGroupBox";
            this.streamsGroupBox.Size = new System.Drawing.Size(730, 136);
            this.streamsGroupBox.TabIndex = 0;
            this.streamsGroupBox.TabStop = false;
            this.streamsGroupBox.Text = "Streams";
            // 
            // listViewStreamFiles
            // 
            this.listViewStreamFiles.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.listViewStreamFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderFileName,
            this.columnHeaderIndex,
            this.columnHeaderFileLength,
            this.columnHeaderFileEstimatedBytes,
            this.columnHeaderFileMeasuredBytes});
            this.listViewStreamFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewStreamFiles.FullRowSelect = true;
            this.listViewStreamFiles.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewStreamFiles.HideSelection = false;
            this.listViewStreamFiles.Location = new System.Drawing.Point(3, 16);
            this.listViewStreamFiles.MultiSelect = false;
            this.listViewStreamFiles.Name = "listViewStreamFiles";
            this.listViewStreamFiles.Size = new System.Drawing.Size(724, 117);
            this.listViewStreamFiles.TabIndex = 0;
            this.listViewStreamFiles.Tag = "Double-click a stream to open it in the default program";
            this.listViewStreamFiles.UseCompatibleStateImageBehavior = false;
            this.listViewStreamFiles.View = System.Windows.Forms.View.Details;
            this.listViewStreamFiles.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listViewStreamFiles_MouseClick);
            this.listViewStreamFiles.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listViewStreamFiles_MouseDoubleClick);
            // 
            // columnHeaderFileName
            // 
            this.columnHeaderFileName.Text = "Stream File";
            this.columnHeaderFileName.Width = 82;
            // 
            // columnHeaderIndex
            // 
            this.columnHeaderIndex.Text = "Index";
            // 
            // columnHeaderFileLength
            // 
            this.columnHeaderFileLength.Text = "Length";
            this.columnHeaderFileLength.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeaderFileLength.Width = 77;
            // 
            // columnHeaderFileEstimatedBytes
            // 
            this.columnHeaderFileEstimatedBytes.Text = "Estimated Bytes";
            this.columnHeaderFileEstimatedBytes.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeaderFileEstimatedBytes.Width = 119;
            // 
            // columnHeaderFileMeasuredBytes
            // 
            this.columnHeaderFileMeasuredBytes.Text = "Measured Bytes";
            this.columnHeaderFileMeasuredBytes.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeaderFileMeasuredBytes.Width = 125;
            // 
            // tracksGroupBox
            // 
            this.tracksGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tracksGroupBox.Controls.Add(this.listViewStreams);
            this.tracksGroupBox.Location = new System.Drawing.Point(4, -1);
            this.tracksGroupBox.Name = "tracksGroupBox";
            this.tracksGroupBox.Size = new System.Drawing.Size(730, 96);
            this.tracksGroupBox.TabIndex = 0;
            this.tracksGroupBox.TabStop = false;
            this.tracksGroupBox.Text = "Tracks";
            // 
            // listViewStreams
            // 
            this.listViewStreams.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderStreamCodec,
            this.columnHeaderStreamLanguage,
            this.columnHeaderBitrate,
            this.columnHeaderDescription});
            this.listViewStreams.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewStreams.FullRowSelect = true;
            this.listViewStreams.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewStreams.HideSelection = false;
            this.listViewStreams.Location = new System.Drawing.Point(3, 16);
            this.listViewStreams.MultiSelect = false;
            this.listViewStreams.Name = "listViewStreams";
            this.listViewStreams.Size = new System.Drawing.Size(724, 77);
            this.listViewStreams.TabIndex = 0;
            this.listViewStreams.UseCompatibleStateImageBehavior = false;
            this.listViewStreams.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderStreamCodec
            // 
            this.columnHeaderStreamCodec.Text = "Codec";
            this.columnHeaderStreamCodec.Width = 103;
            // 
            // columnHeaderStreamLanguage
            // 
            this.columnHeaderStreamLanguage.Text = "Language";
            this.columnHeaderStreamLanguage.Width = 151;
            // 
            // columnHeaderBitrate
            // 
            this.columnHeaderBitrate.Text = "Bit Rate";
            // 
            // columnHeaderDescription
            // 
            this.columnHeaderDescription.Text = "Description";
            this.columnHeaderDescription.Width = 306;
            // 
            // splitContainerTracksOuter
            // 
            this.splitContainerTracksOuter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerTracksOuter.Location = new System.Drawing.Point(3, 16);
            this.splitContainerTracksOuter.Name = "splitContainerTracksOuter";
            this.splitContainerTracksOuter.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerTracksOuter.Panel1
            // 
            this.splitContainerTracksOuter.Panel1.Controls.Add(this.labelPlaylist);
            this.splitContainerTracksOuter.Panel1.Controls.Add(this.comboBoxPlaylist);
            this.splitContainerTracksOuter.Panel1.Controls.Add(this.listViewVideoTracks);
            this.splitContainerTracksOuter.Panel1.Controls.Add(this.labelVideoTracks);
            // 
            // splitContainerTracksOuter.Panel2
            // 
            this.splitContainerTracksOuter.Panel2.Controls.Add(this.splitContainerTracksInner);
            this.splitContainerTracksOuter.Size = new System.Drawing.Size(480, 363);
            this.splitContainerTracksOuter.SplitterDistance = 118;
            this.splitContainerTracksOuter.TabIndex = 5;
            // 
            // labelPlaylist
            // 
            this.labelPlaylist.AutoSize = true;
            this.labelPlaylist.Location = new System.Drawing.Point(5, 4);
            this.labelPlaylist.Name = "labelPlaylist";
            this.labelPlaylist.Size = new System.Drawing.Size(42, 13);
            this.labelPlaylist.TabIndex = 0;
            this.labelPlaylist.Text = "Playlist:";
            // 
            // comboBoxPlaylist
            // 
            this.comboBoxPlaylist.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxPlaylist.FormattingEnabled = true;
            this.comboBoxPlaylist.Location = new System.Drawing.Point(92, 1);
            this.comboBoxPlaylist.Name = "comboBoxPlaylist";
            this.comboBoxPlaylist.Size = new System.Drawing.Size(121, 21);
            this.comboBoxPlaylist.TabIndex = 0;
            this.comboBoxPlaylist.SelectedIndexChanged += new System.EventHandler(this.comboBoxPlaylist_SelectedIndexChanged);
            // 
            // listViewVideoTracks
            // 
            this.listViewVideoTracks.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewVideoTracks.CheckBoxes = true;
            this.listViewVideoTracks.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderVideoCodec,
            this.columnHeaderVideoResolution,
            this.columnHeaderVideoFrameRate,
            this.columnHeaderVideoAspectRatio});
            this.listViewVideoTracks.FullRowSelect = true;
            this.listViewVideoTracks.Location = new System.Drawing.Point(92, 28);
            this.listViewVideoTracks.Name = "listViewVideoTracks";
            this.listViewVideoTracks.Size = new System.Drawing.Size(384, 88);
            this.listViewVideoTracks.TabIndex = 1;
            this.listViewVideoTracks.UseCompatibleStateImageBehavior = false;
            this.listViewVideoTracks.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderVideoCodec
            // 
            this.columnHeaderVideoCodec.Text = "Codec";
            this.columnHeaderVideoCodec.Width = 134;
            // 
            // columnHeaderVideoResolution
            // 
            this.columnHeaderVideoResolution.Text = "Resolution";
            this.columnHeaderVideoResolution.Width = 70;
            // 
            // columnHeaderVideoFrameRate
            // 
            this.columnHeaderVideoFrameRate.Text = "Frame Rate";
            this.columnHeaderVideoFrameRate.Width = 74;
            // 
            // columnHeaderVideoAspectRatio
            // 
            this.columnHeaderVideoAspectRatio.Text = "Aspect Ratio";
            this.columnHeaderVideoAspectRatio.Width = 78;
            // 
            // labelVideoTracks
            // 
            this.labelVideoTracks.AutoSize = true;
            this.labelVideoTracks.Location = new System.Drawing.Point(5, 32);
            this.labelVideoTracks.Name = "labelVideoTracks";
            this.labelVideoTracks.Size = new System.Drawing.Size(69, 13);
            this.labelVideoTracks.TabIndex = 4;
            this.labelVideoTracks.Text = "Video tracks:";
            // 
            // splitContainerTracksInner
            // 
            this.splitContainerTracksInner.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerTracksInner.Location = new System.Drawing.Point(0, 0);
            this.splitContainerTracksInner.Name = "splitContainerTracksInner";
            this.splitContainerTracksInner.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerTracksInner.Panel1
            // 
            this.splitContainerTracksInner.Panel1.Controls.Add(this.listViewAudioTracks);
            this.splitContainerTracksInner.Panel1.Controls.Add(this.labelAudioTracks);
            // 
            // splitContainerTracksInner.Panel2
            // 
            this.splitContainerTracksInner.Panel2.Controls.Add(this.listViewSubtitleTracks);
            this.splitContainerTracksInner.Panel2.Controls.Add(this.labelSubtitleTracks);
            this.splitContainerTracksInner.Size = new System.Drawing.Size(480, 241);
            this.splitContainerTracksInner.SplitterDistance = 120;
            this.splitContainerTracksInner.TabIndex = 0;
            // 
            // listViewAudioTracks
            // 
            this.listViewAudioTracks.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewAudioTracks.CheckBoxes = true;
            this.listViewAudioTracks.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderAudioCodec,
            this.columnHeaderAudioLanguage,
            this.columnHeaderAudioChannels});
            this.listViewAudioTracks.FullRowSelect = true;
            this.listViewAudioTracks.Location = new System.Drawing.Point(91, 3);
            this.listViewAudioTracks.Name = "listViewAudioTracks";
            this.listViewAudioTracks.Size = new System.Drawing.Size(386, 114);
            this.listViewAudioTracks.TabIndex = 0;
            this.listViewAudioTracks.UseCompatibleStateImageBehavior = false;
            this.listViewAudioTracks.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderAudioCodec
            // 
            this.columnHeaderAudioCodec.Text = "Codec";
            this.columnHeaderAudioCodec.Width = 137;
            // 
            // columnHeaderAudioLanguage
            // 
            this.columnHeaderAudioLanguage.Text = "Language";
            this.columnHeaderAudioLanguage.Width = 100;
            // 
            // columnHeaderAudioChannels
            // 
            this.columnHeaderAudioChannels.Text = "Channels";
            // 
            // labelAudioTracks
            // 
            this.labelAudioTracks.AutoSize = true;
            this.labelAudioTracks.Location = new System.Drawing.Point(5, 5);
            this.labelAudioTracks.Name = "labelAudioTracks";
            this.labelAudioTracks.Size = new System.Drawing.Size(69, 13);
            this.labelAudioTracks.TabIndex = 5;
            this.labelAudioTracks.Text = "Audio tracks:";
            // 
            // listViewSubtitleTracks
            // 
            this.listViewSubtitleTracks.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewSubtitleTracks.CheckBoxes = true;
            this.listViewSubtitleTracks.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderSubtitleCodec,
            this.columnHeaderSubtitleLanguage});
            this.listViewSubtitleTracks.FullRowSelect = true;
            this.listViewSubtitleTracks.Location = new System.Drawing.Point(91, 3);
            this.listViewSubtitleTracks.Name = "listViewSubtitleTracks";
            this.listViewSubtitleTracks.Size = new System.Drawing.Size(386, 111);
            this.listViewSubtitleTracks.TabIndex = 0;
            this.listViewSubtitleTracks.UseCompatibleStateImageBehavior = false;
            this.listViewSubtitleTracks.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderSubtitleCodec
            // 
            this.columnHeaderSubtitleCodec.Text = "Codec";
            this.columnHeaderSubtitleCodec.Width = 152;
            // 
            // columnHeaderSubtitleLanguage
            // 
            this.columnHeaderSubtitleLanguage.Text = "Language";
            this.columnHeaderSubtitleLanguage.Width = 100;
            // 
            // labelSubtitleTracks
            // 
            this.labelSubtitleTracks.AutoSize = true;
            this.labelSubtitleTracks.Location = new System.Drawing.Point(5, 5);
            this.labelSubtitleTracks.Name = "labelSubtitleTracks";
            this.labelSubtitleTracks.Size = new System.Drawing.Size(77, 13);
            this.labelSubtitleTracks.TabIndex = 6;
            this.labelSubtitleTracks.Text = "Subtitle tracks:";
            // 
            // FormDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(781, 749);
            this.Controls.Add(this.buttonRescan);
            this.Controls.Add(this.buttonBrowse);
            this.Controls.Add(this.textBoxSource);
            this.Controls.Add(this.labelSource);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.continueButton);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FormDetails";
            this.Text = "BDAutoMuxer v0.0.0";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormDetails_FormClosing);
            this.tabControl.ResumeLayout(false);
            this.tabPageDisc.ResumeLayout(false);
            this.tabPageDisc.PerformLayout();
            this.tabPageOutput.ResumeLayout(false);
            this.groupBoxOutput.ResumeLayout(false);
            this.groupBoxOutput.PerformLayout();
            this.groupBoxTracks.ResumeLayout(false);
            this.groupBoxFilter.ResumeLayout(false);
            this.groupBoxFilter.PerformLayout();
            this.groupBoxMasterOverride.ResumeLayout(false);
            this.groupBoxMasterOverride.PerformLayout();
            this.tabPageProgress.ResumeLayout(false);
            this.groupBoxTsMuxerProgress.ResumeLayout(false);
            this.groupBoxTsMuxerProgress.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainerDiscOuter.Panel1.ResumeLayout(false);
            this.splitContainerDiscOuter.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerDiscOuter)).EndInit();
            this.splitContainerDiscOuter.ResumeLayout(false);
            this.panelMoviePoster.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMoviePoster)).EndInit();
            this.panelMovieDetails.ResumeLayout(false);
            this.panelMovieDetails.PerformLayout();
            this.playlistsSplitContainerOuter.Panel1.ResumeLayout(false);
            this.playlistsSplitContainerOuter.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.playlistsSplitContainerOuter)).EndInit();
            this.playlistsSplitContainerOuter.ResumeLayout(false);
            this.playlistsGroupBox.ResumeLayout(false);
            this.playlistsGroupBox.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.playlistDataGridView)).EndInit();
            this.playlistsSplitContainerInner.Panel1.ResumeLayout(false);
            this.playlistsSplitContainerInner.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.playlistsSplitContainerInner)).EndInit();
            this.playlistsSplitContainerInner.ResumeLayout(false);
            this.streamsGroupBox.ResumeLayout(false);
            this.tracksGroupBox.ResumeLayout(false);
            this.splitContainerTracksOuter.Panel1.ResumeLayout(false);
            this.splitContainerTracksOuter.Panel1.PerformLayout();
            this.splitContainerTracksOuter.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerTracksOuter)).EndInit();
            this.splitContainerTracksOuter.ResumeLayout(false);
            this.splitContainerTracksInner.Panel1.ResumeLayout(false);
            this.splitContainerTracksInner.Panel1.PerformLayout();
            this.splitContainerTracksInner.Panel2.ResumeLayout(false);
            this.splitContainerTracksInner.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerTracksInner)).EndInit();
            this.splitContainerTracksInner.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button continueButton;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPageDisc;
        private System.Windows.Forms.TabPage tabPageOutput;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.TabPage tabPageProgress;
        private System.Windows.Forms.GroupBox groupBoxMasterOverride;
        private System.Windows.Forms.ComboBox comboBoxAudienceLanguage;
        private System.Windows.Forms.Label labelAudienceLanguage;
        private System.Windows.Forms.GroupBox groupBoxFilter;
        private System.Windows.Forms.Label labelVideoLanguage;
        private System.Windows.Forms.GroupBox groupBoxOutput;
        private System.Windows.Forms.Label labelOutputFileExtension;
        private System.Windows.Forms.Button buttonOutputDir;
        private System.Windows.Forms.TextBox textBoxOutputDir;
        private System.Windows.Forms.TextBox textBoxOutputFileName;
        private System.Windows.Forms.Label labelOutputFileName;
        private System.Windows.Forms.Label labelOutputDirectory;
        private System.Windows.Forms.GroupBox groupBoxTracks;
        private System.Windows.Forms.ComboBox comboBoxPlaylist;
        private System.Windows.Forms.Label labelPlaylist;
        private System.Windows.Forms.ListBox listBoxSubtitleLanguages;
        private System.Windows.Forms.ListBox listBoxAudioLanguages;
        private System.Windows.Forms.ComboBox comboBoxCommentary;
        private System.Windows.Forms.ComboBox comboBoxCut;
        private System.Windows.Forms.ComboBox comboBoxVideoLanguage;
        private System.Windows.Forms.Label labelSubtitleLanguages;
        private System.Windows.Forms.Label labelAudioLanguages;
        private System.Windows.Forms.Label labelCommentary;
        private System.Windows.Forms.Label labelCut;
        private System.Windows.Forms.TextBox textBoxReplaceSpaces;
        private System.Windows.Forms.CheckBox checkBoxReplaceSpaces;
        private System.Windows.Forms.Button buttonSubmitToDB;
        private System.Windows.Forms.GroupBox groupBoxTsMuxerProgress;
        private System.Windows.Forms.ProgressBar progressBarTsMuxer;
        private System.Windows.Forms.Label labelTsMuxerProgress;
        private System.Windows.Forms.TextBox textBoxTsMuxerCommandLine;
        private System.Windows.Forms.Label labelTsMuxerCommandLine;
        private System.Windows.Forms.Label labelTsMuxerTimeRemaining;
        private System.Windows.Forms.Label labelTsMuxerTimeElapsed;
        private System.Windows.Forms.Label labelTsMuxerTimeSeparator;
        private System.Windows.Forms.Label labelTsMuxerTimeRemainingElapsed;
        private System.Windows.Forms.Label labelOutputPlaceholders;
        private System.Windows.Forms.TextBox textBoxOutputFileNameHint;
        private System.Windows.Forms.TextBox textBoxOutputFileNamePreview;
        private System.Windows.Forms.TextBox textBoxOutputDirPreview;
        private BDAutoMuxer.views.SplitContainerWithDivider splitContainerTracksOuter;
        private System.Windows.Forms.ListView listViewVideoTracks;
        private System.Windows.Forms.ColumnHeader columnHeaderVideoCodec;
        private System.Windows.Forms.ColumnHeader columnHeaderVideoResolution;
        private System.Windows.Forms.ColumnHeader columnHeaderVideoFrameRate;
        private System.Windows.Forms.ColumnHeader columnHeaderVideoAspectRatio;
        private System.Windows.Forms.Label labelVideoTracks;
        private BDAutoMuxer.views.SplitContainerWithDivider splitContainerTracksInner;
        private System.Windows.Forms.ListView listViewAudioTracks;
        private System.Windows.Forms.ColumnHeader columnHeaderAudioCodec;
        private System.Windows.Forms.ColumnHeader columnHeaderAudioLanguage;
        private System.Windows.Forms.ColumnHeader columnHeaderAudioChannels;
        private System.Windows.Forms.Label labelAudioTracks;
        private System.Windows.Forms.ListView listViewSubtitleTracks;
        private System.Windows.Forms.ColumnHeader columnHeaderSubtitleCodec;
        private System.Windows.Forms.ColumnHeader columnHeaderSubtitleLanguage;
        private System.Windows.Forms.Label labelSubtitleTracks;
        private views.SplitContainerWithDivider splitContainerDiscOuter;
        private System.Windows.Forms.MaskedTextBox maskedTextBoxYear;
        private System.Windows.Forms.ComboBox discLanguageComboBox;
        private System.Windows.Forms.Label discLanguageLabel;
        private System.Windows.Forms.Label movieNameLabel;
        private System.Windows.Forms.ListView searchResultListView;
        private System.Windows.Forms.ColumnHeader NameColumn;
        private System.Windows.Forms.ColumnHeader YearColumn;
        private System.Windows.Forms.ColumnHeader PopularityColumn;
        private System.Windows.Forms.TextBox movieNameTextBox;
        private System.Windows.Forms.Button searchButton;
        private views.SplitContainerWithDivider playlistsSplitContainerOuter;
        private System.Windows.Forms.GroupBox playlistsGroupBox;
        private System.Windows.Forms.Button buttonUnselectAll;
        private System.Windows.Forms.Button buttonSelectAll;
        private System.Windows.Forms.CheckBox showAllPlaylistsCheckbox;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView playlistDataGridView;
        private views.SplitContainerWithDivider playlistsSplitContainerInner;
        private System.Windows.Forms.GroupBox streamsGroupBox;
        private System.Windows.Forms.ListView listViewStreamFiles;
        private System.Windows.Forms.ColumnHeader columnHeaderFileName;
        private System.Windows.Forms.ColumnHeader columnHeaderIndex;
        private System.Windows.Forms.ColumnHeader columnHeaderFileLength;
        private System.Windows.Forms.ColumnHeader columnHeaderFileEstimatedBytes;
        private System.Windows.Forms.ColumnHeader columnHeaderFileMeasuredBytes;
        private System.Windows.Forms.GroupBox tracksGroupBox;
        private System.Windows.Forms.ListView listViewStreams;
        private System.Windows.Forms.ColumnHeader columnHeaderStreamCodec;
        private System.Windows.Forms.ColumnHeader columnHeaderStreamLanguage;
        private System.Windows.Forms.ColumnHeader columnHeaderBitrate;
        private System.Windows.Forms.ColumnHeader columnHeaderDescription;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar;
        private System.Windows.Forms.ToolStripStatusLabel progressLabel;
        private System.Windows.Forms.Label hiddenTrackLabel;
        private System.Windows.Forms.Panel panelMovieDetails;
        private System.Windows.Forms.Panel panelMoviePoster;
        private System.Windows.Forms.PictureBox pictureBoxMoviePoster;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openDiscToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rescanToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem submitToDbToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem remuxerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.Button buttonRescan;
        private System.Windows.Forms.Button buttonBrowse;
        private System.Windows.Forms.TextBox textBoxSource;
        private System.Windows.Forms.Label labelSource;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.ToolStripMenuItem checkForUpdatesToolStripMenuItem;
    }
}