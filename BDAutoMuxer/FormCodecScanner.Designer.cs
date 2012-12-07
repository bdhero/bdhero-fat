namespace BDAutoMuxer
{
    partial class FormCodecScanner
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
            this.groupBoxProgress = new System.Windows.Forms.GroupBox();
            this.textBoxCurFile = new System.Windows.Forms.TextBox();
            this.textBoxStdOut = new System.Windows.Forms.TextBox();
            this.labelPercent = new System.Windows.Forms.Label();
            this.labelCount = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.splitContainerWithDivider = new BDAutoMuxer.views.SplitContainerWithDivider();
            this.groupBoxInput = new System.Windows.Forms.GroupBox();
            this.textBoxInputFiles = new System.Windows.Forms.TextBox();
            this.buttonScan = new System.Windows.Forms.Button();
            this.groupBoxProgress.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerWithDivider)).BeginInit();
            this.splitContainerWithDivider.Panel1.SuspendLayout();
            this.splitContainerWithDivider.Panel2.SuspendLayout();
            this.splitContainerWithDivider.SuspendLayout();
            this.groupBoxInput.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxProgress
            // 
            this.groupBoxProgress.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxProgress.Controls.Add(this.textBoxCurFile);
            this.groupBoxProgress.Controls.Add(this.textBoxStdOut);
            this.groupBoxProgress.Controls.Add(this.labelPercent);
            this.groupBoxProgress.Controls.Add(this.labelCount);
            this.groupBoxProgress.Controls.Add(this.progressBar1);
            this.groupBoxProgress.Location = new System.Drawing.Point(3, 3);
            this.groupBoxProgress.Name = "groupBoxProgress";
            this.groupBoxProgress.Size = new System.Drawing.Size(375, 514);
            this.groupBoxProgress.TabIndex = 9;
            this.groupBoxProgress.TabStop = false;
            this.groupBoxProgress.Text = "MediaInfo Progress";
            // 
            // textBoxCurFile
            // 
            this.textBoxCurFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxCurFile.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxCurFile.Location = new System.Drawing.Point(7, 73);
            this.textBoxCurFile.Name = "textBoxCurFile";
            this.textBoxCurFile.ReadOnly = true;
            this.textBoxCurFile.Size = new System.Drawing.Size(362, 13);
            this.textBoxCurFile.TabIndex = 0;
            // 
            // textBoxStdOut
            // 
            this.textBoxStdOut.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxStdOut.HideSelection = false;
            this.textBoxStdOut.Location = new System.Drawing.Point(7, 99);
            this.textBoxStdOut.MaxLength = 99999999;
            this.textBoxStdOut.Multiline = true;
            this.textBoxStdOut.Name = "textBoxStdOut";
            this.textBoxStdOut.ReadOnly = true;
            this.textBoxStdOut.Size = new System.Drawing.Size(362, 409);
            this.textBoxStdOut.TabIndex = 1;
            // 
            // labelPercent
            // 
            this.labelPercent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelPercent.AutoSize = true;
            this.labelPercent.Location = new System.Drawing.Point(321, 50);
            this.labelPercent.Name = "labelPercent";
            this.labelPercent.Size = new System.Drawing.Size(42, 13);
            this.labelPercent.TabIndex = 2;
            this.labelPercent.Text = "  0.00%";
            this.labelPercent.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelCount
            // 
            this.labelCount.AutoSize = true;
            this.labelCount.Location = new System.Drawing.Point(7, 50);
            this.labelCount.Name = "labelCount";
            this.labelCount.Size = new System.Drawing.Size(98, 13);
            this.labelCount.TabIndex = 1;
            this.labelCount.Text = "Scanning file 0 of 0";
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(7, 20);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(362, 23);
            this.progressBar1.TabIndex = 0;
            // 
            // splitContainerWithDivider
            // 
            this.splitContainerWithDivider.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainerWithDivider.Location = new System.Drawing.Point(13, 12);
            this.splitContainerWithDivider.Name = "splitContainerWithDivider";
            // 
            // splitContainerWithDivider.Panel1
            // 
            this.splitContainerWithDivider.Panel1.Controls.Add(this.groupBoxInput);
            // 
            // splitContainerWithDivider.Panel2
            // 
            this.splitContainerWithDivider.Panel2.Controls.Add(this.groupBoxProgress);
            this.splitContainerWithDivider.Size = new System.Drawing.Size(767, 520);
            this.splitContainerWithDivider.SplitterDistance = 382;
            this.splitContainerWithDivider.TabIndex = 10;
            // 
            // groupBoxInput
            // 
            this.groupBoxInput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxInput.Controls.Add(this.textBoxInputFiles);
            this.groupBoxInput.Location = new System.Drawing.Point(3, 3);
            this.groupBoxInput.Name = "groupBoxInput";
            this.groupBoxInput.Size = new System.Drawing.Size(376, 514);
            this.groupBoxInput.TabIndex = 0;
            this.groupBoxInput.TabStop = false;
            this.groupBoxInput.Text = "Media File Paths (separated by newlines)";
            // 
            // textBoxInputFiles
            // 
            this.textBoxInputFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxInputFiles.Location = new System.Drawing.Point(7, 20);
            this.textBoxInputFiles.Multiline = true;
            this.textBoxInputFiles.Name = "textBoxInputFiles";
            this.textBoxInputFiles.Size = new System.Drawing.Size(363, 488);
            this.textBoxInputFiles.TabIndex = 0;
            this.textBoxInputFiles.Text = "C:\\file1.m2ts\r\nE:\\file2.mkv\r\nF:\\file3.wav\r\n...";
            // 
            // buttonScan
            // 
            this.buttonScan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonScan.Location = new System.Drawing.Point(705, 538);
            this.buttonScan.Name = "buttonScan";
            this.buttonScan.Size = new System.Drawing.Size(75, 23);
            this.buttonScan.TabIndex = 1;
            this.buttonScan.Text = "Scan";
            this.buttonScan.UseVisualStyleBackColor = true;
            this.buttonScan.Click += new System.EventHandler(this.buttonScan_Click);
            // 
            // FormCodecScanner
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(792, 573);
            this.Controls.Add(this.buttonScan);
            this.Controls.Add(this.splitContainerWithDivider);
            this.Name = "FormCodecScanner";
            this.Text = "Codec Scanner";
            this.groupBoxProgress.ResumeLayout(false);
            this.groupBoxProgress.PerformLayout();
            this.splitContainerWithDivider.Panel1.ResumeLayout(false);
            this.splitContainerWithDivider.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerWithDivider)).EndInit();
            this.splitContainerWithDivider.ResumeLayout(false);
            this.groupBoxInput.ResumeLayout(false);
            this.groupBoxInput.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxProgress;
        private System.Windows.Forms.TextBox textBoxStdOut;
        private System.Windows.Forms.Label labelPercent;
        private System.Windows.Forms.Label labelCount;
        private System.Windows.Forms.ProgressBar progressBar1;
        private views.SplitContainerWithDivider splitContainerWithDivider;
        private System.Windows.Forms.GroupBox groupBoxInput;
        private System.Windows.Forms.Button buttonScan;
        private System.Windows.Forms.TextBox textBoxInputFiles;
        private System.Windows.Forms.TextBox textBoxCurFile;
    }
}