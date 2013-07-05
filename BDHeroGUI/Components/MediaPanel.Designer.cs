namespace BDHeroGUI.Components
{
    partial class MediaPanel
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
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.linkLabelTitle = new DotNetUtils.Controls.LinkLabel2();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Media:";
            // 
            // pictureBox
            // 
            this.pictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.pictureBox.Location = new System.Drawing.Point(4, 17);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(129, 160);
            this.pictureBox.TabIndex = 1;
            this.pictureBox.TabStop = false;
            // 
            // linkLabelTitle
            // 
            this.linkLabelTitle.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.linkLabelTitle.HoverColor = System.Drawing.Color.Empty;
            this.linkLabelTitle.Location = new System.Drawing.Point(140, 17);
            this.linkLabelTitle.Name = "linkLabelTitle";
            this.linkLabelTitle.RegularColor = System.Drawing.Color.Empty;
            this.linkLabelTitle.Size = new System.Drawing.Size(212, 14);
            this.linkLabelTitle.TabIndex = 2;
            this.linkLabelTitle.Text = "Star Wars: Episode IV: A New Hope (1977)";
            // 
            // MediaPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.linkLabelTitle);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.label1);
            this.Name = "MediaPanel";
            this.Size = new System.Drawing.Size(360, 180);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox;
        private DotNetUtils.Controls.LinkLabel2 linkLabelTitle;
    }
}
