namespace BDHeroGUI.Forms
{
    partial class FormDiscInfo
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
            this.labelQuickSummary = new System.Windows.Forms.Label();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.buttonClose = new System.Windows.Forms.Button();
            this.discInfoMetadataPanel = new BDHeroGUI.Components.DiscInfoMetadataPanel();
            this.discInfoFeaturesPanel = new BDHeroGUI.Components.DiscInfoFeaturesPanel();
            this.tabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "BD-ROM:";
            // 
            // labelQuickSummary
            // 
            this.labelQuickSummary.AutoSize = true;
            this.labelQuickSummary.Location = new System.Drawing.Point(73, 13);
            this.labelQuickSummary.Name = "labelQuickSummary";
            this.labelQuickSummary.Size = new System.Drawing.Size(110, 13);
            this.labelQuickSummary.TabIndex = 1;
            this.labelQuickSummary.Text = "VOLUME_LABEL D:\\";
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Controls.Add(this.tabPage1);
            this.tabControl.Controls.Add(this.tabPage2);
            this.tabControl.Location = new System.Drawing.Point(12, 38);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(573, 400);
            this.tabControl.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.discInfoMetadataPanel);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(565, 374);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Metadata";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.discInfoFeaturesPanel);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(565, 374);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Features";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonClose.Location = new System.Drawing.Point(510, 444);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 23);
            this.buttonClose.TabIndex = 3;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // discInfoMetadataPanel
            // 
            this.discInfoMetadataPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.discInfoMetadataPanel.Location = new System.Drawing.Point(3, 3);
            this.discInfoMetadataPanel.Name = "discInfoMetadataPanel";
            this.discInfoMetadataPanel.Size = new System.Drawing.Size(559, 368);
            this.discInfoMetadataPanel.TabIndex = 0;
            // 
            // discInfoFeaturesPanel
            // 
            this.discInfoFeaturesPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.discInfoFeaturesPanel.Location = new System.Drawing.Point(3, 3);
            this.discInfoFeaturesPanel.Name = "discInfoFeaturesPanel";
            this.discInfoFeaturesPanel.Size = new System.Drawing.Size(559, 368);
            this.discInfoFeaturesPanel.TabIndex = 0;
            // 
            // FormDiscInfo
            // 
            this.AcceptButton = this.buttonClose;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonClose;
            this.ClientSize = new System.Drawing.Size(597, 479);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.labelQuickSummary);
            this.Controls.Add(this.label1);
            this.Name = "FormDiscInfo";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Disc Info";
            this.tabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelQuickSummary;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private Components.DiscInfoMetadataPanel discInfoMetadataPanel;
        private System.Windows.Forms.Button buttonClose;
        private Components.DiscInfoFeaturesPanel discInfoFeaturesPanel;
    }
}