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
            this.panel1 = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.buttonRemoveLPCM = new System.Windows.Forms.Button();
            this.buttonAddLPCM = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.buttonRemoveSubtitles = new System.Windows.Forms.Button();
            this.buttonAddSubtitles = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.listViewOutputTracks = new System.Windows.Forms.ListView();
            this.columnHeaderName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderCodec = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderResolution = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label9 = new System.Windows.Forms.Label();
            this.listViewLPCM = new System.Windows.Forms.ListView();
            this.listViewSubtitles = new System.Windows.Forms.ListView();
            this.columnHeaderSource = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderLPCMFilename = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderLPCMChannels = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderSubtitleFilename = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderSubtitleType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.statusStrip.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox2.SuspendLayout();
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
            this.label2.Location = new System.Drawing.Point(6, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(112, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "MKV (HandBrake):";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 85);
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
            this.label5.Location = new System.Drawing.Point(5, 224);
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
            this.buttonInputMkvBrowse.Location = new System.Drawing.Point(591, 50);
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
            this.buttonInputChaptersBrowse.Location = new System.Drawing.Point(591, 79);
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
            this.textBoxInputMkv.Location = new System.Drawing.Point(124, 53);
            this.textBoxInputMkv.Name = "textBoxInputMkv";
            this.textBoxInputMkv.Size = new System.Drawing.Size(461, 20);
            this.textBoxInputMkv.TabIndex = 2;
            // 
            // textBoxInputChapters
            // 
            this.textBoxInputChapters.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxInputChapters.Location = new System.Drawing.Point(124, 82);
            this.textBoxInputChapters.Name = "textBoxInputChapters";
            this.textBoxInputChapters.Size = new System.Drawing.Size(461, 20);
            this.textBoxInputChapters.TabIndex = 4;
            // 
            // textBoxOutputMkv
            // 
            this.textBoxOutputMkv.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxOutputMkv.Location = new System.Drawing.Point(111, 221);
            this.textBoxOutputMkv.Name = "textBoxOutputMkv";
            this.textBoxOutputMkv.Size = new System.Drawing.Size(485, 20);
            this.textBoxOutputMkv.TabIndex = 3;
            // 
            // buttonOutputMkvBrowse
            // 
            this.buttonOutputMkvBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOutputMkvBrowse.Location = new System.Drawing.Point(602, 219);
            this.buttonOutputMkvBrowse.Name = "buttonOutputMkvBrowse";
            this.buttonOutputMkvBrowse.Size = new System.Drawing.Size(75, 23);
            this.buttonOutputMkvBrowse.TabIndex = 4;
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
            this.statusStrip.Location = new System.Drawing.Point(0, 580);
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
            this.buttonClose.Location = new System.Drawing.Point(620, 544);
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
            this.buttonRemux.Location = new System.Drawing.Point(539, 544);
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
            this.groupBox1.Controls.Add(this.panel1);
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
            this.groupBox1.Size = new System.Drawing.Size(683, 271);
            this.groupBox1.TabIndex = 23;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Source files";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.splitContainer1);
            this.panel1.Location = new System.Drawing.Point(5, 118);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(671, 147);
            this.panel1.TabIndex = 25;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.listViewLPCM);
            this.splitContainer1.Panel1.Controls.Add(this.buttonRemoveLPCM);
            this.splitContainer1.Panel1.Controls.Add(this.buttonAddLPCM);
            this.splitContainer1.Panel1.Controls.Add(this.label7);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.listViewSubtitles);
            this.splitContainer1.Panel2.Controls.Add(this.buttonRemoveSubtitles);
            this.splitContainer1.Panel2.Controls.Add(this.buttonAddSubtitles);
            this.splitContainer1.Panel2.Controls.Add(this.label8);
            this.splitContainer1.Size = new System.Drawing.Size(671, 147);
            this.splitContainer1.SplitterDistance = 333;
            this.splitContainer1.TabIndex = 0;
            // 
            // buttonRemoveLPCM
            // 
            this.buttonRemoveLPCM.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonRemoveLPCM.Location = new System.Drawing.Point(84, 121);
            this.buttonRemoveLPCM.Name = "buttonRemoveLPCM";
            this.buttonRemoveLPCM.Size = new System.Drawing.Size(75, 23);
            this.buttonRemoveLPCM.TabIndex = 2;
            this.buttonRemoveLPCM.Text = "Remove";
            this.buttonRemoveLPCM.UseVisualStyleBackColor = true;
            // 
            // buttonAddLPCM
            // 
            this.buttonAddLPCM.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonAddLPCM.Location = new System.Drawing.Point(3, 121);
            this.buttonAddLPCM.Name = "buttonAddLPCM";
            this.buttonAddLPCM.Size = new System.Drawing.Size(75, 23);
            this.buttonAddLPCM.TabIndex = 1;
            this.buttonAddLPCM.Text = "Add...";
            this.buttonAddLPCM.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(1, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(68, 13);
            this.label7.TabIndex = 0;
            this.label7.Text = "LPCM audio:";
            // 
            // buttonRemoveSubtitles
            // 
            this.buttonRemoveSubtitles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonRemoveSubtitles.Location = new System.Drawing.Point(85, 120);
            this.buttonRemoveSubtitles.Name = "buttonRemoveSubtitles";
            this.buttonRemoveSubtitles.Size = new System.Drawing.Size(75, 23);
            this.buttonRemoveSubtitles.TabIndex = 2;
            this.buttonRemoveSubtitles.Text = "Remove";
            this.buttonRemoveSubtitles.UseVisualStyleBackColor = true;
            // 
            // buttonAddSubtitles
            // 
            this.buttonAddSubtitles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonAddSubtitles.Location = new System.Drawing.Point(4, 120);
            this.buttonAddSubtitles.Name = "buttonAddSubtitles";
            this.buttonAddSubtitles.Size = new System.Drawing.Size(75, 23);
            this.buttonAddSubtitles.TabIndex = 1;
            this.buttonAddSubtitles.Text = "Add...";
            this.buttonAddSubtitles.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(97, 13);
            this.label8.TabIndex = 0;
            this.label8.Text = "Additional subtitles:";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.listViewOutputTracks);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.comboBoxKeepAudioFrom);
            this.groupBox2.Controls.Add(this.comboBoxKeepSubtitlesFrom);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.buttonOutputMkvBrowse);
            this.groupBox2.Controls.Add(this.textBoxOutputMkv);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Location = new System.Drawing.Point(12, 290);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(683, 248);
            this.groupBox2.TabIndex = 24;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Output";
            // 
            // listViewOutputTracks
            // 
            this.listViewOutputTracks.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewOutputTracks.CheckBoxes = true;
            this.listViewOutputTracks.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderName,
            this.columnHeaderCodec,
            this.columnHeaderResolution,
            this.columnHeaderSource});
            this.listViewOutputTracks.FullRowSelect = true;
            this.listViewOutputTracks.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewOutputTracks.HideSelection = false;
            this.listViewOutputTracks.LabelEdit = true;
            this.listViewOutputTracks.Location = new System.Drawing.Point(111, 74);
            this.listViewOutputTracks.MultiSelect = false;
            this.listViewOutputTracks.Name = "listViewOutputTracks";
            this.listViewOutputTracks.Size = new System.Drawing.Size(565, 139);
            this.listViewOutputTracks.TabIndex = 2;
            this.listViewOutputTracks.UseCompatibleStateImageBehavior = false;
            this.listViewOutputTracks.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderName
            // 
            this.columnHeaderName.Text = "Name";
            this.columnHeaderName.Width = 160;
            // 
            // columnHeaderCodec
            // 
            this.columnHeaderCodec.Text = "Codec";
            this.columnHeaderCodec.Width = 160;
            // 
            // columnHeaderResolution
            // 
            this.columnHeaderResolution.Text = "Resolution";
            this.columnHeaderResolution.Width = 80;
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
            // listViewLPCM
            // 
            this.listViewLPCM.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewLPCM.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderLPCMFilename,
            this.columnHeaderLPCMChannels});
            this.listViewLPCM.FullRowSelect = true;
            this.listViewLPCM.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewLPCM.HideSelection = false;
            this.listViewLPCM.Location = new System.Drawing.Point(4, 16);
            this.listViewLPCM.Name = "listViewLPCM";
            this.listViewLPCM.Size = new System.Drawing.Size(326, 97);
            this.listViewLPCM.TabIndex = 3;
            this.listViewLPCM.UseCompatibleStateImageBehavior = false;
            this.listViewLPCM.View = System.Windows.Forms.View.Details;
            // 
            // listViewSubtitles
            // 
            this.listViewSubtitles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewSubtitles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderSubtitleFilename,
            this.columnHeaderSubtitleType});
            this.listViewSubtitles.FullRowSelect = true;
            this.listViewSubtitles.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewSubtitles.HideSelection = false;
            this.listViewSubtitles.Location = new System.Drawing.Point(4, 16);
            this.listViewSubtitles.Name = "listViewSubtitles";
            this.listViewSubtitles.Size = new System.Drawing.Size(327, 97);
            this.listViewSubtitles.TabIndex = 3;
            this.listViewSubtitles.UseCompatibleStateImageBehavior = false;
            this.listViewSubtitles.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderSource
            // 
            this.columnHeaderSource.Text = "Source";
            // 
            // columnHeaderLPCMFilename
            // 
            this.columnHeaderLPCMFilename.Text = "Filename";
            this.columnHeaderLPCMFilename.Width = 260;
            // 
            // columnHeaderLPCMChannels
            // 
            this.columnHeaderLPCMChannels.Text = "Channels";
            // 
            // columnHeaderSubtitleFilename
            // 
            this.columnHeaderSubtitleFilename.Text = "Filename";
            this.columnHeaderSubtitleFilename.Width = 260;
            // 
            // columnHeaderSubtitleType
            // 
            this.columnHeaderSubtitleType.Text = "Type";
            // 
            // FormRemux
            // 
            this.AcceptButton = this.buttonRemux;
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonClose;
            this.ClientSize = new System.Drawing.Size(707, 602);
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
            this.panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
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
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button buttonRemoveLPCM;
        private System.Windows.Forms.Button buttonAddLPCM;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button buttonRemoveSubtitles;
        private System.Windows.Forms.Button buttonAddSubtitles;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ListView listViewOutputTracks;
        private System.Windows.Forms.ColumnHeader columnHeaderName;
        private System.Windows.Forms.ColumnHeader columnHeaderCodec;
        private System.Windows.Forms.ColumnHeader columnHeaderResolution;
        private System.Windows.Forms.ListView listViewLPCM;
        private System.Windows.Forms.ListView listViewSubtitles;
        private System.Windows.Forms.ColumnHeader columnHeaderSource;
        private System.Windows.Forms.ColumnHeader columnHeaderLPCMFilename;
        private System.Windows.Forms.ColumnHeader columnHeaderLPCMChannels;
        private System.Windows.Forms.ColumnHeader columnHeaderSubtitleFilename;
        private System.Windows.Forms.ColumnHeader columnHeaderSubtitleType;
    }
}