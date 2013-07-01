using DotNetUtils.Controls;

namespace BDHeroGUI.Components
{
    partial class PlaylistGridControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.listView = new System.Windows.Forms.ListView();
            this.columnHeaderFileName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderLength = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderChapterCount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderFileSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderCut = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderVideoLanguage = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderWarnings = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // listView
            // 
            this.listView.AllowColumnReorder = true;
            this.listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderFileName,
            this.columnHeaderLength,
            this.columnHeaderChapterCount,
            this.columnHeaderFileSize,
            this.columnHeaderType,
            this.columnHeaderCut,
            this.columnHeaderVideoLanguage,
            this.columnHeaderWarnings});
            this.listView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView.FullRowSelect = true;
            this.listView.GridLines = true;
            this.listView.HideSelection = false;
            this.listView.Location = new System.Drawing.Point(0, 0);
            this.listView.MultiSelect = false;
            this.listView.Name = "listView";
            this.listView.ShowItemToolTips = true;
            this.listView.Size = new System.Drawing.Size(709, 558);
            this.listView.TabIndex = 0;
            this.listView.UseCompatibleStateImageBehavior = false;
            this.listView.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderFileName
            // 
            this.columnHeaderFileName.Text = "Name";
            this.columnHeaderFileName.Width = 80;
            // 
            // columnHeaderLength
            // 
            this.columnHeaderLength.Text = "Duration";
            this.columnHeaderLength.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeaderLength.Width = 80;
            // 
            // columnHeaderChapterCount
            // 
            this.columnHeaderChapterCount.Text = "Chapters";
            this.columnHeaderChapterCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // columnHeaderFileSize
            // 
            this.columnHeaderFileSize.Text = "Size (bytes)";
            this.columnHeaderFileSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeaderFileSize.Width = 120;
            // 
            // columnHeaderType
            // 
            this.columnHeaderType.Text = "Type";
            this.columnHeaderType.Width = 100;
            // 
            // columnHeaderCut
            // 
            this.columnHeaderCut.Text = "Cut";
            this.columnHeaderCut.Width = 100;
            // 
            // columnHeaderVideoLanguage
            // 
            this.columnHeaderVideoLanguage.Text = "Video Language";
            this.columnHeaderVideoLanguage.Width = 120;
            // 
            // columnHeaderWarnings
            // 
            this.columnHeaderWarnings.Text = "Warnings";
            this.columnHeaderWarnings.Width = 160;
            // 
            // PlaylistGridControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.listView);
            this.Name = "PlaylistGridControl";
            this.Size = new System.Drawing.Size(709, 558);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listView;
        private System.Windows.Forms.ColumnHeader columnHeaderFileName;
        private System.Windows.Forms.ColumnHeader columnHeaderLength;
        private System.Windows.Forms.ColumnHeader columnHeaderChapterCount;
        private System.Windows.Forms.ColumnHeader columnHeaderFileSize;
        private System.Windows.Forms.ColumnHeader columnHeaderVideoLanguage;
        private System.Windows.Forms.ColumnHeader columnHeaderCut;
        private System.Windows.Forms.ColumnHeader columnHeaderType;
        private System.Windows.Forms.ColumnHeader columnHeaderWarnings;

    }
}
