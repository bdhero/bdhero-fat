using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OSUtils.DriveDetector
{
    public interface IDriveDetector
    {
        /// <summary>
        /// Events signalized to the client app.
        /// Add handlers for these events in your form to be notified of removable device events 
        /// </summary>
        event DriveDetectorEventHandler DeviceArrived;

        event DriveDetectorEventHandler DeviceRemoved;
        event DriveDetectorEventHandler QueryRemove;

        /// <summary>
        /// Gets the value indicating whether the query remove event will be fired.
        /// </summary>
        bool IsQueryHooked { get; }

        /// <summary>
        /// Gets letter of drive which is currently hooked. Empty string if none.
        /// See also IsQueryHooked.
        /// </summary>
        string HookedDrive { get; }

        /// <summary>
        /// Gets the file stream for file which this class opened on a drive to be notified
        /// about it's removal. 
        /// This will be null unless you specified a file to open (DriveDetector opens root directory of the flash drive) 
        /// </summary>
        FileStream OpenedFile { get; }

        /// <summary>
        /// Hooks specified drive to receive a message when it is being removed.  
        /// This can be achieved also by setting e.HookQueryRemove to true in your 
        /// DeviceArrived event handler. 
        /// By default DriveDetector will open the root directory of the flash drive to obtain notification handle
        /// from Windows (to learn when the drive is about to be removed). 
        /// </summary>
        /// <param name="fileOnDrive">Drive letter or relative path to a file on the drive which should be 
        /// used to get a handle - required for registering to receive query remove messages.
        /// If only drive letter is specified (e.g. "D:\\", root directory of the drive will be opened.</param>
        /// <returns>true if hooked ok, false otherwise</returns>
        bool EnableQueryRemove(string fileOnDrive);

        /// <summary>
        /// Unhooks any currently hooked drive so that the query remove 
        /// message is not generated for it.
        /// </summary>
        void DisableQueryRemove();

        /// <summary>
        /// Message handler which must be called from client form.
        /// Processes Windows messages and calls event handlers. 
        /// </summary>
        /// <param name="m"></param>
        void WndProc(ref Message m);
    }
}
