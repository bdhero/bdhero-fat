namespace BDInfo
{
    partial class FormMoviePlaylist
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
            this.showAllPlaylistsCheckbox = new System.Windows.Forms.CheckBox();
            this.playlistDataGridView = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.cancelButton = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.submitButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.playlistDataGridView)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // showAllPlaylistsCheckbox
            // 
            this.showAllPlaylistsCheckbox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.showAllPlaylistsCheckbox.AutoSize = true;
            this.showAllPlaylistsCheckbox.Location = new System.Drawing.Point(3, 7);
            this.showAllPlaylistsCheckbox.Name = "showAllPlaylistsCheckbox";
            this.showAllPlaylistsCheckbox.Size = new System.Drawing.Size(105, 17);
            this.showAllPlaylistsCheckbox.TabIndex = 2;
            this.showAllPlaylistsCheckbox.Text = "Show all playlists";
            this.showAllPlaylistsCheckbox.UseVisualStyleBackColor = true;
            this.showAllPlaylistsCheckbox.CheckedChanged += new System.EventHandler(this.showAllPlaylistsCheckbox_CheckedChanged);
            // 
            // playlistDataGridView
            // 
            this.playlistDataGridView.AllowUserToAddRows = false;
            this.playlistDataGridView.AllowUserToDeleteRows = false;
            this.playlistDataGridView.AllowUserToResizeRows = false;
            this.playlistDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.playlistDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.playlistDataGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.playlistDataGridView.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.playlistDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.playlistDataGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
            this.playlistDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.playlistDataGridView.Location = new System.Drawing.Point(3, 3);
            this.playlistDataGridView.MultiSelect = false;
            this.playlistDataGridView.Name = "playlistDataGridView";
            this.playlistDataGridView.Size = new System.Drawing.Size(653, 250);
            this.playlistDataGridView.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.playlistDataGridView);
            this.panel1.Location = new System.Drawing.Point(13, 25);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(659, 256);
            this.panel1.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Select main movies:";
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(582, 3);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 4;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this.submitButton);
            this.panel2.Controls.Add(this.cancelButton);
            this.panel2.Controls.Add(this.showAllPlaylistsCheckbox);
            this.panel2.Location = new System.Drawing.Point(12, 287);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(660, 29);
            this.panel2.TabIndex = 6;
            // 
            // submitButton
            // 
            this.submitButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.submitButton.Enabled = false;
            this.submitButton.Location = new System.Drawing.Point(501, 3);
            this.submitButton.Name = "submitButton";
            this.submitButton.Size = new System.Drawing.Size(75, 23);
            this.submitButton.TabIndex = 3;
            this.submitButton.Text = "Submit";
            this.submitButton.UseVisualStyleBackColor = true;
            // 
            // FormMoviePlaylist
            // 
            this.AcceptButton = this.submitButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(684, 328);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.Name = "FormMoviePlaylist";
            this.Text = "Select Playlists";
            ((System.ComponentModel.ISupportInitialize)(this.playlistDataGridView)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox showAllPlaylistsCheckbox;
        private System.Windows.Forms.DataGridView playlistDataGridView;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button submitButton;
    }
}