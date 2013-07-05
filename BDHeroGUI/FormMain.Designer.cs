using DotNetUtils.Controls;

namespace BDHeroGUI
{
    partial class FormMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxStatus = new System.Windows.Forms.TextBox();
            this.buttonScan = new System.Windows.Forms.Button();
            this.buttonCancelScan = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonCancelConvert = new System.Windows.Forms.Button();
            this.buttonConvert = new System.Windows.Forms.Button();
            this.splitContainerMain = new DotNetUtils.Controls.SplitContainerWithDivider();
            this.textBoxOutput = new DotNetUtils.Controls.FileTextBox();
            this.progressBar = new DotNetUtils.Controls.ProgressBar2();
            this.textBoxInput = new DotNetUtils.Controls.FileTextBox();
            this.splitContainerTop = new DotNetUtils.Controls.SplitContainerWithDivider();
            this.playlistListView = new BDHeroGUI.Components.PlaylistListView();
            this.tracksPanel = new BDHeroGUI.Components.TracksPanel();
            this.mediaPanel = new BDHeroGUI.Components.MediaPanel();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerTop)).BeginInit();
            this.splitContainerTop.Panel1.SuspendLayout();
            this.splitContainerTop.Panel2.SuspendLayout();
            this.splitContainerTop.SuspendLayout();
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
            this.label3.Location = new System.Drawing.Point(14, 507);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Status:";
            // 
            // textBoxStatus
            // 
            this.textBoxStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxStatus.Location = new System.Drawing.Point(17, 523);
            this.textBoxStatus.Multiline = true;
            this.textBoxStatus.Name = "textBoxStatus";
            this.textBoxStatus.ReadOnly = true;
            this.textBoxStatus.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxStatus.Size = new System.Drawing.Size(1063, 37);
            this.textBoxStatus.TabIndex = 7;
            // 
            // buttonScan
            // 
            this.buttonScan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonScan.Location = new System.Drawing.Point(924, 13);
            this.buttonScan.Name = "buttonScan";
            this.buttonScan.Size = new System.Drawing.Size(75, 23);
            this.buttonScan.TabIndex = 1;
            this.buttonScan.Text = "Scan";
            this.buttonScan.UseVisualStyleBackColor = true;
            this.buttonScan.Click += new System.EventHandler(this.buttonScan_Click);
            // 
            // buttonCancelScan
            // 
            this.buttonCancelScan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancelScan.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancelScan.Location = new System.Drawing.Point(1005, 13);
            this.buttonCancelScan.Name = "buttonCancelScan";
            this.buttonCancelScan.Size = new System.Drawing.Size(75, 23);
            this.buttonCancelScan.TabIndex = 2;
            this.buttonCancelScan.Text = "Cancel";
            this.buttonCancelScan.UseVisualStyleBackColor = true;
            this.buttonCancelScan.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 485);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Output MKV file:";
            // 
            // buttonCancelConvert
            // 
            this.buttonCancelConvert.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancelConvert.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancelConvert.Location = new System.Drawing.Point(1005, 480);
            this.buttonCancelConvert.Name = "buttonCancelConvert";
            this.buttonCancelConvert.Size = new System.Drawing.Size(75, 23);
            this.buttonCancelConvert.TabIndex = 6;
            this.buttonCancelConvert.Text = "Cancel";
            this.buttonCancelConvert.UseVisualStyleBackColor = true;
            this.buttonCancelConvert.Click += new System.EventHandler(this.buttonCancelConvert_Click);
            // 
            // buttonConvert
            // 
            this.buttonConvert.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonConvert.Location = new System.Drawing.Point(924, 480);
            this.buttonConvert.Name = "buttonConvert";
            this.buttonConvert.Size = new System.Drawing.Size(75, 23);
            this.buttonConvert.TabIndex = 5;
            this.buttonConvert.Text = "Convert";
            this.buttonConvert.UseVisualStyleBackColor = true;
            this.buttonConvert.Click += new System.EventHandler(this.buttonConvert_Click);
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainerMain.Location = new System.Drawing.Point(12, 43);
            this.splitContainerMain.Name = "splitContainerMain";
            this.splitContainerMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.Controls.Add(this.splitContainerTop);
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.tracksPanel);
            this.splitContainerMain.Size = new System.Drawing.Size(1068, 431);
            this.splitContainerMain.SplitterDistance = 124;
            this.splitContainerMain.TabIndex = 3;
            // 
            // textBoxOutput
            // 
            this.textBoxOutput.AllowAnyExtension = false;
            this.textBoxOutput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxOutput.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.textBoxOutput.DialogTitle = "Save MKV file:";
            this.textBoxOutput.DialogType = DotNetUtils.Controls.DialogType.SaveFile;
            this.textBoxOutput.FileExtensions = null;
            this.textBoxOutput.Location = new System.Drawing.Point(104, 480);
            this.textBoxOutput.Name = "textBoxOutput";
            this.textBoxOutput.SelectedPath = "W:\\BDHero\\test.mkv";
            this.textBoxOutput.Size = new System.Drawing.Size(814, 24);
            this.textBoxOutput.TabIndex = 4;
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(17, 566);
            this.progressBar.Maximum = 100000;
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(1063, 23);
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
            this.textBoxInput.AllowAnyExtension = false;
            this.textBoxInput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxInput.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.textBoxInput.DialogTitle = "Select a BD-ROM folder:";
            this.textBoxInput.DialogType = DotNetUtils.Controls.DialogType.OpenDirectory;
            this.textBoxInput.FileExtensions = null;
            this.textBoxInput.Location = new System.Drawing.Point(104, 13);
            this.textBoxInput.Name = "textBoxInput";
            this.textBoxInput.OverwritePrompt = false;
            this.textBoxInput.SelectedPath = "W:\\BD\\49123204_BLACK_HAWK_DOWN";
            this.textBoxInput.ShowNewFolderButton = false;
            this.textBoxInput.Size = new System.Drawing.Size(814, 24);
            this.textBoxInput.TabIndex = 0;
            this.textBoxInput.SelectedPathChanged += new System.EventHandler(this.textBoxInput_SelectedPathChanged);
            // 
            // splitContainerTop
            // 
            this.splitContainerTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerTop.Location = new System.Drawing.Point(0, 0);
            this.splitContainerTop.Name = "splitContainerTop";
            // 
            // splitContainerTop.Panel1
            // 
            this.splitContainerTop.Panel1.Controls.Add(this.playlistListView);
            // 
            // splitContainerTop.Panel2
            // 
            this.splitContainerTop.Panel2.Controls.Add(this.mediaPanel);
            this.splitContainerTop.Size = new System.Drawing.Size(1068, 124);
            this.splitContainerTop.SplitterDistance = 692;
            this.splitContainerTop.TabIndex = 7;
            // 
            // playlistListView
            // 
            this.playlistListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.playlistListView.Location = new System.Drawing.Point(0, 0);
            this.playlistListView.Name = "playlistListView";
            this.playlistListView.Playlists = null;
            this.playlistListView.SelectedPlaylist = null;
            this.playlistListView.Size = new System.Drawing.Size(692, 124);
            this.playlistListView.TabIndex = 1;
            // 
            // tracksPanel
            // 
            this.tracksPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tracksPanel.Location = new System.Drawing.Point(0, 0);
            this.tracksPanel.Name = "tracksPanel";
            this.tracksPanel.Playlist = null;
            this.tracksPanel.Size = new System.Drawing.Size(1068, 303);
            this.tracksPanel.TabIndex = 0;
            // 
            // mediaPanel
            // 
            this.mediaPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mediaPanel.Location = new System.Drawing.Point(0, 0);
            this.mediaPanel.Name = "mediaPanel";
            this.mediaPanel.Size = new System.Drawing.Size(372, 124);
            this.mediaPanel.TabIndex = 0;
            // 
            // FormMain
            // 
            this.AcceptButton = this.buttonScan;
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancelScan;
            this.ClientSize = new System.Drawing.Size(1084, 601);
            this.Controls.Add(this.splitContainerMain);
            this.Controls.Add(this.buttonCancelConvert);
            this.Controls.Add(this.buttonConvert);
            this.Controls.Add(this.textBoxOutput);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buttonCancelScan);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.buttonScan);
            this.Controls.Add(this.textBoxStatus);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxInput);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormMain";
            this.Text = "BDHero GUI";
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.FormMain_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.FormMain_DragEnter);
            this.DragLeave += new System.EventHandler(this.FormMain_DragLeave);
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
            this.splitContainerMain.ResumeLayout(false);
            this.splitContainerTop.Panel1.ResumeLayout(false);
            this.splitContainerTop.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerTop)).EndInit();
            this.splitContainerTop.ResumeLayout(false);
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
        private System.Windows.Forms.Button buttonCancelScan;
        private DotNetUtils.Controls.FileTextBox textBoxOutput;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonCancelConvert;
        private System.Windows.Forms.Button buttonConvert;
        private DotNetUtils.Controls.SplitContainerWithDivider splitContainerMain;
        private Components.TracksPanel tracksPanel;
        private SplitContainerWithDivider splitContainerTop;
        private Components.PlaylistListView playlistListView;
        private Components.MediaPanel mediaPanel;
    }
}

