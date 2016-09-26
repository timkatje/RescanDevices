using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;


namespace RescanDevices
{
    public static class Win32Api
    {
        // Define the ulFlag values and return codes
        public const int CM_LOCATE_DEVNODE_NORMAL = 0x00000000;
        public const int CM_REENUMERATE_NORMAL = 0x00000000;
        public const int CM_REENUMERATE_RETRY_INSTALLATION = 0x00000002;
        public const int CR_SUCCESS = 0x00000000;

        // Import the CfgMgr32 DLL file to locate the root DevNode
        [DllImport("CfgMgr32.dll", SetLastError = true)]
        public static extern int CM_Locate_DevNodeA(ref int pdnDevInst, string pDeviceID, int ulFlags);

        // Import the CfgMgr32 DLL file to reenumerate the devices under the root DevNode
        [DllImport("CfgMgr32.dll", SetLastError = true)]
        public static extern int CM_Reenumerate_DevNode(int dnDevInst, int ulFlags);
    }

    class Program
    {
        static void Main(string[] args)
        {
            int pdnDevInst = 0;

            // Locate the root devnode
            if (Win32Api.CM_Locate_DevNodeA(ref pdnDevInst, null, Win32Api.CM_LOCATE_DEVNODE_NORMAL) != Win32Api.CR_SUCCESS)
                throw new Exception("Error when trying to locate the root DevNode");

            // Attempt to reenumerate (and reinstalls) all devices under the root node
            if (Win32Api.CM_Reenumerate_DevNode(pdnDevInst, Win32Api.CM_REENUMERATE_RETRY_INSTALLATION) != Win32Api.CR_SUCCESS)
                throw new Exception("Error when trying to reenumerate the root DevNode");
        }
    }
}
