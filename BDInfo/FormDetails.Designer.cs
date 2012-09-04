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
            this.tracksGroupBox = new System.Windows.Forms.GroupBox();
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
            this.continueButton.Location = new System.Drawing.Point(539, 561);
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
            this.cancelButton.Location = new System.Drawing.Point(620, 561);
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
            this.tabControl.Size = new System.Drawing.Size(683, 543);
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
            this.tabPageDisc.Size = new System.Drawing.Size(675, 517);
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
            this.tabPagePlaylists.Size = new System.Drawing.Size(675, 517);
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
            this.playlistsSplitContainerOuter.Size = new System.Drawing.Size(663, 504);
            this.playlistsSplitContainerOuter.SplitterDistance = 213;
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
            this.playlistsGroupBox.Size = new System.Drawing.Size(656, 206);
            this.playlistsGroupBox.TabIndex = 5;
            this.playlistsGroupBox.TabStop = false;
            this.playlistsGroupBox.Text = "Playlists";
            // 
            // showAllPlaylistsCheckbox
            // 
            this.showAllPlaylistsCheckbox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.showAllPlaylistsCheckbox.AutoSize = true;
            this.showAllPlaylistsCheckbox.Location = new System.Drawing.Point(6, 183);
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
            this.panel1.Size = new System.Drawing.Size(644, 158);
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
            this.playlistDataGridView.Size = new System.Drawing.Size(638, 152);
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
            this.playlistsSplitContainerInner.Size = new System.Drawing.Size(660, 284);
            this.playlistsSplitContainerInner.SplitterDistance = 143;
            this.playlistsSplitContainerInner.TabIndex = 0;
            // 
            // streamsGroupBox
            // 
            this.streamsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.streamsGroupBox.Location = new System.Drawing.Point(4, 4);
            this.streamsGroupBox.Name = "streamsGroupBox";
            this.streamsGroupBox.Size = new System.Drawing.Size(653, 136);
            this.streamsGroupBox.TabIndex = 0;
            this.streamsGroupBox.TabStop = false;
            this.streamsGroupBox.Text = "Streams";
            // 
            // tracksGroupBox
            // 
            this.tracksGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tracksGroupBox.Location = new System.Drawing.Point(4, 4);
            this.tracksGroupBox.Name = "tracksGroupBox";
            this.tracksGroupBox.Size = new System.Drawing.Size(653, 130);
            this.tracksGroupBox.TabIndex = 0;
            this.tracksGroupBox.TabStop = false;
            this.tracksGroupBox.Text = "Tracks";
            // 
            // tabPageRip
            // 
            this.tabPageRip.Location = new System.Drawing.Point(4, 22);
            this.tabPageRip.Name = "tabPageRip";
            this.tabPageRip.Size = new System.Drawing.Size(681, 519);
            this.tabPageRip.TabIndex = 2;
            this.tabPageRip.Text = "Rip";
            this.tabPageRip.UseVisualStyleBackColor = true;
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel,
            this.progressBar});
            this.statusStrip.Location = new System.Drawing.Point(0, 587);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(707, 22);
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
            this.ClientSize = new System.Drawing.Size(707, 609);
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
    }
}