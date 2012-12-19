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
            this.progressLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStripProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonRemux = new System.Windows.Forms.Button();
            this.comboBoxKeepAudioFrom = new System.Windows.Forms.ComboBox();
            this.comboBoxKeepSubtitlesFrom = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.panelInputSubtitles = new System.Windows.Forms.FlowLayoutPanel();
            this.labelInputSubtitlesNone = new System.Windows.Forms.Label();
            this.panelInputLPCM = new System.Windows.Forms.FlowLayoutPanel();
            this.labelInputLPCMNone = new System.Windows.Forms.Label();
            this.panelInputMkvAudio = new System.Windows.Forms.FlowLayoutPanel();
            this.labelInputMkvAudioNone = new System.Windows.Forms.Label();
            this.panelInputM2tsAudio = new System.Windows.Forms.FlowLayoutPanel();
            this.labelInputM2tsAudioNone = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.buttonMoveDown = new System.Windows.Forms.Button();
            this.buttonMoveUp = new System.Windows.Forms.Button();
            this.objectListViewTracks = new BrightIdeasSoftware.ObjectListView();
            this.olvColumnTitle = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnCodec = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnResolution = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnSource = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.label9 = new System.Windows.Forms.Label();
            this.linkLabelAddSubtitles = new BDAutoMuxer.views.LinkLabel2();
            this.linkLabelEditSubtitles = new BDAutoMuxer.views.LinkLabel2();
            this.linkLabelClearSubtitles = new BDAutoMuxer.views.LinkLabel2();
            this.linkLabelAddLPCM = new BDAutoMuxer.views.LinkLabel2();
            this.linkLabelEditLPCM = new BDAutoMuxer.views.LinkLabel2();
            this.linkLabelClearLPCM = new BDAutoMuxer.views.LinkLabel2();
            this.statusStrip.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panelInputSubtitles.SuspendLayout();
            this.panelInputLPCM.SuspendLayout();
            this.panelInputMkvAudio.SuspendLayout();
            this.panelInputM2tsAudio.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.objectListViewTracks)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(6, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "M2TS (source):";
            // 
            // textBoxInputM2ts
            // 
            this.textBoxInputM2ts.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxInputM2ts.Location = new System.Drawing.Point(124, 24);
            this.textBoxInputM2ts.Name = "textBoxInputM2ts";
            this.textBoxInputM2ts.Size = new System.Drawing.Size(461, 20);
            this.textBoxInputM2ts.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(6, 79);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(112, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "MKV (HandBrake):";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 131);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Chapters:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(87, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Keep audio from:";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(5, 218);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(70, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Output file:";
            // 
            // buttonInputM2tsBrowse
            // 
            this.buttonInputM2tsBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonInputM2tsBrowse.Location = new System.Drawing.Point(591, 21);
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
            this.buttonInputMkvBrowse.Location = new System.Drawing.Point(591, 73);
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
            this.buttonInputChaptersBrowse.Location = new System.Drawing.Point(591, 125);
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
            this.textBoxInputMkv.Location = new System.Drawing.Point(124, 76);
            this.textBoxInputMkv.Name = "textBoxInputMkv";
            this.textBoxInputMkv.Size = new System.Drawing.Size(461, 20);
            this.textBoxInputMkv.TabIndex = 2;
            // 
            // textBoxInputChapters
            // 
            this.textBoxInputChapters.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxInputChapters.Location = new System.Drawing.Point(124, 128);
            this.textBoxInputChapters.Name = "textBoxInputChapters";
            this.textBoxInputChapters.Size = new System.Drawing.Size(461, 20);
            this.textBoxInputChapters.TabIndex = 4;
            // 
            // textBoxOutputMkv
            // 
            this.textBoxOutputMkv.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxOutputMkv.Location = new System.Drawing.Point(111, 215);
            this.textBoxOutputMkv.Name = "textBoxOutputMkv";
            this.textBoxOutputMkv.Size = new System.Drawing.Size(474, 20);
            this.textBoxOutputMkv.TabIndex = 5;
            // 
            // buttonOutputMkvBrowse
            // 
            this.buttonOutputMkvBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOutputMkvBrowse.Location = new System.Drawing.Point(591, 213);
            this.buttonOutputMkvBrowse.Name = "buttonOutputMkvBrowse";
            this.buttonOutputMkvBrowse.Size = new System.Drawing.Size(75, 23);
            this.buttonOutputMkvBrowse.TabIndex = 6;
            this.buttonOutputMkvBrowse.Text = "Browse...";
            this.buttonOutputMkvBrowse.UseVisualStyleBackColor = true;
            this.buttonOutputMkvBrowse.Click += new System.EventHandler(this.buttonOutputMkvBrowse_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusStripLabel,
            this.progressLabel,
            this.statusStripProgressBar});
            this.statusStrip.Location = new System.Drawing.Point(0, 519);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(707, 22);
            this.statusStrip.TabIndex = 19;
            this.statusStrip.Text = "statusStrip";
            // 
            // statusStripLabel
            // 
            this.statusStripLabel.Name = "statusStripLabel";
            this.statusStripLabel.Size = new System.Drawing.Size(456, 17);
            this.statusStripLabel.Spring = true;
            this.statusStripLabel.Text = "Drag files onto this window";
            this.statusStripLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // progressLabel
            // 
            this.progressLabel.Name = "progressLabel";
            this.progressLabel.Size = new System.Drawing.Size(34, 17);
            this.progressLabel.Text = "0.0%";
            this.progressLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonClose.Location = new System.Drawing.Point(620, 483);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 23);
            this.buttonClose.TabIndex = 1;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonRemux
            // 
            this.buttonRemux.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonRemux.Location = new System.Drawing.Point(539, 483);
            this.buttonRemux.Name = "buttonRemux";
            this.buttonRemux.Size = new System.Drawing.Size(75, 23);
            this.buttonRemux.TabIndex = 0;
            this.buttonRemux.Text = "Mux!";
            this.buttonRemux.UseVisualStyleBackColor = true;
            this.buttonRemux.Click += new System.EventHandler(this.buttonRemux_Click);
            // 
            // comboBoxKeepAudioFrom
            // 
            this.comboBoxKeepAudioFrom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxKeepAudioFrom.FormattingEnabled = true;
            this.comboBoxKeepAudioFrom.Location = new System.Drawing.Point(111, 19);
            this.comboBoxKeepAudioFrom.Name = "comboBoxKeepAudioFrom";
            this.comboBoxKeepAudioFrom.Size = new System.Drawing.Size(121, 21);
            this.comboBoxKeepAudioFrom.TabIndex = 0;
            // 
            // comboBoxKeepSubtitlesFrom
            // 
            this.comboBoxKeepSubtitlesFrom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxKeepSubtitlesFrom.FormattingEnabled = true;
            this.comboBoxKeepSubtitlesFrom.Location = new System.Drawing.Point(111, 46);
            this.comboBoxKeepSubtitlesFrom.Name = "comboBoxKeepSubtitlesFrom";
            this.comboBoxKeepSubtitlesFrom.Size = new System.Drawing.Size(121, 21);
            this.comboBoxKeepSubtitlesFrom.TabIndex = 1;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 49);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(99, 13);
            this.label6.TabIndex = 21;
            this.label6.Text = "Keep subtitles from:";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.panelInputSubtitles);
            this.groupBox1.Controls.Add(this.panelInputLPCM);
            this.groupBox1.Controls.Add(this.panelInputMkvAudio);
            this.groupBox1.Controls.Add(this.panelInputM2tsAudio);
            this.groupBox1.Controls.Add(this.buttonInputM2tsBrowse);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.textBoxInputM2ts);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.buttonInputMkvBrowse);
            this.groupBox1.Controls.Add(this.buttonInputChaptersBrowse);
            this.groupBox1.Controls.Add(this.textBoxInputMkv);
            this.groupBox1.Controls.Add(this.textBoxInputChapters);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(683, 211);
            this.groupBox1.TabIndex = 23;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Source files";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 184);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(97, 13);
            this.label8.TabIndex = 11;
            this.label8.Text = "Additional subtitles:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 158);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(68, 13);
            this.label7.TabIndex = 9;
            this.label7.Text = "LPCM audio:";
            // 
            // panelInputSubtitles
            // 
            this.panelInputSubtitles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelInputSubtitles.Controls.Add(this.labelInputSubtitlesNone);
            this.panelInputSubtitles.Controls.Add(this.linkLabelAddSubtitles);
            this.panelInputSubtitles.Controls.Add(this.linkLabelEditSubtitles);
            this.panelInputSubtitles.Controls.Add(this.linkLabelClearSubtitles);
            this.panelInputSubtitles.Location = new System.Drawing.Point(124, 180);
            this.panelInputSubtitles.Name = "panelInputSubtitles";
            this.panelInputSubtitles.Size = new System.Drawing.Size(461, 20);
            this.panelInputSubtitles.TabIndex = 10;
            // 
            // labelInputSubtitlesNone
            // 
            this.labelInputSubtitlesNone.AutoSize = true;
            this.labelInputSubtitlesNone.Location = new System.Drawing.Point(3, 3);
            this.labelInputSubtitlesNone.Margin = new System.Windows.Forms.Padding(3);
            this.labelInputSubtitlesNone.Name = "labelInputSubtitlesNone";
            this.labelInputSubtitlesNone.Size = new System.Drawing.Size(37, 13);
            this.labelInputSubtitlesNone.TabIndex = 2;
            this.labelInputSubtitlesNone.Text = "(none)";
            // 
            // panelInputLPCM
            // 
            this.panelInputLPCM.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelInputLPCM.Controls.Add(this.labelInputLPCMNone);
            this.panelInputLPCM.Controls.Add(this.linkLabelAddLPCM);
            this.panelInputLPCM.Controls.Add(this.linkLabelEditLPCM);
            this.panelInputLPCM.Controls.Add(this.linkLabelClearLPCM);
            this.panelInputLPCM.Location = new System.Drawing.Point(124, 154);
            this.panelInputLPCM.Name = "panelInputLPCM";
            this.panelInputLPCM.Size = new System.Drawing.Size(461, 20);
            this.panelInputLPCM.TabIndex = 8;
            // 
            // labelInputLPCMNone
            // 
            this.labelInputLPCMNone.AutoSize = true;
            this.labelInputLPCMNone.Location = new System.Drawing.Point(3, 3);
            this.labelInputLPCMNone.Margin = new System.Windows.Forms.Padding(3);
            this.labelInputLPCMNone.Name = "labelInputLPCMNone";
            this.labelInputLPCMNone.Size = new System.Drawing.Size(37, 13);
            this.labelInputLPCMNone.TabIndex = 2;
            this.labelInputLPCMNone.Text = "(none)";
            // 
            // panelInputMkvAudio
            // 
            this.panelInputMkvAudio.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelInputMkvAudio.Controls.Add(this.labelInputMkvAudioNone);
            this.panelInputMkvAudio.Location = new System.Drawing.Point(124, 102);
            this.panelInputMkvAudio.Name = "panelInputMkvAudio";
            this.panelInputMkvAudio.Size = new System.Drawing.Size(461, 20);
            this.panelInputMkvAudio.TabIndex = 7;
            // 
            // labelInputMkvAudioNone
            // 
            this.labelInputMkvAudioNone.AutoSize = true;
            this.labelInputMkvAudioNone.Location = new System.Drawing.Point(3, 3);
            this.labelInputMkvAudioNone.Margin = new System.Windows.Forms.Padding(3);
            this.labelInputMkvAudioNone.Name = "labelInputMkvAudioNone";
            this.labelInputMkvAudioNone.Size = new System.Drawing.Size(37, 13);
            this.labelInputMkvAudioNone.TabIndex = 1;
            this.labelInputMkvAudioNone.Text = "(none)";
            // 
            // panelInputM2tsAudio
            // 
            this.panelInputM2tsAudio.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelInputM2tsAudio.Controls.Add(this.labelInputM2tsAudioNone);
            this.panelInputM2tsAudio.Location = new System.Drawing.Point(124, 50);
            this.panelInputM2tsAudio.Name = "panelInputM2tsAudio";
            this.panelInputM2tsAudio.Size = new System.Drawing.Size(461, 20);
            this.panelInputM2tsAudio.TabIndex = 6;
            // 
            // labelInputM2tsAudioNone
            // 
            this.labelInputM2tsAudioNone.AutoSize = true;
            this.labelInputM2tsAudioNone.Location = new System.Drawing.Point(3, 3);
            this.labelInputM2tsAudioNone.Margin = new System.Windows.Forms.Padding(3);
            this.labelInputM2tsAudioNone.Name = "labelInputM2tsAudioNone";
            this.labelInputM2tsAudioNone.Size = new System.Drawing.Size(37, 13);
            this.labelInputM2tsAudioNone.TabIndex = 0;
            this.labelInputM2tsAudioNone.Text = "(none)";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.buttonMoveDown);
            this.groupBox2.Controls.Add(this.buttonMoveUp);
            this.groupBox2.Controls.Add(this.objectListViewTracks);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.comboBoxKeepAudioFrom);
            this.groupBox2.Controls.Add(this.comboBoxKeepSubtitlesFrom);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.buttonOutputMkvBrowse);
            this.groupBox2.Controls.Add(this.textBoxOutputMkv);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Location = new System.Drawing.Point(12, 229);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(683, 248);
            this.groupBox2.TabIndex = 24;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Output";
            // 
            // buttonMoveDown
            // 
            this.buttonMoveDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonMoveDown.Location = new System.Drawing.Point(591, 103);
            this.buttonMoveDown.Name = "buttonMoveDown";
            this.buttonMoveDown.Size = new System.Drawing.Size(75, 23);
            this.buttonMoveDown.TabIndex = 4;
            this.buttonMoveDown.Text = "Move Down";
            this.buttonMoveDown.UseVisualStyleBackColor = true;
            // 
            // buttonMoveUp
            // 
            this.buttonMoveUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonMoveUp.Location = new System.Drawing.Point(591, 74);
            this.buttonMoveUp.Name = "buttonMoveUp";
            this.buttonMoveUp.Size = new System.Drawing.Size(75, 23);
            this.buttonMoveUp.TabIndex = 3;
            this.buttonMoveUp.Text = "Move Up";
            this.buttonMoveUp.UseVisualStyleBackColor = true;
            // 
            // objectListViewTracks
            // 
            this.objectListViewTracks.AllColumns.Add(this.olvColumnTitle);
            this.objectListViewTracks.AllColumns.Add(this.olvColumnCodec);
            this.objectListViewTracks.AllColumns.Add(this.olvColumnResolution);
            this.objectListViewTracks.AllColumns.Add(this.olvColumnSource);
            this.objectListViewTracks.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.objectListViewTracks.CheckBoxes = true;
            this.objectListViewTracks.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumnTitle,
            this.olvColumnCodec,
            this.olvColumnResolution,
            this.olvColumnSource});
            this.objectListViewTracks.EmptyListMsg = "No input files selected";
            this.objectListViewTracks.FullRowSelect = true;
            this.objectListViewTracks.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.objectListViewTracks.Location = new System.Drawing.Point(111, 74);
            this.objectListViewTracks.MultiSelect = false;
            this.objectListViewTracks.Name = "objectListViewTracks";
            this.objectListViewTracks.Size = new System.Drawing.Size(474, 135);
            this.objectListViewTracks.TabIndex = 2;
            this.objectListViewTracks.UseCompatibleStateImageBehavior = false;
            this.objectListViewTracks.View = System.Windows.Forms.View.Details;
            // 
            // olvColumnTitle
            // 
            this.olvColumnTitle.CellPadding = null;
            this.olvColumnTitle.Hideable = false;
            this.olvColumnTitle.Text = "Title";
            this.olvColumnTitle.Width = 180;
            // 
            // olvColumnCodec
            // 
            this.olvColumnCodec.CellPadding = null;
            this.olvColumnCodec.Text = "Codec";
            this.olvColumnCodec.Width = 140;
            // 
            // olvColumnResolution
            // 
            this.olvColumnResolution.CellPadding = null;
            this.olvColumnResolution.Text = "Resolution";
            this.olvColumnResolution.Width = 70;
            // 
            // olvColumnSource
            // 
            this.olvColumnSource.CellPadding = null;
            this.olvColumnSource.Text = "Source";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 74);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(72, 13);
            this.label9.TabIndex = 24;
            this.label9.Text = "Select tracks:";
            // 
            // linkLabelAddSubtitles
            // 
            this.linkLabelAddSubtitles.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.linkLabelAddSubtitles.HoverColor = System.Drawing.Color.Empty;
            this.linkLabelAddSubtitles.Location = new System.Drawing.Point(46, 3);
            this.linkLabelAddSubtitles.Name = "linkLabelAddSubtitles";
            this.linkLabelAddSubtitles.RegularColor = System.Drawing.Color.Empty;
            this.linkLabelAddSubtitles.Size = new System.Drawing.Size(26, 14);
            this.linkLabelAddSubtitles.TabIndex = 0;
            this.linkLabelAddSubtitles.Text = "add";
            this.linkLabelAddSubtitles.Click += new System.EventHandler(this.linkLabelAddSubtitles_Click);
            // 
            // linkLabelEditSubtitles
            // 
            this.linkLabelEditSubtitles.Enabled = false;
            this.linkLabelEditSubtitles.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.linkLabelEditSubtitles.HoverColor = System.Drawing.Color.Empty;
            this.linkLabelEditSubtitles.Location = new System.Drawing.Point(78, 3);
            this.linkLabelEditSubtitles.Name = "linkLabelEditSubtitles";
            this.linkLabelEditSubtitles.RegularColor = System.Drawing.Color.Empty;
            this.linkLabelEditSubtitles.Size = new System.Drawing.Size(25, 14);
            this.linkLabelEditSubtitles.TabIndex = 1;
            this.linkLabelEditSubtitles.Text = "edit";
            // 
            // linkLabelClearSubtitles
            // 
            this.linkLabelClearSubtitles.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.linkLabelClearSubtitles.HoverColor = System.Drawing.Color.Empty;
            this.linkLabelClearSubtitles.Location = new System.Drawing.Point(109, 3);
            this.linkLabelClearSubtitles.Name = "linkLabelClearSubtitles";
            this.linkLabelClearSubtitles.RegularColor = System.Drawing.Color.Empty;
            this.linkLabelClearSubtitles.Size = new System.Drawing.Size(31, 14);
            this.linkLabelClearSubtitles.TabIndex = 4;
            this.linkLabelClearSubtitles.Text = "clear";
            this.linkLabelClearSubtitles.Click += new System.EventHandler(this.linkLabelClearSubtitles_Click);
            // 
            // linkLabelAddLPCM
            // 
            this.linkLabelAddLPCM.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.linkLabelAddLPCM.HoverColor = System.Drawing.Color.Empty;
            this.linkLabelAddLPCM.Location = new System.Drawing.Point(46, 3);
            this.linkLabelAddLPCM.Name = "linkLabelAddLPCM";
            this.linkLabelAddLPCM.RegularColor = System.Drawing.Color.Empty;
            this.linkLabelAddLPCM.Size = new System.Drawing.Size(26, 14);
            this.linkLabelAddLPCM.TabIndex = 0;
            this.linkLabelAddLPCM.Text = "add";
            this.linkLabelAddLPCM.Click += new System.EventHandler(this.linkLabelAddLPCM_Click);
            // 
            // linkLabelEditLPCM
            // 
            this.linkLabelEditLPCM.Enabled = false;
            this.linkLabelEditLPCM.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.linkLabelEditLPCM.HoverColor = System.Drawing.Color.Empty;
            this.linkLabelEditLPCM.Location = new System.Drawing.Point(78, 3);
            this.linkLabelEditLPCM.Name = "linkLabelEditLPCM";
            this.linkLabelEditLPCM.RegularColor = System.Drawing.Color.Empty;
            this.linkLabelEditLPCM.Size = new System.Drawing.Size(25, 14);
            this.linkLabelEditLPCM.TabIndex = 1;
            this.linkLabelEditLPCM.Text = "edit";
            // 
            // linkLabelClearLPCM
            // 
            this.linkLabelClearLPCM.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.linkLabelClearLPCM.HoverColor = System.Drawing.Color.Empty;
            this.linkLabelClearLPCM.Location = new System.Drawing.Point(109, 3);
            this.linkLabelClearLPCM.Name = "linkLabelClearLPCM";
            this.linkLabelClearLPCM.RegularColor = System.Drawing.Color.Empty;
            this.linkLabelClearLPCM.Size = new System.Drawing.Size(31, 14);
            this.linkLabelClearLPCM.TabIndex = 3;
            this.linkLabelClearLPCM.Text = "clear";
            this.linkLabelClearLPCM.Click += new System.EventHandler(this.linkLabelClearLPCM_Click);
            // 
            // FormRemux
            // 
            this.AcceptButton = this.buttonRemux;
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonClose;
            this.ClientSize = new System.Drawing.Size(707, 541);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonRemux);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.statusStrip);
            this.Name = "FormRemux";
            this.Text = "Remuxer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormRemux_FormClosing);
            this.Load += new System.EventHandler(this.FormRemux_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.FormRemux_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.FormRemux_DragEnter);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panelInputSubtitles.ResumeLayout(false);
            this.panelInputSubtitles.PerformLayout();
            this.panelInputLPCM.ResumeLayout(false);
            this.panelInputLPCM.PerformLayout();
            this.panelInputMkvAudio.ResumeLayout(false);
            this.panelInputMkvAudio.PerformLayout();
            this.panelInputM2tsAudio.ResumeLayout(false);
            this.panelInputM2tsAudio.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.objectListViewTracks)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxInputM2ts;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
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
        private System.Windows.Forms.ToolStripStatusLabel progressLabel;
        private System.Windows.Forms.ComboBox comboBoxKeepAudioFrom;
        private System.Windows.Forms.ComboBox comboBoxKeepSubtitlesFrom;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.FlowLayoutPanel panelInputSubtitles;
        private System.Windows.Forms.FlowLayoutPanel panelInputLPCM;
        private System.Windows.Forms.FlowLayoutPanel panelInputMkvAudio;
        private System.Windows.Forms.FlowLayoutPanel panelInputM2tsAudio;
        private System.Windows.Forms.Button buttonMoveDown;
        private System.Windows.Forms.Button buttonMoveUp;
        private BrightIdeasSoftware.ObjectListView objectListViewTracks;
        private BrightIdeasSoftware.OLVColumn olvColumnTitle;
        private BrightIdeasSoftware.OLVColumn olvColumnCodec;
        private BrightIdeasSoftware.OLVColumn olvColumnResolution;
        private BrightIdeasSoftware.OLVColumn olvColumnSource;
        private views.LinkLabel2 linkLabelEditSubtitles;
        private views.LinkLabel2 linkLabelEditLPCM;
        private System.Windows.Forms.Label labelInputSubtitlesNone;
        private System.Windows.Forms.Label labelInputLPCMNone;
        private System.Windows.Forms.Label labelInputMkvAudioNone;
        private System.Windows.Forms.Label labelInputM2tsAudioNone;
        private views.LinkLabel2 linkLabelAddSubtitles;
        private views.LinkLabel2 linkLabelAddLPCM;
        private views.LinkLabel2 linkLabelClearSubtitles;
        private views.LinkLabel2 linkLabelClearLPCM;
    }
}