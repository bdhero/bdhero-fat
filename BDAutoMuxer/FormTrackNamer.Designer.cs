namespace BDAutoMuxer
{
    partial class FormTrackNamer
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
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxMkvPropEditPath = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.textBoxMkvInfoPath = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.textBoxToolsDirPath = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBoxProgress = new System.Windows.Forms.GroupBox();
            this.textBoxCurFile = new System.Windows.Forms.TextBox();
            this.textBoxStdOut = new System.Windows.Forms.TextBox();
            this.labelPercent = new System.Windows.Forms.Label();
            this.labelCount = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.splitContainerWithDivider = new BDAutoMuxer.views.SplitContainerWithDivider();
            this.groupBoxInput = new System.Windows.Forms.GroupBox();
            this.buttonScan = new System.Windows.Forms.Button();
            this.textBoxInputFiles = new System.Windows.Forms.TextBox();
            this.groupBoxProgress.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerWithDivider)).BeginInit();
            this.splitContainerWithDivider.Panel1.SuspendLayout();
            this.splitContainerWithDivider.Panel2.SuspendLayout();
            this.splitContainerWithDivider.SuspendLayout();
            this.groupBoxInput.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Path to mkvinfo.exe:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(125, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Path to mkvpropedit.exe:";
            // 
            // textBoxMkvPropEditPath
            // 
            this.textBoxMkvPropEditPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxMkvPropEditPath.Location = new System.Drawing.Point(144, 39);
            this.textBoxMkvPropEditPath.Name = "textBoxMkvPropEditPath";
            this.textBoxMkvPropEditPath.Size = new System.Drawing.Size(555, 20);
            this.textBoxMkvPropEditPath.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(705, 37);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Browse...";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(705, 8);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "Browse...";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // textBoxMkvInfoPath
            // 
            this.textBoxMkvInfoPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxMkvInfoPath.Location = new System.Drawing.Point(144, 10);
            this.textBoxMkvInfoPath.Name = "textBoxMkvInfoPath";
            this.textBoxMkvInfoPath.Size = new System.Drawing.Size(555, 20);
            this.textBoxMkvInfoPath.TabIndex = 0;
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button3.Location = new System.Drawing.Point(705, 66);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 5;
            this.button3.Text = "Browse...";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // textBoxToolsDirPath
            // 
            this.textBoxToolsDirPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxToolsDirPath.Location = new System.Drawing.Point(144, 68);
            this.textBoxToolsDirPath.Name = "textBoxToolsDirPath";
            this.textBoxToolsDirPath.Size = new System.Drawing.Size(555, 20);
            this.textBoxToolsDirPath.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 71);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Tools directory:";
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
            this.groupBoxProgress.Size = new System.Drawing.Size(375, 450);
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
            this.textBoxStdOut.Size = new System.Drawing.Size(362, 345);
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
            this.splitContainerWithDivider.Location = new System.Drawing.Point(13, 105);
            this.splitContainerWithDivider.Name = "splitContainerWithDivider";
            // 
            // splitContainerWithDivider.Panel1
            // 
            this.splitContainerWithDivider.Panel1.Controls.Add(this.groupBoxInput);
            // 
            // splitContainerWithDivider.Panel2
            // 
            this.splitContainerWithDivider.Panel2.Controls.Add(this.groupBoxProgress);
            this.splitContainerWithDivider.Size = new System.Drawing.Size(767, 456);
            this.splitContainerWithDivider.SplitterDistance = 382;
            this.splitContainerWithDivider.TabIndex = 10;
            // 
            // groupBoxInput
            // 
            this.groupBoxInput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxInput.Controls.Add(this.buttonScan);
            this.groupBoxInput.Controls.Add(this.textBoxInputFiles);
            this.groupBoxInput.Location = new System.Drawing.Point(3, 3);
            this.groupBoxInput.Name = "groupBoxInput";
            this.groupBoxInput.Size = new System.Drawing.Size(376, 450);
            this.groupBoxInput.TabIndex = 0;
            this.groupBoxInput.TabStop = false;
            this.groupBoxInput.Text = "Input Media Files";
            // 
            // buttonScan
            // 
            this.buttonScan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonScan.Location = new System.Drawing.Point(295, 421);
            this.buttonScan.Name = "buttonScan";
            this.buttonScan.Size = new System.Drawing.Size(75, 23);
            this.buttonScan.TabIndex = 1;
            this.buttonScan.Text = "Scan";
            this.buttonScan.UseVisualStyleBackColor = true;
            this.buttonScan.Click += new System.EventHandler(this.buttonScan_Click);
            // 
            // textBoxInputFiles
            // 
            this.textBoxInputFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxInputFiles.Location = new System.Drawing.Point(7, 20);
            this.textBoxInputFiles.Multiline = true;
            this.textBoxInputFiles.Name = "textBoxInputFiles";
            this.textBoxInputFiles.Size = new System.Drawing.Size(363, 395);
            this.textBoxInputFiles.TabIndex = 0;
            // 
            // FormTrackNamer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(792, 573);
            this.Controls.Add(this.splitContainerWithDivider);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.textBoxToolsDirPath);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.textBoxMkvInfoPath);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBoxMkvPropEditPath);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "FormTrackNamer";
            this.Text = "FormTrackNamer";
            this.groupBoxProgress.ResumeLayout(false);
            this.groupBoxProgress.PerformLayout();
            this.splitContainerWithDivider.Panel1.ResumeLayout(false);
            this.splitContainerWithDivider.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerWithDivider)).EndInit();
            this.splitContainerWithDivider.ResumeLayout(false);
            this.groupBoxInput.ResumeLayout(false);
            this.groupBoxInput.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxMkvPropEditPath;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox textBoxMkvInfoPath;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox textBoxToolsDirPath;
        private System.Windows.Forms.Label label3;
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