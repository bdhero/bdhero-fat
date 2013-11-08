using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using DotNetUtils.Annotations;

namespace WindowsOSUtils.JobObjects
{
    /// <summary>An interface to the Windows Job Objects API.</summary>
    /// <seealso cref="https://www-auth.cs.wisc.edu/lists/htcondor-users/2009-June/msg00106.shtml" />
    public static class JobObjectController
    {
        #region State

        /// <summary>
        /// This is a pseudo handle to the "any" job object handle.
        /// </summary>
        private static readonly IntPtr AnyJobHandle = IntPtr.Zero;

        #endregion

        #region Controlling Methods

        /// <summary>
        /// Starts a process outside of the current Job Object.
        /// The child process will not inherit the current parent process' Job Object,
        /// nor will it belong to any Job Object at all.
        /// </summary>
        /// <param name="startInfo">Process start information</param>
        /// <returns>
        /// The child process if it was started successfully, or <c>null</c> if the process could not be started
        /// (e.g., if the process is the Visual Studio <c>vhost.exe</c> application).
        /// </returns>
        /// <remarks>
        /// This method can be used to break out of the Program Compatibility Assistant (PCA)
        /// Job Object that is automatically created when an application runs on a newer
        /// version of Windows than it was marked as compatible with in its application manifest.
        /// </remarks>
        /// <seealso cref="http://blogs.msdn.com/b/cjacks/archive/2009/07/10/how-to-work-around-program-compatibility-assistant-pca-jobobjects-interfering-with-your-jobobjects.aspx"/>
        /// <seealso cref="http://blogs.msdn.com/b/alejacma/archive/2012/03/09/why-is-my-process-in-a-job-if-i-didn-t-put-it-there.aspx"/>
        [CanBeNull]
        public static Process CreateProcessInSeparateJob(ProcessStartInfo startInfo)
        {
            IntPtr newJobHandle = WinAPI.CreateJobObject(
              IntPtr.Zero,
              null);

            if (newJobHandle == IntPtr.Zero)
            {
                int error = Marshal.GetLastWin32Error();
                throw new ApplicationException("Failed calling CreateJobObject, error = " + error);
            }

            var securityAttributes = new SECURITY_ATTRIBUTES();
            securityAttributes.nLength = Marshal.SizeOf(typeof (SECURITY_ATTRIBUTES));
            securityAttributes.lpSecurityDescriptor = IntPtr.Zero;
            securityAttributes.bInheritHandle = false;

            var startupInfo = new STARTUPINFO();
            startupInfo.cb = Marshal.SizeOf(typeof (STARTUPINFO));

            var processInformation = new PROCESS_INFORMATION();

            bool bInheritHandles = false;
            IntPtr lpEnvironment = IntPtr.Zero;
            string lpCurrentDirectory = null;

            var result = WinAPI.CreateProcess(startInfo.FileName,
                                              startInfo.Arguments,
                                              ref securityAttributes,
                                              ref securityAttributes,
                                              bInheritHandles,
                                              ProcessCreationFlags.CREATE_BREAKAWAY_FROM_JOB,
                                              lpEnvironment,
                                              lpCurrentDirectory,
                                              ref startupInfo,
                                              out processInformation);

            if (!result)
            {
                int error = Marshal.GetLastWin32Error();
                throw new ApplicationException("Failed calling CreateProcess, error = " + error);
            }

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

        #endregion
    }
}
