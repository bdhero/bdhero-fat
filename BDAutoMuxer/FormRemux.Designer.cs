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
            this.buttonInputM2tsBrowse = new System.Windows.Forms.Button();
            this.buttonInputMkvBrowse = new System.Windows.Forms.Button();
            this.buttonInputChaptersBrowse = new System.Windows.Forms.Button();
            this.textBoxInputMkv = new System.Windows.Forms.TextBox();
            this.textBoxInputChapters = new System.Windows.Forms.TextBox();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.statusStripLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.progressLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStripProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonRemux = new System.Windows.Forms.Button();
            this.groupBoxInput = new System.Windows.Forms.GroupBox();
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
            this.groupBoxOutput = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonOutputMkvBrowse = new System.Windows.Forms.Button();
            this.textBoxOutputMkv = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.comboBoxKeepAudioFrom = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBoxKeepSubtitlesFrom = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.splitContainerTracks = new BDAutoMuxer.views.SplitContainerWithDivider();
            this.buttonMoveDownAudio = new System.Windows.Forms.Button();
            this.buttonMoveUpAudio = new System.Windows.Forms.Button();
            this.objectListViewAudioTracks = new BrightIdeasSoftware.ObjectListView();
            this.audioTitle = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.audioCodec = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.audioChannels = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.audioSource = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.audioLanguage = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.audioDefault = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.audioForced = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.label9 = new System.Windows.Forms.Label();
            this.buttonMoveDownSubtitles = new System.Windows.Forms.Button();
            this.buttonMoveUpSubtitles = new System.Windows.Forms.Button();
            this.objectListViewSubtitleTracks = new BrightIdeasSoftware.ObjectListView();
            this.subtitleTitle = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.subtitleCodec = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.subtitleSource = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.subtitleLanguage = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.subtitleDefault = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.subtitleForced = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.label10 = new System.Windows.Forms.Label();
            this.linkLabelAddSubtitles = new BDAutoMuxer.views.LinkLabel2();
            this.linkLabelEditSubtitles = new BDAutoMuxer.views.LinkLabel2();
            this.linkLabelClearSubtitles = new BDAutoMuxer.views.LinkLabel2();
            this.linkLabelAddLPCM = new BDAutoMuxer.views.LinkLabel2();
            this.linkLabelEditLPCM = new BDAutoMuxer.views.LinkLabel2();
            this.linkLabelClearLPCM = new BDAutoMuxer.views.LinkLabel2();
            this.statusStrip.SuspendLayout();
            this.groupBoxInput.SuspendLayout();
            this.panelInputSubtitles.SuspendLayout();
            this.panelInputLPCM.SuspendLayout();
            this.panelInputMkvAudio.SuspendLayout();
            this.panelInputM2tsAudio.SuspendLayout();
            this.groupBoxOutput.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerTracks)).BeginInit();
            this.splitContainerTracks.Panel1.SuspendLayout();
            this.splitContainerTracks.Panel2.SuspendLayout();
            this.splitContainerTracks.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.objectListViewAudioTracks)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.objectListViewSubtitleTracks)).BeginInit();
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
            this.textBoxInputM2ts.Size = new System.Drawing.Size(641, 20);
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
            // buttonInputM2tsBrowse
            // 
            this.buttonInputM2tsBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonInputM2tsBrowse.Location = new System.Drawing.Point(771, 21);
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
            this.buttonInputMkvBrowse.Location = new System.Drawing.Point(771, 73);
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
            this.buttonInputChaptersBrowse.Location = new System.Drawing.Point(771, 125);
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
            this.textBoxInputMkv.Size = new System.Drawing.Size(641, 20);
            this.textBoxInputMkv.TabIndex = 2;
            // 
            // textBoxInputChapters
            // 
            this.textBoxInputChapters.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxInputChapters.Location = new System.Drawing.Point(124, 128);
            this.textBoxInputChapters.Name = "textBoxInputChapters";
            this.textBoxInputChapters.Size = new System.Drawing.Size(641, 20);
            this.textBoxInputChapters.TabIndex = 4;
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusStripLabel,
            this.progressLabel,
            this.statusStripProgressBar});
            this.statusStrip.Location = new System.Drawing.Point(0, 660);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(887, 22);
            this.statusStrip.TabIndex = 19;
            this.statusStrip.Text = "statusStrip";
            // 
            // statusStripLabel
            // 
            this.statusStripLabel.Name = "statusStripLabel";
            this.statusStripLabel.Size = new System.Drawing.Size(636, 17);
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
            this.buttonClose.Location = new System.Drawing.Point(800, 624);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 23);
            this.buttonClose.TabIndex = 3;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonRemux
            // 
            this.buttonRemux.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonRemux.Location = new System.Drawing.Point(719, 624);
            this.buttonRemux.Name = "buttonRemux";
            this.buttonRemux.Size = new System.Drawing.Size(75, 23);
            this.buttonRemux.TabIndex = 2;
            this.buttonRemux.Text = "Mux!";
            this.buttonRemux.UseVisualStyleBackColor = true;
            this.buttonRemux.Click += new System.EventHandler(this.buttonRemux_Click);
            // 
            // groupBoxInput
            // 
            this.groupBoxInput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxInput.Controls.Add(this.label8);
            this.groupBoxInput.Controls.Add(this.label7);
            this.groupBoxInput.Controls.Add(this.panelInputSubtitles);
            this.groupBoxInput.Controls.Add(this.panelInputLPCM);
            this.groupBoxInput.Controls.Add(this.panelInputMkvAudio);
            this.groupBoxInput.Controls.Add(this.panelInputM2tsAudio);
            this.groupBoxInput.Controls.Add(this.buttonInputM2tsBrowse);
            this.groupBoxInput.Controls.Add(this.label1);
            this.groupBoxInput.Controls.Add(this.textBoxInputM2ts);
            this.groupBoxInput.Controls.Add(this.label2);
            this.groupBoxInput.Controls.Add(this.label3);
            this.groupBoxInput.Controls.Add(this.buttonInputMkvBrowse);
            this.groupBoxInput.Controls.Add(this.buttonInputChaptersBrowse);
            this.groupBoxInput.Controls.Add(this.textBoxInputMkv);
            this.groupBoxInput.Controls.Add(this.textBoxInputChapters);
            this.groupBoxInput.Location = new System.Drawing.Point(12, 12);
            this.groupBoxInput.Name = "groupBoxInput";
            this.groupBoxInput.Size = new System.Drawing.Size(863, 211);
            this.groupBoxInput.TabIndex = 0;
            this.groupBoxInput.TabStop = false;
            this.groupBoxInput.Text = "Source files";
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
            this.panelInputSubtitles.Size = new System.Drawing.Size(641, 20);
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
            this.panelInputLPCM.Size = new System.Drawing.Size(641, 20);
            this.panelInputLPCM.TabIndex = 6;
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
            this.panelInputMkvAudio.Size = new System.Drawing.Size(641, 20);
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
            this.panelInputM2tsAudio.Size = new System.Drawing.Size(641, 20);
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
            // groupBoxOutput
            // 
            this.groupBoxOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxOutput.Controls.Add(this.panel1);
            this.groupBoxOutput.Controls.Add(this.comboBoxKeepAudioFrom);
            this.groupBoxOutput.Controls.Add(this.label4);
            this.groupBoxOutput.Controls.Add(this.splitContainerTracks);
            this.groupBoxOutput.Controls.Add(this.comboBoxKeepSubtitlesFrom);
            this.groupBoxOutput.Controls.Add(this.label6);
            this.groupBoxOutput.Location = new System.Drawing.Point(12, 229);
            this.groupBoxOutput.Name = "groupBoxOutput";
            this.groupBoxOutput.Size = new System.Drawing.Size(863, 389);
            this.groupBoxOutput.TabIndex = 1;
            this.groupBoxOutput.TabStop = false;
            this.groupBoxOutput.Text = "Output";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.buttonOutputMkvBrowse);
            this.panel1.Controls.Add(this.textBoxOutputMkv);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Location = new System.Drawing.Point(6, 354);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(842, 29);
            this.panel1.TabIndex = 3;
            // 
            // buttonOutputMkvBrowse
            // 
            this.buttonOutputMkvBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOutputMkvBrowse.Location = new System.Drawing.Point(765, 4);
            this.buttonOutputMkvBrowse.Name = "buttonOutputMkvBrowse";
            this.buttonOutputMkvBrowse.Size = new System.Drawing.Size(75, 23);
            this.buttonOutputMkvBrowse.TabIndex = 1;
            this.buttonOutputMkvBrowse.Text = "Browse...";
            this.buttonOutputMkvBrowse.UseVisualStyleBackColor = true;
            // 
            // textBoxOutputMkv
            // 
            this.textBoxOutputMkv.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxOutputMkv.Location = new System.Drawing.Point(118, 6);
            this.textBoxOutputMkv.Name = "textBoxOutputMkv";
            this.textBoxOutputMkv.Size = new System.Drawing.Size(641, 20);
            this.textBoxOutputMkv.TabIndex = 0;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(-1, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(70, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "Output file:";
            // 
            // comboBoxKeepAudioFrom
            // 
            this.comboBoxKeepAudioFrom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxKeepAudioFrom.FormattingEnabled = true;
            this.comboBoxKeepAudioFrom.Location = new System.Drawing.Point(124, 19);
            this.comboBoxKeepAudioFrom.Name = "comboBoxKeepAudioFrom";
            this.comboBoxKeepAudioFrom.Size = new System.Drawing.Size(121, 21);
            this.comboBoxKeepAudioFrom.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(87, 13);
            this.label4.TabIndex = 26;
            this.label4.Text = "Keep audio from:";
            // 
            // comboBoxKeepSubtitlesFrom
            // 
            this.comboBoxKeepSubtitlesFrom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxKeepSubtitlesFrom.FormattingEnabled = true;
            this.comboBoxKeepSubtitlesFrom.Location = new System.Drawing.Point(124, 46);
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
            this.label6.TabIndex = 27;
            this.label6.Text = "Keep subtitles from:";
            // 
            // splitContainerTracks
            // 
            this.splitContainerTracks.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainerTracks.Location = new System.Drawing.Point(6, 73);
            this.splitContainerTracks.Name = "splitContainerTracks";
            this.splitContainerTracks.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerTracks.Panel1
            // 
            this.splitContainerTracks.Panel1.Controls.Add(this.buttonMoveDownAudio);
            this.splitContainerTracks.Panel1.Controls.Add(this.buttonMoveUpAudio);
            this.splitContainerTracks.Panel1.Controls.Add(this.objectListViewAudioTracks);
            this.splitContainerTracks.Panel1.Controls.Add(this.label9);
            // 
            // splitContainerTracks.Panel2
            // 
            this.splitContainerTracks.Panel2.Controls.Add(this.buttonMoveDownSubtitles);
            this.splitContainerTracks.Panel2.Controls.Add(this.buttonMoveUpSubtitles);
            this.splitContainerTracks.Panel2.Controls.Add(this.objectListViewSubtitleTracks);
            this.splitContainerTracks.Panel2.Controls.Add(this.label10);
            this.splitContainerTracks.Size = new System.Drawing.Size(851, 275);
            this.splitContainerTracks.SplitterDistance = 156;
            this.splitContainerTracks.TabIndex = 2;
            // 
            // buttonMoveDownAudio
            // 
            this.buttonMoveDownAudio.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonMoveDownAudio.Location = new System.Drawing.Point(765, 29);
            this.buttonMoveDownAudio.Name = "buttonMoveDownAudio";
            this.buttonMoveDownAudio.Size = new System.Drawing.Size(75, 23);
            this.buttonMoveDownAudio.TabIndex = 2;
            this.buttonMoveDownAudio.Text = "Move Down";
            this.buttonMoveDownAudio.UseVisualStyleBackColor = true;
            this.buttonMoveDownAudio.Click += new System.EventHandler(this.buttonMoveDownAudio_Click);
            // 
            // buttonMoveUpAudio
            // 
            this.buttonMoveUpAudio.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonMoveUpAudio.Location = new System.Drawing.Point(765, 0);
            this.buttonMoveUpAudio.Name = "buttonMoveUpAudio";
            this.buttonMoveUpAudio.Size = new System.Drawing.Size(75, 23);
            this.buttonMoveUpAudio.TabIndex = 1;
            this.buttonMoveUpAudio.Text = "Move Up";
            this.buttonMoveUpAudio.UseVisualStyleBackColor = true;
            this.buttonMoveUpAudio.Click += new System.EventHandler(this.buttonMoveUpAudio_Click);
            // 
            // objectListViewAudioTracks
            // 
            this.objectListViewAudioTracks.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.objectListViewAudioTracks.AllColumns.Add(this.audioTitle);
            this.objectListViewAudioTracks.AllColumns.Add(this.audioCodec);
            this.objectListViewAudioTracks.AllColumns.Add(this.audioChannels);
            this.objectListViewAudioTracks.AllColumns.Add(this.audioSource);
            this.objectListViewAudioTracks.AllColumns.Add(this.audioLanguage);
            this.objectListViewAudioTracks.AllColumns.Add(this.audioDefault);
            this.objectListViewAudioTracks.AllColumns.Add(this.audioForced);
            this.objectListViewAudioTracks.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.objectListViewAudioTracks.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.SingleClick;
            this.objectListViewAudioTracks.CheckBoxes = true;
            this.objectListViewAudioTracks.CheckedAspectName = "IsSelected";
            this.objectListViewAudioTracks.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.audioTitle,
            this.audioCodec,
            this.audioChannels,
            this.audioSource,
            this.audioLanguage,
            this.audioDefault,
            this.audioForced});
            this.objectListViewAudioTracks.EmptyListMsg = "No input files selected";
            this.objectListViewAudioTracks.FullRowSelect = true;
            this.objectListViewAudioTracks.Location = new System.Drawing.Point(118, 0);
            this.objectListViewAudioTracks.MultiSelect = false;
            this.objectListViewAudioTracks.Name = "objectListViewAudioTracks";
            this.objectListViewAudioTracks.SelectColumnsMenuStaysOpen = false;
            this.objectListViewAudioTracks.ShowCommandMenuOnRightClick = true;
            this.objectListViewAudioTracks.ShowGroups = false;
            this.objectListViewAudioTracks.ShowImagesOnSubItems = true;
            this.objectListViewAudioTracks.ShowItemCountOnGroups = true;
            this.objectListViewAudioTracks.Size = new System.Drawing.Size(641, 153);
            this.objectListViewAudioTracks.TabIndex = 0;
            this.objectListViewAudioTracks.TintSortColumn = true;
            this.objectListViewAudioTracks.UseCompatibleStateImageBehavior = false;
            this.objectListViewAudioTracks.UseExplorerTheme = true;
            this.objectListViewAudioTracks.UseSubItemCheckBoxes = true;
            this.objectListViewAudioTracks.View = System.Windows.Forms.View.Details;
            this.objectListViewAudioTracks.SelectedIndexChanged += new System.EventHandler(this.objectListViewAudioTracks_SelectedIndexChanged);
            // 
            // audioTitle
            // 
            this.audioTitle.CellPadding = null;
            this.audioTitle.Groupable = false;
            this.audioTitle.Hideable = false;
            this.audioTitle.Text = "Title";
            this.audioTitle.Width = 180;
            // 
            // audioCodec
            // 
            this.audioCodec.CellPadding = null;
            this.audioCodec.Groupable = false;
            this.audioCodec.IsEditable = false;
            this.audioCodec.Text = "Codec";
            this.audioCodec.Width = 140;
            // 
            // audioChannels
            // 
            this.audioChannels.CellPadding = null;
            this.audioChannels.Groupable = false;
            this.audioChannels.IsEditable = false;
            this.audioChannels.Text = "Channels";
            this.audioChannels.Width = 70;
            // 
            // audioSource
            // 
            this.audioSource.CellPadding = null;
            this.audioSource.Groupable = false;
            this.audioSource.IsEditable = false;
            this.audioSource.Text = "Source";
            // 
            // audioLanguage
            // 
            this.audioLanguage.CellPadding = null;
            this.audioLanguage.Groupable = false;
            this.audioLanguage.IsEditable = false;
            this.audioLanguage.Text = "Language";
            // 
            // audioDefault
            // 
            this.audioDefault.CellPadding = null;
            this.audioDefault.CheckBoxes = true;
            this.audioDefault.Groupable = false;
            this.audioDefault.Hideable = false;
            this.audioDefault.Text = "Default";
            this.audioDefault.ToolTipText = "Players should prefer default tracks";
            // 
            // audioForced
            // 
            this.audioForced.CellPadding = null;
            this.audioForced.CheckBoxes = true;
            this.audioForced.Groupable = false;
            this.audioForced.Hideable = false;
            this.audioForced.Text = "Forced";
            this.audioForced.ToolTipText = "Players should always play forced tracks even if the user does not explicitly sel" +
                "ect or enable them";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(0, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(69, 13);
            this.label9.TabIndex = 28;
            this.label9.Text = "Select audio:";
            // 
            // buttonMoveDownSubtitles
            // 
            this.buttonMoveDownSubtitles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonMoveDownSubtitles.Location = new System.Drawing.Point(765, 32);
            this.buttonMoveDownSubtitles.Name = "buttonMoveDownSubtitles";
            this.buttonMoveDownSubtitles.Size = new System.Drawing.Size(75, 23);
            this.buttonMoveDownSubtitles.TabIndex = 2;
            this.buttonMoveDownSubtitles.Text = "Move Down";
            this.buttonMoveDownSubtitles.UseVisualStyleBackColor = true;
            this.buttonMoveDownSubtitles.Click += new System.EventHandler(this.buttonMoveDownSubtitles_Click);
            // 
            // buttonMoveUpSubtitles
            // 
            this.buttonMoveUpSubtitles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonMoveUpSubtitles.Location = new System.Drawing.Point(765, 3);
            this.buttonMoveUpSubtitles.Name = "buttonMoveUpSubtitles";
            this.buttonMoveUpSubtitles.Size = new System.Drawing.Size(75, 23);
            this.buttonMoveUpSubtitles.TabIndex = 1;
            this.buttonMoveUpSubtitles.Text = "Move Up";
            this.buttonMoveUpSubtitles.UseVisualStyleBackColor = true;
            this.buttonMoveUpSubtitles.Click += new System.EventHandler(this.buttonMoveUpSubtitles_Click);
            // 
            // objectListViewSubtitleTracks
            // 
            this.objectListViewSubtitleTracks.AllColumns.Add(this.subtitleTitle);
            this.objectListViewSubtitleTracks.AllColumns.Add(this.subtitleCodec);
            this.objectListViewSubtitleTracks.AllColumns.Add(this.subtitleSource);
            this.objectListViewSubtitleTracks.AllColumns.Add(this.subtitleLanguage);
            this.objectListViewSubtitleTracks.AllColumns.Add(this.subtitleDefault);
            this.objectListViewSubtitleTracks.AllColumns.Add(this.subtitleForced);
            this.objectListViewSubtitleTracks.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.objectListViewSubtitleTracks.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.SingleClick;
            this.objectListViewSubtitleTracks.CheckBoxes = true;
            this.objectListViewSubtitleTracks.CheckedAspectName = "IsSelected";
            this.objectListViewSubtitleTracks.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.subtitleTitle,
            this.subtitleCodec,
            this.subtitleSource,
            this.subtitleLanguage,
            this.subtitleDefault,
            this.subtitleForced});
            this.objectListViewSubtitleTracks.EmptyListMsg = "No input files selected";
            this.objectListViewSubtitleTracks.FullRowSelect = true;
            this.objectListViewSubtitleTracks.Location = new System.Drawing.Point(118, 3);
            this.objectListViewSubtitleTracks.MultiSelect = false;
            this.objectListViewSubtitleTracks.Name = "objectListViewSubtitleTracks";
            this.objectListViewSubtitleTracks.SelectColumnsMenuStaysOpen = false;
            this.objectListViewSubtitleTracks.ShowCommandMenuOnRightClick = true;
            this.objectListViewSubtitleTracks.ShowGroups = false;
            this.objectListViewSubtitleTracks.ShowImagesOnSubItems = true;
            this.objectListViewSubtitleTracks.ShowItemCountOnGroups = true;
            this.objectListViewSubtitleTracks.Size = new System.Drawing.Size(641, 109);
            this.objectListViewSubtitleTracks.TabIndex = 0;
            this.objectListViewSubtitleTracks.TintSortColumn = true;
            this.objectListViewSubtitleTracks.UseCompatibleStateImageBehavior = false;
            this.objectListViewSubtitleTracks.UseExplorerTheme = true;
            this.objectListViewSubtitleTracks.UseSubItemCheckBoxes = true;
            this.objectListViewSubtitleTracks.View = System.Windows.Forms.View.Details;
            this.objectListViewSubtitleTracks.SelectedIndexChanged += new System.EventHandler(this.objectListViewSubtitleTracks_SelectedIndexChanged);
            // 
            // subtitleTitle
            // 
            this.subtitleTitle.CellPadding = null;
            this.subtitleTitle.Groupable = false;
            this.subtitleTitle.Hideable = false;
            this.subtitleTitle.Text = "Title";
            this.subtitleTitle.Width = 180;
            // 
            // subtitleCodec
            // 
            this.subtitleCodec.CellPadding = null;
            this.subtitleCodec.Groupable = false;
            this.subtitleCodec.IsEditable = false;
            this.subtitleCodec.Text = "Codec";
            this.subtitleCodec.Width = 140;
            // 
            // subtitleSource
            // 
            this.subtitleSource.CellPadding = null;
            this.subtitleSource.Groupable = false;
            this.subtitleSource.IsEditable = false;
            this.subtitleSource.Text = "Source";
            // 
            // subtitleLanguage
            // 
            this.subtitleLanguage.CellPadding = null;
            this.subtitleLanguage.Groupable = false;
            this.subtitleLanguage.IsEditable = false;
            this.subtitleLanguage.Text = "Language";
            // 
            // subtitleDefault
            // 
            this.subtitleDefault.CellPadding = null;
            this.subtitleDefault.CheckBoxes = true;
            this.subtitleDefault.Groupable = false;
            this.subtitleDefault.Hideable = false;
            this.subtitleDefault.Text = "Default";
            this.subtitleDefault.ToolTipText = "Players should prefer default tracks";
            // 
            // subtitleForced
            // 
            this.subtitleForced.CellPadding = null;
            this.subtitleForced.CheckBoxes = true;
            this.subtitleForced.Groupable = false;
            this.subtitleForced.Hideable = false;
            this.subtitleForced.Text = "Forced";
            this.subtitleForced.ToolTipText = "Players should always play forced tracks even if the user does not explicitly sel" +
                "ect or enable them";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(0, 3);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(81, 13);
            this.label10.TabIndex = 32;
            this.label10.Text = "Select subtitles:";
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
            this.linkLabelClearSubtitles.TabIndex = 2;
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
            this.linkLabelClearLPCM.TabIndex = 2;
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
            this.ClientSize = new System.Drawing.Size(887, 682);
            this.Controls.Add(this.groupBoxOutput);
            this.Controls.Add(this.groupBoxInput);
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
            this.groupBoxInput.ResumeLayout(false);
            this.groupBoxInput.PerformLayout();
            this.panelInputSubtitles.ResumeLayout(false);
            this.panelInputSubtitles.PerformLayout();
            this.panelInputLPCM.ResumeLayout(false);
            this.panelInputLPCM.PerformLayout();
            this.panelInputMkvAudio.ResumeLayout(false);
            this.panelInputMkvAudio.PerformLayout();
            this.panelInputM2tsAudio.ResumeLayout(false);
            this.panelInputM2tsAudio.PerformLayout();
            this.groupBoxOutput.ResumeLayout(false);
            this.groupBoxOutput.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.splitContainerTracks.Panel1.ResumeLayout(false);
            this.splitContainerTracks.Panel1.PerformLayout();
            this.splitContainerTracks.Panel2.ResumeLayout(false);
            this.splitContainerTracks.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerTracks)).EndInit();
            this.splitContainerTracks.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.objectListViewAudioTracks)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.objectListViewSubtitleTracks)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxInputM2ts;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonInputM2tsBrowse;
        private System.Windows.Forms.Button buttonInputMkvBrowse;
        private System.Windows.Forms.Button buttonInputChaptersBrowse;
        private System.Windows.Forms.TextBox textBoxInputMkv;
        private System.Windows.Forms.TextBox textBoxInputChapters;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel statusStripLabel;
        private System.Windows.Forms.ToolStripProgressBar statusStripProgressBar;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonRemux;
        private System.Windows.Forms.ToolStripStatusLabel progressLabel;
        private System.Windows.Forms.GroupBox groupBoxInput;
        private System.Windows.Forms.GroupBox groupBoxOutput;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.FlowLayoutPanel panelInputSubtitles;
        private System.Windows.Forms.FlowLayoutPanel panelInputLPCM;
        private System.Windows.Forms.FlowLayoutPanel panelInputMkvAudio;
        private System.Windows.Forms.FlowLayoutPanel panelInputM2tsAudio;
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
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button buttonOutputMkvBrowse;
        private System.Windows.Forms.TextBox textBoxOutputMkv;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboBoxKeepAudioFrom;
        private System.Windows.Forms.Label label4;
        private views.SplitContainerWithDivider splitContainerTracks;
        private System.Windows.Forms.Button buttonMoveDownAudio;
        private System.Windows.Forms.Button buttonMoveUpAudio;
        private BrightIdeasSoftware.ObjectListView objectListViewAudioTracks;
        private BrightIdeasSoftware.OLVColumn audioTitle;
        private BrightIdeasSoftware.OLVColumn audioCodec;
        private BrightIdeasSoftware.OLVColumn audioChannels;
        private BrightIdeasSoftware.OLVColumn audioSource;
        private BrightIdeasSoftware.OLVColumn audioLanguage;
        private BrightIdeasSoftware.OLVColumn audioDefault;
        private BrightIdeasSoftware.OLVColumn audioForced;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button buttonMoveDownSubtitles;
        private System.Windows.Forms.Button buttonMoveUpSubtitles;
        private BrightIdeasSoftware.ObjectListView objectListViewSubtitleTracks;
        private BrightIdeasSoftware.OLVColumn subtitleTitle;
        private BrightIdeasSoftware.OLVColumn subtitleCodec;
        private BrightIdeasSoftware.OLVColumn subtitleSource;
        private BrightIdeasSoftware.OLVColumn subtitleLanguage;
        private BrightIdeasSoftware.OLVColumn subtitleDefault;
        private BrightIdeasSoftware.OLVColumn subtitleForced;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox comboBoxKeepSubtitlesFrom;
        private System.Windows.Forms.Label label6;
    }
}