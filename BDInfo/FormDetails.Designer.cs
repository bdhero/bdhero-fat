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
            this.tabPageRip = new System.Windows.Forms.TabPage();
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
            this.continueButton.Location = new System.Drawing.Point(556, 577);
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
            this.cancelButton.Location = new System.Drawing.Point(637, 577);
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
            this.movieNameTextBox.Size = new System.Drawing.Size(456, 20);
            this.movieNameTextBox.TabIndex = 2;
            // 
            // searchButton
            // 
            this.searchButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.searchButton.Location = new System.Drawing.Point(558, 31);
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
            this.searchResultListView.Size = new System.Drawing.Size(650, 433);
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
            this.tabControl.Controls.Add(this.tabPageRip);
            this.tabControl.Location = new System.Drawing.Point(12, 12);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(700, 559);
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
            this.tabPageDisc.Size = new System.Drawing.Size(692, 533);
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
            this.tabPagePlaylists.Size = new System.Drawing.Size(692, 533);
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
            this.playlistsSplitContainerOuter.Size = new System.Drawing.Size(680, 520);
            this.playlistsSplitContainerOuter.SplitterDistance = 219;
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
            this.playlistsGroupBox.Size = new System.Drawing.Size(673, 212);
            this.playlistsGroupBox.TabIndex = 5;
            this.playlistsGroupBox.TabStop = false;
            this.playlistsGroupBox.Text = "Playlists";
            // 
            // showAllPlaylistsCheckbox
            // 
            this.showAllPlaylistsCheckbox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.showAllPlaylistsCheckbox.AutoSize = true;
            this.showAllPlaylistsCheckbox.Location = new System.Drawing.Point(6, 189);
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
            this.panel1.Size = new System.Drawing.Size(661, 164);
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
            this.playlistDataGridView.Location = new System.Drawing.Point(3, 3);
            this.playlistDataGridView.MultiSelect = false;
            this.playlistDataGridView.Name = "playlistDataGridView";
            this.playlistDataGridView.Size = new System.Drawing.Size(655, 158);
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
            this.playlistsSplitContainerInner.Size = new System.Drawing.Size(677, 294);
            this.playlistsSplitContainerInner.SplitterDistance = 149;
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
            this.streamsGroupBox.Size = new System.Drawing.Size(670, 142);
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
            this.listViewStreamFiles.Size = new System.Drawing.Size(664, 123);
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
            this.tracksGroupBox.Size = new System.Drawing.Size(670, 134);
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
            this.listViewStreams.Size = new System.Drawing.Size(664, 115);
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
            // tabPageRip
            // 
            this.tabPageRip.Location = new System.Drawing.Point(4, 22);
            this.tabPageRip.Name = "tabPageRip";
            this.tabPageRip.Size = new System.Drawing.Size(692, 533);
            this.tabPageRip.TabIndex = 2;
            this.tabPageRip.Text = "Rip";
            this.tabPageRip.UseVisualStyleBackColor = true;
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel,
            this.progressBar});
            this.statusStrip.Location = new System.Drawing.Point(0, 603);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(724, 22);
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
            this.ClientSize = new System.Drawing.Size(724, 625);
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
        private System.Windows.Forms.TabPage tabPageRip;
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
    }
}