using System;
using System.IO;

namespace OSUtils.DriveDetector
{
    /// <summary>
    /// Our class for passing in custom arguments to our event handlers.
    /// </summary>
    public class DriveDetectorEventArgs : EventArgs
    {
        /// <summary>
        /// Get/Set the value indicating that the event should be cancelled 
        /// Only in QueryRemove handler.
        /// </summary>
        public bool Cancel;

        /// <summary>
        /// Information about the drive.
        /// </summary>
        public DriveInfo DriveInfo;

        /// <summary>
        /// Set to true in your DeviceArrived event handler if you wish to receive the 
        /// QueryRemove event for this drive. 
        /// </summary>
        public bool HookQueryRemove;
    }
}