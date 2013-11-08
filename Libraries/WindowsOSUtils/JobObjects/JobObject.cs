using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using DotNetUtils.Annotations;
using OSUtils;
using OSUtils.JobObjects;

namespace WindowsOSUtils.JobObjects
{
    /// <summary>
    ///     Represents a Windows Job Object that groups two or more processes together.
    /// </summary>
    /// <remarks>
    ///     A job object allows groups of processes to be managed as a unit.
    ///     Job objects are namable, securable, sharable objects that control attributes
    ///     of the processes associated with them. Operations performed on a job object
    ///     affect all processes associated with the job object. Examples include
    ///     enforcing limits such as working set size and process priority or terminating
    ///     all processes associated with a job.
    /// </remarks>
    /// <see cref="http://skolima.blogspot.com/2012/09/handling-native-api-in-managed.html" />
    public sealed class JobObject : IJobObject
    {
        private readonly IntPtr _jobObjectHandle;

        private bool _disposed;

        /// <exception cref="Win32Exception">
        ///     Thrown if the operating system was unable to create a new Job Object.
        /// </exception>
        [UsedImplicitly]
        public JobObject()
        {
            _jobObjectHandle = PInvokeUtils.Try(() => WinAPI.CreateJobObject(IntPtr.Zero, null));
        }

        // TODO: Better IDisposable implementation
        #region IDisposable Members

        /// <exception cref="Win32Exception">Thrown if the handle to the Job Object could not be released.</exception>
        public void Dispose()
        {
            if (_disposed) { return; }

            _disposed = true;

            if (_jobObjectHandle == IntPtr.Zero) { return; }

            PInvokeUtils.Try(() => WinAPI.CloseHandle(_jobObjectHandle));
        }

        #endregion

        /// <summary>
        ///     Assigns the given process to this Job Object.
        /// </summary>
        /// <param name="process">Process to assign to this Job Object.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="process"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">
        ///     Thrown if <paramref name="process"/> already belongs to a Job Object.
        /// </exception>
        /// <exception cref="Win32Exception">
        ///     Thrown if the operating system was unable to assign <paramref name="process"/> to the Job Object.
        /// </exception>
        public void Assign(Process process)
        {
            if (process == null)
                throw new ArgumentNullException("process");

            if (AlreadyAssigned(process))
                return;

            if (HasJobObject(process))
                throw new InvalidOperationException(
                    "Requested process already belongs to another job group.  Check http://stackoverflow.com/a/4232259/3205 for help.");

            PInvokeUtils.Try(() => WinAPI.AssignProcessToJobObject(_jobObjectHandle, process.Handle));
        }

        // TODO: PICK AN IMPLEMENTATION
        public void KillOnClose()
        {
            var type = JobObjectInfoClass.ExtendedLimitInformation;

            var v1 = false;

            if (v1)
            {
                var limit = new JobObjectExtendedLimitInformation
                            {
                                BasicLimitInformation =
                                    new JobObjectBasicLimitInformation {LimitFlags = LimitFlags.LimitKillOnJobClose}
                            };

                var length = (uint) Marshal.SizeOf(typeof (JobObjectExtendedLimitInformation));

                PInvokeUtils.Try(() => SetInformationJobObject(_jobObjectHandle, type, ref limit, length));
            }
            else
            {
                var limit = CreateKillOnCloseJobObjectInfo();
                var length = GetKillOnCloseJobObjectInfoLength();

                PInvokeUtils.Try(() => WinAPI.SetInformationJobObject(_jobObjectHandle, type, ref limit, length));
            }
        }

        private static uint GetKillOnCloseJobObjectInfoLength()
        {
            var type = WinAPI.Is32Bit
                           ? typeof (ExtendedLimits32)
                           : typeof (ExtendedLimits64);
            return (uint) Marshal.SizeOf(type);
        }

        private static JobObjectInfo CreateKillOnCloseJobObjectInfo()
        {
            return WinAPI.Is32Bit
                       ? CreateKillOnCloseJobObjectInfo32()
                       : CreateKillOnCloseJobObjectInfo64();
        }

        private static JobObjectInfo CreateKillOnCloseJobObjectInfo32()
        {
            return new JobObjectInfo
            {
                extendedLimits32 = new ExtendedLimits32
                {
                    BasicLimits = new BasicLimits32
                    {
                        LimitFlags = LimitFlags.LimitKillOnJobClose
                    }
                }
            };
        }

        private static JobObjectInfo CreateKillOnCloseJobObjectInfo64()
        {
            return new JobObjectInfo
            {
                extendedLimits64 = new ExtendedLimits64
                {
                    BasicLimits = new BasicLimits64
                    {
                        LimitFlags = LimitFlags.LimitKillOnJobClose
                    }
                }
            };
        }

        /// <summary>
        ///     Determines if the given <paramref name="process"/> already belongs to <b>this</b> Job Object.
        /// </summary>
        /// <param name="process">Process to check Job Object membership of.</param>
        /// <returns>
        ///     <c>true</c> if the given <paramref name="process"/> already belongs to this Job Object;
        ///     otherwise <c>false</c>.
        /// </returns>
        private bool AlreadyAssigned(Process process)
        {
            return JobObjectManager.IsProcessInJob(process, _jobObjectHandle);
        }

        /// <summary>
        ///     Determines if the given <paramref name="process"/> already belongs to <b>another</b> Job Object.
        /// </summary>
        /// <param name="process">Process to check Job Object membership of.</param>
        /// <returns>
        ///     <c>true</c> if the given <paramref name="process"/> already belongs to another Job Object;
        ///     otherwise <c>false</c>.
        /// </returns>
        private static bool HasJobObject(Process process)
        {
            return JobObjectManager.HasJobObject(process);
        }

        #region Win32 P/Invoke Interop

//        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Auto)]
//        private static extern bool AssignProcessToJobObject(IntPtr hJob, IntPtr hProcess);
//
//        [DllImport("kernel32", SetLastError = true)]
//        private static extern bool CloseHandle(IntPtr hJob);
//
        [DllImport("kernel32", SetLastError = true)]
        private static extern bool SetInformationJobObject(IntPtr hJob, JobObjectInfoClass jobObjectInfoClass,
                                                           [In] ref JobObjectExtendedLimitInformation lpJobObjectInfo,
                                                           uint cbJobObjectInfoLength);

        #endregion
    }
}
