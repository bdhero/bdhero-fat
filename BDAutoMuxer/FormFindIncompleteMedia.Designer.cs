using DotNetUtils.Controls;

namespace BDAutoMuxer
{
    partial class FormFindIncompleteMedia
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
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxSourceDir = new System.Windows.Forms.TextBox();
            this.buttonBrowse = new System.Windows.Forms.Button();
            this.buttonScan = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.splitContainerWithDivider1 = new SplitContainerWithDivider();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxMissingChapters = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxMissingSubtitles = new System.Windows.Forms.TextBox();
            this.labelProgress = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerWithDivider1)).BeginInit();
            this.splitContainerWithDivider1.Panel1.SuspendLayout();
            this.splitContainerWithDivider1.Panel2.SuspendLayout();
            this.splitContainerWithDivider1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Scan directory:";
            // 
            // textBoxSourceDir
            // 
            this.textBoxSourceDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxSourceDir.Location = new System.Drawing.Point(97, 14);
            this.textBoxSourceDir.Name = "textBoxSourceDir";
            this.textBoxSourceDir.Size = new System.Drawing.Size(532, 20);
            this.textBoxSourceDir.TabIndex = 1;
            // 
            // buttonBrowse
            // 
            this.buttonBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonBrowse.Enabled = false;
            this.buttonBrowse.Location = new System.Drawing.Point(635, 12);
            this.buttonBrowse.Name = "buttonBrowse";
            this.buttonBrowse.Size = new System.Drawing.Size(75, 23);
            this.buttonBrowse.TabIndex = 2;
            this.buttonBrowse.Text = "Browse...";
            this.buttonBrowse.UseVisualStyleBackColor = true;
            // 
            // buttonScan
            // 
            this.buttonScan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonScan.Location = new System.Drawing.Point(635, 41);
            this.buttonScan.Name = "buttonScan";
            this.buttonScan.Size = new System.Drawing.Size(75, 23);
            this.buttonScan.TabIndex = 3;
            this.buttonScan.Text = "Scan!";
            this.buttonScan.UseVisualStyleBackColor = true;
            this.buttonScan.Click += new System.EventHandler(this.buttonScan_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.splitContainerWithDivider1);
            this.groupBox1.Location = new System.Drawing.Point(12, 70);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(698, 477);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Scan Results";
            // 
            // splitContainerWithDivider1
            // 
            this.splitContainerWithDivider1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainerWithDivider1.Location = new System.Drawing.Point(3, 16);
            this.splitContainerWithDivider1.Name = "splitContainerWithDivider1";
            this.splitContainerWithDivider1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerWithDivider1.Panel1
            // 
            this.splitContainerWithDivider1.Panel1.Controls.Add(this.label2);
            this.splitContainerWithDivider1.Panel1.Controls.Add(this.textBoxMissingChapters);
            // 
            // splitContainerWithDivider1.Panel2
            // 
            this.splitContainerWithDivider1.Panel2.Controls.Add(this.label3);
            this.splitContainerWithDivider1.Panel2.Controls.Add(this.textBoxMissingSubtitles);
            this.splitContainerWithDivider1.Size = new System.Drawing.Size(692, 458);
            this.splitContainerWithDivider1.SplitterDistance = 231;
            this.splitContainerWithDivider1.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Missing chapters:";
            // 
            // textBoxMissingChapters
            // 
            this.textBoxMissingChapters.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxMissingChapters.Location = new System.Drawing.Point(3, 16);
            this.textBoxMissingChapters.Multiline = true;
            this.textBoxMissingChapters.Name = "textBoxMissingChapters";
            this.textBoxMissingChapters.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxMissingChapters.Size = new System.Drawing.Size(686, 212);
            this.textBoxMissingChapters.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(86, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Missing subtitles:";
            // 
            // textBoxMissingSubtitles
            // 
            this.textBoxMissingSubtitles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxMissingSubtitles.Location = new System.Drawing.Point(3, 16);
            this.textBoxMissingSubtitles.Multiline = true;
            this.textBoxMissingSubtitles.Name = "textBoxMissingSubtitles";
            this.textBoxMissingSubtitles.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxMissingSubtitles.Size = new System.Drawing.Size(686, 204);
            this.textBoxMissingSubtitles.TabIndex = 7;
            // 
            // labelProgress
            // 
            this.labelProgress.AutoSize = true;
            this.labelProgress.Location = new System.Drawing.Point(13, 46);
            this.labelProgress.Name = "labelProgress";
            this.labelProgress.Size = new System.Drawing.Size(37, 13);
            this.labelProgress.TabIndex = 5;
            this.labelProgress.Text = "Status";
            // 
            // FormFindIncompleteMedia
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(722, 559);
            this.Controls.Add(this.labelProgress);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonScan);
            this.Controls.Add(this.buttonBrowse);
            this.Controls.Add(this.textBoxSourceDir);
            this.Controls.Add(this.label1);
            this.Name = "FormFindIncompleteMedia";
            this.Text = "Find Incomplete MKVs";
            this.groupBox1.ResumeLayout(false);
            this.splitContainerWithDivider1.Panel1.ResumeLayout(false);
            this.splitContainerWithDivider1.Panel1.PerformLayout();
            this.splitContainerWithDivider1.Panel2.ResumeLayout(false);
            this.splitContainerWithDivider1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerWithDivider1)).EndInit();
            this.splitContainerWithDivider1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxSourceDir;
        private System.Windows.Forms.Button buttonBrowse;
        private System.Windows.Forms.Button buttonScan;
        private System.Windows.Forms.GroupBox groupBox1;
        private SplitContainerWithDivider splitContainerWithDivider1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxMissingChapters;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxMissingSubtitles;
        private System.Windows.Forms.Label labelProgress;
    }
}