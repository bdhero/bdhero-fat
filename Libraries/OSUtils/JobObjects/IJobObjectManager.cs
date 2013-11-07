using System.Diagnostics;

namespace OSUtils.JobObjects
{
    /// <summary>
    ///     Interface for a high-level "static" class that returns information about processes and Job Objects.
    /// </summary>
    public interface IJobObjectManager
    {
        /// <summary>
        ///     Determines if the given <paramref name="process"/> is currently associated with a Windows Job Object.
        /// </summary>
        /// <param name="process">The process to check.</param>
        /// <returns><c>true</c> if <paramref name="process"/> belongs to a Job Object; otherwise <c>false</c>.</returns>
        bool IsAssignedToJob(Process process);

        /// <summary>
        ///     Checks if the current process belongs to a Job Object and, if so, attempts to start a new process
        ///     that does not belong to a Job Object.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         This method can be used to break out of the Program Compatibility Assistant (PCA)
        ///         Job Object that is automatically created when an application runs on a newer
        ///         version of Windows than it was marked as compatible with in its application manifest.
        ///     </para>
        ///     <para>
        ///         The spawned process will not inherit the current parent process' Job Object,
        ///         nor will it belong to any Job Object at all.
        ///     </para>
        ///     <para>
        ///         This method will <em>not</em> work with Visual Studio's <c>vhost.exe</c> debugging process.
        ///     </para>
        /// </remarks>
        /// <param name="args">Arguments passed to the startup program's <c>Main()</c> method.</param>
        /// <returns>
        ///     <c>true</c> if the current process belongs to a Job Object and was successfully restarted
        ///     without a Job; otherwise <c>false</c>.
        /// </returns>
        /// <seealso cref="http://blogs.msdn.com/b/cjacks/archive/2009/07/10/how-to-work-around-program-compatibility-assistant-pca-jobobjects-interfering-with-your-jobobjects.aspx"/>
        /// <seealso cref="http://blogs.msdn.com/b/alejacma/archive/2012/03/09/why-is-my-process-in-a-job-if-i-didn-t-put-it-there.aspx"/>
        bool TryBypassPCA(string[] args);
    }
}
