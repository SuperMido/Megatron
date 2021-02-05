using System.Runtime.InteropServices;

namespace SeleniumAutomationTests
{
    public class OsPlatform : IOsPlatform
    {
        public bool isWindows()
        {
            return System.Runtime.InteropServices.RuntimeInformation
                .IsOSPlatform(OSPlatform.Windows);
        }

        public bool isMacOs()
        {
            return System.Runtime.InteropServices.RuntimeInformation
                .IsOSPlatform(OSPlatform.OSX);
        }

        public bool isLinux()
        {
            return System.Runtime.InteropServices.RuntimeInformation
                .IsOSPlatform(OSPlatform.Linux);
        }
    }
}