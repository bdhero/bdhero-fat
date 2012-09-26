using System;
using System.Drawing;
using Microsoft.WindowsAPICodePack.Taskbar;

namespace BDAutoMuxer.views
{
    /// <summary>
    /// XP-safe static wrapper for Microsoft.WindowsAPICodePack.Taskbar.TaskbarManager
    /// </summary>
    public class TaskbarProgress // : TaskbarManager
    {
// ReSharper disable InconsistentNaming
        private static readonly TaskbarManager taskbarManager;
// ReSharper restore InconsistentNaming

        static TaskbarProgress()
        {
            if (IsPlatformSupported)
            {
                taskbarManager = TaskbarManager.Instance;
            }
        }

        public static bool IsPlatformSupported
        {
            get { return TaskbarManager.IsPlatformSupported; }
        }

        public static void SetOverlayIcon(Icon icon, string accessibilityText)
        {
            if (IsPlatformSupported)
                taskbarManager.SetOverlayIcon(icon, accessibilityText);
        }

        public static void SetOverlayIcon(IntPtr windowHandle, Icon icon, string accessibilityText)
        {
            if (IsPlatformSupported)
                taskbarManager.SetOverlayIcon(windowHandle, icon, accessibilityText);
        }

        public static void SetProgressState(TaskbarProgressBarState state)
        {
            if (IsPlatformSupported)
                taskbarManager.SetProgressState(state);
        }

        public static void SetProgressState(TaskbarProgressBarState state, IntPtr windowHandle)
        {
            if (IsPlatformSupported)
                taskbarManager.SetProgressState(state, windowHandle);
        }

        public static void SetProgressValue(int currentValue, int maximumValue)
        {
            if (IsPlatformSupported)
                taskbarManager.SetProgressValue(currentValue, maximumValue);
        }

        public static void SetProgressValue(int currentValue, int maximumValue, IntPtr windowHandle)
        {
            if (IsPlatformSupported)
                taskbarManager.SetProgressValue(currentValue, maximumValue, windowHandle);
        }
    }
}
