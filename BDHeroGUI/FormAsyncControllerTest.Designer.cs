using DotNetUtils.Controls;

namespace BDHeroGUI
{
    partial class FormAsyncControllerTest
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
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxStatus = new System.Windows.Forms.TextBox();
            this.buttonScan = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonCancelConvert = new System.Windows.Forms.Button();
            this.buttonConvert = new System.Windows.Forms.Button();
            this.checkBoxShowAllPlaylists = new System.Windows.Forms.CheckBox();
            this.splitContainerWithDivider1 = new DotNetUtils.Controls.SplitContainerWithDivider();
            this.playlistListView = new BDHeroGUI.Components.PlaylistListView();
            this.tracksPanel = new BDHeroGUI.Components.TracksPanel();
            this.textBoxOutput = new DotNetUtils.Controls.FileTextBox();
            this.linkLabelShowFilterWindow = new DotNetUtils.Controls.LinkLabel2();
            this.progressBar = new DotNetUtils.Controls.ProgressBar2();
            this.textBoxInput = new DotNetUtils.Controls.FileTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerWithDivider1)).BeginInit();
            this.splitContainerWithDivider1.Panel1.SuspendLayout();
            this.splitContainerWithDivider1.Panel2.SuspendLayout();
            this.splitContainerWithDivider1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Input BD-ROM:";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 579);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Status:";
            // 
            // textBoxStatus
            // 
            this.textBoxStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxStatus.Location = new System.Drawing.Point(17, 595);
            this.textBoxStatus.Multiline = true;
            this.textBoxStatus.Name = "textBoxStatus";
            this.textBoxStatus.ReadOnly = true;
            this.textBoxStatus.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxStatus.Size = new System.Drawing.Size(1074, 67);
            this.textBoxStatus.TabIndex = 8;
            // 
            // buttonScan
            // 
            this.buttonScan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonScan.Location = new System.Drawing.Point(935, 43);
            this.buttonScan.Name = "buttonScan";
            this.buttonScan.Size = new System.Drawing.Size(75, 23);
            this.buttonScan.TabIndex = 1;
            this.buttonScan.Text = "Scan";
            this.buttonScan.UseVisualStyleBackColor = true;
            this.buttonScan.Click += new System.EventHandler(this.buttonMux_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(1016, 43);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 72);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Playlists:";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 537);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Output folder:";
            // 
            // buttonCancelConvert
            // 
            this.buttonCancelConvert.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancelConvert.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancelConvert.Location = new System.Drawing.Point(1016, 562);
            this.buttonCancelConvert.Name = "buttonCancelConvert";
            this.buttonCancelConvert.Size = new System.Drawing.Size(75, 23);
            this.buttonCancelConvert.TabIndex = 7;
            this.buttonCancelConvert.Text = "Cancel";
            this.buttonCancelConvert.UseVisualStyleBackColor = true;
            this.buttonCancelConvert.Click += new System.EventHandler(this.buttonCancelConvert_Click);
            // 
            // buttonConvert
            // 
            this.buttonConvert.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonConvert.Location = new System.Drawing.Point(935, 562);
            this.buttonConvert.Name = "buttonConvert";
            this.buttonConvert.Size = new System.Drawing.Size(75, 23);
            this.buttonConvert.TabIndex = 6;
            this.buttonConvert.Text = "Convert";
            this.buttonConvert.UseVisualStyleBackColor = true;
            this.buttonConvert.Click += new System.EventHandler(this.buttonConvert_Click);
            // 
            // checkBoxShowAllPlaylists
            // 
            this.checkBoxShowAllPlaylists.AutoSize = true;
            this.checkBoxShowAllPlaylists.Location = new System.Drawing.Point(112, 71);
            this.checkBoxShowAllPlaylists.Name = "checkBoxShowAllPlaylists";
            this.checkBoxShowAllPlaylists.Size = new System.Drawing.Size(66, 17);
            this.checkBoxShowAllPlaylists.TabIndex = 4;
            this.checkBoxShowAllPlaylists.Text = "Show &all";
            this.checkBoxShowAllPlaylists.UseVisualStyleBackColor = true;
            this.checkBoxShowAllPlaylists.CheckedChanged += new System.EventHandler(this.checkBoxShowAllPlaylists_CheckedChanged);
            // 
            // splitContainerWithDivider1
            // 
            this.splitContainerWithDivider1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainerWithDivider1.Location = new System.Drawing.Point(12, 94);
            this.splitContainerWithDivider1.Name = "splitContainerWithDivider1";
            // 
            // splitContainerWithDivider1.Panel1
            // 
            this.splitContainerWithDivider1.Panel1.Controls.Add(this.playlistListView);
            // 
            // splitContainerWithDivider1.Panel2
            // 
            this.splitContainerWithDivider1.Panel2.Controls.Add(this.tracksPanel);
            this.splitContainerWithDivider1.Size = new System.Drawing.Size(1079, 432);
            this.splitContainerWithDivider1.SplitterDistance = 536;
            this.splitContainerWithDivider1.TabIndex = 5;
            // 
            // playlistListView
            // 
            this.playlistListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.playlistListView.Location = new System.Drawing.Point(0, 0);
            this.playlistListView.Name = "playlistListView";
            this.playlistListView.Playlists = null;
            this.playlistListView.SelectedPlaylist = null;
            this.playlistListView.Size = new System.Drawing.Size(536, 432);
            this.playlistListView.TabIndex = 0;
            // 
            // tracksPanel
            // 
            this.tracksPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tracksPanel.Location = new System.Drawing.Point(0, 0);
            this.tracksPanel.Name = "tracksPanel";
            this.tracksPanel.Playlist = null;
            this.tracksPanel.Size = new System.Drawing.Size(539, 432);
            this.tracksPanel.TabIndex = 0;
            // 
            // textBoxOutput
            // 
            this.textBoxOutput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxOutput.DialogTitle = "Save MKV file to folder:";
            this.textBoxOutput.DialogType = DotNetUtils.Controls.DialogType.OpenDirectory;
            this.textBoxOutput.Location = new System.Drawing.Point(100, 532);
            this.textBoxOutput.Name = "textBoxOutput";
            this.textBoxOutput.OverwritePrompt = false;
            this.textBoxOutput.SelectedPath = "X:\\BDHero\\";
            this.textBoxOutput.ShowNewFolderButton = false;
            this.textBoxOutput.Size = new System.Drawing.Size(991, 24);
            this.textBoxOutput.TabIndex = 5;
            // 
            // linkLabelShowFilterWindow
            // 
            this.linkLabelShowFilterWindow.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.linkLabelShowFilterWindow.HoverColor = System.Drawing.Color.Empty;
            this.linkLabelShowFilterWindow.Location = new System.Drawing.Point(67, 72);
            this.linkLabelShowFilterWindow.Name = "linkLabelShowFilterWindow";
            this.linkLabelShowFilterWindow.RegularColor = System.Drawing.Color.Empty;
            this.linkLabelShowFilterWindow.Size = new System.Drawing.Size(39, 14);
            this.linkLabelShowFilterWindow.TabIndex = 3;
            this.linkLabelShowFilterWindow.Text = "Filter...";
            this.linkLabelShowFilterWindow.Click += new System.EventHandler(this.linkLabelShowFilterWindow_Click);
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(17, 668);
            this.progressBar.Maximum = 100000;
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(1074, 23);
            this.progressBar.Step = 1;
            this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar.TabIndex = 11;
            this.progressBar.TextOutline = true;
            this.progressBar.TextOutlineColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.progressBar.TextOutlineWidth = 2;
            this.progressBar.UseCustomColors = false;
            this.progressBar.ValuePercent = 0D;
            // 
            // textBoxInput
            // 
            this.textBoxInput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxInput.DialogTitle = "Select a BD-ROM folder:";
            this.textBoxInput.DialogType = DotNetUtils.Controls.DialogType.OpenDirectory;
            this.textBoxInput.Location = new System.Drawing.Point(100, 13);
            this.textBoxInput.Name = "textBoxInput";
            this.textBoxInput.OverwritePrompt = false;
            this.textBoxInput.SelectedPath = "X:\\BD\\49123204_BLACK_HAWK_DOWN";
            this.textBoxInput.ShowNewFolderButton = false;
            this.textBoxInput.Size = new System.Drawing.Size(991, 24);
            this.textBoxInput.TabIndex = 0;
            // 
            // FormAsyncControllerTest
            // 
            this.AcceptButton = this.buttonScan;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(1103, 703);
            this.Controls.Add(this.splitContainerWithDivider1);
            this.Controls.Add(this.checkBoxShowAllPlaylists);
            this.Controls.Add(this.buttonCancelConvert);
            this.Controls.Add(this.buttonConvert);
            this.Controls.Add(this.textBoxOutput);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.linkLabelShowFilterWindow);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.buttonScan);
            this.Controls.Add(this.textBoxStatus);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxInput);
            this.Controls.Add(this.label1);
            this.Name = "FormAsyncControllerTest";
            this.Text = "BDHero Async Controller Test";
            this.splitContainerWithDivider1.Panel1.ResumeLayout(false);
            this.splitContainerWithDivider1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerWithDivider1)).EndInit();
            this.splitContainerWithDivider1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private DotNetUtils.Controls.FileTextBox textBoxInput;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxStatus;
        private System.Windows.Forms.Button buttonScan;
        private DotNetUtils.Controls.ProgressBar2 progressBar;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label label4;
        private DotNetUtils.Controls.LinkLabel2 linkLabelShowFilterWindow;
        private DotNetUtils.Controls.FileTextBox textBoxOutput;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonCancelConvert;
        private System.Windows.Forms.Button buttonConvert;
        private System.Windows.Forms.CheckBox checkBoxShowAllPlaylists;
        private DotNetUtils.Controls.SplitContainerWithDivider splitContainerWithDivider1;
        private Components.PlaylistListView playlistListView;
        private Components.TracksPanel tracksPanel;
    }
}

