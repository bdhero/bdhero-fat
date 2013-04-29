using System.Windows.Forms;

namespace CoverArtResizer
{
    partial class DraggableImage
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemCopyPath = new System.Windows.Forms.ToolStripMenuItem();
            this.pictureBox = new CoverArtResizer.TransparentPictureBox();
            this.label = new CoverArtResizer.TransparentLabel();
            this.contextMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemCopyPath});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(153, 48);
            this.contextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip_Opening);
            // 
            // toolStripMenuItemCopyPath
            // 
            this.toolStripMenuItemCopyPath.Name = "toolStripMenuItemCopyPath";
            this.toolStripMenuItemCopyPath.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItemCopyPath.Text = "Copy &path";
            this.toolStripMenuItemCopyPath.Click += new System.EventHandler(this.toolStripMenuItemCopyPath_Click);
            // 
            // pictureBox
            // 
            this.pictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox.Location = new System.Drawing.Point(3, 20);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(120, 178);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox.TabIndex = 1;
            this.pictureBox.TabStop = false;
            // 
            // label
            // 
            this.label.AutoSize = true;
            this.label.Location = new System.Drawing.Point(3, 4);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(67, 13);
            this.label.TabIndex = 0;
            this.label.Text = "filename.png";
            // 
            // DraggableImage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ContextMenuStrip = this.contextMenuStrip;
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.label);
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Name = "DraggableImage";
            this.Size = new System.Drawing.Size(124, 201);
            this.contextMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TransparentLabel label;
        private TransparentPictureBox pictureBox;
        private ContextMenuStrip contextMenuStrip;
        private ToolStripMenuItem toolStripMenuItemCopyPath;
    }
}
