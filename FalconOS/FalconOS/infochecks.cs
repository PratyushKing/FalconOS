using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FalconOS
{
    internal static class infochecks
    {
        public static string isVM()
        {
            if (Cosmos.System.VMTools.IsQEMU || Cosmos.System.VMTools.IsVirtualBox || Cosmos.System.VMTools.IsVMWare)
                return "Yes";
            return "No";
        }

        public static bool isLetter(char letter)
        {
            char[] letters = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', '#', '/', '_', '|', ' ' };
            foreach (char l in letters)
            {
                if (l == letter)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
