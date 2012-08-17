namespace BDInfo
{
    partial class FormMovieName
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
            this.movieNameLabel = new System.Windows.Forms.Label();
            this.movieNameBox = new System.Windows.Forms.ComboBox();
            this.queryButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // movieNameLabel
            // 
            this.movieNameLabel.AutoSize = true;
            this.movieNameLabel.Location = new System.Drawing.Point(13, 13);
            this.movieNameLabel.Name = "movieNameLabel";
            this.movieNameLabel.Size = new System.Drawing.Size(68, 13);
            this.movieNameLabel.TabIndex = 0;
            this.movieNameLabel.Text = "Movie name:";
            // 
            // movieNameBox
            // 
            this.movieNameBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.movieNameBox.Enabled = false;
            this.movieNameBox.FormattingEnabled = true;
            this.movieNameBox.Location = new System.Drawing.Point(13, 30);
            this.movieNameBox.Name = "movieNameBox";
            this.movieNameBox.Size = new System.Drawing.Size(347, 21);
            this.movieNameBox.TabIndex = 1;
            // 
            // queryButton
            // 
            this.queryButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.queryButton.Enabled = false;
            this.queryButton.Location = new System.Drawing.Point(285, 61);
            this.queryButton.Name = "queryButton";
            this.queryButton.Size = new System.Drawing.Size(75, 23);
            this.queryButton.TabIndex = 2;
            this.queryButton.Text = "Query DB";
            this.queryButton.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(204, 61);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 3;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // FormMovieName
            // 
            this.AcceptButton = this.queryButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(372, 96);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.queryButton);
            this.Controls.Add(this.movieNameBox);
            this.Controls.Add(this.movieNameLabel);
            this.Name = "FormMovieName";
            this.Text = "FormMovieName";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label movieNameLabel;
        private System.Windows.Forms.ComboBox movieNameBox;
        private System.Windows.Forms.Button queryButton;
        private System.Windows.Forms.Button cancelButton;
    }
}