namespace BDInfo
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
            this.movieNameLabel = new System.Windows.Forms.Label();
            this.continueButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.movieNameTextBox = new System.Windows.Forms.TextBox();
            this.searchButton = new System.Windows.Forms.Button();
            this.searchResultListView = new System.Windows.Forms.ListView();
            this.NameColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.YearColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.PopularityColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageDisc = new System.Windows.Forms.TabPage();
            this.discLanguageComboBox = new System.Windows.Forms.ComboBox();
            this.discLanguageLabel = new System.Windows.Forms.Label();
            this.tabPagePlaylists = new System.Windows.Forms.TabPage();
            this.playlistsSplitContainerOuter = new System.Windows.Forms.SplitContainer();
            this.playlistsGroupBox = new System.Windows.Forms.GroupBox();
            this.showAllPlaylistsCheckbox = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.playlistDataGridView = new System.Windows.Forms.DataGridView();
            this.playlistsSplitContainerInner = new System.Windows.Forms.SplitContainer();
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
            this.tabPageOutput = new System.Windows.Forms.TabPage();
            this.groupBoxOutput = new System.Windows.Forms.GroupBox();
            this.textBoxReplaceSpaces = new System.Windows.Forms.TextBox();
            this.checkBoxReplaceSpaces = new System.Windows.Forms.CheckBox();
            this.labelOutputFileNamePreview = new System.Windows.Forms.Label();
            this.labelOutputFileNameHelp = new System.Windows.Forms.Label();
            this.labelOutputFileExtension = new System.Windows.Forms.Label();
            this.buttonOutputDir = new System.Windows.Forms.Button();
            this.textBoxOutputDir = new System.Windows.Forms.TextBox();
            this.textBoxOutputFileName = new System.Windows.Forms.TextBox();
            this.labelOutputPreview = new System.Windows.Forms.Label();
            this.labelOutputFileName = new System.Windows.Forms.Label();
            this.labelOutputDirectory = new System.Windows.Forms.Label();
            this.groupBoxTracks = new System.Windows.Forms.GroupBox();
            this.listViewSubtitleTracks = new System.Windows.Forms.ListView();
            this.columnHeaderSubtitleId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderSubtitleLanguage = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.listViewAudioTracks = new System.Windows.Forms.ListView();
            this.columnHeaderCodec = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderLanguage = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderChannels = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.checkedListBoxVideoResolution = new System.Windows.Forms.CheckedListBox();
            this.labelSubtitleTracks = new System.Windows.Forms.Label();
            this.labelAudioTracks = new System.Windows.Forms.Label();
            this.labelVideoResolution = new System.Windows.Forms.Label();
            this.comboBoxPlaylist = new System.Windows.Forms.ComboBox();
            this.labelPlaylist = new System.Windows.Forms.Label();
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
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.progressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.tabControl.SuspendLayout();
            this.tabPageDisc.SuspendLayout();
            this.tabPagePlaylists.SuspendLayout();
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
            this.tabPageOutput.SuspendLayout();
            this.groupBoxOutput.SuspendLayout();
            this.groupBoxTracks.SuspendLayout();
            this.groupBoxFilter.SuspendLayout();
            this.groupBoxMasterOverride.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // movieNameLabel
            // 
            this.movieNameLabel.AutoSize = true;
            this.movieNameLabel.Location = new System.Drawing.Point(6, 36);
            this.movieNameLabel.Name = "movieNameLabel";
            this.movieNameLabel.Size = new System.Drawing.Size(68, 13);
            this.movieNameLabel.TabIndex = 0;
            this.movieNameLabel.Text = "Movie name:";
            // 
            // continueButton
            // 
            this.continueButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.continueButton.Enabled = false;
            this.continueButton.Location = new System.Drawing.Point(551, 613);
            this.continueButton.Name = "continueButton";
            this.continueButton.Size = new System.Drawing.Size(75, 23);
            this.continueButton.TabIndex = 98;
            this.continueButton.Text = "Continue";
            this.continueButton.UseVisualStyleBackColor = true;
            this.continueButton.Click += new System.EventHandler(this.continueButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(632, 613);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 99;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // movieNameTextBox
            // 
            this.movieNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.movieNameTextBox.Location = new System.Drawing.Point(96, 33);
            this.movieNameTextBox.Name = "movieNameTextBox";
            this.movieNameTextBox.Size = new System.Drawing.Size(481, 20);
            this.movieNameTextBox.TabIndex = 2;
            // 
            // searchButton
            // 
            this.searchButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.searchButton.Location = new System.Drawing.Point(583, 31);
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(98, 23);
            this.searchButton.TabIndex = 3;
            this.searchButton.Text = "Search TMDb";
            this.searchButton.UseVisualStyleBackColor = true;
            this.searchButton.Click += new System.EventHandler(this.searchButton_Click);
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
            this.searchResultListView.Location = new System.Drawing.Point(6, 60);
            this.searchResultListView.MultiSelect = false;
            this.searchResultListView.Name = "searchResultListView";
            this.searchResultListView.Size = new System.Drawing.Size(675, 461);
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
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Controls.Add(this.tabPageDisc);
            this.tabControl.Controls.Add(this.tabPagePlaylists);
            this.tabControl.Controls.Add(this.tabPageOutput);
            this.tabControl.Controls.Add(this.tabPageProgress);
            this.tabControl.Location = new System.Drawing.Point(12, 12);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(695, 595);
            this.tabControl.TabIndex = 0;
            // 
            // tabPageDisc
            // 
            this.tabPageDisc.Controls.Add(this.discLanguageComboBox);
            this.tabPageDisc.Controls.Add(this.discLanguageLabel);
            this.tabPageDisc.Controls.Add(this.movieNameLabel);
            this.tabPageDisc.Controls.Add(this.searchResultListView);
            this.tabPageDisc.Controls.Add(this.movieNameTextBox);
            this.tabPageDisc.Controls.Add(this.searchButton);
            this.tabPageDisc.Location = new System.Drawing.Point(4, 22);
            this.tabPageDisc.Name = "tabPageDisc";
            this.tabPageDisc.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDisc.Size = new System.Drawing.Size(687, 569);
            this.tabPageDisc.TabIndex = 0;
            this.tabPageDisc.Text = "Disc";
            this.tabPageDisc.UseVisualStyleBackColor = true;
            // 
            // discLanguageComboBox
            // 
            this.discLanguageComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.discLanguageComboBox.FormattingEnabled = true;
            this.discLanguageComboBox.Location = new System.Drawing.Point(96, 6);
            this.discLanguageComboBox.Name = "discLanguageComboBox";
            this.discLanguageComboBox.Size = new System.Drawing.Size(121, 21);
            this.discLanguageComboBox.TabIndex = 1;
            this.discLanguageComboBox.SelectedIndexChanged += new System.EventHandler(this.discLanguageComboBox_SelectedIndexChanged);
            // 
            // discLanguageLabel
            // 
            this.discLanguageLabel.AutoSize = true;
            this.discLanguageLabel.Location = new System.Drawing.Point(6, 9);
            this.discLanguageLabel.Name = "discLanguageLabel";
            this.discLanguageLabel.Size = new System.Drawing.Size(80, 13);
            this.discLanguageLabel.TabIndex = 3;
            this.discLanguageLabel.Text = "Main language:";
            // 
            // tabPagePlaylists
            // 
            this.tabPagePlaylists.Controls.Add(this.playlistsSplitContainerOuter);
            this.tabPagePlaylists.Location = new System.Drawing.Point(4, 22);
            this.tabPagePlaylists.Name = "tabPagePlaylists";
            this.tabPagePlaylists.Padding = new System.Windows.Forms.Padding(3);
            this.tabPagePlaylists.Size = new System.Drawing.Size(687, 569);
            this.tabPagePlaylists.TabIndex = 1;
            this.tabPagePlaylists.Text = "Playlists";
            this.tabPagePlaylists.UseVisualStyleBackColor = true;
            // 
            // playlistsSplitContainerOuter
            // 
            this.playlistsSplitContainerOuter.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.playlistsSplitContainerOuter.BackColor = System.Drawing.Color.Transparent;
            this.playlistsSplitContainerOuter.Location = new System.Drawing.Point(6, 7);
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
            this.playlistsSplitContainerOuter.Size = new System.Drawing.Size(675, 556);
            this.playlistsSplitContainerOuter.SplitterDistance = 234;
            this.playlistsSplitContainerOuter.TabIndex = 5;
            // 
            // playlistsGroupBox
            // 
            this.playlistsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.playlistsGroupBox.Controls.Add(this.showAllPlaylistsCheckbox);
            this.playlistsGroupBox.Controls.Add(this.panel1);
            this.playlistsGroupBox.Location = new System.Drawing.Point(4, 4);
            this.playlistsGroupBox.Name = "playlistsGroupBox";
            this.playlistsGroupBox.Size = new System.Drawing.Size(668, 227);
            this.playlistsGroupBox.TabIndex = 5;
            this.playlistsGroupBox.TabStop = false;
            this.playlistsGroupBox.Text = "Playlists";
            // 
            // showAllPlaylistsCheckbox
            // 
            this.showAllPlaylistsCheckbox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.showAllPlaylistsCheckbox.AutoSize = true;
            this.showAllPlaylistsCheckbox.Location = new System.Drawing.Point(6, 204);
            this.showAllPlaylistsCheckbox.Name = "showAllPlaylistsCheckbox";
            this.showAllPlaylistsCheckbox.Size = new System.Drawing.Size(105, 17);
            this.showAllPlaylistsCheckbox.TabIndex = 5;
            this.showAllPlaylistsCheckbox.Text = "Show all playlists";
            this.showAllPlaylistsCheckbox.UseVisualStyleBackColor = true;
            this.showAllPlaylistsCheckbox.CheckedChanged += new System.EventHandler(this.showAllPlaylistsCheckbox_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.playlistDataGridView);
            this.panel1.Location = new System.Drawing.Point(6, 19);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(656, 179);
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
            this.playlistDataGridView.Size = new System.Drawing.Size(650, 173);
            this.playlistDataGridView.TabIndex = 1;
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
            this.playlistsSplitContainerInner.Size = new System.Drawing.Size(672, 315);
            this.playlistsSplitContainerInner.SplitterDistance = 159;
            this.playlistsSplitContainerInner.TabIndex = 0;
            // 
            // streamsGroupBox
            // 
            this.streamsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.streamsGroupBox.Controls.Add(this.listViewStreamFiles);
            this.streamsGroupBox.Location = new System.Drawing.Point(4, 4);
            this.streamsGroupBox.Name = "streamsGroupBox";
            this.streamsGroupBox.Size = new System.Drawing.Size(665, 152);
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
            this.listViewStreamFiles.Enabled = false;
            this.listViewStreamFiles.FullRowSelect = true;
            this.listViewStreamFiles.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewStreamFiles.HideSelection = false;
            this.listViewStreamFiles.Location = new System.Drawing.Point(3, 16);
            this.listViewStreamFiles.MultiSelect = false;
            this.listViewStreamFiles.Name = "listViewStreamFiles";
            this.listViewStreamFiles.Size = new System.Drawing.Size(659, 133);
            this.listViewStreamFiles.TabIndex = 7;
            this.listViewStreamFiles.UseCompatibleStateImageBehavior = false;
            this.listViewStreamFiles.View = System.Windows.Forms.View.Details;
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
            this.tracksGroupBox.Location = new System.Drawing.Point(4, 4);
            this.tracksGroupBox.Name = "tracksGroupBox";
            this.tracksGroupBox.Size = new System.Drawing.Size(665, 145);
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
            this.listViewStreams.Enabled = false;
            this.listViewStreams.FullRowSelect = true;
            this.listViewStreams.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewStreams.HideSelection = false;
            this.listViewStreams.Location = new System.Drawing.Point(3, 16);
            this.listViewStreams.MultiSelect = false;
            this.listViewStreams.Name = "listViewStreams";
            this.listViewStreams.Size = new System.Drawing.Size(659, 126);
            this.listViewStreams.TabIndex = 8;
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
            // tabPageOutput
            // 
            this.tabPageOutput.Controls.Add(this.groupBoxOutput);
            this.tabPageOutput.Controls.Add(this.groupBoxTracks);
            this.tabPageOutput.Controls.Add(this.groupBoxFilter);
            this.tabPageOutput.Controls.Add(this.groupBoxMasterOverride);
            this.tabPageOutput.Location = new System.Drawing.Point(4, 22);
            this.tabPageOutput.Name = "tabPageOutput";
            this.tabPageOutput.Size = new System.Drawing.Size(687, 569);
            this.tabPageOutput.TabIndex = 2;
            this.tabPageOutput.Text = "Output";
            this.tabPageOutput.UseVisualStyleBackColor = true;
            // 
            // groupBoxOutput
            // 
            this.groupBoxOutput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxOutput.Controls.Add(this.textBoxReplaceSpaces);
            this.groupBoxOutput.Controls.Add(this.checkBoxReplaceSpaces);
            this.groupBoxOutput.Controls.Add(this.labelOutputFileNamePreview);
            this.groupBoxOutput.Controls.Add(this.labelOutputFileNameHelp);
            this.groupBoxOutput.Controls.Add(this.labelOutputFileExtension);
            this.groupBoxOutput.Controls.Add(this.buttonOutputDir);
            this.groupBoxOutput.Controls.Add(this.textBoxOutputDir);
            this.groupBoxOutput.Controls.Add(this.textBoxOutputFileName);
            this.groupBoxOutput.Controls.Add(this.labelOutputPreview);
            this.groupBoxOutput.Controls.Add(this.labelOutputFileName);
            this.groupBoxOutput.Controls.Add(this.labelOutputDirectory);
            this.groupBoxOutput.Location = new System.Drawing.Point(4, 450);
            this.groupBoxOutput.Name = "groupBoxOutput";
            this.groupBoxOutput.Size = new System.Drawing.Size(680, 116);
            this.groupBoxOutput.TabIndex = 3;
            this.groupBoxOutput.TabStop = false;
            this.groupBoxOutput.Text = "Output";
            // 
            // textBoxReplaceSpaces
            // 
            this.textBoxReplaceSpaces.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxReplaceSpaces.Enabled = false;
            this.textBoxReplaceSpaces.Location = new System.Drawing.Point(641, 70);
            this.textBoxReplaceSpaces.Name = "textBoxReplaceSpaces";
            this.textBoxReplaceSpaces.Size = new System.Drawing.Size(29, 20);
            this.textBoxReplaceSpaces.TabIndex = 10;
            this.textBoxReplaceSpaces.Text = ".";
            this.textBoxReplaceSpaces.TextChanged += new System.EventHandler(this.textBoxReplaceSpaces_TextChanged);
            this.textBoxReplaceSpaces.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxReplaceSpaces_KeyPress);
            // 
            // checkBoxReplaceSpaces
            // 
            this.checkBoxReplaceSpaces.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxReplaceSpaces.AutoSize = true;
            this.checkBoxReplaceSpaces.Location = new System.Drawing.Point(504, 72);
            this.checkBoxReplaceSpaces.Name = "checkBoxReplaceSpaces";
            this.checkBoxReplaceSpaces.Size = new System.Drawing.Size(128, 17);
            this.checkBoxReplaceSpaces.TabIndex = 9;
            this.checkBoxReplaceSpaces.Text = "Replace spaces with:";
            this.checkBoxReplaceSpaces.UseVisualStyleBackColor = true;
            this.checkBoxReplaceSpaces.CheckedChanged += new System.EventHandler(this.checkBoxReplaceSpaces_CheckedChanged);
            // 
            // labelOutputFileNamePreview
            // 
            this.labelOutputFileNamePreview.AutoSize = true;
            this.labelOutputFileNamePreview.Location = new System.Drawing.Point(64, 93);
            this.labelOutputFileNamePreview.Name = "labelOutputFileNamePreview";
            this.labelOutputFileNamePreview.Size = new System.Drawing.Size(177, 13);
            this.labelOutputFileNamePreview.TabIndex = 8;
            this.labelOutputFileNamePreview.Text = "The Incredibles (2006) [1080p].m2ts";
            // 
            // labelOutputFileNameHelp
            // 
            this.labelOutputFileNameHelp.AutoSize = true;
            this.labelOutputFileNameHelp.Location = new System.Drawing.Point(67, 70);
            this.labelOutputFileNameHelp.Name = "labelOutputFileNameHelp";
            this.labelOutputFileNameHelp.Size = new System.Drawing.Size(117, 13);
            this.labelOutputFileNameHelp.TabIndex = 7;
            this.labelOutputFileNameHelp.Text = "%title%  %year%  %res%";
            // 
            // labelOutputFileExtension
            // 
            this.labelOutputFileExtension.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelOutputFileExtension.AutoSize = true;
            this.labelOutputFileExtension.Location = new System.Drawing.Point(638, 46);
            this.labelOutputFileExtension.Name = "labelOutputFileExtension";
            this.labelOutputFileExtension.Size = new System.Drawing.Size(32, 13);
            this.labelOutputFileExtension.TabIndex = 6;
            this.labelOutputFileExtension.Text = ".m2ts";
            // 
            // buttonOutputDir
            // 
            this.buttonOutputDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOutputDir.Location = new System.Drawing.Point(599, 15);
            this.buttonOutputDir.Name = "buttonOutputDir";
            this.buttonOutputDir.Size = new System.Drawing.Size(75, 23);
            this.buttonOutputDir.TabIndex = 5;
            this.buttonOutputDir.Text = "Browse...";
            this.buttonOutputDir.UseVisualStyleBackColor = true;
            this.buttonOutputDir.Click += new System.EventHandler(this.buttonOutputDir_Click);
            // 
            // textBoxOutputDir
            // 
            this.textBoxOutputDir.AllowDrop = true;
            this.textBoxOutputDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxOutputDir.Location = new System.Drawing.Point(67, 17);
            this.textBoxOutputDir.Name = "textBoxOutputDir";
            this.textBoxOutputDir.Size = new System.Drawing.Size(526, 20);
            this.textBoxOutputDir.TabIndex = 4;
            this.textBoxOutputDir.DragDrop += new System.Windows.Forms.DragEventHandler(this.textBoxOutputDir_DragDrop);
            this.textBoxOutputDir.DragEnter += new System.Windows.Forms.DragEventHandler(this.textBoxOutputDir_DragEnter);
            // 
            // textBoxOutputFileName
            // 
            this.textBoxOutputFileName.AllowDrop = true;
            this.textBoxOutputFileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxOutputFileName.Location = new System.Drawing.Point(67, 43);
            this.textBoxOutputFileName.Name = "textBoxOutputFileName";
            this.textBoxOutputFileName.Size = new System.Drawing.Size(565, 20);
            this.textBoxOutputFileName.TabIndex = 3;
            this.textBoxOutputFileName.Text = "%title% (%year%) [%res%]";
            this.textBoxOutputFileName.TextChanged += new System.EventHandler(this.textBoxOutputFileName_TextChanged);
            this.textBoxOutputFileName.DragDrop += new System.Windows.Forms.DragEventHandler(this.textBoxOutputFileName_DragDrop);
            this.textBoxOutputFileName.DragEnter += new System.Windows.Forms.DragEventHandler(this.textBoxOutputFileName_DragEnter);
            // 
            // labelOutputPreview
            // 
            this.labelOutputPreview.AutoSize = true;
            this.labelOutputPreview.Location = new System.Drawing.Point(6, 93);
            this.labelOutputPreview.Name = "labelOutputPreview";
            this.labelOutputPreview.Size = new System.Drawing.Size(48, 13);
            this.labelOutputPreview.TabIndex = 2;
            this.labelOutputPreview.Text = "Preview:";
            // 
            // labelOutputFileName
            // 
            this.labelOutputFileName.AutoSize = true;
            this.labelOutputFileName.Location = new System.Drawing.Point(6, 46);
            this.labelOutputFileName.Name = "labelOutputFileName";
            this.labelOutputFileName.Size = new System.Drawing.Size(55, 13);
            this.labelOutputFileName.TabIndex = 1;
            this.labelOutputFileName.Text = "File name:";
            // 
            // labelOutputDirectory
            // 
            this.labelOutputDirectory.AutoSize = true;
            this.labelOutputDirectory.Location = new System.Drawing.Point(6, 20);
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
            this.groupBoxTracks.Controls.Add(this.listViewSubtitleTracks);
            this.groupBoxTracks.Controls.Add(this.listViewAudioTracks);
            this.groupBoxTracks.Controls.Add(this.checkedListBoxVideoResolution);
            this.groupBoxTracks.Controls.Add(this.labelSubtitleTracks);
            this.groupBoxTracks.Controls.Add(this.labelAudioTracks);
            this.groupBoxTracks.Controls.Add(this.labelVideoResolution);
            this.groupBoxTracks.Controls.Add(this.comboBoxPlaylist);
            this.groupBoxTracks.Controls.Add(this.labelPlaylist);
            this.groupBoxTracks.Location = new System.Drawing.Point(256, 61);
            this.groupBoxTracks.Name = "groupBoxTracks";
            this.groupBoxTracks.Size = new System.Drawing.Size(428, 383);
            this.groupBoxTracks.TabIndex = 2;
            this.groupBoxTracks.TabStop = false;
            this.groupBoxTracks.Text = "Select Playlist / Tracks";
            // 
            // listViewSubtitleTracks
            // 
            this.listViewSubtitleTracks.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewSubtitleTracks.CheckBoxes = true;
            this.listViewSubtitleTracks.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderSubtitleId,
            this.columnHeaderSubtitleLanguage});
            this.listViewSubtitleTracks.Location = new System.Drawing.Point(99, 250);
            this.listViewSubtitleTracks.Name = "listViewSubtitleTracks";
            this.listViewSubtitleTracks.Size = new System.Drawing.Size(323, 127);
            this.listViewSubtitleTracks.TabIndex = 7;
            this.listViewSubtitleTracks.UseCompatibleStateImageBehavior = false;
            this.listViewSubtitleTracks.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderSubtitleId
            // 
            this.columnHeaderSubtitleId.Text = "#";
            this.columnHeaderSubtitleId.Width = 46;
            // 
            // columnHeaderSubtitleLanguage
            // 
            this.columnHeaderSubtitleLanguage.Text = "Language";
            this.columnHeaderSubtitleLanguage.Width = 100;
            // 
            // listViewAudioTracks
            // 
            this.listViewAudioTracks.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewAudioTracks.CheckBoxes = true;
            this.listViewAudioTracks.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderCodec,
            this.columnHeaderLanguage,
            this.columnHeaderChannels});
            this.listViewAudioTracks.Location = new System.Drawing.Point(99, 146);
            this.listViewAudioTracks.Name = "listViewAudioTracks";
            this.listViewAudioTracks.Size = new System.Drawing.Size(323, 97);
            this.listViewAudioTracks.TabIndex = 6;
            this.listViewAudioTracks.UseCompatibleStateImageBehavior = false;
            this.listViewAudioTracks.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderCodec
            // 
            this.columnHeaderCodec.Text = "Codec";
            this.columnHeaderCodec.Width = 100;
            // 
            // columnHeaderLanguage
            // 
            this.columnHeaderLanguage.Text = "Language";
            this.columnHeaderLanguage.Width = 100;
            // 
            // columnHeaderChannels
            // 
            this.columnHeaderChannels.Text = "Channels";
            // 
            // checkedListBoxVideoResolution
            // 
            this.checkedListBoxVideoResolution.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkedListBoxVideoResolution.FormattingEnabled = true;
            this.checkedListBoxVideoResolution.Location = new System.Drawing.Point(99, 45);
            this.checkedListBoxVideoResolution.Name = "checkedListBoxVideoResolution";
            this.checkedListBoxVideoResolution.Size = new System.Drawing.Size(323, 94);
            this.checkedListBoxVideoResolution.TabIndex = 5;
            // 
            // labelSubtitleTracks
            // 
            this.labelSubtitleTracks.AutoSize = true;
            this.labelSubtitleTracks.Location = new System.Drawing.Point(7, 253);
            this.labelSubtitleTracks.Name = "labelSubtitleTracks";
            this.labelSubtitleTracks.Size = new System.Drawing.Size(77, 13);
            this.labelSubtitleTracks.TabIndex = 4;
            this.labelSubtitleTracks.Text = "Subtitle tracks:";
            // 
            // labelAudioTracks
            // 
            this.labelAudioTracks.AutoSize = true;
            this.labelAudioTracks.Location = new System.Drawing.Point(7, 149);
            this.labelAudioTracks.Name = "labelAudioTracks";
            this.labelAudioTracks.Size = new System.Drawing.Size(69, 13);
            this.labelAudioTracks.TabIndex = 3;
            this.labelAudioTracks.Text = "Audio tracks:";
            // 
            // labelVideoResolution
            // 
            this.labelVideoResolution.AutoSize = true;
            this.labelVideoResolution.Location = new System.Drawing.Point(7, 48);
            this.labelVideoResolution.Name = "labelVideoResolution";
            this.labelVideoResolution.Size = new System.Drawing.Size(85, 13);
            this.labelVideoResolution.TabIndex = 2;
            this.labelVideoResolution.Text = "Video resolution:";
            // 
            // comboBoxPlaylist
            // 
            this.comboBoxPlaylist.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxPlaylist.FormattingEnabled = true;
            this.comboBoxPlaylist.Location = new System.Drawing.Point(98, 18);
            this.comboBoxPlaylist.Name = "comboBoxPlaylist";
            this.comboBoxPlaylist.Size = new System.Drawing.Size(121, 21);
            this.comboBoxPlaylist.TabIndex = 1;
            this.comboBoxPlaylist.SelectedIndexChanged += new System.EventHandler(this.comboBoxPlaylist_SelectedIndexChanged);
            // 
            // labelPlaylist
            // 
            this.labelPlaylist.AutoSize = true;
            this.labelPlaylist.Location = new System.Drawing.Point(7, 20);
            this.labelPlaylist.Name = "labelPlaylist";
            this.labelPlaylist.Size = new System.Drawing.Size(42, 13);
            this.labelPlaylist.TabIndex = 0;
            this.labelPlaylist.Text = "Playlist:";
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
            this.groupBoxFilter.Size = new System.Drawing.Size(246, 383);
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
            this.listBoxSubtitleLanguages.TabIndex = 9;
            this.listBoxSubtitleLanguages.SelectedValueChanged += new System.EventHandler(this.FilterControlChanged);
            // 
            // listBoxAudioLanguages
            // 
            this.listBoxAudioLanguages.FormattingEnabled = true;
            this.listBoxAudioLanguages.Location = new System.Drawing.Point(115, 101);
            this.listBoxAudioLanguages.Name = "listBoxAudioLanguages";
            this.listBoxAudioLanguages.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBoxAudioLanguages.Size = new System.Drawing.Size(120, 56);
            this.listBoxAudioLanguages.TabIndex = 8;
            this.listBoxAudioLanguages.SelectedValueChanged += new System.EventHandler(this.FilterControlChanged);
            // 
            // comboBoxCommentary
            // 
            this.comboBoxCommentary.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCommentary.FormattingEnabled = true;
            this.comboBoxCommentary.Location = new System.Drawing.Point(115, 73);
            this.comboBoxCommentary.Name = "comboBoxCommentary";
            this.comboBoxCommentary.Size = new System.Drawing.Size(121, 21);
            this.comboBoxCommentary.TabIndex = 7;
            this.comboBoxCommentary.SelectedValueChanged += new System.EventHandler(this.FilterControlChanged);
            // 
            // comboBoxCut
            // 
            this.comboBoxCut.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCut.FormattingEnabled = true;
            this.comboBoxCut.Location = new System.Drawing.Point(115, 45);
            this.comboBoxCut.Name = "comboBoxCut";
            this.comboBoxCut.Size = new System.Drawing.Size(121, 21);
            this.comboBoxCut.TabIndex = 6;
            this.comboBoxCut.SelectedValueChanged += new System.EventHandler(this.FilterControlChanged);
            // 
            // comboBoxVideoLanguage
            // 
            this.comboBoxVideoLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxVideoLanguage.FormattingEnabled = true;
            this.comboBoxVideoLanguage.Location = new System.Drawing.Point(115, 17);
            this.comboBoxVideoLanguage.Name = "comboBoxVideoLanguage";
            this.comboBoxVideoLanguage.Size = new System.Drawing.Size(121, 21);
            this.comboBoxVideoLanguage.TabIndex = 5;
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
            this.groupBoxMasterOverride.Size = new System.Drawing.Size(680, 50);
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
            this.comboBoxAudienceLanguage.TabIndex = 1;
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
            this.tabPageProgress.Location = new System.Drawing.Point(4, 22);
            this.tabPageProgress.Name = "tabPageProgress";
            this.tabPageProgress.Size = new System.Drawing.Size(687, 569);
            this.tabPageProgress.TabIndex = 3;
            this.tabPageProgress.Text = "Progress";
            this.tabPageProgress.UseVisualStyleBackColor = true;
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel,
            this.progressBar});
            this.statusStrip.Location = new System.Drawing.Point(0, 639);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(719, 22);
            this.statusStrip.TabIndex = 6;
            this.statusStrip.Text = "Status Strip";
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(39, 17);
            this.statusLabel.Text = "Status";
            // 
            // progressBar
            // 
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(100, 16);
            // 
            // FormDetails
            // 
            this.AcceptButton = this.continueButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(719, 661);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.continueButton);
            this.Name = "FormDetails";
            this.Text = "Blu-ray Details";
            this.Resize += new System.EventHandler(this.FormDetails_Resize);
            this.tabControl.ResumeLayout(false);
            this.tabPageDisc.ResumeLayout(false);
            this.tabPageDisc.PerformLayout();
            this.tabPagePlaylists.ResumeLayout(false);
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
            this.tabPageOutput.ResumeLayout(false);
            this.groupBoxOutput.ResumeLayout(false);
            this.groupBoxOutput.PerformLayout();
            this.groupBoxTracks.ResumeLayout(false);
            this.groupBoxTracks.PerformLayout();
            this.groupBoxFilter.ResumeLayout(false);
            this.groupBoxFilter.PerformLayout();
            this.groupBoxMasterOverride.ResumeLayout(false);
            this.groupBoxMasterOverride.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label movieNameLabel;
        private System.Windows.Forms.Button continueButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.TextBox movieNameTextBox;
        private System.Windows.Forms.Button searchButton;
        private System.Windows.Forms.ListView searchResultListView;
        private System.Windows.Forms.ColumnHeader NameColumn;
        private System.Windows.Forms.ColumnHeader YearColumn;
        private System.Windows.Forms.ColumnHeader PopularityColumn;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPageDisc;
        private System.Windows.Forms.TabPage tabPagePlaylists;
        private System.Windows.Forms.TabPage tabPageOutput;
        private System.Windows.Forms.ComboBox discLanguageComboBox;
        private System.Windows.Forms.Label discLanguageLabel;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.ToolStripProgressBar progressBar;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView playlistDataGridView;
        private System.Windows.Forms.SplitContainer playlistsSplitContainerOuter;
        private System.Windows.Forms.GroupBox playlistsGroupBox;
        private System.Windows.Forms.CheckBox showAllPlaylistsCheckbox;
        private System.Windows.Forms.SplitContainer playlistsSplitContainerInner;
        private System.Windows.Forms.GroupBox streamsGroupBox;
        private System.Windows.Forms.GroupBox tracksGroupBox;
        private System.Windows.Forms.ListView listViewStreamFiles;
        private System.Windows.Forms.ColumnHeader columnHeaderFileName;
        private System.Windows.Forms.ColumnHeader columnHeaderIndex;
        private System.Windows.Forms.ColumnHeader columnHeaderFileLength;
        private System.Windows.Forms.ColumnHeader columnHeaderFileEstimatedBytes;
        private System.Windows.Forms.ColumnHeader columnHeaderFileMeasuredBytes;
        private System.Windows.Forms.ListView listViewStreams;
        private System.Windows.Forms.ColumnHeader columnHeaderStreamCodec;
        private System.Windows.Forms.ColumnHeader columnHeaderStreamLanguage;
        private System.Windows.Forms.ColumnHeader columnHeaderBitrate;
        private System.Windows.Forms.ColumnHeader columnHeaderDescription;
        private System.Windows.Forms.TabPage tabPageProgress;
        private System.Windows.Forms.GroupBox groupBoxMasterOverride;
        private System.Windows.Forms.ComboBox comboBoxAudienceLanguage;
        private System.Windows.Forms.Label labelAudienceLanguage;
        private System.Windows.Forms.GroupBox groupBoxFilter;
        private System.Windows.Forms.Label labelVideoLanguage;
        private System.Windows.Forms.GroupBox groupBoxOutput;
        private System.Windows.Forms.Label labelOutputFileNamePreview;
        private System.Windows.Forms.Label labelOutputFileNameHelp;
        private System.Windows.Forms.Label labelOutputFileExtension;
        private System.Windows.Forms.Button buttonOutputDir;
        private System.Windows.Forms.TextBox textBoxOutputDir;
        private System.Windows.Forms.TextBox textBoxOutputFileName;
        private System.Windows.Forms.Label labelOutputPreview;
        private System.Windows.Forms.Label labelOutputFileName;
        private System.Windows.Forms.Label labelOutputDirectory;
        private System.Windows.Forms.GroupBox groupBoxTracks;
        private System.Windows.Forms.ListView listViewSubtitleTracks;
        private System.Windows.Forms.ColumnHeader columnHeaderSubtitleId;
        private System.Windows.Forms.ColumnHeader columnHeaderSubtitleLanguage;
        private System.Windows.Forms.ListView listViewAudioTracks;
        private System.Windows.Forms.ColumnHeader columnHeaderCodec;
        private System.Windows.Forms.ColumnHeader columnHeaderLanguage;
        private System.Windows.Forms.ColumnHeader columnHeaderChannels;
        private System.Windows.Forms.CheckedListBox checkedListBoxVideoResolution;
        private System.Windows.Forms.Label labelSubtitleTracks;
        private System.Windows.Forms.Label labelAudioTracks;
        private System.Windows.Forms.Label labelVideoResolution;
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
    }
}