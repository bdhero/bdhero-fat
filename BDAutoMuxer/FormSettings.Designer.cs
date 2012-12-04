//============================================================================
// BDInfo - Blu-ray Video and Audio Analysis Tool
// Copyright © 2010 Cinema Squid
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
//=============================================================================

namespace BDAutoMuxer
{
    partial class FormSettings
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
            this.checkBoxFilterLoopingPlaylists = new System.Windows.Forms.CheckBox();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.checkBoxKeepStreamOrder = new System.Windows.Forms.CheckBox();
            this.checkBoxFilterShortPlaylists = new System.Windows.Forms.CheckBox();
            this.textBoxFilterShortPlaylistsValue = new System.Windows.Forms.TextBox();
            this.labelPlaylistLength = new System.Windows.Forms.Label();
            this.checkBoxEnableSSIF = new System.Windows.Forms.CheckBox();
            this.labelApiKey = new System.Windows.Forms.Label();
            this.textBoxApiKey = new System.Windows.Forms.TextBox();
            this.buttonCheckForUpdates = new System.Windows.Forms.Button();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageBDInfo = new System.Windows.Forms.TabPage();
            this.tabPageOutput = new System.Windows.Forms.TabPage();
            this.groupBoxOutputPrefs = new System.Windows.Forms.GroupBox();
            this.checkedListBoxAudioCodecs = new System.Windows.Forms.CheckedListBox();
            this.comboBoxAudienceLanguage = new System.Windows.Forms.ComboBox();
            this.labelAudioCodecs = new System.Windows.Forms.Label();
            this.labelAudienceLanguage = new System.Windows.Forms.Label();
            this.tabPageAdvanced = new System.Windows.Forms.TabPage();
            this.checkBoxCheckForUpdates = new System.Windows.Forms.CheckBox();
            this.tabControl.SuspendLayout();
            this.tabPageBDInfo.SuspendLayout();
            this.tabPageOutput.SuspendLayout();
            this.groupBoxOutputPrefs.SuspendLayout();
            this.tabPageAdvanced.SuspendLayout();
            this.SuspendLayout();
            // 
            // checkBoxFilterLoopingPlaylists
            // 
            this.checkBoxFilterLoopingPlaylists.AutoSize = true;
            this.checkBoxFilterLoopingPlaylists.Checked = true;
            this.checkBoxFilterLoopingPlaylists.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxFilterLoopingPlaylists.Location = new System.Drawing.Point(6, 29);
            this.checkBoxFilterLoopingPlaylists.Name = "checkBoxFilterLoopingPlaylists";
            this.checkBoxFilterLoopingPlaylists.Size = new System.Drawing.Size(177, 17);
            this.checkBoxFilterLoopingPlaylists.TabIndex = 3;
            this.checkBoxFilterLoopingPlaylists.Text = "Filter playlists that contain loops.";
            this.checkBoxFilterLoopingPlaylists.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(333, 245);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 6;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.Location = new System.Drawing.Point(252, 245);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 5;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // checkBoxKeepStreamOrder
            // 
            this.checkBoxKeepStreamOrder.AutoSize = true;
            this.checkBoxKeepStreamOrder.Enabled = false;
            this.checkBoxKeepStreamOrder.Location = new System.Drawing.Point(6, 6);
            this.checkBoxKeepStreamOrder.Name = "checkBoxKeepStreamOrder";
            this.checkBoxKeepStreamOrder.Size = new System.Drawing.Size(165, 17);
            this.checkBoxKeepStreamOrder.TabIndex = 4;
            this.checkBoxKeepStreamOrder.Text = "Keep original stream ordering.";
            this.checkBoxKeepStreamOrder.UseVisualStyleBackColor = true;
            // 
            // checkBoxFilterShortPlaylists
            // 
            this.checkBoxFilterShortPlaylists.AutoSize = true;
            this.checkBoxFilterShortPlaylists.Checked = true;
            this.checkBoxFilterShortPlaylists.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxFilterShortPlaylists.Location = new System.Drawing.Point(6, 52);
            this.checkBoxFilterShortPlaylists.Name = "checkBoxFilterShortPlaylists";
            this.checkBoxFilterShortPlaylists.Size = new System.Drawing.Size(150, 17);
            this.checkBoxFilterShortPlaylists.TabIndex = 7;
            this.checkBoxFilterShortPlaylists.Text = "Filter playlists with length <";
            this.checkBoxFilterShortPlaylists.UseVisualStyleBackColor = true;
            // 
            // textBoxFilterShortPlaylistsValue
            // 
            this.textBoxFilterShortPlaylistsValue.Location = new System.Drawing.Point(162, 50);
            this.textBoxFilterShortPlaylistsValue.MaxLength = 4;
            this.textBoxFilterShortPlaylistsValue.Name = "textBoxFilterShortPlaylistsValue";
            this.textBoxFilterShortPlaylistsValue.Size = new System.Drawing.Size(41, 20);
            this.textBoxFilterShortPlaylistsValue.TabIndex = 8;
            this.textBoxFilterShortPlaylistsValue.Text = "20";
            // 
            // labelPlaylistLength
            // 
            this.labelPlaylistLength.AutoSize = true;
            this.labelPlaylistLength.Location = new System.Drawing.Point(204, 54);
            this.labelPlaylistLength.Name = "labelPlaylistLength";
            this.labelPlaylistLength.Size = new System.Drawing.Size(24, 13);
            this.labelPlaylistLength.TabIndex = 9;
            this.labelPlaylistLength.Text = "sec";
            // 
            // checkBoxEnableSSIF
            // 
            this.checkBoxEnableSSIF.AutoSize = true;
            this.checkBoxEnableSSIF.Location = new System.Drawing.Point(6, 75);
            this.checkBoxEnableSSIF.Name = "checkBoxEnableSSIF";
            this.checkBoxEnableSSIF.Size = new System.Drawing.Size(134, 17);
            this.checkBoxEnableSSIF.TabIndex = 12;
            this.checkBoxEnableSSIF.Text = "Enable SSIF scanning.";
            this.checkBoxEnableSSIF.UseVisualStyleBackColor = true;
            // 
            // labelApiKey
            // 
            this.labelApiKey.AutoSize = true;
            this.labelApiKey.Location = new System.Drawing.Point(6, 9);
            this.labelApiKey.Name = "labelApiKey";
            this.labelApiKey.Size = new System.Drawing.Size(47, 13);
            this.labelApiKey.TabIndex = 13;
            this.labelApiKey.Text = "API key:";
            // 
            // textBoxApiKey
            // 
            this.textBoxApiKey.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxApiKey.Location = new System.Drawing.Point(59, 6);
            this.textBoxApiKey.Name = "textBoxApiKey";
            this.textBoxApiKey.Size = new System.Drawing.Size(316, 20);
            this.textBoxApiKey.TabIndex = 14;
            // 
            // buttonCheckForUpdates
            // 
            this.buttonCheckForUpdates.Location = new System.Drawing.Point(175, 32);
            this.buttonCheckForUpdates.Name = "buttonCheckForUpdates";
            this.buttonCheckForUpdates.Size = new System.Drawing.Size(75, 23);
            this.buttonCheckForUpdates.TabIndex = 16;
            this.buttonCheckForUpdates.Text = "Check now";
            this.buttonCheckForUpdates.UseVisualStyleBackColor = true;
            this.buttonCheckForUpdates.Click += new System.EventHandler(this.buttonCheckForUpdates_Click);
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Controls.Add(this.tabPageBDInfo);
            this.tabControl.Controls.Add(this.tabPageOutput);
            this.tabControl.Controls.Add(this.tabPageAdvanced);
            this.tabControl.Location = new System.Drawing.Point(12, 12);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(396, 227);
            this.tabControl.TabIndex = 17;
            // 
            // tabPageBDInfo
            // 
            this.tabPageBDInfo.Controls.Add(this.checkBoxKeepStreamOrder);
            this.tabPageBDInfo.Controls.Add(this.checkBoxFilterLoopingPlaylists);
            this.tabPageBDInfo.Controls.Add(this.checkBoxFilterShortPlaylists);
            this.tabPageBDInfo.Controls.Add(this.textBoxFilterShortPlaylistsValue);
            this.tabPageBDInfo.Controls.Add(this.labelPlaylistLength);
            this.tabPageBDInfo.Controls.Add(this.checkBoxEnableSSIF);
            this.tabPageBDInfo.Location = new System.Drawing.Point(4, 22);
            this.tabPageBDInfo.Name = "tabPageBDInfo";
            this.tabPageBDInfo.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageBDInfo.Size = new System.Drawing.Size(388, 201);
            this.tabPageBDInfo.TabIndex = 0;
            this.tabPageBDInfo.Text = "BDInfo";
            this.tabPageBDInfo.UseVisualStyleBackColor = true;
            // 
            // tabPageOutput
            // 
            this.tabPageOutput.Controls.Add(this.groupBoxOutputPrefs);
            this.tabPageOutput.Location = new System.Drawing.Point(4, 22);
            this.tabPageOutput.Name = "tabPageOutput";
            this.tabPageOutput.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageOutput.Size = new System.Drawing.Size(388, 201);
            this.tabPageOutput.TabIndex = 2;
            this.tabPageOutput.Text = "Output";
            this.tabPageOutput.UseVisualStyleBackColor = true;
            // 
            // groupBoxOutputPrefs
            // 
            this.groupBoxOutputPrefs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxOutputPrefs.Controls.Add(this.checkedListBoxAudioCodecs);
            this.groupBoxOutputPrefs.Controls.Add(this.comboBoxAudienceLanguage);
            this.groupBoxOutputPrefs.Controls.Add(this.labelAudioCodecs);
            this.groupBoxOutputPrefs.Controls.Add(this.labelAudienceLanguage);
            this.groupBoxOutputPrefs.Location = new System.Drawing.Point(7, 7);
            this.groupBoxOutputPrefs.Name = "groupBoxOutputPrefs";
            this.groupBoxOutputPrefs.Size = new System.Drawing.Size(370, 187);
            this.groupBoxOutputPrefs.TabIndex = 0;
            this.groupBoxOutputPrefs.TabStop = false;
            this.groupBoxOutputPrefs.Text = "Preferred Filter Settings";
            // 
            // checkedListBoxAudioCodecs
            // 
            this.checkedListBoxAudioCodecs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.checkedListBoxAudioCodecs.FormattingEnabled = true;
            this.checkedListBoxAudioCodecs.Location = new System.Drawing.Point(115, 46);
            this.checkedListBoxAudioCodecs.Name = "checkedListBoxAudioCodecs";
            this.checkedListBoxAudioCodecs.Size = new System.Drawing.Size(249, 124);
            this.checkedListBoxAudioCodecs.TabIndex = 3;
            // 
            // comboBoxAudienceLanguage
            // 
            this.comboBoxAudienceLanguage.FormattingEnabled = true;
            this.comboBoxAudienceLanguage.Location = new System.Drawing.Point(115, 19);
            this.comboBoxAudienceLanguage.Name = "comboBoxAudienceLanguage";
            this.comboBoxAudienceLanguage.Size = new System.Drawing.Size(189, 21);
            this.comboBoxAudienceLanguage.TabIndex = 2;
            // 
            // labelAudioCodecs
            // 
            this.labelAudioCodecs.AutoSize = true;
            this.labelAudioCodecs.Location = new System.Drawing.Point(7, 46);
            this.labelAudioCodecs.Name = "labelAudioCodecs";
            this.labelAudioCodecs.Size = new System.Drawing.Size(75, 13);
            this.labelAudioCodecs.TabIndex = 1;
            this.labelAudioCodecs.Text = "Audio codecs:";
            // 
            // labelAudienceLanguage
            // 
            this.labelAudienceLanguage.AutoSize = true;
            this.labelAudienceLanguage.Location = new System.Drawing.Point(7, 22);
            this.labelAudienceLanguage.Name = "labelAudienceLanguage";
            this.labelAudienceLanguage.Size = new System.Drawing.Size(102, 13);
            this.labelAudienceLanguage.TabIndex = 0;
            this.labelAudienceLanguage.Text = "Audience language:";
            // 
            // tabPageAdvanced
            // 
            this.tabPageAdvanced.Controls.Add(this.checkBoxCheckForUpdates);
            this.tabPageAdvanced.Controls.Add(this.buttonCheckForUpdates);
            this.tabPageAdvanced.Controls.Add(this.labelApiKey);
            this.tabPageAdvanced.Controls.Add(this.textBoxApiKey);
            this.tabPageAdvanced.Location = new System.Drawing.Point(4, 22);
            this.tabPageAdvanced.Name = "tabPageAdvanced";
            this.tabPageAdvanced.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageAdvanced.Size = new System.Drawing.Size(388, 201);
            this.tabPageAdvanced.TabIndex = 1;
            this.tabPageAdvanced.Text = "Advanced";
            this.tabPageAdvanced.UseVisualStyleBackColor = true;
            // 
            // checkBoxCheckForUpdates
            // 
            this.checkBoxCheckForUpdates.AutoSize = true;
            this.checkBoxCheckForUpdates.Checked = global::BDAutoMuxer.Properties.Settings.Default.CheckForUpdates;
            this.checkBoxCheckForUpdates.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxCheckForUpdates.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::BDAutoMuxer.Properties.Settings.Default, "CheckForUpdates", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBoxCheckForUpdates.Location = new System.Drawing.Point(6, 36);
            this.checkBoxCheckForUpdates.Name = "checkBoxCheckForUpdates";
            this.checkBoxCheckForUpdates.Size = new System.Drawing.Size(163, 17);
            this.checkBoxCheckForUpdates.TabIndex = 15;
            this.checkBoxCheckForUpdates.Text = "Check for updates on startup";
            this.checkBoxCheckForUpdates.UseVisualStyleBackColor = true;
            // 
            // FormSettings
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(420, 280);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(376, 216);
            this.Name = "FormSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "BDAutoMuxer Settings";
            this.tabControl.ResumeLayout(false);
            this.tabPageBDInfo.ResumeLayout(false);
            this.tabPageBDInfo.PerformLayout();
            this.tabPageOutput.ResumeLayout(false);
            this.groupBoxOutputPrefs.ResumeLayout(false);
            this.groupBoxOutputPrefs.PerformLayout();
            this.tabPageAdvanced.ResumeLayout(false);
            this.tabPageAdvanced.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBoxFilterLoopingPlaylists;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.CheckBox checkBoxKeepStreamOrder;
        private System.Windows.Forms.CheckBox checkBoxFilterShortPlaylists;
        private System.Windows.Forms.TextBox textBoxFilterShortPlaylistsValue;
        private System.Windows.Forms.Label labelPlaylistLength;
        private System.Windows.Forms.CheckBox checkBoxEnableSSIF;
        private System.Windows.Forms.Label labelApiKey;
        private System.Windows.Forms.TextBox textBoxApiKey;
        private System.Windows.Forms.CheckBox checkBoxCheckForUpdates;
        private System.Windows.Forms.Button buttonCheckForUpdates;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPageBDInfo;
        private System.Windows.Forms.TabPage tabPageAdvanced;
        private System.Windows.Forms.TabPage tabPageOutput;
        private System.Windows.Forms.GroupBox groupBoxOutputPrefs;
        private System.Windows.Forms.CheckedListBox checkedListBoxAudioCodecs;
        private System.Windows.Forms.ComboBox comboBoxAudienceLanguage;
        private System.Windows.Forms.Label labelAudioCodecs;
        private System.Windows.Forms.Label labelAudienceLanguage;
    }
}