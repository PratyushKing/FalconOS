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
                //rooter
                runAsRoot(txt);
            } else
            {
                try
                {
                    var rooters = File.ReadAllText(data.baseDir + "Config\\root.conf");
                    var passwords = File.ReadAllText(data.baseDir + "Config\\passwd.conf");
                    if (rooters.Contains(currentUser + "\n"))
                    {
                        var pass = "";
                        var key = new ConsoleKeyInfo();
                        while (!(key.Key == ConsoleKey.Enter))
                        {
                            if (key.Key == ConsoleKey.Backspace)
                            {
                                Console.SetCursorPosition(0, Console.CursorTop);
                                if (pass.Length > 0)
                                {
                                    pass = pass.Remove(pass.Length - 2, 1);
                                }
                                Console.Write("[sudo] Password for " + Kernel.cUser + ": " + new string('*', pass.Length - 1) + " ");
                                Console.SetCursorPosition(0, Console.CursorTop);
                                Console.Write("[sudo] Password for " + Kernel.cUser + ": " + new string('*', pass.Length - 1));
                                continue;
                            }
                            key = Console.ReadKey(true);
                            pass += key.KeyChar;
                            Console.Write("*");
                        }
                        if (pass.Length > 0)
                        {
                            pass = pass.Remove(pass.Length - 1, 1);
                        }
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
