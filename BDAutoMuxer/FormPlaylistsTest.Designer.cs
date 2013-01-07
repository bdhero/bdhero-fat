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
            this.panel1 = new System.Windows.Forms.Panel();
            this.objectListViewPlaylists = new BrightIdeasSoftware.ObjectListView();
            this.type = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.filename = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.length = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.chapterCount = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.filesize = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.language = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.cut = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.warnings = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.objectListViewPlaylists)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.objectListViewPlaylists);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(676, 554);
            this.panel1.TabIndex = 0;
            // 
            // objectListViewPlaylists
            // 
            this.objectListViewPlaylists.AllColumns.Add(this.type);
            this.objectListViewPlaylists.AllColumns.Add(this.filename);
            this.objectListViewPlaylists.AllColumns.Add(this.length);
            this.objectListViewPlaylists.AllColumns.Add(this.chapterCount);
            this.objectListViewPlaylists.AllColumns.Add(this.filesize);
            this.objectListViewPlaylists.AllColumns.Add(this.language);
            this.objectListViewPlaylists.AllColumns.Add(this.cut);
            this.objectListViewPlaylists.AllColumns.Add(this.warnings);
            this.objectListViewPlaylists.AllowColumnReorder = true;
            this.objectListViewPlaylists.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.type,
            this.filename,
            this.length,
            this.chapterCount,
            this.filesize,
            this.language,
            this.cut,
            this.warnings});
            this.objectListViewPlaylists.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectListViewPlaylists.Location = new System.Drawing.Point(0, 0);
            this.objectListViewPlaylists.Name = "objectListViewPlaylists";
            this.objectListViewPlaylists.SelectColumnsOnRightClickBehaviour = BrightIdeasSoftware.ObjectListView.ColumnSelectBehaviour.Submenu;
            this.objectListViewPlaylists.ShowCommandMenuOnRightClick = true;
            this.objectListViewPlaylists.ShowGroups = false;
            this.objectListViewPlaylists.Size = new System.Drawing.Size(676, 554);
            this.objectListViewPlaylists.TabIndex = 0;
            this.objectListViewPlaylists.UseCompatibleStateImageBehavior = false;
            this.objectListViewPlaylists.View = System.Windows.Forms.View.Details;
            // 
            // type
            // 
            this.type.AspectName = "Type";
            this.type.CellPadding = null;
            this.type.Hideable = false;
            this.type.Text = "Video Type";
            // 
            // filename
            // 
            this.filename.AspectName = "Filename";
            this.filename.CellPadding = null;
            this.filename.Groupable = false;
            this.filename.Hideable = false;
            this.filename.IsEditable = false;
            this.filename.Text = "Filename";
            // 
            // length
            // 
            this.length.AspectName = "LengthHuman";
            this.length.CellPadding = null;
            this.length.IsEditable = false;
            this.length.Text = "Runtime";
            this.length.ToolTipText = "Length (a.k.a., duration) of the playlist";
            // 
            // chapterCount
            // 
            this.chapterCount.AspectName = "ChapterCount";
            this.chapterCount.AspectToStringFormat = "{0:n0}";
            this.chapterCount.CellPadding = null;
            this.chapterCount.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.chapterCount.IsEditable = false;
            this.chapterCount.Text = "# Chapters";
            this.chapterCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.chapterCount.ToolTipText = "";
            // 
            // filesize
            // 
            this.filesize.AspectName = "Filesize";
            this.filesize.AspectToStringFormat = "{0:n0}";
            this.filesize.CellPadding = null;
            this.filesize.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.filesize.IsEditable = false;
            this.filesize.Text = "Size (in bytes)";
            this.filesize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // language
            // 
            this.language.AspectName = "VideoLanguage";
            this.language.CellPadding = null;
            this.language.Text = "Video Language";
            // 
            // cut
            // 
            this.cut.AspectName = "Cut";
            this.cut.CellPadding = null;
            this.cut.Text = "Cut";
            // 
            // warnings
            // 
            this.warnings.AspectName = "Warnings";
            this.warnings.CellPadding = null;
            this.warnings.IsEditable = false;
            this.warnings.Text = "Warnings";
            // 
            // FormPlaylistsTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(700, 578);
            this.Controls.Add(this.panel1);
            this.Name = "FormPlaylistsTest";
            this.Text = "FormPlaylistsTest";
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.objectListViewPlaylists)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private BrightIdeasSoftware.ObjectListView objectListViewPlaylists;
        private BrightIdeasSoftware.OLVColumn type;
        private BrightIdeasSoftware.OLVColumn filename;
        private BrightIdeasSoftware.OLVColumn length;
        private BrightIdeasSoftware.OLVColumn chapterCount;
        private BrightIdeasSoftware.OLVColumn filesize;
        private BrightIdeasSoftware.OLVColumn language;
        private BrightIdeasSoftware.OLVColumn cut;
        private BrightIdeasSoftware.OLVColumn warnings;


    }
}