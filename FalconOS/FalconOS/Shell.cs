using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using Sys = Cosmos.System;
using System.Threading.Tasks;
using System.Data;
using System.Threading;
using System.IO;

namespace FalconOS
{
    public class Shell
    {
        public string os; public string ver;
        
        public Shell(string os, string ver)
        {
            this.os = os;
            this.ver = ver;
        }

        public void exec(string cmd)
        {
            data.ProcMgr.addProc(cmd);
            if (cmd == "starty")
            {
                log.print("Yindow Manager", "Starting window manager");
                Thread.Sleep(1500);
            } else if (cmd == "sysctl reboot")
            {
                log.print("System", "Rebooting.");
                Thread.Sleep(1000);
                Sys.Power.Reboot();
            } else if (cmd == "sysctl shutdown")
            {
                log.print("System", "Shutting down.");
                Thread.Sleep(1000);
                Sys.Power.Shutdown();
            } else if (cmd == "sysctl info")
            {
                Console.WriteLine("Basic System Info:");
                Console.WriteLine("Disk Space: " + data.fs.GetTotalFreeSpace("0:\\") + "/" + data.fs.GetTotalSize("0:\\"));
                Console.WriteLine("Virtual Machine? [VirtualBox/VMWare/QEMU]: " + Sys.VMTools.IsVirtualBox + "/" + Sys.VMTools.IsVMWare + "/" + Sys.VMTools.IsQEMU);
                Console.WriteLine("Disks Count: " + data.fs.Disks.Count.ToString());
            } else if (cmd.StartsWith("sysctl "))
            {
                Console.WriteLine("Invalid Args");
            } else if (cmd.StartsWith("falnfo"))
            {
                Console.WriteLine("Falnfo 1.0");
                if (cmd.StartsWith("falnfo -a"))
                {
                    Console.WriteLine("FalconOS v1.0.1\nBase version v1.0.0\nFalVM 0.5");
                } else if (cmd.StartsWith("falnfo -f"))
                {
                    Console.WriteLine("FalconOS v1.0.1");
                } else if (cmd.StartsWith("falnfo -b"))
                {
                    Console.WriteLine("Base version v1.0.1");
                } else if (cmd.StartsWith("falnfo -vm"))
                {
                    Console.WriteLine("FalVM 0.5");
                } else
                {
                    Console.WriteLine("Falnfo: Unrecognized Command");
                }
            } else if (cmd.StartsWith("make "))
            {
                try
                {
                    File.Create(data.currentDir + cmd.Replace("make ", ""));
                }
                catch (Exception)
                {
                    Console.WriteLine("Can't 'MAKE' a file.");
                }
            }
            else if (cmd.StartsWith("rem "))
            {
                try
                {
                    File.Delete(data.currentDir + cmd.Replace("rem ", ""));
                }
                catch (Exception)
                {
                    Console.WriteLine("Can't remove the file.");
                }
            }
            else if (cmd.StartsWith("makedir "))
            {
                try
                {
                    Directory.CreateDirectory(data.currentDir + cmd.Replace("makedir ", ""));
                }
                catch (Exception)
                {
                    Console.WriteLine("Can't make the directory.");
                }
            }
            else if (cmd.StartsWith("remdir "))
            {
                try
                {
                    Directory.Delete(data.currentDir + cmd.Replace("remdir ", ""));
                }
                catch (Exception)
                {
                    Console.WriteLine("Can't remove the directory. [Make sure it's an empty directory]");
                }
            }
            else if (cmd.StartsWith("changedir "))
            {
                if (Directory.Exists(cmd.Replace("changedir ", data.currentDir + "\\")))
                {
                    data.currentDir += "\\" + cmd.Replace("changedir ", "") + "\\";
                }
            }
            else if (cmd.StartsWith("listdir"))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(Directory.GetDirectories(data.currentDir));
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(Directory.GetFiles(data.currentDir) + "\n");
            } else if (cmd.StartsWith("dirinfo"))
            {
                //Console.WriteLine("Directory made on: " + Directory.GetCreationTime(data.currentDir).ToString());
                //Console.WriteLine("Last modified    : " + Directory.GetLastWriteTime(data.currentDir).ToString());
                //Console.WriteLine("Directory Parent : " + Directory.GetParent(data.currentDir).ToString());
            } else if (cmd.StartsWith("fedit "))
            {
                feditor.Run(cmd.Split(' '));                
            } else if (cmd.StartsWith("clear"))
            {
                Console.Clear();
            } else if (cmd.StartsWith("exit"))
            {
                Sys.Power.Shutdown();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Unrecognized Command!");
            }
            Console.ResetColor();
            data.ProcMgr.removeProc(cmd);
            return; 
        }
    }
}
