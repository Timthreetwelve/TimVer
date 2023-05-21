// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer.Helpers
{
    public static class DiskDriveHelpers
    {
        /// <summary>
        /// Determines whether the specified drive is Fixed.
        /// </summary>
        /// <param name="drive">The drive to check.</param>
        /// <returns>
        ///   <c>true</c> if specified drive is Fixed; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsDriveFixed(char drive)
        {
            if (drive is '\0')
            {
                return false;
            }
            foreach (DriveInfo driveInfo in DriveInfo.GetDrives())
            {
                if (driveInfo.Name.StartsWith(drive.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    return driveInfo.DriveType == DriveType.Fixed;
                }
            }
            return false;
        }
    }
}
