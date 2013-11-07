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
        /// This is a pseudo handle to the currently running process.
        /// </summary>
        private static readonly IntPtr CurrentProcessHandle = WinAPI.GetCurrentProcess();

        /// <summary>
        /// This is a pseudo handle to the "any" job object handle.
        /// </summary>
        private static readonly IntPtr AnyJobHandle = IntPtr.Zero;

        /// <summary>
        /// The job created for this process.
        /// </summary>
        private static IntPtr _currentJobHandle = IntPtr.Zero;

        #endregion

        #region Native 32/64 Bit Switching Flag

        /// <summary>
        /// The structures returned by Windows are different sizes depending on whether
        /// the operating system is running in 32bit or 64bit mode.
        /// </summary>
        private static readonly bool Is32Bit = (IntPtr.Size == 4);

        #endregion

        #region Controlling Methods

        /// <summary>
        /// Checks to see if the current process is a member of any Windows job.
        /// </summary>
        /// <returns>True if the current process is a member of a job, false otherwise</returns>
        public static bool IsCurrentProcessMemberOfAnyJob()
        {
            return IsProcessMemberOfAnyJob(Process.GetCurrentProcess());
        }

        /// <summary>
        /// Checks to see if the given process is a member of any Windows job.
        /// </summary>
        /// <param name="process"></param>
        /// <returns>True if the give process is a member of a job, false otherwise</returns>
        public static bool IsProcessMemberOfAnyJob(Process process)
        {
            return IsProcessMemberOfAnyJob(process.Handle);
        }

        /// <summary>
        /// Checks to see if the given process is a member of any Windows job.
        /// </summary>
        /// <param name="processHandle"></param>
        /// <returns>True if the give process is a member of a job, false otherwise</returns>
        public static bool IsProcessMemberOfAnyJob(IntPtr processHandle)
        {
            bool isInJob;
            bool result;

            result = WinAPI.IsProcessInJob(
              processHandle,
              AnyJobHandle,
              out isInJob);

            if (!result)
            {
                int error = Marshal.GetLastWin32Error();
                throw new ApplicationException("Failed calling IsProcessInJob, error = " + error);
            }

            return isInJob;
        }

        /// <summary>
        /// Creates a new job object and assigns the current process to that job.
        /// </summary>
        public static void CreateNewJobForCurrentProcess()
        {
            IntPtr newJobHandle = WinAPI.CreateJobObject(
              IntPtr.Zero,
              null);

            if (newJobHandle == IntPtr.Zero)
            {
                int error = Marshal.GetLastWin32Error();
                throw new ApplicationException("Failed calling CreateJobObject, error = " + error);
            }

            bool result = WinAPI.AssignProcessToJobObject(
              newJobHandle,
              CurrentProcessHandle);

            if (!result)
            {
                int error = Marshal.GetLastWin32Error();
                throw new ApplicationException("Failed calling AssignProcessToJobObject, error = " + error);
            }

            _currentJobHandle = newJobHandle;
        }

        /// <summary>
        /// Creates a child process outside the current Job Object that was created by the
        /// Program Compatibility Assistant (PCA).
        /// </summary>
        /// <param name="args">Arguments to pass to the child process</param>
        /// <returns><c>true</c> if the child process started successfully and did not exit within 2 seconds; otherwise <c>false</c></returns>
        public static bool BreakCurrentProcessOutOfPCAJobObject(string[] args)
        {
            if (!IsCurrentProcessMemberOfAnyJob())
                return false;

            using (var currentProcess = Process.GetCurrentProcess())
            {
                var processStartInfo = new ProcessStartInfo(currentProcess.MainModule.FileName, new ArgumentList(args).ToString());

                var process = CreateProcessInSeparateJob(processStartInfo);

                if (process == null)
                    return false;

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

            _currentJobHandle = newJobHandle;

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

        /// <summary>
        /// Gets the current process memory limit settings for the job object the
        /// current process belongs to.
        /// </summary>
        /// <param name="processMemoryLimit">Out parameter set to the maximum amount of
        /// commit memory a single individual process in the job may consume in bytes.</param>
        /// <returns>True if the limit is enforced, false otherwise.</returns>
        public static bool GetProcessMemoryLimit(out ulong processMemoryLimit)
        {
            JobObjectInfo jobObjectInfo;
            IntPtr currentJob = IntPtr.Zero;
            bool result;
            int returnLength;

            if (Is32Bit)
            {
                result = WinAPI.QueryInformationJobObject(
                  _currentJobHandle,
                  JobObjectInfoClass.ExtendedLimitInformation,
                  out jobObjectInfo,
                  Marshal.SizeOf(typeof(ExtendedLimits32)),
                  out returnLength
                );

                if (!result)
                {
                    int error = Marshal.GetLastWin32Error();
                    throw new ApplicationException("Failed calling QueryInformationJobObject, error = " + error);
                }

                Trace.Assert(
                  returnLength == Marshal.SizeOf(typeof(ExtendedLimits32)),
                  "QueryInformationJobObject returned " + returnLength + " bytes instead of expected " + Marshal.SizeOf(typeof(ExtendedLimits32))
                );

                processMemoryLimit = jobObjectInfo.extendedLimits32.ProcessMemoryLimit;
                return (jobObjectInfo.basicLimits32.LimitFlags & LimitFlags.LimitProcessMemory) != 0;
            }
            else
            {
                result = WinAPI.QueryInformationJobObject(
                  _currentJobHandle,
                  JobObjectInfoClass.BasicLimitInformation,
                  out jobObjectInfo,
                  Marshal.SizeOf(typeof(BasicLimits64)),
                  out returnLength
                );

                if (!result)
                {
                    int error = Marshal.GetLastWin32Error();
                    throw new ApplicationException("Failed calling QueryInformationJobObject, error = " + error);
                }

                result = WinAPI.QueryInformationJobObject(
                  _currentJobHandle,
                  JobObjectInfoClass.ExtendedLimitInformation,
                  out jobObjectInfo,
                  Marshal.SizeOf(typeof(ExtendedLimits64)),
                  out returnLength
                );

                if (!result)
                {
                    int error = Marshal.GetLastWin32Error();
                    throw new ApplicationException("Failed calling QueryInformationJobObject, error = " + error);
                }

                Trace.Assert(
                  returnLength == Marshal.SizeOf(typeof(ExtendedLimits64)),
                  "QueryInformationJobObject returned " + returnLength + " bytes instead of expected " + Marshal.SizeOf(typeof(ExtendedLimits64))
                );

                processMemoryLimit = jobObjectInfo.extendedLimits64.ProcessMemoryLimit;
                return (jobObjectInfo.basicLimits64.LimitFlags & LimitFlags.LimitProcessMemory) != 0;
            }
        }

        /// <summary>
        /// Set the process memory limit in the current job to the given value.
        /// </summary>
        /// <param name="processMemoryLimit">The maximum amount of commit memory a
        /// single individual process in the job may consume in bytes.</param>
        public static void SetProcessMemoryLimit(ulong processMemoryLimit)
        {
            ChangeJob(ji =>
            {
                Trace.Assert(processMemoryLimit <= uint.MaxValue,
                  "Trying to set processMemoryLimit too high for a 32 bit platform");
                ji.basicLimits32.LimitFlags |= LimitFlags.LimitProcessMemory;
                ji.extendedLimits32.ProcessMemoryLimit = (uint)processMemoryLimit;
                return ji;
            },
              ji =>
              {
                  ji.basicLimits64.LimitFlags |= LimitFlags.LimitProcessMemory;
                  ji.extendedLimits64.ProcessMemoryLimit = processMemoryLimit;
                  return ji;
              });
        }

        /// <summary>
        /// change something about the job
        /// </summary>
        private static void ChangeJob(
          Func<JobObjectInfo, JobObjectInfo> alter32,
          Func<JobObjectInfo, JobObjectInfo> alter64)
        {
            JobObjectInfo jobObjectInfo;
            IntPtr currentJob = IntPtr.Zero;
            bool result;
            int returnLength;

            if (Is32Bit)
            {
                result = WinAPI.QueryInformationJobObject(
                  _currentJobHandle,
                  JobObjectInfoClass.ExtendedLimitInformation,
                  out jobObjectInfo,
                  Marshal.SizeOf(typeof(ExtendedLimits32)),
                  out returnLength);

                if (!result)
                {
                    int error = Marshal.GetLastWin32Error();
                    throw new ApplicationException("Failed calling QueryInformationJobObject, error = " + error);
                }

                Trace.Assert(
                  returnLength == Marshal.SizeOf(typeof(ExtendedLimits32)),
                  "QueryInformationJobObject returned " + returnLength + " bytes instead of expected "
                  + Marshal.SizeOf(typeof(ExtendedLimits32)));
                jobObjectInfo = alter32(jobObjectInfo);

                result = WinAPI.SetInformationJobObject(
                  _currentJobHandle,
                  JobObjectInfoClass.ExtendedLimitInformation,
                  ref jobObjectInfo,
                  Marshal.SizeOf(typeof(ExtendedLimits32)));

                if (!result)
                {
                    int error = Marshal.GetLastWin32Error();
                    var hresult = Marshal.GetHRForLastWin32Error();
                    throw new ApplicationException("Failed calling SetInformationJobObject, error = " + error
                      + " hresult = " + hresult);
                }
            }
            else
            {
                result = WinAPI.QueryInformationJobObject(
                  _currentJobHandle,
                  JobObjectInfoClass.ExtendedLimitInformation,
                  out jobObjectInfo,
                  Marshal.SizeOf(typeof(ExtendedLimits64)),
                  out returnLength);

                if (!result)
                {
                    int error = Marshal.GetLastWin32Error();
                    throw new ApplicationException("Failed calling QueryInformationJobObject, error = " + error);
                }

                Trace.Assert(
                  returnLength == Marshal.SizeOf(typeof(ExtendedLimits64)),
                  "QueryInformationJobObject returned " + returnLength + " bytes instead of expected "
                    + Marshal.SizeOf(typeof(ExtendedLimits64)));

                jobObjectInfo = alter64(jobObjectInfo);

                result = WinAPI.SetInformationJobObject(
                  _currentJobHandle,
                  JobObjectInfoClass.ExtendedLimitInformation,
                  ref jobObjectInfo,
                  Marshal.SizeOf(typeof(ExtendedLimits64)));

                if (!result)
                {
                    int error = Marshal.GetLastWin32Error();
                    throw new ApplicationException("Failed calling SetInformationJobObject, error = " + error);
                }
            }
        }
        #endregion

        #region Utility methods

        /// <summary>
        /// Limits the memory usage of this process (and any children) to a fixed value.
        /// Also locks the processor affinity of all child processes to the same as the current process.
        /// </summary>
        /// <param name="jobMemoryLimit">Set to a non zero value in bytes to limit usage</param>
        public static void StandardJobLimits(
          ulong jobMemoryLimit)
        {
            if (IsCurrentProcessMemberOfAnyJob())
                throw new InvalidOperationException("this process is already under the control of a job object!");
            CreateNewJobForCurrentProcess();
            SetJobMemoryLimit(jobMemoryLimit);
            LinkAffinityToCurrent();
        }

        /// <summary>
        /// Limits the memory usage of this process (and any children) to a fixed value.
        /// </summary>
        /// <param name="jobMemoryLimit">Set to a non zero value in bytes to limit usage</param>
        public static void SetJobMemoryLimit(
          ulong jobMemoryLimit)
        {
            if (jobMemoryLimit > 0)
            {
                ChangeJob(
                  ji =>
                  {
                      ji.basicLimits32.LimitFlags |= LimitFlags.LimitJobMemory;
                      ji.extendedLimits32.JobMemoryLimit = (uint)jobMemoryLimit;
                      return ji;
                  },
                  ji =>
                  {
                      ji.basicLimits64.LimitFlags |= LimitFlags.LimitJobMemory;
                      ji.extendedLimits64.JobMemoryLimit = jobMemoryLimit;
                      return ji;
                  });
            }
        }

        /// <summary>
        /// Locks the processor affinity of all child processes to the same as the current process.
        /// </summary>
        public static void LinkAffinityToCurrent()
        {
            ChangeJob(
              ji =>
              {
                  ji.basicLimits32.LimitFlags |= LimitFlags.LimitAffinity;
                  ji.basicLimits32.Affinity = Process.GetCurrentProcess().ProcessorAffinity;
                  return ji;
              },
              ji =>
              {
                  ji.basicLimits64.LimitFlags |= LimitFlags.LimitAffinity;
                  ji.basicLimits64.Affinity = Process.GetCurrentProcess().ProcessorAffinity;
                  return ji;
              });
        }

        #endregion
    }
}
