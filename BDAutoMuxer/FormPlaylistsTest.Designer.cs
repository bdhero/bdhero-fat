namespace BDAutoMuxer
{
    partial class FormPlaylistsTest
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
            this.splitContainerWithDivider1 = new BDAutoMuxer.views.SplitContainerWithDivider();
            this.panel2 = new System.Windows.Forms.Panel();
            this.checkBoxHideMisc = new System.Windows.Forms.CheckBox();
            this.checkBoxHideSpecialFeatures = new System.Windows.Forms.CheckBox();
            this.checkBoxHideCommentary = new System.Windows.Forms.CheckBox();
            this.checkBoxHideLowQuality = new System.Windows.Forms.CheckBox();
            this.checkBoxHideBogus = new System.Windows.Forms.CheckBox();
            this.playlistsListViewWrapper = new System.Windows.Forms.Panel();
            this.objectListViewPlaylists = new BrightIdeasSoftware.ObjectListView();
            this.playlistTypeColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.playlistFilenameColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.playlistLengthColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.playlistChapterCountColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.playlistFilesizeColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.playlistLanguageColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.playlistCutColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.playlistWarningsColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.tracksListViewWrapper = new System.Windows.Forms.Panel();
            this.objectListViewTracks = new BrightIdeasSoftware.ObjectListView();
            this.trackTypeColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.trackCodecColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.trackQualityColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.trackColumnIndex = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerWithDivider1)).BeginInit();
            this.splitContainerWithDivider1.Panel1.SuspendLayout();
            this.splitContainerWithDivider1.Panel2.SuspendLayout();
            this.splitContainerWithDivider1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.playlistsListViewWrapper.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.objectListViewPlaylists)).BeginInit();
            this.tracksListViewWrapper.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.objectListViewTracks)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainerWithDivider1
            // 
            this.splitContainerWithDivider1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainerWithDivider1.Location = new System.Drawing.Point(12, 12);
            this.splitContainerWithDivider1.Name = "splitContainerWithDivider1";
            this.splitContainerWithDivider1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerWithDivider1.Panel1
            // 
            this.splitContainerWithDivider1.Panel1.Controls.Add(this.panel2);
            // 
            // splitContainerWithDivider1.Panel2
            // 
            this.splitContainerWithDivider1.Panel2.Controls.Add(this.tracksListViewWrapper);
            this.splitContainerWithDivider1.Size = new System.Drawing.Size(818, 554);
            this.splitContainerWithDivider1.SplitterDistance = 298;
            this.splitContainerWithDivider1.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.checkBoxHideMisc);
            this.panel2.Controls.Add(this.checkBoxHideSpecialFeatures);
            this.panel2.Controls.Add(this.checkBoxHideCommentary);
            this.panel2.Controls.Add(this.checkBoxHideLowQuality);
            this.panel2.Controls.Add(this.checkBoxHideBogus);
            this.panel2.Controls.Add(this.playlistsListViewWrapper);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(818, 298);
            this.panel2.TabIndex = 0;
            // 
            // checkBoxHideMisc
            // 
            this.checkBoxHideMisc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxHideMisc.AutoSize = true;
            this.checkBoxHideMisc.Location = new System.Drawing.Point(258, 278);
            this.checkBoxHideMisc.Name = "checkBoxHideMisc";
            this.checkBoxHideMisc.Size = new System.Drawing.Size(80, 17);
            this.checkBoxHideMisc.TabIndex = 2;
            this.checkBoxHideMisc.Text = "Show misc.";
            this.checkBoxHideMisc.UseVisualStyleBackColor = true;
            this.checkBoxHideMisc.CheckedChanged += new System.EventHandler(this.UpdatePlaylists);
            // 
            // checkBoxHideSpecialFeatures
            // 
            this.checkBoxHideSpecialFeatures.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxHideSpecialFeatures.AutoSize = true;
            this.checkBoxHideSpecialFeatures.Location = new System.Drawing.Point(122, 278);
            this.checkBoxHideSpecialFeatures.Name = "checkBoxHideSpecialFeatures";
            this.checkBoxHideSpecialFeatures.Size = new System.Drawing.Size(130, 17);
            this.checkBoxHideSpecialFeatures.TabIndex = 1;
            this.checkBoxHideSpecialFeatures.Text = "Show special features";
            this.checkBoxHideSpecialFeatures.UseVisualStyleBackColor = true;
            this.checkBoxHideSpecialFeatures.CheckedChanged += new System.EventHandler(this.UpdatePlaylists);
            // 
            // checkBoxHideCommentary
            // 
            this.checkBoxHideCommentary.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxHideCommentary.AutoSize = true;
            this.checkBoxHideCommentary.Location = new System.Drawing.Point(3, 278);
            this.checkBoxHideCommentary.Name = "checkBoxHideCommentary";
            this.checkBoxHideCommentary.Size = new System.Drawing.Size(113, 17);
            this.checkBoxHideCommentary.TabIndex = 0;
            this.checkBoxHideCommentary.Text = "Show commentary";
            this.checkBoxHideCommentary.UseVisualStyleBackColor = true;
            this.checkBoxHideCommentary.CheckedChanged += new System.EventHandler(this.UpdatePlaylists);
            // 
            // checkBoxHideLowQuality
            // 
            this.checkBoxHideLowQuality.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxHideLowQuality.AutoSize = true;
            this.checkBoxHideLowQuality.Checked = true;
            this.checkBoxHideLowQuality.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxHideLowQuality.Location = new System.Drawing.Point(712, 278);
            this.checkBoxHideLowQuality.Name = "checkBoxHideLowQuality";
            this.checkBoxHideLowQuality.Size = new System.Drawing.Size(100, 17);
            this.checkBoxHideLowQuality.TabIndex = 4;
            this.checkBoxHideLowQuality.Text = "Hide low quality";
            this.checkBoxHideLowQuality.UseVisualStyleBackColor = true;
            this.checkBoxHideLowQuality.CheckedChanged += new System.EventHandler(this.UpdatePlaylists);
            // 
            // checkBoxHideBogus
            // 
            this.checkBoxHideBogus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxHideBogus.AutoSize = true;
            this.checkBoxHideBogus.Checked = true;
            this.checkBoxHideBogus.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxHideBogus.Location = new System.Drawing.Point(626, 278);
            this.checkBoxHideBogus.Name = "checkBoxHideBogus";
            this.checkBoxHideBogus.Size = new System.Drawing.Size(80, 17);
            this.checkBoxHideBogus.TabIndex = 3;
            this.checkBoxHideBogus.Text = "Hide bogus";
            this.checkBoxHideBogus.UseVisualStyleBackColor = true;
            this.checkBoxHideBogus.CheckedChanged += new System.EventHandler(this.UpdatePlaylists);
            // 
            // playlistsListViewWrapper
            // 
            this.playlistsListViewWrapper.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.playlistsListViewWrapper.Controls.Add(this.objectListViewPlaylists);
            this.playlistsListViewWrapper.Location = new System.Drawing.Point(3, 3);
            this.playlistsListViewWrapper.Name = "playlistsListViewWrapper";
            this.playlistsListViewWrapper.Size = new System.Drawing.Size(812, 269);
            this.playlistsListViewWrapper.TabIndex = 0;
            // 
            // objectListViewPlaylists
            // 
            this.objectListViewPlaylists.AllColumns.Add(this.playlistFilenameColumn);
            this.objectListViewPlaylists.AllColumns.Add(this.playlistTypeColumn);
            this.objectListViewPlaylists.AllColumns.Add(this.playlistLengthColumn);
            this.objectListViewPlaylists.AllColumns.Add(this.playlistChapterCountColumn);
            this.objectListViewPlaylists.AllColumns.Add(this.playlistFilesizeColumn);
            this.objectListViewPlaylists.AllColumns.Add(this.playlistLanguageColumn);
            this.objectListViewPlaylists.AllColumns.Add(this.playlistCutColumn);
            this.objectListViewPlaylists.AllColumns.Add(this.playlistWarningsColumn);
            this.objectListViewPlaylists.AllowColumnReorder = true;
            this.objectListViewPlaylists.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.SingleClick;
            this.objectListViewPlaylists.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.playlistFilenameColumn,
            this.playlistTypeColumn,
            this.playlistLengthColumn,
            this.playlistChapterCountColumn,
            this.playlistFilesizeColumn,
            this.playlistLanguageColumn,
            this.playlistCutColumn,
            this.playlistWarningsColumn});
            this.objectListViewPlaylists.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectListViewPlaylists.FullRowSelect = true;
            this.objectListViewPlaylists.GridLines = true;
            this.objectListViewPlaylists.Location = new System.Drawing.Point(0, 0);
            this.objectListViewPlaylists.Name = "objectListViewPlaylists";
            this.objectListViewPlaylists.SelectColumnsOnRightClickBehaviour = BrightIdeasSoftware.ObjectListView.ColumnSelectBehaviour.Submenu;
            this.objectListViewPlaylists.ShowCommandMenuOnRightClick = true;
            this.objectListViewPlaylists.ShowGroups = false;
            this.objectListViewPlaylists.ShowImagesOnSubItems = true;
            this.objectListViewPlaylists.Size = new System.Drawing.Size(812, 269);
            this.objectListViewPlaylists.TabIndex = 0;
            this.objectListViewPlaylists.UseCompatibleStateImageBehavior = false;
            this.objectListViewPlaylists.UseExplorerTheme = true;
            this.objectListViewPlaylists.UseSubItemCheckBoxes = true;
            this.objectListViewPlaylists.View = System.Windows.Forms.View.Details;
            this.objectListViewPlaylists.SelectedIndexChanged += new System.EventHandler(this.SelectedPlaylistChanged);
            // 
            // playlistTypeColumn
            // 
            this.playlistTypeColumn.AspectName = "TypeDisplayable";
            this.playlistTypeColumn.CellPadding = null;
            this.playlistTypeColumn.DisplayIndex = 0;
            this.playlistTypeColumn.Hideable = false;
            this.playlistTypeColumn.Text = "Video Type";
            this.playlistTypeColumn.Width = 72;
            // 
            // playlistFilenameColumn
            // 
            this.playlistFilenameColumn.AspectName = "Filename";
            this.playlistFilenameColumn.CellPadding = null;
            this.playlistFilenameColumn.DisplayIndex = 1;
            this.playlistFilenameColumn.Groupable = false;
            this.playlistFilenameColumn.Hideable = false;
            this.playlistFilenameColumn.IsEditable = false;
            this.playlistFilenameColumn.Text = "Filename";
            this.playlistFilenameColumn.Width = 67;
            // 
            // playlistLengthColumn
            // 
            this.playlistLengthColumn.AspectName = "LengthHuman";
            this.playlistLengthColumn.CellPadding = null;
            this.playlistLengthColumn.IsEditable = false;
            this.playlistLengthColumn.Text = "Runtime";
            this.playlistLengthColumn.ToolTipText = "Length (a.k.a., duration) of the playlist";
            this.playlistLengthColumn.Width = 72;
            // 
            // playlistChapterCountColumn
            // 
            this.playlistChapterCountColumn.AspectName = "ChapterCount";
            this.playlistChapterCountColumn.AspectToStringFormat = "{0:n0}";
            this.playlistChapterCountColumn.CellPadding = null;
            this.playlistChapterCountColumn.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.playlistChapterCountColumn.IsEditable = false;
            this.playlistChapterCountColumn.Text = "# Chapters";
            this.playlistChapterCountColumn.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.playlistChapterCountColumn.ToolTipText = "";
            this.playlistChapterCountColumn.Width = 69;
            // 
            // playlistFilesizeColumn
            // 
            this.playlistFilesizeColumn.AspectName = "Filesize";
            this.playlistFilesizeColumn.AspectToStringFormat = "{0:n0}";
            this.playlistFilesizeColumn.CellPadding = null;
            this.playlistFilesizeColumn.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.playlistFilesizeColumn.IsEditable = false;
            this.playlistFilesizeColumn.Text = "Size (in bytes)";
            this.playlistFilesizeColumn.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.playlistFilesizeColumn.Width = 85;
            // 
            // playlistLanguageColumn
            // 
            this.playlistLanguageColumn.AspectName = "VideoLanguage.ComboBoxWrapper";
            this.playlistLanguageColumn.CellPadding = null;
            this.playlistLanguageColumn.Text = "Video Language";
            this.playlistLanguageColumn.Width = 96;
            // 
            // playlistCutColumn
            // 
            this.playlistCutColumn.AspectName = "Cut";
            this.playlistCutColumn.CellPadding = null;
            this.playlistCutColumn.Text = "Cut";
            // 
            // playlistWarningsColumn
            // 
            this.playlistWarningsColumn.AspectName = "Warnings";
            this.playlistWarningsColumn.CellPadding = null;
            this.playlistWarningsColumn.IsEditable = false;
            this.playlistWarningsColumn.Text = "Warnings";
            this.playlistWarningsColumn.Width = 77;
            // 
            // tracksListViewWrapper
            // 
            this.tracksListViewWrapper.Controls.Add(this.objectListViewTracks);
            this.tracksListViewWrapper.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tracksListViewWrapper.Location = new System.Drawing.Point(0, 0);
            this.tracksListViewWrapper.Name = "tracksListViewWrapper";
            this.tracksListViewWrapper.Size = new System.Drawing.Size(818, 252);
            this.tracksListViewWrapper.TabIndex = 0;
            // 
            // objectListViewTracks
            // 
            this.objectListViewTracks.AllColumns.Add(this.trackColumnIndex);
            this.objectListViewTracks.AllColumns.Add(this.trackTypeColumn);
            this.objectListViewTracks.AllColumns.Add(this.trackCodecColumn);
            this.objectListViewTracks.AllColumns.Add(this.trackQualityColumn);
            this.objectListViewTracks.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.SingleClick;
            this.objectListViewTracks.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.trackColumnIndex,
            this.trackTypeColumn,
            this.trackCodecColumn,
            this.trackQualityColumn});
            this.objectListViewTracks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectListViewTracks.FullRowSelect = true;
            this.objectListViewTracks.GridLines = true;
            this.objectListViewTracks.Location = new System.Drawing.Point(0, 0);
            this.objectListViewTracks.Name = "objectListViewTracks";
            this.objectListViewTracks.SelectColumnsOnRightClickBehaviour = BrightIdeasSoftware.ObjectListView.ColumnSelectBehaviour.Submenu;
            this.objectListViewTracks.ShowCommandMenuOnRightClick = true;
            this.objectListViewTracks.ShowGroups = false;
            this.objectListViewTracks.ShowImagesOnSubItems = true;
            this.objectListViewTracks.Size = new System.Drawing.Size(818, 252);
            this.objectListViewTracks.TabIndex = 0;
            this.objectListViewTracks.UseCompatibleStateImageBehavior = false;
            this.objectListViewTracks.UseExplorerTheme = true;
            this.objectListViewTracks.View = System.Windows.Forms.View.Details;
            // 
            // trackTypeColumn
            // 
            this.trackTypeColumn.AspectName = "TypeDisplayable";
            this.trackTypeColumn.CellPadding = null;
            this.trackTypeColumn.Hideable = false;
            this.trackTypeColumn.Text = "Track Type";
            // 
            // trackCodecColumn
            // 
            this.trackCodecColumn.AspectName = "Codec";
            this.trackCodecColumn.CellPadding = null;
            this.trackCodecColumn.IsEditable = false;
            this.trackCodecColumn.Text = "Codec";
            // 
            // trackQualityColumn
            // 
            this.trackQualityColumn.AspectName = "QualityDisplayable";
            this.trackQualityColumn.CellPadding = null;
            this.trackQualityColumn.IsEditable = false;
            this.trackQualityColumn.Text = "Quality";
            // 
            // trackColumnIndex
            // 
            this.trackColumnIndex.AspectName = "Index";
            this.trackColumnIndex.CellPadding = null;
            this.trackColumnIndex.IsEditable = false;
            this.trackColumnIndex.Text = "Index";
            // 
            // FormPlaylistsTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(842, 578);
            this.Controls.Add(this.splitContainerWithDivider1);
            this.Name = "FormPlaylistsTest";
            this.Text = "FormPlaylistsTest";
            this.splitContainerWithDivider1.Panel1.ResumeLayout(false);
            this.splitContainerWithDivider1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerWithDivider1)).EndInit();
            this.splitContainerWithDivider1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.playlistsListViewWrapper.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.objectListViewPlaylists)).EndInit();
            this.tracksListViewWrapper.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.objectListViewTracks)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel playlistsListViewWrapper;
        private BrightIdeasSoftware.ObjectListView objectListViewPlaylists;
        private BrightIdeasSoftware.OLVColumn playlistTypeColumn;
        private BrightIdeasSoftware.OLVColumn playlistFilenameColumn;
        private BrightIdeasSoftware.OLVColumn playlistLengthColumn;
        private BrightIdeasSoftware.OLVColumn playlistChapterCountColumn;
        private BrightIdeasSoftware.OLVColumn playlistFilesizeColumn;
        private BrightIdeasSoftware.OLVColumn playlistLanguageColumn;
        private BrightIdeasSoftware.OLVColumn playlistCutColumn;
        private BrightIdeasSoftware.OLVColumn playlistWarningsColumn;
        private views.SplitContainerWithDivider splitContainerWithDivider1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.CheckBox checkBoxHideLowQuality;
        private System.Windows.Forms.CheckBox checkBoxHideBogus;
        private System.Windows.Forms.Panel tracksListViewWrapper;
        private BrightIdeasSoftware.ObjectListView objectListViewTracks;
        private BrightIdeasSoftware.OLVColumn trackTypeColumn;
        private BrightIdeasSoftware.OLVColumn trackCodecColumn;
        private BrightIdeasSoftware.OLVColumn trackQualityColumn;
        private System.Windows.Forms.CheckBox checkBoxHideSpecialFeatures;
        private System.Windows.Forms.CheckBox checkBoxHideCommentary;
        private System.Windows.Forms.CheckBox checkBoxHideMisc;
        private BrightIdeasSoftware.OLVColumn trackColumnIndex;


    }
}