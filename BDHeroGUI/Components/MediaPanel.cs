using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using BDHero.JobQueue;
using BDHeroGUI.Properties;
using DotNetUtils;
using DotNetUtils.Annotations;
using DotNetUtils.Extensions;
using DotNetUtils.TaskUtils;

namespace BDHeroGUI.Components
{
    public partial class MediaPanel : UserControl
    {
        private static readonly log4net.ILog Logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private const double DefaultRatio = 2.0 / 3.0;

        [CanBeNull]
        public CoverArt SelectedCoverArt
        {
            get
            {
                return _selectedCoverArt;
            }
            private set
            {
                _selectedCoverArt = value;
                pictureBox.Image = SelectedCoverArt != null ? SelectedCoverArt.Image : Resources.no_poster_w185;
                AutoResize();
            }
        }

        private CoverArt _selectedCoverArt;

        public Job Job
        {
            set
            {
                _job = value;
                LoadSearchResults();
            }
        }

        private Job _job;

        private readonly ToolTip _pictureBoxToolTip = new ToolTip();

        public event EventHandler SelectedMediaChanged;

        public Action Search;

        public MediaPanel()
        {
            InitializeComponent();
            Resize += (sender, args) => AutoResize();
            comboBoxMedia.SelectedIndexChanged += (sender, args) => OnSelectedMediaChanged();
            LoadSearchResults();
        }

        private void AutoResize()
        {
            var ratio = DefaultRatio;
            if (SelectedCoverArt != null && SelectedCoverArt.Image != null)
            {
                var image = SelectedCoverArt.Image;
                ratio = ((double) image.Width) / image.Height;
            }
            var width = ratio * pictureBox.Height;
            splitContainer.SplitterDistance = (int) Math.Ceiling(width);
        }

        private void PopulateComboBox(ICollection<ReleaseMedium> releaseMedia)
        {
            comboBoxMedia.Items.Clear();

            releaseMedia.ForEach(item => comboBoxMedia.Items.Add(item));

            var hasItems = releaseMedia.Any();

            if (hasItems)
                comboBoxMedia.SelectedIndex = 0;

            comboBoxMedia.Enabled = hasItems;
        }

        private void LoadSearchResults()
        {
            var media = _job != null
                            ? _job.Movies.OfType<ReleaseMedium>().ToArray()
                            : new Collection<ReleaseMedium>().ToArray();

            PopulateComboBox(media);
            OnSelectedMediaChanged();
        }

        private void OnSelectedMediaChanged()
        {
            if (comboBoxMedia.Items.Count > 0)
                _job.Movies.ForEach(AutoSelect);

            LoadCoverArt();

            if (SelectedMediaChanged != null)
                SelectedMediaChanged(this, EventArgs.Empty);

            if (SelectedMovieHasValidUrl)
            {
                pictureBox.Cursor = Cursors.Hand;
                _pictureBoxToolTip.SetToolTip(pictureBox, SelectedMovieUrl);
            }
            else
            {
                pictureBox.Cursor = Cursors.Default;
                _pictureBoxToolTip.RemoveAll();
            }
        }

        private void AutoSelect(Movie movie, int i)
        {
            movie.IsSelected = (i == comboBoxMedia.SelectedIndex);
        }

        private void LoadCoverArt()
        {
            SelectedCoverArt = null;

            if (comboBoxMedia.Items.Count == 0)
                return;

            var index = comboBoxMedia.SelectedIndex;

            if (index == -1)
                return;

            var movie = _job.Movies[index];

            var coverArt = movie.CoverArtImages.FirstOrDefault();
            if (coverArt == null) return;

            new TaskBuilder()
                .OnCurrentThread()
                .DoWork(delegate(IThreadInvoker invoker, CancellationToken token)
                {
                    var image = coverArt.Image;
                })
                .Fail(delegate(Exception exception)
                {
                    Logger.Error("Unable to fetch poster image", exception);
                    SelectedCoverArt = null;
                })
                .Succeed(delegate
                {
                    SelectedCoverArt = coverArt;
                })
                .Build()
                .Start()
                ;
        }

        private void linkLabelSearch_Click(object sender, EventArgs e)
        {
            if (Search != null)
                Search();
        }

        private bool SelectedMovieHasValidUrl
        {
            get { return !string.IsNullOrWhiteSpace(SelectedMovieUrl); }
        }

        private string SelectedMovieUrl
        {
            get
            {
                if (_job != null && _job.SelectedReleaseMedium != null)
                    return _job.SelectedReleaseMedium.Url;
                return null;
            }
        }

        private void pictureBox_Click(object sender, EventArgs e)
        {
            if (SelectedMovieHasValidUrl)
            {
                Process.Start(_job.SelectedReleaseMedium.Url);
            }
        }
    }
}
