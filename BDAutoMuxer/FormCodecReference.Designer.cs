namespace BDAutoMuxer
{
    partial class FormCodecReference
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
            this.listViewCodecs = new System.Windows.Forms.ListView();
            this.columnHeaderCommonName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderOfficialName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderAlternateNames = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderCodecId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderCore = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderCompression = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderMuxable = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.buttonClose = new System.Windows.Forms.Button();
            this.textBoxDescription = new System.Windows.Forms.TextBox();
            this.labelOfficialBluray = new System.Windows.Forms.Label();
            this.labelDescription = new System.Windows.Forms.Label();
            this.labelOfficialDVD = new System.Windows.Forms.Label();
            this.labelOfficialBlurayValue = new System.Windows.Forms.Label();
            this.labelOfficialDVDValue = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // listViewCodecs
            // 
            this.listViewCodecs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewCodecs.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderCommonName,
            this.columnHeaderOfficialName,
            this.columnHeaderAlternateNames,
            this.columnHeaderCodecId,
            this.columnHeaderCore,
            this.columnHeaderCompression,
            this.columnHeaderMuxable});
            this.listViewCodecs.FullRowSelect = true;
            this.listViewCodecs.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewCodecs.HideSelection = false;
            this.listViewCodecs.Location = new System.Drawing.Point(13, 13);
            this.listViewCodecs.MultiSelect = false;
            this.listViewCodecs.Name = "listViewCodecs";
            this.listViewCodecs.Size = new System.Drawing.Size(737, 373);
            this.listViewCodecs.TabIndex = 0;
            this.listViewCodecs.UseCompatibleStateImageBehavior = false;
            this.listViewCodecs.View = System.Windows.Forms.View.Details;
            this.listViewCodecs.SelectedIndexChanged += new System.EventHandler(this.listViewCodecs_SelectedIndexChanged);
            // 
            // columnHeaderCommonName
            // 
            this.columnHeaderCommonName.Text = "Common Name";
            this.columnHeaderCommonName.Width = 100;
            // 
            // columnHeaderOfficialName
            // 
            this.columnHeaderOfficialName.Text = "Official Name";
            this.columnHeaderOfficialName.Width = 152;
            // 
            // columnHeaderAlternateNames
            // 
            this.columnHeaderAlternateNames.Text = "Alternate Names";
            this.columnHeaderAlternateNames.Width = 146;
            // 
            // columnHeaderCodecId
            // 
            this.columnHeaderCodecId.Tag = "Codec ID header value in MKV containers";
            this.columnHeaderCodecId.Text = "MKV Codec ID";
            this.columnHeaderCodecId.Width = 139;
            // 
            // columnHeaderCore
            // 
            this.columnHeaderCore.Tag = "Inner stream that provides backwards compatibility with older players";
            this.columnHeaderCore.Text = "Core";
            this.columnHeaderCore.Width = 45;
            // 
            // columnHeaderCompression
            // 
            this.columnHeaderCompression.Text = "Compression";
            this.columnHeaderCompression.Width = 76;
            // 
            // columnHeaderMuxable
            // 
            this.columnHeaderMuxable.Tag = "Capable of being muxed with freely available software (tsMuxeR, mkvmerge, eac3to," +
                " etc.)";
            this.columnHeaderMuxable.Text = "Muxable";
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonClose.Location = new System.Drawing.Point(675, 496);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 23);
            this.buttonClose.TabIndex = 2;
            this.buttonClose.Text = "&Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // textBoxDescription
            // 
            this.textBoxDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxDescription.Location = new System.Drawing.Point(13, 418);
            this.textBoxDescription.Multiline = true;
            this.textBoxDescription.Name = "textBoxDescription";
            this.textBoxDescription.ReadOnly = true;
            this.textBoxDescription.Size = new System.Drawing.Size(536, 72);
            this.textBoxDescription.TabIndex = 1;
            // 
            // labelOfficialBluray
            // 
            this.labelOfficialBluray.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.labelOfficialBluray.AutoSize = true;
            this.labelOfficialBluray.Location = new System.Drawing.Point(555, 418);
            this.labelOfficialBluray.Name = "labelOfficialBluray";
            this.labelOfficialBluray.Size = new System.Drawing.Size(110, 13);
            this.labelOfficialBluray.TabIndex = 4;
            this.labelOfficialBluray.Text = "Official Blu-ray codec:";
            // 
            // labelDescription
            // 
            this.labelDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelDescription.AutoSize = true;
            this.labelDescription.Location = new System.Drawing.Point(10, 399);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(63, 13);
            this.labelDescription.TabIndex = 5;
            this.labelDescription.Text = "Description:";
            // 
            // labelOfficialDVD
            // 
            this.labelOfficialDVD.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.labelOfficialDVD.AutoSize = true;
            this.labelOfficialDVD.Location = new System.Drawing.Point(556, 435);
            this.labelOfficialDVD.Name = "labelOfficialDVD";
            this.labelOfficialDVD.Size = new System.Drawing.Size(101, 13);
            this.labelOfficialDVD.TabIndex = 6;
            this.labelOfficialDVD.Text = "Official DVD codec:";
            // 
            // labelOfficialBlurayValue
            // 
            this.labelOfficialBlurayValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.labelOfficialBlurayValue.Location = new System.Drawing.Point(671, 418);
            this.labelOfficialBlurayValue.Name = "labelOfficialBlurayValue";
            this.labelOfficialBlurayValue.Size = new System.Drawing.Size(79, 13);
            this.labelOfficialBlurayValue.TabIndex = 7;
            this.labelOfficialBlurayValue.Text = "Yes (optional)";
            // 
            // labelOfficialDVDValue
            // 
            this.labelOfficialDVDValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.labelOfficialDVDValue.Location = new System.Drawing.Point(671, 435);
            this.labelOfficialDVDValue.Name = "labelOfficialDVDValue";
            this.labelOfficialDVDValue.Size = new System.Drawing.Size(79, 13);
            this.labelOfficialDVDValue.TabIndex = 8;
            this.labelOfficialDVDValue.Text = "Yes (required)";
            // 
            // FormCodecReference
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonClose;
            this.ClientSize = new System.Drawing.Size(762, 531);
            this.Controls.Add(this.labelOfficialDVDValue);
            this.Controls.Add(this.labelOfficialBlurayValue);
            this.Controls.Add(this.labelOfficialDVD);
            this.Controls.Add(this.labelDescription);
            this.Controls.Add(this.labelOfficialBluray);
            this.Controls.Add(this.textBoxDescription);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.listViewCodecs);
            this.Name = "FormCodecReference";
            this.Text = "Codec Reference";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listViewCodecs;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.ColumnHeader columnHeaderCommonName;
        private System.Windows.Forms.ColumnHeader columnHeaderOfficialName;
        private System.Windows.Forms.ColumnHeader columnHeaderAlternateNames;
        private System.Windows.Forms.ColumnHeader columnHeaderCodecId;
        private System.Windows.Forms.ColumnHeader columnHeaderMuxable;
        private System.Windows.Forms.TextBox textBoxDescription;
        private System.Windows.Forms.Label labelOfficialBluray;
        private System.Windows.Forms.Label labelDescription;
        private System.Windows.Forms.Label labelOfficialDVD;
        private System.Windows.Forms.Label labelOfficialBlurayValue;
        private System.Windows.Forms.Label labelOfficialDVDValue;
        private System.Windows.Forms.ColumnHeader columnHeaderCore;
        private System.Windows.Forms.ColumnHeader columnHeaderCompression;
    }
}