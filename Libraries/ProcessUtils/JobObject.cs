using System;
using System.Runtime.InteropServices;
using System.Diagnostics;

// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable InconsistentNaming
// TODO: See http://jobobjectwrapper.codeplex.com/ for a (potentially) better solution
namespace ProcessUtils
{
    /// <see cref="http://stackoverflow.com/a/538238/467582"/>
    /// <see cref="http://stackoverflow.com/a/9164742/467582"/>
    /// <see cref="http://stackoverflow.com/a/4657392/467582"/>
    public class JobObject : IDisposable
    {
        /// <summary>
        /// Causes all processes associated with the job to terminate when the last handle to the job is closed.
        /// This limit requires use of a JOBOBJECT_EXTENDED_LIMIT_INFORMATION structure. Its BasicLimitInformation member is a JOBOBJECT_BASIC_LIMIT_INFORMATION structure.
        /// </summary>
        /// <see cref="http://msdn.microsoft.com/en-us/library/windows/desktop/ms684147(v=vs.85).aspx"/>
        private const int JOB_OBJECT_LIMIT_KILL_ON_JOB_CLOSE = 0x00002000;

        private IntPtr _handle;
        private bool _disposed;

        public JobObject()
        {
            _handle = CreateJobObject(IntPtr.Zero, null);

            var extendedInfo = new JOBOBJECT_EXTENDED_LIMIT_INFORMATION
                {
                    BasicLimitInformation = new JOBOBJECT_BASIC_LIMIT_INFORMATION
                        {
                            LimitFlags = JOB_OBJECT_LIMIT_KILL_ON_JOB_CLOSE
                        }
                };

            int length = Marshal.SizeOf(typeof(JOBOBJECT_EXTENDED_LIMIT_INFORMATION));
            IntPtr extendedInfoPtr = Marshal.AllocHGlobal(length);
            Marshal.StructureToPtr(extendedInfo, extendedInfoPtr, false);

            if (!SetInformationJobObject(_handle, JobObjectInfoType.ExtendedLimitInformation, extendedInfoPtr, (uint)length))
                throw new Exception(string.Format("Unable to set information.  Error: {0}", Marshal.GetLastWin32Error()));
        }

        #region IDisposable implementation

        ~JobObject()
        {
            Dispose(false);
        }

        /// <see cref="http://stackoverflow.com/a/538238/467582"/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <see cref="http://stackoverflow.com/a/538238/467582"/>
        private void Dispose(bool freeManagedObjectsAlso)
        {
            if (_disposed)
                return;

            // Dispose managed objects (e.g., database connections, large images)
            if (freeManagedObjectsAlso) { }

            CloseHandle(_handle);

            _handle = IntPtr.Zero;
            _disposed = true;
        }

        #endregion

        public bool AddProcess(IntPtr processHandle)
        {
            return AssignProcessToJobObject(_handle, processHandle);
        }

        public bool AddProcess(int processId)
        {
            return AddProcess(Process.GetProcessById(processId).Handle);
        }

        public bool AddProcess(Process process)
        {
            var id = process.Id;
            return AddProcess(process.Handle);
        }

        #region Win32 DLL imports

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        static extern IntPtr CreateJobObject(IntPtr a, string lpName);

        [DllImport("kernel32.dll")]
        static extern bool SetInformationJobObject(IntPtr hJob, JobObjectInfoType infoType, IntPtr lpJobObjectInfo, UInt32 cbJobObjectInfoLength);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool AssignProcessToJobObject(IntPtr job, IntPtr process);

        /// <param name="hObject">handle to job object</param>
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool CloseHandle(IntPtr hObject);

        #endregion
    }

    #region Structs and Enums

    [StructLayout(LayoutKind.Sequential)]
    struct IO_COUNTERS
    {
        public UInt64 ReadOperationCount;
        public UInt64 WriteOperationCount;
        public UInt64 OtherOperationCount;
        public UInt64 ReadTransferCount;
        public UInt64 WriteTransferCount;
        public UInt64 OtherTransferCount;
    }


    [StructLayout(LayoutKind.Sequential)]
    struct JOBOBJECT_BASIC_LIMIT_INFORMATION
    {
        public Int64 PerProcessUserTimeLimit;
        public Int64 PerJobUserTimeLimit;
        public UInt32 LimitFlags;
        public UIntPtr MinimumWorkingSetSize;
        public UIntPtr MaximumWorkingSetSize;
        public UInt32 ActiveProcessLimit;
        public UIntPtr Affinity;
        public UInt32 PriorityClass;
        public UInt32 SchedulingClass;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SECURITY_ATTRIBUTES
    {
        public UInt32 nLength;
        public IntPtr lpSecurityDescriptor;
        public Int32 bInheritHandle;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct JOBOBJECT_EXTENDED_LIMIT_INFORMATION
    {
        public JOBOBJECT_BASIC_LIMIT_INFORMATION BasicLimitInformation;
        public IO_COUNTERS IoInfo;
        public UIntPtr ProcessMemoryLimit;
        public UIntPtr JobMemoryLimit;
        public UIntPtr PeakProcessMemoryUsed;
        public UIntPtr PeakJobMemoryUsed;
    }

    public enum JobObjectInfoType
    {
        AssociateCompletionPortInformation = 7,
        BasicLimitInformation = 2,
        BasicUIRestrictions = 4,
        EndOfJobTimeInformation = 6,
        ExtendedLimitInformation = 9,
        SecurityLimitInformation = 5,
        GroupInformation = 11
    }

    #endregion
}
