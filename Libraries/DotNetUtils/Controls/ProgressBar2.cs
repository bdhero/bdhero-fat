using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DotNetUtils.Controls
{
    /// <summary>
    /// A more sensible extension of the standard <see cref="ProgressBar"/> class that adds
    /// a getter/setter for <see cref="ProgressBar.Value"/> that accepts a <code>double</code>
    /// in the range 0.0 to 100.0.  It also supports custom colors to denote state.
    /// </summary>
    public class ProgressBar2 : ProgressBar
    {
        /// <summary>
        /// Gets or sets the value of the progress bar from 0.0 to 100.0.
        /// </summary>
        public double ValuePercent
        {
            get { return _valuePercent; }
            set
            {
                _valuePercent = value;
                Value = (int) (value * Maximum / 100.0);
            }
        }

        /// <summary>
        /// Gets or sets whether the progress bar should use custom background gradients
        /// to indicate its state (e.g., paused, error, success) or the standard Windows gradient (green).
        /// </summary>
        public bool CustomColors
        {
            get { return GetStyle(ControlStyles.UserPaint); }
            set { SetStyle(ControlStyles.UserPaint, value); }
        }

        private double _valuePercent;

        public ProgressBar2()
        {
            Maximum = 100 * 1000;
        }

        public void SetError()
        {
            ForeColor = Color.Red;
            BackColor = Color.DarkRed;
        }

        public void SetPaused()
        {
            ForeColor = Color.LightGoldenrodYellow;
            BackColor = Color.Yellow;
        }

        public void SetInfo()
        {
            ForeColor = Color.LightSkyBlue;
            BackColor = Color.DeepSkyBlue;
        }

        public void SetSuccess()
        {
            ForeColor = Color.GreenYellow;
            BackColor = Color.Green;
        }

        public void SetMuted()
        {
            ForeColor = Color.DarkGray;
            BackColor = Color.Gray;
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            if (!CustomColors)
            {
                base.OnPaintBackground(pevent);
            }
            // None... Helps control the flicker.
        }

        /// <seealso cref="http://stackoverflow.com/a/7490884/467582"/>
        protected override void OnPaint(PaintEventArgs e)
        {
            if (!CustomColors)
            {
                base.OnPaint(e);
                return;
            }

            const int inset = 2; // A single inset value to control the sizing of the inner rect.

            using (Image offscreenImage = new Bitmap(Width, Height))
            {
                using (Graphics offscreen = Graphics.FromImage(offscreenImage))
                {
                    Rectangle rect = new Rectangle(0, 0, Width, Height);

                    if (ProgressBarRenderer.IsSupported)
                        ProgressBarRenderer.DrawHorizontalBar(offscreen, rect);

                    rect.Inflate(new Size(-inset, -inset)); // Deflate inner rect.
                    rect.Width = (int)(rect.Width * ((double)Value / Maximum));
                    if (rect.Width == 0) rect.Width = 1; // Can't draw rec with width of 0.

                    LinearGradientBrush brush = new LinearGradientBrush(rect, ForeColor, BackColor, LinearGradientMode.Vertical);
                    offscreen.FillRectangle(brush, inset, inset, rect.Width, rect.Height);

                    e.Graphics.DrawImage(offscreenImage, 0, 0);
                    offscreenImage.Dispose();
                }
            }
        }
    }
}
