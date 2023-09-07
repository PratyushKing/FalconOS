using Cosmos.System.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
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
            else if (!(File.ReadAllText(data.baseDir + "Config\\user.conf").Contains(usr)))
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
                if (File.ReadAllText(data.baseDir + "Config\\passwd.conf").Contains(pass))
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
                    log.sPrint("Disk Space: " + data.fs.GetTotalFreeSpace(data.baseDir + "") + "/" + data.fs.GetTotalSize(data.baseDir + ""));
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

        public static bool vextras(string args)
        {
            Canvas test = FullScreenCanvas.GetFullScreenCanvas(new Mode(640, 480, ColorDepth.ColorDepth32));
            test.Clear();
            var pos = 0;
            while (true)
            {
                Sys.MouseManager.ScreenWidth = 640;
                Sys.MouseManager.ScreenHeight = 480;
                test.DrawImageAlpha(new Bitmap(data.background), 0, 0);
                test.DrawImageAlpha(new Bitmap(data.slogo), 640/2 - ((int)new Bitmap(data.slogo).Width / 2),480/2 - ((int)new Bitmap(data.slogo).Height / 2));
                test.DrawFilledRectangle(Color.White, 640 / 2 - ((int)new Bitmap(data.slogo).Width / 2), 480 / 2 - ((int)new Bitmap(data.slogo).Height / 2) + ((int)new Bitmap(data.slogo).Height) + 10, ((int)new Bitmap(data.slogo).Width - 1), 8);
                test.DrawFilledRectangle(Color.FromArgb(92, 87, 255), 640 / 2 - ((int)new Bitmap(data.slogo).Width / 2), 480 / 2 - ((int)new Bitmap(data.slogo).Height / 2) + ((int)new Bitmap(data.slogo).Height) + 10, pos, 8);
                if (pos > ((int)new Bitmap(data.slogo).Width - 1))
                {
                    test.Disable();
                    return true;
                }
                else
                {
                    Thread.Sleep(10);
                    pos += 30;
                }
                test.Display();
            }
            //return false;
        }
    }
}
