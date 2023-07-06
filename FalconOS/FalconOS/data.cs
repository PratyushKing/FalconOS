using Cosmos.System.FileSystem;
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
        public static string vername = "BETA";
        public static string ver = "v1.0.1";

        public static string lastCMD = "";

        public static processMgr ProcMgr;
    }
}
