using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace ProcessUtils
{
    /// <summary>
    /// Represents a Windows Job Object that groups two or more processes together.
    /// </summary>
    /// <remarks>
    /// A job object allows groups of processes to be managed as a unit.
    /// Job objects are namable, securable, sharable objects that control attributes
    /// of the processes associated with them. Operations performed on a job object
    /// affect all processes associated with the job object. Examples include
    /// enforcing limits such as working set size and process priority or terminating
    /// all processes associated with a job.
    /// </remarks>
    /// <see cref="http://skolima.blogspot.com/2012/09/handling-native-api-in-managed.html"/>
    public sealed class JobObject2 : IDisposable
    {
        private readonly IntPtr _jobObjectHandle;
        private bool _disposed;

        private JobObject2(IntPtr jobObjectHandle)
        {
            _jobObjectHandle = jobObjectHandle;
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;
            CloseHandleCheckingResult(_jobObjectHandle);
        }

        #endregion

        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern IntPtr CreateJobObject(IntPtr lpJobAttributes, string lpName);

        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern bool AssignProcessToJobObject(IntPtr hJob, IntPtr hProcess);

        [DllImport("kernel32", SetLastError = true)]
        private static extern bool CloseHandle(IntPtr hJob);

        [DllImport("kernel32", SetLastError = true)]
        private static extern bool SetInformationJobObject(IntPtr hJob, JobObjectInfoClass jobObjectInfoClass,
                                                           [In] ref JobObjectExtendedLimitInformation lpJobObjectInfo,
                                                           uint cbJobObjectInfoLength);

        [DllImport("kernel32", SetLastError = true)]
        private static extern uint ResumeThread(IntPtr hThread);

        [DllImport("kernel32", SetLastError = true)]
        private static extern bool IsProcessInJob(IntPtr processHandle, IntPtr jobHandle, out bool result);

        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern bool CreateProcess(string lpApplicationName, string lpCommandLine,
                                                 IntPtr lpProcessAttributes, IntPtr lpThreadAttributes,
                                                 bool bInheritHandles, ProcessCreationFlags dwCreationFlags,
                                                 IntPtr lpEnvironment,
                                                 string lpCurrentDirectory, ref StartupInfo lpStartupInfo,
                                                 out ProcessInformation lpProcessInformation);

        public static JobObject2 Create()
        {
            IntPtr result = CreateJobObject(IntPtr.Zero, null);
            if (result == IntPtr.Zero)
                throw new Win32Exception();

            return new JobObject2(result);
        }

        public void AssignCurrentProcess()
        {
            using (Process process = Process.GetCurrentProcess())
                AssignProcess(process);
        }

        public void AssignProcess(Process process)
        {
            if (process == null)
                throw new ArgumentNullException("process");
            AssignProcess(process.Handle);
        }

        private void CloseHandleCheckingResult(IntPtr handle)
        {
            bool result = CloseHandle(handle);
            if (!result)
                throw new Win32Exception();
        }

        private void AssignProcess(IntPtr processHandle)
        {
            if (IsProcessInJobCheckingResult(processHandle, _jobObjectHandle))
                return;

            if (IsProcessInJobCheckingResult(processHandle, IntPtr.Zero))
                throw new InvalidOperationException(
                    "Requested process already belongs to another job group. Check http://stackoverflow.com/a/4232259/3205 for help.");

            bool result = AssignProcessToJobObject(_jobObjectHandle, processHandle);
            if (!result)
                throw new Win32Exception();
        }

        private bool IsProcessInJobCheckingResult(IntPtr processHandle, IntPtr jobObjectHandle)
        {
            bool status;
            bool result = IsProcessInJob(processHandle, jobObjectHandle, out status);
            if (!result)
                throw new Win32Exception();
            return status;
        }

        public void StartProcessInJob(ProcessStartInfo processStartInfo)
        {
            var startInfo = new StartupInfo();
            ProcessInformation processInfo;
            // this doesn't seem to set Environment.CurrentDirectory, workaround needed
            string workingDirectory = string.IsNullOrEmpty(processStartInfo.WorkingDirectory)
                                          ? null
                                          : processStartInfo.WorkingDirectory;
            bool result = CreateProcess(processStartInfo.FileName, ' ' + processStartInfo.Arguments, IntPtr.Zero,
                                        IntPtr.Zero,
                                        false, ProcessCreationFlags.CreateSuspended, IntPtr.Zero,
                                        workingDirectory, ref startInfo, out processInfo);
            if (!result)
                throw new Win32Exception();

            AssignProcess(processInfo.hProcess);

            uint status = ResumeThread(processInfo.hThread);
            if (status != 0 && status != 1)
                throw new Win32Exception();

            CloseHandleCheckingResult(processInfo.hProcess);
            CloseHandleCheckingResult(processInfo.hThread);
        }

        public void SetKillOnClose()
        {
            var limit = new JobObjectExtendedLimitInformation
            {
                BasicLimitInformation =
                    new JobObjectBasicLimitInformation { LimitFlags = LimitFlags.JobObjectLimitKillOnJobClose }
            };
            bool result = SetInformationJobObject(
                _jobObjectHandle, JobObjectInfoClass.ExtendedLimitInformation,
                ref limit,
                (uint)Marshal.SizeOf(typeof(JobObjectExtendedLimitInformation)));
            if (!result)
                throw new Win32Exception();
        }

        #region Nested type: IoCounters

        [StructLayout(LayoutKind.Sequential)]
        private struct IoCounters
        {
            public readonly UInt64 ReadOperationCount;
            public readonly UInt64 WriteOperationCount;
            public readonly UInt64 OtherOperationCount;
            public readonly UInt64 ReadTransferCount;
            public readonly UInt64 WriteTransferCount;
            public readonly UInt64 OtherTransferCount;
        }

        #endregion

        #region Nested type: JobObjectBasicLimitInformation

        [StructLayout(LayoutKind.Sequential)]
        private struct JobObjectBasicLimitInformation
        {
            public readonly Int64 PerProcessUserTimeLimit;
            public readonly Int64 PerJobUserTimeLimit;
            public LimitFlags LimitFlags;
            public readonly UIntPtr MinimumWorkingSetSize;
            public readonly UIntPtr MaximumWorkingSetSize;
            public readonly Int16 ActiveProcessLimit;
            public readonly Int64 Affinity;
            public readonly Int16 PriorityClass;
            public readonly Int16 SchedulingClass;
        }

        #endregion

        #region Nested type: JobObjectExtendedLimitInformation

        [StructLayout(LayoutKind.Sequential)]
        private struct JobObjectExtendedLimitInformation
        {
            public JobObjectBasicLimitInformation BasicLimitInformation;
            public readonly IoCounters IoInfo;
            public readonly UIntPtr ProcessMemoryLimit;
            public readonly UIntPtr JobMemoryLimit;
            public readonly UIntPtr PeakProcessMemoryUsed;
            public readonly UIntPtr PeakJobMemoryUsed;
        }

        #endregion

        #region Nested type: JobObjectInfoClass

        private enum JobObjectInfoClass
        {
            ExtendedLimitInformation = 9
        }

        #endregion

        #region Nested type: LimitFlags

        [Flags]
        private enum LimitFlags : ushort
        {
            JobObjectLimitKillOnJobClose = 0x00002000
        }

        #endregion

        #region Nested type: ProcessCreationFlags

        [Flags]
        private enum ProcessCreationFlags : uint
        {
            CreateSuspended = 0x00000004
        }

        #endregion

        #region Nested type: ProcessInformation

        [StructLayout(LayoutKind.Sequential)]
        public struct ProcessInformation
        {
            public IntPtr hProcess;
            public IntPtr hThread;
            public uint dwProcessId;
            public uint dwThreadId;
        }

        #endregion

        #region Nested type: SecurityAttributes

        [StructLayout(LayoutKind.Sequential)]
        public struct SecurityAttributes
        {
            public int length;
            public IntPtr lpSecurityDescriptor;
            public bool bInheritHandle;
        }

        #endregion

        #region Nested type: StartupInfo

        [StructLayout(LayoutKind.Sequential)]
        public struct StartupInfo
        {
            public uint cb;
            public string lpReserved;
            public string lpDesktop;
            public string lpTitle;
            public uint dwX;
            public uint dwY;
            public uint dwXSize;
            public uint dwYSize;
            public uint dwXCountChars;
            public uint dwYCountChars;
            public uint dwFillAttribute;
            public uint dwFlags;
            public short wShowWindow;
            public short cbReserved2;
            public IntPtr lpReserved2;
            public IntPtr hStdInput;
            public IntPtr hStdOutput;
            public IntPtr hStdError;
        }

        #endregion
    }
}
