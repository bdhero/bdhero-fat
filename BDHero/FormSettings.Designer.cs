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

using DotNetUtils.Controls;

namespace BDHero
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
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.tabPageAdvanced = new System.Windows.Forms.TabPage();
            this.checkBoxUseMainMovieDb = new System.Windows.Forms.CheckBox();
            this.checkBoxCheckForUpdates = new System.Windows.Forms.CheckBox();
            this.buttonCheckForUpdates = new System.Windows.Forms.Button();
            this.labelApiKey = new System.Windows.Forms.Label();
            this.textBoxApiKey = new System.Windows.Forms.TextBox();
            this.tabPageOutput = new System.Windows.Forms.TabPage();
            this.label2 = new System.Windows.Forms.Label();
            this.checkBoxSelectHighestChannelCount = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.splitButtonSelectAudioCodecs = new SplitButton();
            this.checkedListBoxAudioCodecs = new System.Windows.Forms.CheckedListBox();
            this.comboBoxAudienceLanguage = new System.Windows.Forms.ComboBox();
            this.labelAudioCodecs = new System.Windows.Forms.Label();
            this.labelAudienceLanguage = new System.Windows.Forms.Label();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageAdvanced.SuspendLayout();
            this.tabPageOutput.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(405, 221);
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
            this.buttonOK.Location = new System.Drawing.Point(324, 221);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 5;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.SaveSettings);
            // 
            // tabPageAdvanced
            // 
            this.tabPageAdvanced.Controls.Add(this.checkBoxUseMainMovieDb);
            this.tabPageAdvanced.Controls.Add(this.checkBoxCheckForUpdates);
            this.tabPageAdvanced.Controls.Add(this.buttonCheckForUpdates);
            this.tabPageAdvanced.Controls.Add(this.labelApiKey);
            this.tabPageAdvanced.Controls.Add(this.textBoxApiKey);
            this.tabPageAdvanced.Location = new System.Drawing.Point(4, 22);
            this.tabPageAdvanced.Name = "tabPageAdvanced";
            this.tabPageAdvanced.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageAdvanced.Size = new System.Drawing.Size(460, 177);
            this.tabPageAdvanced.TabIndex = 1;
            this.tabPageAdvanced.Text = "Advanced";
            this.tabPageAdvanced.UseVisualStyleBackColor = true;
            // 
            // checkBoxUseMainMovieDb
            // 
            this.checkBoxUseMainMovieDb.AutoSize = true;
            this.checkBoxUseMainMovieDb.Checked = true;
            this.checkBoxUseMainMovieDb.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxUseMainMovieDb.Location = new System.Drawing.Point(6, 6);
            this.checkBoxUseMainMovieDb.Name = "checkBoxUseMainMovieDb";
            this.checkBoxUseMainMovieDb.Size = new System.Drawing.Size(222, 17);
            this.checkBoxUseMainMovieDb.TabIndex = 0;
            this.checkBoxUseMainMovieDb.Text = "Use online &database to improve accuracy";
            this.checkBoxUseMainMovieDb.UseVisualStyleBackColor = true;
            // 
            // checkBoxCheckForUpdates
            // 
            this.checkBoxCheckForUpdates.AutoSize = true;
            this.checkBoxCheckForUpdates.Checked = global::BDHero.Properties.Settings.Default.CheckForUpdates;
            this.checkBoxCheckForUpdates.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxCheckForUpdates.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::BDHero.Properties.Settings.Default, "CheckForUpdates", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBoxCheckForUpdates.Location = new System.Drawing.Point(6, 33);
            this.checkBoxCheckForUpdates.Name = "checkBoxCheckForUpdates";
            this.checkBoxCheckForUpdates.Size = new System.Drawing.Size(163, 17);
            this.checkBoxCheckForUpdates.TabIndex = 1;
            this.checkBoxCheckForUpdates.Text = "Check for &updates on startup";
            this.checkBoxCheckForUpdates.UseVisualStyleBackColor = true;
            // 
            // buttonCheckForUpdates
            // 
            this.buttonCheckForUpdates.Location = new System.Drawing.Point(175, 29);
            this.buttonCheckForUpdates.Name = "buttonCheckForUpdates";
            this.buttonCheckForUpdates.Size = new System.Drawing.Size(75, 23);
            this.buttonCheckForUpdates.TabIndex = 2;
            this.buttonCheckForUpdates.Text = "&Check now";
            this.buttonCheckForUpdates.UseVisualStyleBackColor = true;
            this.buttonCheckForUpdates.Click += new System.EventHandler(this.buttonCheckForUpdates_Click);
            // 
            // labelApiKey
            // 
            this.labelApiKey.AutoSize = true;
            this.labelApiKey.Location = new System.Drawing.Point(6, 61);
            this.labelApiKey.Name = "labelApiKey";
            this.labelApiKey.Size = new System.Drawing.Size(47, 13);
            this.labelApiKey.TabIndex = 13;
            this.labelApiKey.Text = "API key:";
            // 
            // textBoxApiKey
            // 
            this.textBoxApiKey.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxApiKey.Location = new System.Drawing.Point(59, 58);
            this.textBoxApiKey.Name = "textBoxApiKey";
            this.textBoxApiKey.Size = new System.Drawing.Size(395, 20);
            this.textBoxApiKey.TabIndex = 3;
            // 
            // tabPageOutput
            // 
            this.tabPageOutput.Controls.Add(this.label2);
            this.tabPageOutput.Controls.Add(this.checkBoxSelectHighestChannelCount);
            this.tabPageOutput.Controls.Add(this.label1);
            this.tabPageOutput.Controls.Add(this.splitButtonSelectAudioCodecs);
            this.tabPageOutput.Controls.Add(this.checkedListBoxAudioCodecs);
            this.tabPageOutput.Controls.Add(this.comboBoxAudienceLanguage);
            this.tabPageOutput.Controls.Add(this.labelAudioCodecs);
            this.tabPageOutput.Controls.Add(this.labelAudienceLanguage);
            this.tabPageOutput.Location = new System.Drawing.Point(4, 22);
            this.tabPageOutput.Name = "tabPageOutput";
            this.tabPageOutput.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageOutput.Size = new System.Drawing.Size(460, 177);
            this.tabPageOutput.TabIndex = 2;
            this.tabPageOutput.Text = "Output";
            this.tabPageOutput.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.ForeColor = System.Drawing.SystemColors.GrayText;
            this.label2.Location = new System.Drawing.Point(4, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(150, 44);
            this.label2.TabIndex = 14;
            this.label2.Text = "If no preferred codecs are available, the first audio track will be selected inst" +
                "ead";
            // 
            // checkBoxSelectHighestChannelCount
            // 
            this.checkBoxSelectHighestChannelCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxSelectHighestChannelCount.AutoSize = true;
            this.checkBoxSelectHighestChannelCount.Checked = global::BDHero.Properties.Settings.Default.SelectHighestChannelCount;
            this.checkBoxSelectHighestChannelCount.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxSelectHighestChannelCount.Location = new System.Drawing.Point(160, 154);
            this.checkBoxSelectHighestChannelCount.Name = "checkBoxSelectHighestChannelCount";
            this.checkBoxSelectHighestChannelCount.Size = new System.Drawing.Size(199, 17);
            this.checkBoxSelectHighestChannelCount.TabIndex = 13;
            this.checkBoxSelectHighestChannelCount.Text = "Select tracks with the most channels";
            this.checkBoxSelectHighestChannelCount.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 155);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Audio channels:";
            // 
            // splitButtonSelectAudioCodecs
            // 
            this.splitButtonSelectAudioCodecs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.splitButtonSelectAudioCodecs.AutoSize = true;
            this.splitButtonSelectAudioCodecs.Location = new System.Drawing.Point(357, 33);
            this.splitButtonSelectAudioCodecs.Name = "splitButtonSelectAudioCodecs";
            this.splitButtonSelectAudioCodecs.Size = new System.Drawing.Size(97, 23);
            this.splitButtonSelectAudioCodecs.TabIndex = 11;
            this.splitButtonSelectAudioCodecs.Text = "Select all";
            this.splitButtonSelectAudioCodecs.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.splitButtonSelectAudioCodecs.UseVisualStyleBackColor = true;
            // 
            // checkedListBoxAudioCodecs
            // 
            this.checkedListBoxAudioCodecs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.checkedListBoxAudioCodecs.FormattingEnabled = true;
            this.checkedListBoxAudioCodecs.Location = new System.Drawing.Point(160, 33);
            this.checkedListBoxAudioCodecs.Name = "checkedListBoxAudioCodecs";
            this.checkedListBoxAudioCodecs.Size = new System.Drawing.Size(191, 109);
            this.checkedListBoxAudioCodecs.TabIndex = 10;
            // 
            // comboBoxAudienceLanguage
            // 
            this.comboBoxAudienceLanguage.FormattingEnabled = true;
            this.comboBoxAudienceLanguage.Location = new System.Drawing.Point(160, 6);
            this.comboBoxAudienceLanguage.Name = "comboBoxAudienceLanguage";
            this.comboBoxAudienceLanguage.Size = new System.Drawing.Size(171, 21);
            this.comboBoxAudienceLanguage.TabIndex = 9;
            // 
            // labelAudioCodecs
            // 
            this.labelAudioCodecs.AutoSize = true;
            this.labelAudioCodecs.Location = new System.Drawing.Point(4, 33);
            this.labelAudioCodecs.Name = "labelAudioCodecs";
            this.labelAudioCodecs.Size = new System.Drawing.Size(126, 13);
            this.labelAudioCodecs.TabIndex = 8;
            this.labelAudioCodecs.Text = "Audio codecs (preferred):";
            // 
            // labelAudienceLanguage
            // 
            this.labelAudienceLanguage.AutoSize = true;
            this.labelAudienceLanguage.Location = new System.Drawing.Point(4, 9);
            this.labelAudienceLanguage.Name = "labelAudienceLanguage";
            this.labelAudienceLanguage.Size = new System.Drawing.Size(153, 13);
            this.labelAudienceLanguage.TabIndex = 7;
            this.labelAudienceLanguage.Text = "Audience language (preferred):";
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Controls.Add(this.tabPageOutput);
            this.tabControl.Controls.Add(this.tabPageAdvanced);
            this.tabControl.Location = new System.Drawing.Point(12, 12);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(468, 203);
            this.tabControl.TabIndex = 17;
            // 
            // FormSettings
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(492, 256);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(500, 280);
            this.Name = "FormSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "BDHero Settings";
            this.tabPageAdvanced.ResumeLayout(false);
            this.tabPageAdvanced.PerformLayout();
            this.tabPageOutput.ResumeLayout(false);
            this.tabPageOutput.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.TabPage tabPageAdvanced;
        private System.Windows.Forms.CheckBox checkBoxCheckForUpdates;
        private System.Windows.Forms.Button buttonCheckForUpdates;
        private System.Windows.Forms.Label labelApiKey;
        private System.Windows.Forms.TextBox textBoxApiKey;
        private System.Windows.Forms.TabPage tabPageOutput;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox checkBoxSelectHighestChannelCount;
        private System.Windows.Forms.Label label1;
        private SplitButton splitButtonSelectAudioCodecs;
        private System.Windows.Forms.CheckedListBox checkedListBoxAudioCodecs;
        private System.Windows.Forms.ComboBox comboBoxAudienceLanguage;
        private System.Windows.Forms.Label labelAudioCodecs;
        private System.Windows.Forms.Label labelAudienceLanguage;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.CheckBox checkBoxUseMainMovieDb;
    }
}