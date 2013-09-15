using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms; // required for Message
using System.Runtime.InteropServices; // required for Marshal
using System.IO;
using Microsoft.Win32.SafeHandles;
using OSUtils.DriveDetector;

// DriveDetector - rev. 1, Oct. 31 2007
// http://www.codeproject.com/Articles/18062/Detecting-USB-Drive-Removal-in-a-C-Program

namespace WindowsOSUtils.DriveDetector
{
    /// <summary>
    /// Detects insertion or removal of removable drives.
    /// Use it in 1 or 2 steps:
    /// 1) Create instance of this class in your project and add handlers for the
    /// DeviceArrived, DeviceRemoved and QueryRemove events.
    /// AND (if you do not want drive detector to creaate a hidden form))
    /// 2) Override WndProc in your form and call DriveDetector's WndProc from there. 
    /// If you do not want to do step 2, just use the DriveDetector constructor without arguments and
    /// it will create its own invisible form to receive messages from Windows.
    /// </summary>
    public class DriveDetector : IDriveDetector, IDisposable
    {
        public event DriveDetectorEventHandler DeviceArrived;
        public event DriveDetectorEventHandler DeviceRemoved;
        public event DriveDetectorEventHandler QueryRemove;

        /// <summary>
        /// Pass in your Form and DriveDetector will not create a hidden form.
        /// </summary>
        /// <param name="control">
        /// Object which will receive Windows messages.
        /// Pass "this" as this argument from your form class.
        /// </param>
        public DriveDetector(Control control)
        {
            Init(control, null);
        }

        /// <summary>
        /// init the DriveDetector object setting also path to file which should be opened
        /// when registering for query remove.
        /// </summary>
        /// <param name="control">
        /// object which will receive Windows messages.
        /// Pass "this" as this argument from your form class.
        /// </param>
        /// <param name="fileToOpen">
        /// Optional. Name of a file on the removable drive which should be opened.
        /// If null, root directory of the drive will be opened. Opening a file is needed for us
        /// to be able to register for the query remove message. TIP: For files use relative path without drive letter.
        /// e.g. "SomeFolder\file_on_flash.txt"
        /// </param>
        private void Init(Control control, string fileToOpen)
        {
            _mFileToOpen = fileToOpen;
            _mFileOnFlash = null;
            _mDeviceNotifyHandle = IntPtr.Zero;
            _mRecipientHandle = control.Handle;
            _mDirHandle = IntPtr.Zero; // handle to the root directory of the flash drive which we open
            _mCurrentDrive = "";
        }

        public bool IsQueryHooked
        {
            get
            {
                return _mDeviceNotifyHandle != IntPtr.Zero;
            }
        }

        public string HookedDrive
        {
            get { return _mCurrentDrive; }
        }

        public FileStream OpenedFile
        {
            get { return _mFileOnFlash; }
        }

        public bool EnableQueryRemove(string fileOnDrive)
        {
            if (string.IsNullOrEmpty(fileOnDrive))
                throw new ArgumentException("Drive path must be supplied to register for Query remove.");

            if (fileOnDrive.Length == 2 && fileOnDrive[1] == ':')
                fileOnDrive += '\\'; // append "\\" if only drive letter with ":" was passed in.

            if (_mDeviceNotifyHandle != IntPtr.Zero)
            {
                // Unregister first...
                RegisterForDeviceChange(false, null);
            }

            var fileName = Path.GetFileName(fileOnDrive);
            if (fileName != null && (fileName.Length == 0 || !File.Exists(fileOnDrive)))
                _mFileToOpen = null; // use root directory...
            else
                _mFileToOpen = fileOnDrive;

            RegisterQuery(Path.GetPathRoot(fileOnDrive));

            return _mDeviceNotifyHandle != IntPtr.Zero;
        }

        public void DisableQueryRemove()
        {
            if (_mDeviceNotifyHandle != IntPtr.Zero)
            {
                RegisterForDeviceChange(false, null);
            }
        }

        /// <summary>
        /// Unregister and close the file we may have opened on the removable drive. 
        /// Garbage collector will call this method.
        /// </summary>
        public void Dispose()
        {
            RegisterForDeviceChange(false, null);
        }

        #region WindowProc

        /// <summary>
        /// Message handler which must be called from client form.
        /// Processes Windows messages and calls event handlers. 
        /// </summary>
        /// <param name="m"></param>
        public void WndProc(ref Message m)
        {
            if (m.Msg != WM_DEVICECHANGE) return;

            int devType;
            char c;

            // WM_DEVICECHANGE can have several meanings depending on the WParam value...
            switch (m.WParam.ToInt32())
            {
                    //
                    // New device has just arrived
                    //
                case DBT_DEVICEARRIVAL:

                    devType = Marshal.ReadInt32(m.LParam, 4);
                    if (devType == DBT_DEVTYP_VOLUME)
                    {
                        DEV_BROADCAST_VOLUME vol = (DEV_BROADCAST_VOLUME)
                                                   Marshal.PtrToStructure(m.LParam, typeof (DEV_BROADCAST_VOLUME));

                        // Get the drive letter
                        c = DriveMaskToLetter(vol.dbcv_unitmask);

                        //
                        // Call the client event handler
                        //
                        // We should create copy of the event before testing it and
                        // calling the delegate - if any
                        DriveDetectorEventHandler tempDeviceArrived = DeviceArrived;
                        if (tempDeviceArrived != null)
                        {
                            DriveDetectorEventArgs e = new DriveDetectorEventArgs();
                            var drivePath = c + @":\";
                            e.DriveInfo = DriveInfo.GetDrives().FirstOrDefault(info => info.Name == drivePath);
                            tempDeviceArrived(this, e);

                            // Register for query remove if requested
                            if (e.HookQueryRemove)
                            {
                                // If something is already hooked, unhook it now
                                if (_mDeviceNotifyHandle != IntPtr.Zero)
                                {
                                    RegisterForDeviceChange(false, null);
                                }

                                RegisterQuery(c + @":\");
                            }
                        } // if  has event handler
                    }
                    break;

                    //
                    // Device is about to be removed
                    // Any application can cancel the removal
                    //
                case DBT_DEVICEQUERYREMOVE:

                    devType = Marshal.ReadInt32(m.LParam, 4);
                    if (devType == DBT_DEVTYP_HANDLE)
                    {
                        // TODO: we could get the handle for which this message is sent
                        // from vol.dbch_handle and compare it against a list of handles for
                        // which we have registered the query remove message (?)
                        //DEV_BROADCAST_HANDLE vol;
                        //vol = (DEV_BROADCAST_HANDLE)
                        //   Marshal.PtrToStructure(m.LParam, typeof(DEV_BROADCAST_HANDLE));
                        // if ( vol.dbch_handle ....

                        //
                        // Call the event handler in client
                        //
                        DriveDetectorEventHandler tempQuery = QueryRemove;
                        if (tempQuery != null)
                        {
                            DriveDetectorEventArgs e = new DriveDetectorEventArgs();
                            var drivePath = _mCurrentDrive; // drive which is hooked
                            e.DriveInfo = DriveInfo.GetDrives().FirstOrDefault(info => info.Name == drivePath);
                            tempQuery(this, e);

                            // If the client wants to cancel, let Windows know
                            if (e.Cancel)
                            {
                                m.Result = (IntPtr) BROADCAST_QUERY_DENY;
                            }
                            else
                            {
                                // Change 28.10.2007: Unregister the notification, this will
                                // close the handle to file or root directory also.
                                // We have to close it anyway to allow the removal so
                                // even if some other app cancels the removal we would not know about it...
                                RegisterForDeviceChange(false, null); // will also close the mFileOnFlash
                            }
                        }
                    }
                    break;

                    //
                    // Device has been removed
                    //
                case DBT_DEVICEREMOVECOMPLETE:

                    devType = Marshal.ReadInt32(m.LParam, 4);
                    if (devType == DBT_DEVTYP_VOLUME)
                    {
                        devType = Marshal.ReadInt32(m.LParam, 4);
                        if (devType == DBT_DEVTYP_VOLUME)
                        {
                            DEV_BROADCAST_VOLUME vol = (DEV_BROADCAST_VOLUME)
                                                       Marshal.PtrToStructure(m.LParam, typeof (DEV_BROADCAST_VOLUME));
                            c = DriveMaskToLetter(vol.dbcv_unitmask);

                            //
                            // Call the client event handler
                            //
                            DriveDetectorEventHandler tempDeviceRemoved = DeviceRemoved;
                            if (tempDeviceRemoved != null)
                            {
                                DriveDetectorEventArgs e = new DriveDetectorEventArgs();
                                var drivePath = c + @":\";
                                e.DriveInfo = DriveInfo.GetDrives().FirstOrDefault(info => info.Name == drivePath);
                                tempDeviceRemoved(this, e);
                            }

                            // TODO: we could unregister the notify handle here if we knew it is the
                            // right drive which has been just removed
                            //RegisterForDeviceChange(false, null);
                        }
                    }
                    break;
            }
        }

        #endregion

        #region Private Area

        /// <summary>
        /// New: 28.10.2007 - handle to root directory of flash drive which is opened
        /// for device notification
        /// </summary>
        private IntPtr _mDirHandle = IntPtr.Zero;

        /// <summary>
        /// Class which contains also handle to the file opened on the flash drive
        /// </summary>
        private FileStream _mFileOnFlash;

        /// <summary>
        /// Name of the file to try to open on the removable drive for query remove registration
        /// </summary>
        private string _mFileToOpen;

        /// <summary>
        /// Handle to file which we keep opened on the drive if query remove message is required by the client
        /// </summary>       
        private IntPtr _mDeviceNotifyHandle;

        /// <summary>
        /// Handle of the window which receives messages from Windows. This will be a form.
        /// </summary>
        private IntPtr _mRecipientHandle;

        /// <summary>
        /// Drive which is currently hooked for query remove
        /// </summary>
        private string _mCurrentDrive;

        // Win32 constants
// ReSharper disable InconsistentNaming
        private const int DBT_DEVTYP_DEVICEINTERFACE = 5;
        private const int DBT_DEVTYP_HANDLE = 6;
        private const int BROADCAST_QUERY_DENY = 0x424D5144;
        private const int WM_DEVICECHANGE = 0x0219;
        private const int DBT_DEVICEARRIVAL = 0x8000; // system detected a new device
        private const int DBT_DEVICEQUERYREMOVE = 0x8001; // Preparing to remove (any program can disable the removal)
        private const int DBT_DEVICEREMOVECOMPLETE = 0x8004; // removed 
        private const int DBT_DEVTYP_VOLUME = 0x00000002; // drive type is logical volume
// ReSharper restore InconsistentNaming

        /// <summary>
        /// Registers for receiving the query remove message for a given drive.
        /// We need to open a handle on that drive and register with this handle. 
        /// Client can specify this file in mFileToOpen or we will open root directory of the drive
        /// </summary>
        /// <param name="drive">drive for which to register.</param>
        private void RegisterQuery(string drive)
        {
            bool register = true;

            if (_mFileToOpen == null)
            {
                // Change 28.10.2007 - Open the root directory if no file specified - leave mFileToOpen null 
                // If client gave us no file, let's pick one on the drive... 
                //mFileToOpen = GetAnyFile(drive);
                //if (mFileToOpen.Length == 0)
                //    return;     // no file found on the flash drive                
            }
            else
            {
                // Make sure the path in mFileToOpen contains valid drive
                // If there is a drive letter in the path, it may be different from the actual
                // letter assigned to the drive now.  We will cut it off and merge the actual drive
                // with the rest of the path.
                if (_mFileToOpen.Contains(":"))
                {
                    string tmp = _mFileToOpen.Substring(3);
                    string root = Path.GetPathRoot(drive);
                    if (root != null)
                        _mFileToOpen = Path.Combine(root, tmp);
                }
                else
                    _mFileToOpen = Path.Combine(drive, _mFileToOpen);
            }

            try
            {
                //mFileOnFlash = new FileStream(mFileToOpen, FileMode.Open);
                // Change 28.10.2007 - Open the root directory 
                if (_mFileToOpen == null) // open root directory
                    _mFileOnFlash = null;
                else
                    _mFileOnFlash = new FileStream(_mFileToOpen, FileMode.Open);
            }
            catch (Exception)
            {
                // just do not register if the file could not be opened
                register = false;
            }

            if (register)
            {
                //RegisterForDeviceChange(true, mFileOnFlash.SafeFileHandle);
                //mCurrentDrive = drive;
                // Change 28.10.2007 - Open the root directory 
                if (_mFileOnFlash == null)
                    RegisterForDeviceChange(drive);
                else
                    // old version
                    RegisterForDeviceChange(true, _mFileOnFlash.SafeFileHandle);

                _mCurrentDrive = drive;
            }
        }

        /// <summary>
        /// New version which gets the handle automatically for specified directory
        /// Only for registering! Unregister with the old version of this function...
        /// </summary>
        /// <param name="dirPath">e.g. C:\dir</param>
        private void RegisterForDeviceChange(string dirPath)
        {
            IntPtr handle = Native.OpenDirectory(dirPath);
            if (handle == IntPtr.Zero)
            {
                _mDeviceNotifyHandle = IntPtr.Zero;
                return;
            }
            _mDirHandle = handle; // save handle for closing it when unregistering

            // Register for handle
            DEV_BROADCAST_HANDLE data = new DEV_BROADCAST_HANDLE();
            data.dbch_devicetype = DBT_DEVTYP_HANDLE;
            data.dbch_reserved = 0;
            data.dbch_nameoffset = 0;
            //data.dbch_data = null;
            //data.dbch_eventguid = 0;
            data.dbch_handle = handle;
            data.dbch_hdevnotify = (IntPtr) 0;
            int size = Marshal.SizeOf(data);
            data.dbch_size = size;
            IntPtr buffer = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(data, buffer, true);

            _mDeviceNotifyHandle = Native.RegisterDeviceNotification(_mRecipientHandle, buffer, 0);
        }

        /// <summary>
        /// Registers to be notified when the volume is about to be removed
        /// This is requierd if you want to get the QUERY REMOVE messages
        /// </summary>
        /// <param name="register">true to register, false to unregister</param>
        /// <param name="fileHandle">handle of a file opened on the removable drive</param>
        private void RegisterForDeviceChange(bool register, SafeFileHandle fileHandle)
        {
            if (register)
            {
                // Register for handle
                DEV_BROADCAST_HANDLE data = new DEV_BROADCAST_HANDLE
                    {
                        dbch_devicetype = DBT_DEVTYP_HANDLE,
                        dbch_reserved = 0,
                        dbch_nameoffset = 0,
                        //dbch_data = null,
                        //dbch_eventguid = 0,
                        dbch_handle = fileHandle.DangerousGetHandle(),
                        dbch_hdevnotify = (IntPtr) 0
                    };
                int size = Marshal.SizeOf(data);
                data.dbch_size = size;
                IntPtr buffer = Marshal.AllocHGlobal(size);
                Marshal.StructureToPtr(data, buffer, true);

                _mDeviceNotifyHandle = Native.RegisterDeviceNotification(_mRecipientHandle, buffer, 0);
            }
            else
            {
                // close the directory handle
                if (_mDirHandle != IntPtr.Zero)
                {
                    Native.CloseDirectoryHandle(_mDirHandle);
                    //    string er = Marshal.GetLastWin32Error().ToString();
                }

                // unregister
                if (_mDeviceNotifyHandle != IntPtr.Zero)
                {
                    Native.UnregisterDeviceNotification(_mDeviceNotifyHandle);
                }

                _mDeviceNotifyHandle = IntPtr.Zero;
                _mDirHandle = IntPtr.Zero;

                _mCurrentDrive = "";
                if (_mFileOnFlash != null)
                {
                    _mFileOnFlash.Close();
                    _mFileOnFlash = null;
                }
            }
        }

        /// <summary>
        /// Gets drive letter from a bit mask where bit 0 = A, bit 1 = B etc.
        /// There can actually be more than one drive in the mask but we 
        /// just use the last one in this case.
        /// </summary>
        /// <param name="mask"></param>
        /// <returns></returns>
        private static char DriveMaskToLetter(int mask)
        {
            const string drives = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            // 1 = A
            // 2 = B
            // 4 = C...
            int cnt = 0;
            int pom = mask/2;
            while (pom != 0)
            {
                // while there is any bit set in the mask
                // shift it to the righ...                
                pom = pom/2;
                cnt++;
            }

            return cnt < drives.Length ? drives[cnt] : '?';
        }

        #endregion

        #region Native Win32 API

        /// <summary>
        /// WinAPI functions
        /// </summary>        
        private static class Native
        {
            /// <summary>
            /// HDEVNOTIFY RegisterDeviceNotification(HANDLE hRecipient, LPVOID NotificationFilter, DWORD Flags);
            /// </summary>
            /// <param name="recipient">Handle</param>
            /// <param name="notificationFilter"></param>
            /// <param name="flags"></param>
            /// <returns></returns>
            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            public static extern IntPtr RegisterDeviceNotification(IntPtr recipient, IntPtr notificationFilter, uint flags);

            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            public static extern uint UnregisterDeviceNotification(IntPtr hHandle);

// ReSharper disable InconsistentNaming

            // CreateFile  - MSDN
            private const uint GENERIC_READ = 0x80000000;
            private const uint OPEN_EXISTING = 3;
            private const uint FILE_SHARE_READ = 0x00000001;
            private const uint FILE_SHARE_WRITE = 0x00000002;
            private const uint FILE_ATTRIBUTE_NORMAL = 128;
            private const uint FILE_FLAG_BACKUP_SEMANTICS = 0x02000000;
            private static readonly IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);

// ReSharper restore InconsistentNaming

            // should be "static extern unsafe"
            [DllImport("kernel32", SetLastError = true)]
            private static extern IntPtr CreateFile(
                string fileName, // file name
                uint desiredAccess, // access mode
                uint shareMode, // share mode
                uint securityAttributes, // Security Attributes
                uint creationDisposition, // how to create
                uint flagsAndAttributes, // file attributes
                int templateFile // handle to template file
                );

            [DllImport("kernel32", SetLastError = true)]
            private static extern bool CloseHandle(
                IntPtr hObject // handle to object
                );

            /// <summary>
            /// Opens a directory, returns it's handle or zero.
            /// </summary>
            /// <param name="dirPath">path to the directory, e.g. "C:\dir"</param>
            /// <returns>handle to the directory. Close it with CloseHandle().</returns>
            public static IntPtr OpenDirectory(string dirPath)
            {
                // open the existing file for reading          
                var handle = CreateFile(
                    dirPath,
                    GENERIC_READ,
                    FILE_SHARE_READ | FILE_SHARE_WRITE,
                    0,
                    OPEN_EXISTING,
                    FILE_FLAG_BACKUP_SEMANTICS | FILE_ATTRIBUTE_NORMAL,
                    0);

                return handle == INVALID_HANDLE_VALUE ? IntPtr.Zero : handle;
            }

            public static bool CloseDirectoryHandle(IntPtr handle)
            {
                return CloseHandle(handle);
            }
        }

// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Local
// ReSharper disable FieldCanBeMadeReadOnly.Local

        // Structure with information for RegisterDeviceNotification.
        [StructLayout(LayoutKind.Sequential)]
        private struct DEV_BROADCAST_HANDLE
        {
            public int dbch_size;
            public int dbch_devicetype;
            public int dbch_reserved;
            public IntPtr dbch_handle;
            public IntPtr dbch_hdevnotify;
            public Guid dbch_eventguid;
            public long dbch_nameoffset;
            //public byte[] dbch_data[1]; // = new byte[1];
            public byte dbch_data;
            public byte dbch_data1;
        }

        // Struct for parameters of the WM_DEVICECHANGE message
        [StructLayout(LayoutKind.Sequential)]
        private struct DEV_BROADCAST_VOLUME
        {
            public int dbcv_size;
            public int dbcv_devicetype;
            public int dbcv_reserved;
            public int dbcv_unitmask;
        }

// ReSharper restore FieldCanBeMadeReadOnly.Local
// ReSharper restore MemberCanBePrivate.Local
// ReSharper restore InconsistentNaming

        #endregion
    }
}