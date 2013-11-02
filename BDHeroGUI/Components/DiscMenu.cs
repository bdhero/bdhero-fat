using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using BDHero.Utils;
using DotNetUtils.Annotations;
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
        private readonly ToolStripSeparator _dividerItem = new ToolStripSeparator();

        private IDriveDetector _detector;

        private bool _isScanning;

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
            var specialItems = new ToolStripItem[] { _noDiscItem, _scanningItem, _dividerItem };

            // ALL menu items present in the dropdown list
            var allMenuItems = DropDownItems.OfType<ToolStripItem>().ToArray();

            // Disc Drive menu items
            var destroyableItems = allMenuItems.Except(specialItems).Except(dummyItems).ToArray();

            foreach (var menuItem in destroyableItems)
            {
                DestroyMenuItem(menuItem);
                DropDownItems.Remove(menuItem);
            }

            foreach (var menuItem in specialItems)
            {
                DropDownItems.Remove(menuItem);
            }
        }

        private void PopulateMenuAsync()
        {
            if (_isScanning) return;

            _isScanning = true;

            DropDownItems.Add(_dividerItem);
            DropDownItems.Add(_scanningItem);

            var menuItems = new ToolStripItem[0];

            new TaskBuilder()
                .OnCurrentThread()
                .DoWork((invoker, token) => menuItems = CreateToolStripItems(Drives))
                .Succeed(delegate
                    {
                        ClearMenu();
                        PopulateMenuSync(menuItems);
                    })
                .Finally(() => _isScanning = false)
                .Build()
                .Start();
        }

        private ToolStripItem[] CreateToolStripItems(DriveInfo[] drives)
        {
            return drives.Select(TryCreateMenuItem).Where(item => item != null).ToArray();
        }

        private void PopulateMenuSync(ToolStripItem[] items)
        {
            DropDownItems.AddRange(items);

            var menuItems = DropDownItems.OfType<ToolStripItem>().Except(new[] { _dummyItem }).ToArray();

            if (!menuItems.Any())
            {
                DropDownItems.Add(_noDiscItem);
            }

            // We need to always keep at least 1 menu item in the dropdown list
            // to prevent the list from being positioned in the upper-left corner
            // of the screen.
            DropDownItems.Remove(_dummyItem);
        }

        [CanBeNull]
        private ToolStripItem TryCreateMenuItem(DriveInfo driveInfo)
        {
            try { return CreateMenuItem(driveInfo); }
            catch { return null; }
        }

        /// <exception cref="IOException">An I/O error occurred (for example, a disk error or a drive was not ready).</exception>
        /// <exception cref="DriveNotFoundException">The drive is not mapped or does not exist.</exception>
        /// <exception cref="SecurityException">The caller does not have the required permission.</exception>
        /// <exception cref="UnauthorizedAccessException">The volume label is being set on a network or CD-ROM drive.-or-Access to the drive information is denied.</exception>
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
