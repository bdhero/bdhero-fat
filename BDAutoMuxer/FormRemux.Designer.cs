namespace BDAutoMuxer
{
    partial class FormRemux
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
            this.textBoxInputM2ts = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.radioButtonUseM2tsAudio = new System.Windows.Forms.RadioButton();
            this.radioButtonUseMkvAudio = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.buttonInputM2tsBrowse = new System.Windows.Forms.Button();
            this.buttonInputMkvBrowse = new System.Windows.Forms.Button();
            this.buttonInputChaptersBrowse = new System.Windows.Forms.Button();
            this.textBoxInputMkv = new System.Windows.Forms.TextBox();
            this.textBoxInputChapters = new System.Windows.Forms.TextBox();
            this.textBoxOutputMkv = new System.Windows.Forms.TextBox();
            this.buttonOutputMkvBrowse = new System.Windows.Forms.Button();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.statusStripLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStripProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonRemux = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "M2TS (source):";
            // 
            // textBoxInputM2ts
            // 
            this.textBoxInputM2ts.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxInputM2ts.Location = new System.Drawing.Point(116, 15);
            this.textBoxInputM2ts.Name = "textBoxInputM2ts";
            this.textBoxInputM2ts.Size = new System.Drawing.Size(362, 20);
            this.textBoxInputM2ts.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "MKV (HandBrake):";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 76);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Chapters:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 104);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(87, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Keep audio from:";
            // 
            // radioButtonUseM2tsAudio
            // 
            this.radioButtonUseM2tsAudio.AutoSize = true;
            this.radioButtonUseM2tsAudio.Checked = true;
            this.radioButtonUseM2tsAudio.Location = new System.Drawing.Point(3, 3);
            this.radioButtonUseM2tsAudio.Name = "radioButtonUseM2tsAudio";
            this.radioButtonUseM2tsAudio.Size = new System.Drawing.Size(95, 17);
            this.radioButtonUseM2tsAudio.TabIndex = 0;
            this.radioButtonUseM2tsAudio.TabStop = true;
            this.radioButtonUseM2tsAudio.Text = "M2TS (source)";
            this.radioButtonUseM2tsAudio.UseVisualStyleBackColor = true;
            // 
            // radioButtonUseMkvAudio
            // 
            this.radioButtonUseMkvAudio.AutoSize = true;
            this.radioButtonUseMkvAudio.Location = new System.Drawing.Point(3, 27);
            this.radioButtonUseMkvAudio.Name = "radioButtonUseMkvAudio";
            this.radioButtonUseMkvAudio.Size = new System.Drawing.Size(111, 17);
            this.radioButtonUseMkvAudio.TabIndex = 1;
            this.radioButtonUseMkvAudio.Text = "MKV (HandBrake)";
            this.radioButtonUseMkvAudio.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.radioButtonUseM2tsAudio);
            this.panel1.Controls.Add(this.radioButtonUseMkvAudio);
            this.panel1.Location = new System.Drawing.Point(116, 99);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(118, 48);
            this.panel1.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 156);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(42, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Output:";
            // 
            // buttonInputM2tsBrowse
            // 
            this.buttonInputM2tsBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonInputM2tsBrowse.Location = new System.Drawing.Point(484, 13);
            this.buttonInputM2tsBrowse.Name = "buttonInputM2tsBrowse";
            this.buttonInputM2tsBrowse.Size = new System.Drawing.Size(75, 23);
            this.buttonInputM2tsBrowse.TabIndex = 1;
            this.buttonInputM2tsBrowse.Text = "Browse...";
            this.buttonInputM2tsBrowse.UseVisualStyleBackColor = true;
            this.buttonInputM2tsBrowse.Click += new System.EventHandler(this.buttonInputM2tsBrowse_Click);
            // 
            // buttonInputMkvBrowse
            // 
            this.buttonInputMkvBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonInputMkvBrowse.Location = new System.Drawing.Point(484, 42);
            this.buttonInputMkvBrowse.Name = "buttonInputMkvBrowse";
            this.buttonInputMkvBrowse.Size = new System.Drawing.Size(75, 23);
            this.buttonInputMkvBrowse.TabIndex = 3;
            this.buttonInputMkvBrowse.Text = "Browse...";
            this.buttonInputMkvBrowse.UseVisualStyleBackColor = true;
            this.buttonInputMkvBrowse.Click += new System.EventHandler(this.buttonInputMkvBrowse_Click);
            // 
            // buttonInputChaptersBrowse
            // 
            this.buttonInputChaptersBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonInputChaptersBrowse.Location = new System.Drawing.Point(484, 71);
            this.buttonInputChaptersBrowse.Name = "buttonInputChaptersBrowse";
            this.buttonInputChaptersBrowse.Size = new System.Drawing.Size(75, 23);
            this.buttonInputChaptersBrowse.TabIndex = 5;
            this.buttonInputChaptersBrowse.Text = "Browse...";
            this.buttonInputChaptersBrowse.UseVisualStyleBackColor = true;
            this.buttonInputChaptersBrowse.Click += new System.EventHandler(this.buttonInputChaptersBrowse_Click);
            // 
            // textBoxInputMkv
            // 
            this.textBoxInputMkv.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxInputMkv.Location = new System.Drawing.Point(116, 44);
            this.textBoxInputMkv.Name = "textBoxInputMkv";
            this.textBoxInputMkv.Size = new System.Drawing.Size(362, 20);
            this.textBoxInputMkv.TabIndex = 2;
            // 
            // textBoxInputChapters
            // 
            this.textBoxInputChapters.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxInputChapters.Location = new System.Drawing.Point(116, 73);
            this.textBoxInputChapters.Name = "textBoxInputChapters";
            this.textBoxInputChapters.Size = new System.Drawing.Size(362, 20);
            this.textBoxInputChapters.TabIndex = 4;
            // 
            // textBoxOutputMkv
            // 
            this.textBoxOutputMkv.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxOutputMkv.Location = new System.Drawing.Point(116, 153);
            this.textBoxOutputMkv.Name = "textBoxOutputMkv";
            this.textBoxOutputMkv.Size = new System.Drawing.Size(362, 20);
            this.textBoxOutputMkv.TabIndex = 6;
            // 
            // buttonOutputMkvBrowse
            // 
            this.buttonOutputMkvBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOutputMkvBrowse.Location = new System.Drawing.Point(484, 151);
            this.buttonOutputMkvBrowse.Name = "buttonOutputMkvBrowse";
            this.buttonOutputMkvBrowse.Size = new System.Drawing.Size(75, 23);
            this.buttonOutputMkvBrowse.TabIndex = 7;
            this.buttonOutputMkvBrowse.Text = "Browse...";
            this.buttonOutputMkvBrowse.UseVisualStyleBackColor = true;
            this.buttonOutputMkvBrowse.Click += new System.EventHandler(this.buttonOutputMkvBrowse_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusStripLabel,
            this.statusStripProgressBar});
            this.statusStrip.Location = new System.Drawing.Point(0, 212);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(571, 22);
            this.statusStrip.TabIndex = 19;
            this.statusStrip.Text = "statusStrip";
            // 
            // statusStripLabel
            // 
            this.statusStripLabel.Name = "statusStripLabel";
            this.statusStripLabel.Size = new System.Drawing.Size(354, 17);
            this.statusStripLabel.Spring = true;
            this.statusStripLabel.Text = "Drag files onto this window";
            this.statusStripLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // statusStripProgressBar
            // 
            this.statusStripProgressBar.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.statusStripProgressBar.Maximum = 1000;
            this.statusStripProgressBar.Name = "statusStripProgressBar";
            this.statusStripProgressBar.Size = new System.Drawing.Size(200, 16);
            this.statusStripProgressBar.Step = 1;
            this.statusStripProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonClose.Location = new System.Drawing.Point(483, 181);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 23);
            this.buttonClose.TabIndex = 9;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonRemux
            // 
            this.buttonRemux.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonRemux.Location = new System.Drawing.Point(402, 181);
            this.buttonRemux.Name = "buttonRemux";
            this.buttonRemux.Size = new System.Drawing.Size(75, 23);
            this.buttonRemux.TabIndex = 8;
            this.buttonRemux.Text = "Mux!";
            this.buttonRemux.UseVisualStyleBackColor = true;
            this.buttonRemux.Click += new System.EventHandler(this.buttonRemux_Click);
            // 
            // FormRemux
            // 
            this.AcceptButton = this.buttonRemux;
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonClose;
            this.ClientSize = new System.Drawing.Size(571, 234);
            this.Controls.Add(this.buttonRemux);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.textBoxOutputMkv);
            this.Controls.Add(this.buttonOutputMkvBrowse);
            this.Controls.Add(this.textBoxInputChapters);
            this.Controls.Add(this.textBoxInputMkv);
            this.Controls.Add(this.buttonInputChaptersBrowse);
            this.Controls.Add(this.buttonInputMkvBrowse);
            this.Controls.Add(this.buttonInputM2tsBrowse);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxInputM2ts);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.Name = "FormRemux";
            this.Text = "Remuxer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormRemux_FormClosing);
            this.Load += new System.EventHandler(this.FormRemux_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.FormRemux_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.FormRemux_DragEnter);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxInputM2ts;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RadioButton radioButtonUseM2tsAudio;
        private System.Windows.Forms.RadioButton radioButtonUseMkvAudio;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button buttonInputM2tsBrowse;
        private System.Windows.Forms.Button buttonInputMkvBrowse;
        private System.Windows.Forms.Button buttonInputChaptersBrowse;
        private System.Windows.Forms.TextBox textBoxInputMkv;
        private System.Windows.Forms.TextBox textBoxInputChapters;
        private System.Windows.Forms.TextBox textBoxOutputMkv;
        private System.Windows.Forms.Button buttonOutputMkvBrowse;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel statusStripLabel;
        private System.Windows.Forms.ToolStripProgressBar statusStripProgressBar;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonRemux;
    }
}