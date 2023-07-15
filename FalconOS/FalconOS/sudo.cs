using Cosmos.System.FileSystem.VFS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace FalconOS
{
    internal class sudo
    {
        public string currentUser;
        public sudo(string user)
        {
            currentUser = user;
        }

        public int execAsRoot(string txt)
        {
            if (this.currentUser == "root")
            {
                runAsRoot(txt);
            } else
            {
                try
                {
                    var rooters = File.ReadAllText("0:\\Config\\root.ers");
                    var passwords = File.ReadAllText("0:\\Config\\pwd.s");
                    if (rooters.Contains(currentUser + "\n"))
                    {
                        Console.Write("Enter password for " + currentUser + ": ");
                        var pass = Console.ReadLine(); 
                        foreach (var password in passwords.Split('\n'))
                        {
                            if (pass == password)
                            {
                                runAsRoot(txt);
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    log.sPrint("User not in root-ers file.");
                }
            }
            return 0;
        }

        public void runAsRoot(string txt)
        {
            var cmd = txt;
            if (cmd == "reboot")
            {
                Cosmos.System.Power.Reboot();
            } else if (cmd == "usrname")
            {
                log.sPrint("ROOT");
            } else if (cmd == "shutdown")
            {
                log.print("Shutdown", "Shutting down the system.");
                Cosmos.System.Power.Shutdown();
            }
            else
            {
                Shell sudoShell = new Shell(data.osname, data.ver);
                sudoShell.exec(cmd, true);
            }
        }
    }
}
