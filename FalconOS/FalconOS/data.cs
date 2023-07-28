using Cosmos.System.FileSystem;
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
        public static string vername = "STABLE";
        public static string ver = "v1.0.1";

        public static string lastCMD = "";
        public static string[] protectedPaths = { "0:\\Config" };

        public static processMgr ProcMgr;
    }
}
