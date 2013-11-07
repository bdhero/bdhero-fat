using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using OSUtils;
using OSUtils.JobObjects;

namespace WindowsOSUtils.JobObjects
{
    public class JobObjectManager : IJobObjectManager
    {
        /// <summary>
        ///     Determines if the given <paramref name="process"/> already belongs to <b>another</b> Job Object.
        /// </summary>
        /// <param name="process">Process to check Job Object membership of.</param>
        /// <returns>
        ///     <c>true</c> if the given <paramref name="process"/> already belongs to another Job Object;
        ///     otherwise <c>false</c>.
        /// </returns>
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

                var process = JobObjectController.CreateProcessInSeparateJob(processStartInfo);

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
            return IsProcessInJob(process, IntPtr.Zero);
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
    }
}
