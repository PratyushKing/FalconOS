using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FalconOS
{
    internal class executor
    {
        public static string arg1 = "0";
        public static string arg2 = "0";
        public static string arg3 = "0";
        public static string arg4 = "0";
        public static string arg5 = "0";

        public static void executeFE(string file)
        {
            if (File.Exists(file))
            {
                var lines = File.ReadAllLines(file);
                foreach (var line in lines)
                {
                    if (line.StartsWith("00")) {
                        if (line.StartsWith("00 1"))
                        {

                        }
                    }
                }
            } else
            {
                log.print("File does not exist.");
            }
        }
    }
}
