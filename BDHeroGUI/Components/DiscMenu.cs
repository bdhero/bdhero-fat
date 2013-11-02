using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using BDHero.Utils;
using DotNetUtils.Forms;
using DotNetUtils.TaskUtils;
using OSUtils.DriveDetector;
using Timer = System.Windows.Forms.Timer;

namespace BDHeroGUI.Components
{
    [DefaultProperty("NoDiscText")]
    [DefaultEvent("DiscSelected")]
    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.StatusStrip)]
    [ToolboxItem(true)]
    public partial class DiscMenu : ToolStripMenuItem
    {
        #region Default text values

        private const string DefaultNoDiscText = "No discs found";
        private const string DefaultScanningText = "Scanning for discs...";

        #endregion

        #region Public members

        [Category("Appearance")]
        [Description("The text that is displayed when no discs are found.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Localizable(true)]
        [DefaultValue(DefaultNoDiscText)]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public string NoDiscText
        {
            get { return _noDiscItem.Text; }
            set { _noDiscItem.Text = value; }
        }

        [Category("Appearance")]
        [Description("The text that is displayed while the menu is scanning for discs.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Localizable(true)]
        [DefaultValue(DefaultScanningText)]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public string ScanningText
        {
            get { return _scanningItem.Text; }
            set { _scanningItem.Text = value; }
        }

        [Description("Invoked whenever a disc is selected (clicked on) by the user.")]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public event DiscMenuItemClickHandler DiscSelected;

        #endregion

        private static DriveInfo[] Drives
        {
            get
            {
                return DriveInfo.GetDrives().Where(BDFileUtils.IsBDROM).ToArray();
            }
        }

        private readonly ToolStripMenuItem _dummyItem = new ToolStripMenuItem("DUMMY") { Enabled = false };
        private readonly ToolStripMenuItem _noDiscItem = new ToolStripMenuItem(DefaultNoDiscText) { Enabled = false };
        private readonly ToolStripMenuItem _scanningItem = new ToolStripMenuItem(DefaultScanningText) { Enabled = false };

        private IDriveDetector _detector;

        private bool _initialized;
        public DiscMenu()
        {
            InitializeComponent();
        }

        public DiscMenu(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        /// <summary>
        /// Initializes the <see cref="BDHeroGUI.Components.DiscMenu"/> for use.
        /// </summary>
        /// <param name="observable">
        /// Windows Forms control (typically a <see cref="Form"/>) to listen for <see cref="Form.WndProc"/> events on.
        /// </param>
        /// <param name="detector">
        /// Drive detector.
        /// </param>
        /// <exception cref="InvalidOperationException">Thrown if this method is called more than once.</exception>
        public void Initialize(IWndProcObservable observable, IDriveDetector detector)
        {
            if (_initialized)
            {
                throw new InvalidOperationException("DiscMenu has already been initialized");
            }

            observable.WndProcMessage += WndProc;

            // TODO: Handle exceptions
            _detector = detector;
            _detector.DeviceArrived += OnDeviceArrived;
            _detector.DeviceRemoved += OnDeviceRemoved;

            DropDownOpened += OnDropDownOpened;
            DropDownClosed += OnDropDownClosed;

            ResetMenu();

            _initialized = true;
        }

        #region Event handlers

        private void OnDeviceArrived(object sender, DriveDetectorEventArgs driveDetectorEventArgs)
        {
            ResetMenu();
        }

        private void OnDeviceRemoved(object sender, DriveDetectorEventArgs driveDetectorEventArgs)
        {
            ResetMenu();
        }

        private void OnDropDownOpened(object sender, EventArgs eventArgs)
        {
            ResetMenu();
        }

        private void OnDropDownClosed(object sender, EventArgs eventArgs)
        {
        }

        private void WndProc(ref Message m)
        {
            _detector.WndProc(ref m);
        }

        private void MenuItemOnClick(object sender, EventArgs eventArgs)
        {
            var menuItem = sender as ToolStripMenuItem;
            if (menuItem == null) return;

            var driveInfo = menuItem.Tag as DriveInfo;
            if (driveInfo == null) return;

            if (DiscSelected != null)
            {
                DiscSelected(driveInfo);
            }
        }

        #endregion

        private void ResetMenu()
        {
            ClearMenu();
            PopulateMenuAsync();
        }

        private void ClearMenu()
        {
            // We need to always keep at least 1 menu item in the dropdown list
            // to prevent the list from being positioned in the upper-left corner
            // of the screen.
            DropDownItems.Add(_dummyItem);

            var dummyItems = new[] { _dummyItem };

            // Special menu items that should NOT be destroyed
            var specialItems = new[] { _noDiscItem, _scanningItem };

            // ALL menu items present in the dropdown list
            var menuItems = DropDownItems.OfType<ToolStripMenuItem>().ToArray();

            // Disc Drive menu items
            var destroyableItems = menuItems.Except(specialItems).Except(dummyItems).ToArray();

            // Special menu items (only those actually present in the dropdown list)
            var removableItems = menuItems.Intersect(specialItems).ToArray();

            foreach (var menuItem in destroyableItems)
            {
                DestroyMenuItem(menuItem);
                DropDownItems.Remove(menuItem);
            }

            foreach (var menuItem in removableItems)
            {
                DropDownItems.Remove(menuItem);
            }
        }

        private void PopulateMenuAsync()
        {
            DropDownItems.Add(_scanningItem);
            DropDownItems.Remove(_dummyItem);

            var drives = new DriveInfo[0];

            new TaskBuilder()
                .OnCurrentThread()
                .DoWork(delegate(IThreadInvoker invoker, CancellationToken token)
                    {
                        drives = Drives;
                    })
                .Succeed(delegate
                    {
                        ClearMenu();
                        PopulateMenuSync(drives);
                    })
                .Build()
                .Start();
        }

        private void PopulateMenuSync(DriveInfo[] drives)
        {
            DropDownItems.AddRange(drives.Select(CreateMenuItem).ToArray());

            var menuItems = DropDownItems.OfType<ToolStripMenuItem>().Except(new [] { _dummyItem }).ToArray();

            if (!menuItems.Any())
            {
                DropDownItems.Add(_noDiscItem);
            }

            // We need to always keep at least 1 menu item in the dropdown list
            // to prevent the list from being positioned in the upper-left corner
            // of the screen.
            DropDownItems.Remove(_dummyItem);
        }

        private ToolStripItem CreateMenuItem(DriveInfo driveInfo)
        {
            var driveLetter = driveInfo.Name;
            var text = string.Format("{0} {1}", driveLetter, driveInfo.VolumeLabel);
            var menuItem = new ToolStripMenuItem(text) { Tag = driveInfo };
            menuItem.Click += MenuItemOnClick;
            return menuItem;
        }

        private void DestroyMenuItem(ToolStripItem menuItem)
        {
            menuItem.Tag = null;
            menuItem.Click -= MenuItemOnClick;
        }
    }

    public delegate void DiscMenuItemClickHandler(DriveInfo driveInfo);
}
