using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using BDHero.JobQueue;
using DotNetUtils;
using DotNetUtils.Annotations;
using DotNetUtils.TaskUtils;

namespace BDHeroGUI.Components
{
    public partial class MediaPanel : UserControl
    {
        private static readonly log4net.ILog Logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [CanBeNull]
        public Image SelectedImage
        {
            get
            {
                return null;
            }
        }

        public Job Job
        {
            set
            {
                _job = value;
                SetMetaData();
            }
        }

        private void SetMetaData()
        {
            if (_job == null) return;

            var movie = _job.Movies.FirstOrDefault();
            if (movie == null) return;

            linkLabelTitle.Text = string.Format("{0} ({1})", movie.Title, movie.ReleaseYear);

            var coverArt = movie.CoverArtImages.FirstOrDefault();
            if (coverArt == null) return;

            new TaskBuilder()
                .OnCurrentThread()
                .DoWork(delegate(IThreadInvoker invoker, CancellationToken token)
                    {
                        var x = coverArt.Image;
                    })
                .Fail(delegate(Exception exception)
                    {
                        Logger.Error("Unable to fetch poster image", exception);
                        pictureBox.Image = null;
                    })
                .Succeed(delegate
                    {
                        pictureBox.Image = coverArt.Image;
                    })
                .Build()
                .Start()
                ;
        }

        private Job _job;

        public MediaPanel()
        {
            InitializeComponent();
        }
    }
}
