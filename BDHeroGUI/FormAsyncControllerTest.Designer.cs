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
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxInput = new System.Windows.Forms.TextBox();
            this.textBoxOutput = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxStatus = new System.Windows.Forms.TextBox();
            this.buttonMux = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.playlistListView = new BDHeroGUI.Components.PlaylistGridControl();
            this.progressBar = new DotNetUtils.Controls.ProgressBar2();
            this.linkLabelShowFilterWindow = new DotNetUtils.Controls.LinkLabel2();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Input BD-ROM:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Output MKV:";
            // 
            // textBoxInput
            // 
            this.textBoxInput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxInput.Location = new System.Drawing.Point(100, 13);
            this.textBoxInput.Name = "textBoxInput";
            this.textBoxInput.Size = new System.Drawing.Size(583, 20);
            this.textBoxInput.TabIndex = 1;
            this.textBoxInput.Text = "X:\\BD\\49123204_BLACK_HAWK_DOWN";
            // 
            // textBoxOutput
            // 
            this.textBoxOutput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxOutput.Location = new System.Drawing.Point(100, 39);
            this.textBoxOutput.Name = "textBoxOutput";
            this.textBoxOutput.Size = new System.Drawing.Size(583, 20);
            this.textBoxOutput.TabIndex = 3;
            this.textBoxOutput.Text = "X:\\BDHero\\";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 384);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Status:";
            // 
            // textBoxStatus
            // 
            this.textBoxStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxStatus.Location = new System.Drawing.Point(17, 400);
            this.textBoxStatus.Multiline = true;
            this.textBoxStatus.Name = "textBoxStatus";
            this.textBoxStatus.ReadOnly = true;
            this.textBoxStatus.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxStatus.Size = new System.Drawing.Size(666, 67);
            this.textBoxStatus.TabIndex = 10;
            // 
            // buttonMux
            // 
            this.buttonMux.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonMux.Location = new System.Drawing.Point(527, 65);
            this.buttonMux.Name = "buttonMux";
            this.buttonMux.Size = new System.Drawing.Size(75, 23);
            this.buttonMux.TabIndex = 4;
            this.buttonMux.Text = "Scan";
            this.buttonMux.UseVisualStyleBackColor = true;
            this.buttonMux.Click += new System.EventHandler(this.buttonMux_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(608, 65);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 5;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 94);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Playlists:";
            // 
            // playlistListView
            // 
            this.playlistListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.playlistListView.Location = new System.Drawing.Point(12, 114);
            this.playlistListView.Name = "playlistListView";
            this.playlistListView.Playlists = null;
            this.playlistListView.Size = new System.Drawing.Size(671, 267);
            this.playlistListView.TabIndex = 8;
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(17, 473);
            this.progressBar.Maximum = 100000;
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(666, 23);
            this.progressBar.Step = 1;
            this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar.TabIndex = 11;
            this.progressBar.TextOutline = true;
            this.progressBar.TextOutlineColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.progressBar.TextOutlineWidth = 2;
            this.progressBar.UseCustomColors = false;
            this.progressBar.ValuePercent = 0D;
            // 
            // linkLabelShowFilterWindow
            // 
            this.linkLabelShowFilterWindow.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.linkLabelShowFilterWindow.HoverColor = System.Drawing.Color.Empty;
            this.linkLabelShowFilterWindow.Location = new System.Drawing.Point(65, 94);
            this.linkLabelShowFilterWindow.Name = "linkLabelShowFilterWindow";
            this.linkLabelShowFilterWindow.RegularColor = System.Drawing.Color.Empty;
            this.linkLabelShowFilterWindow.Size = new System.Drawing.Size(39, 14);
            this.linkLabelShowFilterWindow.TabIndex = 7;
            this.linkLabelShowFilterWindow.Text = "Filter...";
            this.linkLabelShowFilterWindow.Click += new System.EventHandler(this.linkLabelShowFilterWindow_Click);
            // 
            // FormAsyncControllerTest
            // 
            this.AcceptButton = this.buttonMux;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(695, 508);
            this.Controls.Add(this.linkLabelShowFilterWindow);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.playlistListView);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.buttonMux);
            this.Controls.Add(this.textBoxStatus);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxOutput);
            this.Controls.Add(this.textBoxInput);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "FormAsyncControllerTest";
            this.Text = "BDHero Async Controller Test";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxInput;
        private System.Windows.Forms.TextBox textBoxOutput;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxStatus;
        private System.Windows.Forms.Button buttonMux;
        private DotNetUtils.Controls.ProgressBar2 progressBar;
        private System.Windows.Forms.Button buttonCancel;
        private Components.PlaylistGridControl playlistListView;
        private System.Windows.Forms.Label label4;
        private DotNetUtils.Controls.LinkLabel2 linkLabelShowFilterWindow;
    }
}

