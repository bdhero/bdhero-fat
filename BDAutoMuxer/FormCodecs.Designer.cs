namespace BDAutoMuxer
{
    partial class FormCodecs
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
            System.Windows.Forms.ListViewGroup listViewGroup4 = new System.Windows.Forms.ListViewGroup("Video", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup5 = new System.Windows.Forms.ListViewGroup("Audio", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup6 = new System.Windows.Forms.ListViewGroup("Subtitles", System.Windows.Forms.HorizontalAlignment.Left);
            this.listViewCodecs = new System.Windows.Forms.ListView();
            this.buttonClose = new System.Windows.Forms.Button();
            this.columnHeaderCommonName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderOfficialName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderAlternateNames = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderDescription = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
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
            this.columnHeaderDescription});
            this.listViewCodecs.FullRowSelect = true;
            this.listViewCodecs.GridLines = true;
            listViewGroup4.Header = "Video";
            listViewGroup4.Name = "listViewGroupVideo";
            listViewGroup4.Tag = "";
            listViewGroup5.Header = "Audio";
            listViewGroup5.Name = "listViewGroupAudio";
            listViewGroup5.Tag = "";
            listViewGroup6.Header = "Subtitles";
            listViewGroup6.Name = "listViewGroupSubtitles";
            listViewGroup6.Tag = "";
            this.listViewCodecs.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup4,
            listViewGroup5,
            listViewGroup6});
            this.listViewCodecs.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewCodecs.HideSelection = false;
            this.listViewCodecs.Location = new System.Drawing.Point(13, 13);
            this.listViewCodecs.Name = "listViewCodecs";
            this.listViewCodecs.Size = new System.Drawing.Size(620, 385);
            this.listViewCodecs.TabIndex = 0;
            this.listViewCodecs.UseCompatibleStateImageBehavior = false;
            this.listViewCodecs.View = System.Windows.Forms.View.Details;
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonClose.Location = new System.Drawing.Point(558, 404);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 23);
            this.buttonClose.TabIndex = 1;
            this.buttonClose.Text = "&Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // columnHeaderCommonName
            // 
            this.columnHeaderCommonName.Text = "Common Name";
            this.columnHeaderCommonName.Width = 100;
            // 
            // columnHeaderOfficialName
            // 
            this.columnHeaderOfficialName.Text = "Official Name";
            this.columnHeaderOfficialName.Width = 174;
            // 
            // columnHeaderAlternateNames
            // 
            this.columnHeaderAlternateNames.Text = "Alternate Names";
            this.columnHeaderAlternateNames.Width = 169;
            // 
            // columnHeaderDescription
            // 
            this.columnHeaderDescription.Text = "Description";
            this.columnHeaderDescription.Width = 162;
            // 
            // FormCodecs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonClose;
            this.ClientSize = new System.Drawing.Size(645, 439);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.listViewCodecs);
            this.Name = "FormCodecs";
            this.Text = "Codecs";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listViewCodecs;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.ColumnHeader columnHeaderCommonName;
        private System.Windows.Forms.ColumnHeader columnHeaderOfficialName;
        private System.Windows.Forms.ColumnHeader columnHeaderAlternateNames;
        private System.Windows.Forms.ColumnHeader columnHeaderDescription;
    }
}