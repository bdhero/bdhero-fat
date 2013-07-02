namespace BDHeroGUI.Components
{
    partial class VideoTrackListView
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
            this.listViewVideoTracks = new DotNetUtils.Controls.ListView2();
            this.columnHeaderCodec = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderResolution = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderFrameRate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderAspectRatio = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // listViewVideoTracks
            // 
            this.listViewVideoTracks.AllowColumnReorder = true;
            this.listViewVideoTracks.CheckBoxes = true;
            this.listViewVideoTracks.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderCodec,
            this.columnHeaderResolution,
            this.columnHeaderFrameRate,
            this.columnHeaderAspectRatio});
            this.listViewVideoTracks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewVideoTracks.FullRowSelect = true;
            this.listViewVideoTracks.GridLines = true;
            this.listViewVideoTracks.HideSelection = false;
            this.listViewVideoTracks.Location = new System.Drawing.Point(0, 0);
            this.listViewVideoTracks.MultiSelect = false;
            this.listViewVideoTracks.Name = "listViewVideoTracks";
            this.listViewVideoTracks.ShowItemToolTips = true;
            this.listViewVideoTracks.Size = new System.Drawing.Size(518, 333);
            this.listViewVideoTracks.TabIndex = 0;
            this.listViewVideoTracks.UseCompatibleStateImageBehavior = false;
            this.listViewVideoTracks.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderCodec
            // 
            this.columnHeaderCodec.Text = "Codec";
            this.columnHeaderCodec.Width = 80;
            // 
            // columnHeaderResolution
            // 
            this.columnHeaderResolution.Text = "Resolution";
            this.columnHeaderResolution.Width = 80;
            // 
            // columnHeaderFrameRate
            // 
            this.columnHeaderFrameRate.Text = "Frame Rate";
            this.columnHeaderFrameRate.Width = 80;
            // 
            // columnHeaderAspectRatio
            // 
            this.columnHeaderAspectRatio.Text = "Aspect Ratio";
            this.columnHeaderAspectRatio.Width = 80;
            // 
            // VideoTrackListView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.listViewVideoTracks);
            this.Name = "VideoTrackListView";
            this.Size = new System.Drawing.Size(518, 333);
            this.ResumeLayout(false);

        }

        #endregion

        private DotNetUtils.Controls.ListView2 listViewVideoTracks;
        private System.Windows.Forms.ColumnHeader columnHeaderCodec;
        private System.Windows.Forms.ColumnHeader columnHeaderResolution;
        private System.Windows.Forms.ColumnHeader columnHeaderFrameRate;
        private System.Windows.Forms.ColumnHeader columnHeaderAspectRatio;
    }
}
