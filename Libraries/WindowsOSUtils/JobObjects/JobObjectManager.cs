using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using DotNetUtils.Annotations;
using OSUtils;
using OSUtils.JobObjects;

namespace WindowsOSUtils.JobObjects
{
    /// <summary>
    ///     Concrete implementation of the <see cref="IJobObjectManager"/> interface
    ///     that accesses the Windows Job Objects API.
    /// </summary>
    /// <seealso cref="https://www-auth.cs.wisc.edu/lists/htcondor-users/2009-June/msg00106.shtml" />
    public class JobObjectManager : IJobObjectManager
    {
        /// <summary>
        ///     This is a pseudo handle to "any" job object.
        /// </summary>
        private static readonly IntPtr AnyJobHandle = IntPtr.Zero;

        public IJobObject CreateJobObject()
        {
            return new JobObject();
        }

        public bool IsAssignedToJob(Process process)
        {
            return HasJobObject(process);
        }

        public bool TryBypassPCA(string[] args)
        {
            using (var currentProcess = Process.GetCurrentProcess())
            {
                if (!IsAssignedToJob(currentProcess)) { return false; }

                var processStartInfo = new ProcessStartInfo(currentProcess.MainModule.FileName, new ArgumentList(args).ToString());

                var process = CreateProcessInSeparateJob(processStartInfo);

                if (process == null) { return false; }

                try
                {
                    var exited = process.WaitForExit(2000);
                    return !exited;
                }
                catch
                {
                    return false;
                }
            }
        }

        internal static bool HasJobObject(Process process)
        {
            return IsProcessInJob(process, AnyJobHandle);
        }

        /// <summary>
        ///     Determines if the given <paramref name="process"/> belongs to the specified
        ///     <paramref name="jobObjectHandle"/>.
        /// </summary>
        /// <param name="process">Process to check Job Object membership of.</param>
        /// <param name="jobObjectHandle">Job Object to check for membership.</param>
        /// <returns>
        ///     <c>true</c> if the given <paramref name="process"/> belongs to the specified
        ///     <paramref name="jobObjectHandle"/>; otherwise <c>false</c>.
        /// </returns>
        internal static bool IsProcessInJob(Process process, IntPtr jobObjectHandle)
        {
            var status = false;
            PInvokeUtils.Try(() => WinAPI.IsProcessInJob(process.Handle, jobObjectHandle, out status));
            return status;
        }

        /// <summary>
        ///     Starts a process outside of the current Job Object.
        ///     The child process will not inherit the current parent process' Job Object,
        ///     nor will it belong to any Job Object at all.
        /// </summary>
        /// <param name="startInfo">Process start information</param>
        /// <returns>
        ///     The child process if it was started successfully, or <c>null</c> if the process could not be started
        ///     (e.g., if the process is the Visual Studio <c>vhost.exe</c> application).
        /// </returns>
        /// <remarks>
        ///     This method can be used to break out of the Program Compatibility Assistant (PCA)
        ///     Job Object that is automatically created when an application runs on a newer
        ///     version of Windows than it was marked as compatible with in its application manifest.
        /// </remarks>
        /// <seealso cref="http://blogs.msdn.com/b/cjacks/archive/2009/07/10/how-to-work-around-program-compatibility-assistant-pca-jobobjects-interfering-with-your-jobobjects.aspx" />
        /// <seealso cref="http://blogs.msdn.com/b/alejacma/archive/2012/03/09/why-is-my-process-in-a-job-if-i-didn-t-put-it-there.aspx" />
        [CanBeNull]
        private static Process CreateProcessInSeparateJob(ProcessStartInfo startInfo)
        {
            // TODO: Is it necessary to call this method?
            var newJobHandle = PInvokeUtils.Try(() => WinAPI.CreateJobObject(IntPtr.Zero, null));

            var securityAttributes = new SECURITY_ATTRIBUTES
                                     {
                                         nLength = Marshal.SizeOf(typeof (SECURITY_ATTRIBUTES)),
                                         lpSecurityDescriptor = IntPtr.Zero,
                                         bInheritHandle = false
                                     };

            var startupInfo = new STARTUPINFO
                              {
                                  cb = Marshal.SizeOf(typeof (STARTUPINFO))
                              };

            var processInformation = new PROCESS_INFORMATION();

            const bool bInheritHandles = false;
            var lpEnvironment = IntPtr.Zero;
            const string lpCurrentDirectory = null;

            PInvokeUtils.Try(() => WinAPI.CreateProcess(startInfo.FileName,
                                                        startInfo.Arguments,
                                                        ref securityAttributes,
                                                        ref securityAttributes,
                                                        bInheritHandles,
                                                        ProcessCreationFlags.CREATE_BREAKAWAY_FROM_JOB,
                                                        lpEnvironment,
                                                        lpCurrentDirectory,
                                                        ref startupInfo,
                                                        out processInformation));

            try
            {
                return Process.GetProcessById(processInformation.dwProcessId);
            }
            catch
            {
                // Running in Visual Studio w/ Program Compatibility Assistant (PCA) enabled.
                // See http://stackoverflow.com/a/4232259/3205
                return null;
            }
        }
    }
}
