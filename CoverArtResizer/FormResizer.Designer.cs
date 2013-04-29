namespace CoverArtResizer
{
    partial class FormResizer
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
            this.textBoxImageUrl = new System.Windows.Forms.TextBox();
            this.buttonResize = new System.Windows.Forms.Button();
            this.draggableImageSmallCover = new CoverArtResizer.DraggableImage();
            this.draggableImageCover = new CoverArtResizer.DraggableImage();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Image URL:";
            // 
            // textBoxImageUrl
            // 
            this.textBoxImageUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxImageUrl.Location = new System.Drawing.Point(84, 13);
            this.textBoxImageUrl.Name = "textBoxImageUrl";
            this.textBoxImageUrl.Size = new System.Drawing.Size(469, 20);
            this.textBoxImageUrl.TabIndex = 1;
            // 
            // buttonResize
            // 
            this.buttonResize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonResize.Location = new System.Drawing.Point(559, 12);
            this.buttonResize.Name = "buttonResize";
            this.buttonResize.Size = new System.Drawing.Size(75, 23);
            this.buttonResize.TabIndex = 2;
            this.buttonResize.Text = "Resize";
            this.buttonResize.UseVisualStyleBackColor = true;
            // 
            // draggableImageSmallCover
            // 
            this.draggableImageSmallCover.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.draggableImageSmallCover.Cursor = System.Windows.Forms.Cursors.Hand;
            this.draggableImageSmallCover.Location = new System.Drawing.Point(290, 40);
            this.draggableImageSmallCover.Name = "draggableImageSmallCover";
            this.draggableImageSmallCover.Size = new System.Drawing.Size(120, 180);
            this.draggableImageSmallCover.TabIndex = 4;
            // 
            // draggableImageCover
            // 
            this.draggableImageCover.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.draggableImageCover.Cursor = System.Windows.Forms.Cursors.Hand;
            this.draggableImageCover.Location = new System.Drawing.Point(84, 40);
            this.draggableImageCover.Name = "draggableImageCover";
            this.draggableImageCover.Size = new System.Drawing.Size(200, 300);
            this.draggableImageCover.TabIndex = 3;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 355);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(646, 22);
            this.statusStrip1.TabIndex = 5;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(38, 17);
            this.toolStripStatusLabel.Text = "Status";
            // 
            // FormResizer
            // 
            this.AcceptButton = this.buttonResize;
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(646, 377);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.draggableImageSmallCover);
            this.Controls.Add(this.buttonResize);
            this.Controls.Add(this.draggableImageCover);
            this.Controls.Add(this.textBoxImageUrl);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FormResizer";
            this.Text = "FormResizer";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxImageUrl;
        private DraggableImage draggableImageCover;
        private System.Windows.Forms.Button buttonResize;
        private DraggableImage draggableImageSmallCover;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;

    }
}

