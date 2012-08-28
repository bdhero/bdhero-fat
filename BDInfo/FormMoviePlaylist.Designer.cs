namespace BDInfo
{
    partial class FormMoviePlaylist
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
            this.playlistListView = new ComponentOwl.BetterListView.BetterListView();
            this.filenameColumnHeader = new ComponentOwl.BetterListView.BetterListViewColumnHeader();
            this.lengthColumnHeader = new ComponentOwl.BetterListView.BetterListViewColumnHeader();
            this.trackTypeColumnHeader = new ComponentOwl.BetterListView.BetterListViewColumnHeader();
            this.releaseTypeColumnHeader = new ComponentOwl.BetterListView.BetterListViewColumnHeader();
            this.hasCommentaryColumnHeader = new ComponentOwl.BetterListView.BetterListViewColumnHeader();
            this.showAllPlaylistsCheckbox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.playlistListView)).BeginInit();
            this.SuspendLayout();
            // 
            // playlistListView
            // 
            this.playlistListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.playlistListView.CheckBoxes = ComponentOwl.BetterListView.BetterListViewCheckBoxes.TwoState;
            this.playlistListView.Columns.AddRange(new object[] {
            this.filenameColumnHeader,
            this.lengthColumnHeader,
            this.trackTypeColumnHeader,
            this.releaseTypeColumnHeader,
            this.hasCommentaryColumnHeader});
            this.playlistListView.LabelEditModeSubItems = ComponentOwl.BetterListView.BetterListViewLabelEditMode.CustomControl;
            this.playlistListView.Location = new System.Drawing.Point(13, 65);
            this.playlistListView.Name = "playlistListView";
            this.playlistListView.Size = new System.Drawing.Size(685, 278);
            this.playlistListView.TabIndex = 0;
            // 
            // filenameColumnHeader
            // 
            this.filenameColumnHeader.Name = "filenameColumnHeader";
            this.filenameColumnHeader.Text = "Filename";
            // 
            // lengthColumnHeader
            // 
            this.lengthColumnHeader.Name = "lengthColumnHeader";
            this.lengthColumnHeader.Text = "Length";
            // 
            // trackTypeColumnHeader
            // 
            this.trackTypeColumnHeader.Name = "trackTypeColumnHeader";
            this.trackTypeColumnHeader.Text = "Track Type";
            // 
            // releaseTypeColumnHeader
            // 
            this.releaseTypeColumnHeader.Name = "releaseTypeColumnHeader";
            this.releaseTypeColumnHeader.Text = "Release Type";
            // 
            // hasCommentaryColumnHeader
            // 
            this.hasCommentaryColumnHeader.Name = "hasCommentaryColumnHeader";
            this.hasCommentaryColumnHeader.Text = "Has Commentary";
            this.hasCommentaryColumnHeader.Width = 100;
            // 
            // showAllPlaylistsCheckbox
            // 
            this.showAllPlaylistsCheckbox.AutoSize = true;
            this.showAllPlaylistsCheckbox.Location = new System.Drawing.Point(13, 350);
            this.showAllPlaylistsCheckbox.Name = "showAllPlaylistsCheckbox";
            this.showAllPlaylistsCheckbox.Size = new System.Drawing.Size(105, 17);
            this.showAllPlaylistsCheckbox.TabIndex = 1;
            this.showAllPlaylistsCheckbox.Text = "Show all playlists";
            this.showAllPlaylistsCheckbox.UseVisualStyleBackColor = true;
            this.showAllPlaylistsCheckbox.CheckedChanged += new System.EventHandler(this.showAllPlaylistsCheckbox_CheckedChanged);
            // 
            // FormMoviePlaylist
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(710, 426);
            this.Controls.Add(this.showAllPlaylistsCheckbox);
            this.Controls.Add(this.playlistListView);
            this.Name = "FormMoviePlaylist";
            this.Text = "Select Playlists";
            ((System.ComponentModel.ISupportInitialize)(this.playlistListView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ComponentOwl.BetterListView.BetterListView playlistListView;
        private ComponentOwl.BetterListView.BetterListViewColumnHeader filenameColumnHeader;
        private ComponentOwl.BetterListView.BetterListViewColumnHeader lengthColumnHeader;
        private ComponentOwl.BetterListView.BetterListViewColumnHeader trackTypeColumnHeader;
        private ComponentOwl.BetterListView.BetterListViewColumnHeader releaseTypeColumnHeader;
        private ComponentOwl.BetterListView.BetterListViewColumnHeader hasCommentaryColumnHeader;
        private System.Windows.Forms.CheckBox showAllPlaylistsCheckbox;
    }
}