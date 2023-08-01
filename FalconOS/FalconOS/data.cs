using Cosmos.HAL;
using Cosmos.System.FileSystem;
using Cosmos.System.FileSystem.VFS;
using IL2CPU.API.Attribs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FalconOS
{
    internal class data
    {
        public static CosmosVFS fs = new CosmosVFS();

        public static string currentDir = @"0:\";
        public static string lastDir = @"0:";

        public static string osname = "FalconOS";
        public static string vername = "Stick";
        public static string ver = "v1.0.1";

        public static NetworkDevice net = NetworkDevice.GetDeviceByName("eth0");
        [ManifestResourceStream(ResourceName="FalconOS.Resources.cursor.bmp")] public static byte[] cursor;

        public static string lastCMD = "";
        public static string[] protectedPaths = { "0:\\Config" };

        public static processMgr ProcMgr;
    }
}
