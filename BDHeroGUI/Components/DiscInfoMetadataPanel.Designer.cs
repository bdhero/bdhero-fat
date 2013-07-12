namespace BDHeroGUI.Components
{
    partial class DiscInfoMetadataPanel
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.textBoxAllBdmtTitles = new System.Windows.Forms.TextBox();
            this.textBoxVISAN = new System.Windows.Forms.TextBox();
            this.textBoxDboxTitle = new System.Windows.Forms.TextBox();
            this.textBoxAnyDVDDiscInf = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxHardwareVolumeLabel = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.textBoxValidBdmtTitles = new System.Windows.Forms.TextBox();
            this.textBoxIsan = new System.Windows.Forms.TextBox();
            this.textBoxDboxTitleSanitized = new System.Windows.Forms.TextBox();
            this.textBoxVolumeLabelSanitized = new System.Windows.Forms.TextBox();
            this.labelVolumeLabel = new System.Windows.Forms.Label();
            this.labelVolumeLabelSanitized = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.textBoxVolumeLabel = new System.Windows.Forms.TextBox();
            this.buttonVolumeLabel = new System.Windows.Forms.Button();
            this.buttonVolumeLabelSanitized = new System.Windows.Forms.Button();
            this.buttonDboxTitleSanitized = new System.Windows.Forms.Button();
            this.buttonIsan = new System.Windows.Forms.Button();
            this.buttonValidBdmtTitles = new System.Windows.Forms.Button();
            this.buttonHardwareVolumeLabel = new System.Windows.Forms.Button();
            this.buttonAnyDVDDiscInf = new System.Windows.Forms.Button();
            this.buttonDboxTitle = new System.Windows.Forms.Button();
            this.buttonVISAN = new System.Windows.Forms.Button();
            this.buttonAllBdmtTitles = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox2);
            this.splitContainer1.Size = new System.Drawing.Size(899, 374);
            this.splitContainer1.SplitterDistance = 446;
            this.splitContainer1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tableLayoutPanel1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(446, 374);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Raw Metadata";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel1.Controls.Add(this.buttonAllBdmtTitles, 2, 4);
            this.tableLayoutPanel1.Controls.Add(this.buttonVISAN, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.buttonDboxTitle, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.buttonAnyDVDDiscInf, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.textBoxAllBdmtTitles, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.textBoxVISAN, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.textBoxDboxTitle, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.textBoxAnyDVDDiscInf, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label6, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.label7, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.textBoxHardwareVolumeLabel, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonHardwareVolumeLabel, 2, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(437, 352);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // textBoxAllBdmtTitles
            // 
            this.textBoxAllBdmtTitles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxAllBdmtTitles.Location = new System.Drawing.Point(133, 197);
            this.textBoxAllBdmtTitles.Multiline = true;
            this.textBoxAllBdmtTitles.Name = "textBoxAllBdmtTitles";
            this.textBoxAllBdmtTitles.ReadOnly = true;
            this.textBoxAllBdmtTitles.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxAllBdmtTitles.Size = new System.Drawing.Size(241, 152);
            this.textBoxAllBdmtTitles.TabIndex = 8;
            // 
            // textBoxVISAN
            // 
            this.textBoxVISAN.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxVISAN.Location = new System.Drawing.Point(133, 93);
            this.textBoxVISAN.Multiline = true;
            this.textBoxVISAN.Name = "textBoxVISAN";
            this.textBoxVISAN.ReadOnly = true;
            this.textBoxVISAN.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxVISAN.Size = new System.Drawing.Size(241, 98);
            this.textBoxVISAN.TabIndex = 6;
            // 
            // textBoxDboxTitle
            // 
            this.textBoxDboxTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxDboxTitle.Location = new System.Drawing.Point(133, 63);
            this.textBoxDboxTitle.Name = "textBoxDboxTitle";
            this.textBoxDboxTitle.ReadOnly = true;
            this.textBoxDboxTitle.Size = new System.Drawing.Size(241, 20);
            this.textBoxDboxTitle.TabIndex = 4;
            // 
            // textBoxAnyDVDDiscInf
            // 
            this.textBoxAnyDVDDiscInf.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxAnyDVDDiscInf.Location = new System.Drawing.Point(133, 33);
            this.textBoxAnyDVDDiscInf.Name = "textBoxAnyDVDDiscInf";
            this.textBoxAnyDVDDiscInf.ReadOnly = true;
            this.textBoxAnyDVDDiscInf.Size = new System.Drawing.Size(241, 20);
            this.textBoxAnyDVDDiscInf.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 3);
            this.label3.Margin = new System.Windows.Forms.Padding(3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(123, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Hardware Volume Label:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 33);
            this.label4.Margin = new System.Windows.Forms.Padding(3);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(106, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "AnyDVD HD disc.inf:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 63);
            this.label5.Margin = new System.Windows.Forms.Padding(3);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(108, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "D-BOX FilmIndex.xml:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 197);
            this.label6.Margin = new System.Windows.Forms.Padding(3);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(96, 13);
            this.label6.TabIndex = 3;
            this.label6.Text = "bdmt_xxx.xml titles:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 93);
            this.label7.Margin = new System.Windows.Forms.Padding(3);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(124, 13);
            this.label7.TabIndex = 4;
            this.label7.Text = "AACS/mcmf.xml V-ISAN:";
            // 
            // textBoxHardwareVolumeLabel
            // 
            this.textBoxHardwareVolumeLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxHardwareVolumeLabel.Location = new System.Drawing.Point(133, 3);
            this.textBoxHardwareVolumeLabel.Name = "textBoxHardwareVolumeLabel";
            this.textBoxHardwareVolumeLabel.ReadOnly = true;
            this.textBoxHardwareVolumeLabel.Size = new System.Drawing.Size(241, 20);
            this.textBoxHardwareVolumeLabel.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tableLayoutPanel2);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(449, 374);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Derived Metadata";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel2.Controls.Add(this.buttonValidBdmtTitles, 2, 4);
            this.tableLayoutPanel2.Controls.Add(this.buttonIsan, 2, 3);
            this.tableLayoutPanel2.Controls.Add(this.buttonDboxTitleSanitized, 2, 2);
            this.tableLayoutPanel2.Controls.Add(this.buttonVolumeLabelSanitized, 2, 1);
            this.tableLayoutPanel2.Controls.Add(this.textBoxValidBdmtTitles, 1, 4);
            this.tableLayoutPanel2.Controls.Add(this.textBoxIsan, 1, 3);
            this.tableLayoutPanel2.Controls.Add(this.textBoxDboxTitleSanitized, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.textBoxVolumeLabelSanitized, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.labelVolumeLabel, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.labelVolumeLabelSanitized, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.label8, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.label9, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.label10, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.textBoxVolumeLabel, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.buttonVolumeLabel, 2, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 5;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(440, 352);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // textBoxValidBdmtTitles
            // 
            this.textBoxValidBdmtTitles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxValidBdmtTitles.Location = new System.Drawing.Point(167, 197);
            this.textBoxValidBdmtTitles.Multiline = true;
            this.textBoxValidBdmtTitles.Name = "textBoxValidBdmtTitles";
            this.textBoxValidBdmtTitles.ReadOnly = true;
            this.textBoxValidBdmtTitles.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxValidBdmtTitles.Size = new System.Drawing.Size(210, 152);
            this.textBoxValidBdmtTitles.TabIndex = 8;
            // 
            // textBoxIsan
            // 
            this.textBoxIsan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxIsan.Location = new System.Drawing.Point(167, 93);
            this.textBoxIsan.Multiline = true;
            this.textBoxIsan.Name = "textBoxIsan";
            this.textBoxIsan.ReadOnly = true;
            this.textBoxIsan.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxIsan.Size = new System.Drawing.Size(210, 98);
            this.textBoxIsan.TabIndex = 6;
            // 
            // textBoxDboxTitleSanitized
            // 
            this.textBoxDboxTitleSanitized.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxDboxTitleSanitized.Location = new System.Drawing.Point(167, 63);
            this.textBoxDboxTitleSanitized.Name = "textBoxDboxTitleSanitized";
            this.textBoxDboxTitleSanitized.ReadOnly = true;
            this.textBoxDboxTitleSanitized.Size = new System.Drawing.Size(210, 20);
            this.textBoxDboxTitleSanitized.TabIndex = 4;
            // 
            // textBoxVolumeLabelSanitized
            // 
            this.textBoxVolumeLabelSanitized.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxVolumeLabelSanitized.Location = new System.Drawing.Point(167, 33);
            this.textBoxVolumeLabelSanitized.Name = "textBoxVolumeLabelSanitized";
            this.textBoxVolumeLabelSanitized.ReadOnly = true;
            this.textBoxVolumeLabelSanitized.Size = new System.Drawing.Size(210, 20);
            this.textBoxVolumeLabelSanitized.TabIndex = 2;
            // 
            // labelVolumeLabel
            // 
            this.labelVolumeLabel.AutoSize = true;
            this.labelVolumeLabel.Location = new System.Drawing.Point(3, 3);
            this.labelVolumeLabel.Margin = new System.Windows.Forms.Padding(3);
            this.labelVolumeLabel.Name = "labelVolumeLabel";
            this.labelVolumeLabel.Size = new System.Drawing.Size(74, 13);
            this.labelVolumeLabel.TabIndex = 0;
            this.labelVolumeLabel.Text = "Volume Label:";
            // 
            // labelVolumeLabelSanitized
            // 
            this.labelVolumeLabelSanitized.AutoSize = true;
            this.labelVolumeLabelSanitized.Location = new System.Drawing.Point(3, 33);
            this.labelVolumeLabelSanitized.Margin = new System.Windows.Forms.Padding(3);
            this.labelVolumeLabelSanitized.Name = "labelVolumeLabelSanitized";
            this.labelVolumeLabelSanitized.Size = new System.Drawing.Size(124, 13);
            this.labelVolumeLabelSanitized.TabIndex = 1;
            this.labelVolumeLabelSanitized.Text = "Volume Label (sanitized):";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 63);
            this.label8.Margin = new System.Windows.Forms.Padding(3);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(158, 13);
            this.label8.TabIndex = 2;
            this.label8.Text = "D-BOX FilmIndex.xml (sanitized):";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(3, 197);
            this.label9.Margin = new System.Windows.Forms.Padding(3);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(146, 13);
            this.label9.TabIndex = 3;
            this.label9.Text = "bdmt_xxx.xml titles (sanitized):";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(3, 93);
            this.label10.Margin = new System.Windows.Forms.Padding(3);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(114, 13);
            this.label10.TabIndex = 4;
            this.label10.Text = "AACS/mcmf.xml ISAN:";
            // 
            // textBoxVolumeLabel
            // 
            this.textBoxVolumeLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxVolumeLabel.Location = new System.Drawing.Point(167, 3);
            this.textBoxVolumeLabel.Name = "textBoxVolumeLabel";
            this.textBoxVolumeLabel.ReadOnly = true;
            this.textBoxVolumeLabel.Size = new System.Drawing.Size(210, 20);
            this.textBoxVolumeLabel.TabIndex = 0;
            // 
            // buttonVolumeLabel
            // 
            this.buttonVolumeLabel.Location = new System.Drawing.Point(383, 3);
            this.buttonVolumeLabel.Name = "buttonVolumeLabel";
            this.buttonVolumeLabel.Size = new System.Drawing.Size(54, 23);
            this.buttonVolumeLabel.TabIndex = 1;
            this.buttonVolumeLabel.Text = "Open...";
            this.buttonVolumeLabel.UseVisualStyleBackColor = true;
            // 
            // buttonVolumeLabelSanitized
            // 
            this.buttonVolumeLabelSanitized.Location = new System.Drawing.Point(383, 33);
            this.buttonVolumeLabelSanitized.Name = "buttonVolumeLabelSanitized";
            this.buttonVolumeLabelSanitized.Size = new System.Drawing.Size(54, 23);
            this.buttonVolumeLabelSanitized.TabIndex = 3;
            this.buttonVolumeLabelSanitized.Text = "Open...";
            this.buttonVolumeLabelSanitized.UseVisualStyleBackColor = true;
            // 
            // buttonDboxTitleSanitized
            // 
            this.buttonDboxTitleSanitized.Location = new System.Drawing.Point(383, 63);
            this.buttonDboxTitleSanitized.Name = "buttonDboxTitleSanitized";
            this.buttonDboxTitleSanitized.Size = new System.Drawing.Size(54, 23);
            this.buttonDboxTitleSanitized.TabIndex = 5;
            this.buttonDboxTitleSanitized.Text = "Open...";
            this.buttonDboxTitleSanitized.UseVisualStyleBackColor = true;
            // 
            // buttonIsan
            // 
            this.buttonIsan.Location = new System.Drawing.Point(383, 93);
            this.buttonIsan.Name = "buttonIsan";
            this.buttonIsan.Size = new System.Drawing.Size(54, 23);
            this.buttonIsan.TabIndex = 7;
            this.buttonIsan.Text = "Open...";
            this.buttonIsan.UseVisualStyleBackColor = true;
            // 
            // buttonValidBdmtTitles
            // 
            this.buttonValidBdmtTitles.Location = new System.Drawing.Point(383, 197);
            this.buttonValidBdmtTitles.Name = "buttonValidBdmtTitles";
            this.buttonValidBdmtTitles.Size = new System.Drawing.Size(54, 23);
            this.buttonValidBdmtTitles.TabIndex = 9;
            this.buttonValidBdmtTitles.Text = "Open...";
            this.buttonValidBdmtTitles.UseVisualStyleBackColor = true;
            // 
            // buttonHardwareVolumeLabel
            // 
            this.buttonHardwareVolumeLabel.Location = new System.Drawing.Point(380, 3);
            this.buttonHardwareVolumeLabel.Name = "buttonHardwareVolumeLabel";
            this.buttonHardwareVolumeLabel.Size = new System.Drawing.Size(54, 23);
            this.buttonHardwareVolumeLabel.TabIndex = 1;
            this.buttonHardwareVolumeLabel.Text = "Open...";
            this.buttonHardwareVolumeLabel.UseVisualStyleBackColor = true;
            // 
            // buttonAnyDVDDiscInf
            // 
            this.buttonAnyDVDDiscInf.Location = new System.Drawing.Point(380, 33);
            this.buttonAnyDVDDiscInf.Name = "buttonAnyDVDDiscInf";
            this.buttonAnyDVDDiscInf.Size = new System.Drawing.Size(54, 23);
            this.buttonAnyDVDDiscInf.TabIndex = 3;
            this.buttonAnyDVDDiscInf.Text = "Open...";
            this.buttonAnyDVDDiscInf.UseVisualStyleBackColor = true;
            // 
            // buttonDboxTitle
            // 
            this.buttonDboxTitle.Location = new System.Drawing.Point(380, 63);
            this.buttonDboxTitle.Name = "buttonDboxTitle";
            this.buttonDboxTitle.Size = new System.Drawing.Size(54, 23);
            this.buttonDboxTitle.TabIndex = 5;
            this.buttonDboxTitle.Text = "Open...";
            this.buttonDboxTitle.UseVisualStyleBackColor = true;
            // 
            // buttonVISAN
            // 
            this.buttonVISAN.Location = new System.Drawing.Point(380, 93);
            this.buttonVISAN.Name = "buttonVISAN";
            this.buttonVISAN.Size = new System.Drawing.Size(54, 23);
            this.buttonVISAN.TabIndex = 7;
            this.buttonVISAN.Text = "Open...";
            this.buttonVISAN.UseVisualStyleBackColor = true;
            // 
            // buttonAllBdmtTitles
            // 
            this.buttonAllBdmtTitles.Location = new System.Drawing.Point(380, 197);
            this.buttonAllBdmtTitles.Name = "buttonAllBdmtTitles";
            this.buttonAllBdmtTitles.Size = new System.Drawing.Size(54, 23);
            this.buttonAllBdmtTitles.TabIndex = 9;
            this.buttonAllBdmtTitles.Text = "Open...";
            this.buttonAllBdmtTitles.UseVisualStyleBackColor = true;
            // 
            // DiscInfoMetadataPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "DiscInfoMetadataPanel";
            this.Size = new System.Drawing.Size(899, 374);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxAllBdmtTitles;
        private System.Windows.Forms.TextBox textBoxVISAN;
        private System.Windows.Forms.TextBox textBoxDboxTitle;
        private System.Windows.Forms.TextBox textBoxAnyDVDDiscInf;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBoxHardwareVolumeLabel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TextBox textBoxValidBdmtTitles;
        private System.Windows.Forms.TextBox textBoxIsan;
        private System.Windows.Forms.TextBox textBoxDboxTitleSanitized;
        private System.Windows.Forms.TextBox textBoxVolumeLabelSanitized;
        private System.Windows.Forms.Label labelVolumeLabel;
        private System.Windows.Forms.Label labelVolumeLabelSanitized;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox textBoxVolumeLabel;
        private System.Windows.Forms.Button buttonAllBdmtTitles;
        private System.Windows.Forms.Button buttonVISAN;
        private System.Windows.Forms.Button buttonDboxTitle;
        private System.Windows.Forms.Button buttonAnyDVDDiscInf;
        private System.Windows.Forms.Button buttonHardwareVolumeLabel;
        private System.Windows.Forms.Button buttonValidBdmtTitles;
        private System.Windows.Forms.Button buttonIsan;
        private System.Windows.Forms.Button buttonDboxTitleSanitized;
        private System.Windows.Forms.Button buttonVolumeLabelSanitized;
        private System.Windows.Forms.Button buttonVolumeLabel;
    }
}
