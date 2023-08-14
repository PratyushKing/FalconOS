using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Sys = Cosmos.System;

namespace FalconOS
{
    internal static class sysmgr
    {
        public static void login(bool clear = false)
        {
            askuser:
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
            log.sPrint("Login to FalconOS!\nUsername: ", "");
            var usr = Console.ReadLine();
            if (usr == "root")
            {
                Kernel.cUser = "root";
            }
            else if (!(File.ReadAllText("0:\\Config\\user.conf").Contains(usr)))
            {
                Console.WriteLine("Invalid User!");
                goto askuser;
            }
            if (!(usr == "root"))
            {
                passask:
                log.sPrint("Password: ", "");
                var pass = "";
                var key = new ConsoleKeyInfo();
                
                
                while (!(key.Key == ConsoleKey.Enter))
                {
                    if (key.Key == ConsoleKey.Backspace)
                    {
                        if (pass.Length > 0)
                        {
                            pass = pass.Remove(pass.Length - 1, 1);
                        }
                    }
                    key = Console.ReadKey(true);
                    pass += key.KeyChar;
                }
                if (pass.Length > 0)
                {
                    pass = pass.Remove(pass.Length - 1, 1);
                }
                if (File.ReadAllText("0:\\Config\\passwd.conf").Contains(pass))
                {
                    Kernel.cUser = usr;
                }
                else
                {
                    if (!(usr == "root"))
                    {
                        Console.WriteLine("Invalid Password!");
                        pass = "";
                        goto passask;
                    }
                }
            }
            log.sPrint("\nLogging in as " + Kernel.cUser);
            if (clear) { Console.Clear(); }
            log.drawTitleBar("FalconOS: Shell");
            Console.CursorTop += 7;
            if (Kernel.cUser == "root")
            {
                Kernel.asRoot = true;
            }
        }

        public static void sysctl(string args)
        {
            if (args == null)
            {
                log.sPrint("sysctl v1.0\nSysctl stands for System Control, It has basic functions for system controlling!");
            }
            foreach (var arg in args.Split(' '))
            {
                if (arg == "shutdown")
                {
                    log.print("System", "Shutting down.");
                    Thread.Sleep(1000);
                    Cosmos.System.Power.Shutdown();
                }
                else if (arg == "reboot")
                {
                    log.print("System", "Rebooting.");
                    Thread.Sleep(1000);
                    Cosmos.System.Power.Reboot();
                }
                else if (arg == "gui=true")
                {
                    Kernel.gui = true;
                }
                else if (arg == "gui=false")
                {
                    Kernel.gui = false;
                }
                else if (arg == "info")
                {
                    log.sPrint("Basic System Info:");
                    log.sPrint("Disk Space: " + data.fs.GetTotalFreeSpace("0:\\") + "/" + data.fs.GetTotalSize("0:\\"));
                    log.sPrint("Virtual Machine? [VirtualBox/VMWare/QEMU]: " + Sys.VMTools.IsVirtualBox + "/" + Sys.VMTools.IsVMWare + "/" + Sys.VMTools.IsQEMU);
                    log.sPrint("Disks Count: " + data.fs.Disks.Count.ToString());
                }
                else if (arg == "proclist")
                {
                    log.sPrint("Processes(" + data.ProcMgr.processes.Count + "):");
                    foreach (var process in data.ProcMgr.processes)
                    {
                        log.sPrint(process);
                    }
                } else if (arg == "cursor")
                {
                    Console.CursorVisible = true;
                }
            }
        }
    }
}
