using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OSUtils.JobObjects
{
    /// <summary>
    ///     Interface for a factory that creates <see cref="IJobObject"/>s for the current operating system.
    /// </summary>
    public interface IJobObjectFactory
    {
        /// <summary>
        ///     Creates a new instance of a class that implements the <see cref="IJobObject"/> interface
        ///     for the current operating system.
        /// </summary>
        /// <returns>OS-specific <see cref="IJobObject"/> instance.</returns>
        IJobObject CreateJobObject();
    }
}
