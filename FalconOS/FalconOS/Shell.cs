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
using Cosmos.System.FileSystem.VFS;
using Cosmos.System.Graphics;
using Cosmos.System.Network;
using Cosmos.HAL;
using Cosmos.System.Network.IPv4.UDP.DHCP;
using Cosmos.Core;

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

        public void exec(string cmd, bool root = false)
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
            } else if (cmd.StartsWith("sysctl proclist"))
            {
                Console.WriteLine("Processes(" + data.ProcMgr.processes.Count + "):");
                foreach (var process in data.ProcMgr.processes)
                {
                    Console.WriteLine(process);
                }
            } else if (cmd.StartsWith("sysctl console-cursor toggle")) {
                Console.Write("Console-Cursor: ");
                if (Console.CursorVisible)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("OFF");
                    Console.CursorVisible = !Console.CursorVisible;
                    Console.ResetColor();
                } else if (!Console.CursorVisible)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("ON");
                    Console.CursorVisible = !Console.CursorVisible;
                    Console.ResetColor();
                }
            } else if (cmd.StartsWith("sysctl"))
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
            } else if (cmd.StartsWith("touch"))
            {
                try
                {
                    File.Create(cmd.Replace("touch ", data.currentDir));
                }
                catch (Exception)
                {
                    Console.WriteLine("Can't make a file.");
                }
            }
            else if (cmd.StartsWith("rm") && cmd.StartsWith("rm "))
            {
                try
                {
                    VFSManager.DeleteFile(cmd.Replace("rm ", data.currentDir));
                }
                catch (Exception)
                {
                    Console.WriteLine("Can't remove the file.");
                }
            }
            else if (cmd.StartsWith("mkdir") && cmd.StartsWith("mkdir "))
            {
                try
                {
                    if (!Directory.Exists(cmd.Replace("mkdir ", data.currentDir)))
                    {
                        Directory.CreateDirectory(cmd.Replace("mkdir ", data.currentDir));
                    }
                    else
                    {
                        Console.WriteLine("ERROR: Can't make directory, (does it already exist?)");
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("ERROR: Can't make directory, (does it already exist?)");
                }
            }
            else if (cmd.StartsWith("rmdir") && cmd.StartsWith("rmdir "))
            {
                try
                {
                    Directory.Delete(cmd.Replace("rmdir ", data.currentDir));
                }
                catch (Exception)
                {
                    Console.WriteLine("Can't remove the directory.");
                }
            }
            else if (cmd.StartsWith("cd"))
            {
                if (cmd.StartsWith("cd ..."))
                {
                    data.currentDir = "0:\\";
                    return;
                }
                if (Directory.Exists(cmd.Replace("cd ", data.currentDir + "\\")))
                {
                    data.currentDir += cmd.Replace("cd ", "") + "\\";
                    data.lastDir = cmd.Replace("cd ", "") + "\\";
                } else
                {
                    Console.WriteLine("Invalid Directory!");
                }
            } else if (cmd.StartsWith("uname"))
            {
                Console.WriteLine("uname v1.0 utility");
                if (cmd.StartsWith("uname "))
                {
                    if (cmd.StartsWith("uname -a"))
                    {
                        log.programPrint("uname", data.osname + " " + data.vername + " " + data.ver);
                    } else if (cmd.StartsWith("uname -s"))
                    {
                        log.programPrint("uname", "Falcon Stable");
                    } else if (cmd.StartsWith("uname -n"))
                    {
                        log.programPrint("uname", "falcon");
                    } else if (cmd.StartsWith("uname -r"))
                    {
                        log.programPrint("uname", "1.0.1-normal");
                    } else if (cmd.StartsWith("uname -v"))
                    {
                        log.programPrint("uname", "Not available.");
                    } else if (cmd.StartsWith("uname -m") || cmd.StartsWith("uname -p") || cmd.StartsWith("uname -i"))
                    {
                        log.programPrint("uname", "x86");
                    } else if (cmd.StartsWith("uname -o"))
                    {
                        log.programPrint("uname", "Falcon");
                    } else
                    {
                        log.programPrint("uname", "Unrecognized Command!");
                    }
                }
            }
            else if (cmd.StartsWith("ls"))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                foreach (var dirs in Directory.GetDirectories(data.currentDir))
                {
                    Console.Write(dirs + " ");
                }
                Console.ForegroundColor = ConsoleColor.Yellow;
                foreach (var files in Directory.GetFiles(data.currentDir))
                {
                    Console.Write(files + " ");
                }
                Console.Write("\n");
            } else if (cmd.StartsWith("fedit "))
            {
                feditor.Run(cmd.Split(' '));
            } else if (cmd.StartsWith("clear"))
            {
                Console.Clear();
            } else if (cmd.StartsWith("exit"))
            {
                Sys.Power.Shutdown();
            } else if (cmd == "") { }
            else if (cmd.StartsWith("fash "))
            {
                Console.WriteLine("Starting fash..");
                if (File.Exists(cmd.Replace("fash ", data.currentDir)) && cmd.Replace("fash ", data.currentDir).EndsWith(".ash"))
                {
                    string[] lines = File.ReadAllLines(cmd.Replace("fash ", data.currentDir));

                    foreach (string line in lines)
                    {
                        if (!line.StartsWith("! "))
                        {
                            exec(line);
                        }
                    }
                } else
                {
                    Console.WriteLine("Error: Either file doesn't exist [Use: fash <file.ash>] or doesn't end in .ash");
                }
            } else if (cmd.StartsWith("echo"))
            {
                if (cmd.StartsWith("echo --no-new-line \""))
                {
                    cmd = cmd.Replace("echo --no-new-line ", "echo ");
                }
                if (cmd.StartsWith("echo \"") && cmd.EndsWith("\""))
                {
                    Console.WriteLine(cmd.Replace("echo \"", "").TrimEnd('\"'));
                }
            } else if (cmd.StartsWith("sleep"))
            {
                if (cmd.StartsWith("sleep "))
                {
                    try
                    {
                        Thread.Sleep(Convert.ToInt32(cmd.Replace("wait ", "")));
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Invalid time [Use: wait <milliseconds>]");
                    }
                }
            } else if (cmd.StartsWith("cat"))
            {
                if (cmd.StartsWith("cat "))
                {
                    if (File.Exists(cmd.Replace("cat ", data.currentDir)))
                    {
                        Console.WriteLine(File.ReadAllText(cmd.Replace("cat ", data.currentDir)));
                    } else
                    {
                        Console.WriteLine("ERROR: Invalid File. [Use: cat <file>]");
                    }
                }
            } else if (cmd.StartsWith("ver"))
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write(this.os + " ");
                Console.ResetColor();
                Console.WriteLine(this.ver);
            } else if (cmd.StartsWith("falcp"))
            {
                if (cmd.StartsWith("falcp "))
                {
                    if (cmd.StartsWith("falcp --help") || cmd.StartsWith("falcp -h"))
                    {
                        Console.WriteLine("FalCompile [falcp] v0.5");
                        Console.WriteLine("FalCompile is a FalVM compiler to compile .fal files to .fex [Falcon Executable]");
                        Console.WriteLine("Uses:\n   falcp <file> (to get a <file>.fex file that is compiled)\n   falcp --help/-h (to get this prompt)");
                    } else if (cmd.EndsWith(".fa"))
                    {
                        string[] program;
                        if (File.Exists(cmd.Replace("falcp ", "")))
                        {
                            program = File.ReadAllLines(cmd.Replace("falcp ", data.currentDir));
                        } else
                        {
                            log.programPrint("falcp", "ERROR: File doesn't exist.");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("FalCompile [falcp] v0.5");
                    Console.WriteLine("FalCompile is a FalVM compiler to compile .fal files to .fex [Falcon Executable]");
                    Console.WriteLine("Uses:\n   falcp <file>.fa (to get a <file>.fex file that is compiled)\n   falcp --help/-h (to get this prompt)");
                }
            } else if (cmd.StartsWith("pwd"))
            {
                Console.WriteLine(data.currentDir);
            } else if (cmd.StartsWith("sudo "))
            {
                sudo su = new sudo(Kernel.cUser);
                su.execAsRoot(cmd.Replace("sudo ", ""));
            } else if (cmd.StartsWith("usrname"))
            {
                Console.WriteLine(Kernel.cUser);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Unrecognized Command!");
            }
            Console.ResetColor();
            data.lastCMD = cmd;
            data.ProcMgr.removeProc(cmd);
            return; 
        }
    }
}
