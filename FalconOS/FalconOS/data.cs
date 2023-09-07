using Cosmos.HAL;
using Cosmos.System.FileSystem;
using Cosmos.System.FileSystem.VFS;
using Cosmos.System.Graphics;
using IL2CPU.API.Attribs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using PrismAPI;

namespace FalconOS
{
    internal class data
    {
        public static CosmosVFS fs = new CosmosVFS();
        public static PrismAPI.Graphics.Canvas canvas;
        public static processMgr ProcMgr;

        public static string baseDir = @"0:\";
        public static string currentDir = baseDir;
        public static string lastDir = baseDir;
        public static Color accentColor = Color.Cyan;
        public static Color deepAccentColor = Color.DarkCyan;
        public static bool pressed = false;

        public static string osname = "FalconOS";
        public static string vername = "Stick";
        public static string ver = "v1.0.1";

        public static NetworkDevice net = NetworkDevice.GetDeviceByName("eth0");
        [ManifestResourceStream(ResourceName="FalconOS.Resources.cursor.bmp")] public static byte[] cursor;
        [ManifestResourceStream(ResourceName="FalconOS.Resources.falcon_dark.bmp")] public static byte[] logo;
        [ManifestResourceStream(ResourceName = "FalconOS.Resources.falcon_medium.bmp")] public static byte[] slogo;
        [ManifestResourceStream(ResourceName="FalconOS.Resources.wallpaper.bmp")] public static byte[] background;

        public static string lastCMD = "";
        public static string[] protectedPaths = { data.baseDir + "Config" };
        public static bool VExtras = false; //VGA Extras, for weird screen expert stuff
    }
}
