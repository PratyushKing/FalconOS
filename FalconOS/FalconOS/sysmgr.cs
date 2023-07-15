using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FalconOS
{
    internal static class sysmgr
    {
        public static void login(bool clear = false)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
            log.sPrint("Login to FalconOS!\nUsername: ", "");
            var usr = Console.ReadLine();
            if (usr == "root")
            {
                Kernel.cUser = "root";
            }
            else if (!(File.ReadAllText("0:\\Config\\user.s").Contains(usr)))
            {
                log.sPrint("Invalid User, Rebooting!");
                Thread.Sleep(2000);
                Cosmos.System.Power.Reboot();
            }
            if (!(usr == "root"))
            {
                log.sPrint("Password: ", "");
                var pass = Console.ReadLine();
                if (File.ReadAllText("0:\\Config\\pwd.s").Contains(pass))
                {
                    Kernel.cUser = usr;
                }
                else
                {
                    if (!(usr == "root"))
                    {
                        log.sPrint("Invalid password, Rebooting!");
                        Thread.Sleep(3000);
                        Cosmos.System.Power.Reboot();
                    }
                }
            }
            log.sPrint("\nLogging in as " + Kernel.cUser);
            if (clear) { Console.Clear(); }
            log.drawTitleBar("FalconOS: Shell");
            if (Kernel.cUser == "root")
            {
                Kernel.asRoot = true;
            }
        }
    }
}
